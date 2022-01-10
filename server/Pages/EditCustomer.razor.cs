using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;
using Microsoft.Extensions.Configuration;

namespace IyaElepoApp.Pages
{
    public partial class EditCustomerComponent
    {
       private async Task UpdateCustomerAsync()
        {
            try
            {
                //first we update customer object
                IyaElepoApp.Models.ConData.Customer customerUpdateResult = await ConData.UpdateCustomer(CustomerID, customer);
                if(customerUpdateResult != null)
                {
                    if(customerUpdateResult.CustomerID > 0)//successfull update
                    {
                        //first get customer's user record
                        IyaElepoApp.Models.ConData.AspNetUser existingUser = await ConData.FetchAspNetUserByEmail(customerUpdateResult.Email);
                        //delete current user record
                        var deleteResult = await Security.DeleteUser(existingUser.Id);

                        // string[] roles = { "Customer" };
                        var newUser = new Models.ApplicationUser { UserName = customer.Email, Email = customer.Email, Password = userPassword, ConfirmPassword = confirmUserPassword };
                        newUser.RoleNames = new List<string>();
                        newUser.RoleNames.Append("Customer");
                        var createUserResult = await Security.CreateUser(newUser);

                        if(!string.IsNullOrEmpty(createUserResult.Id))
                        {
                            string[] customerNames = customer.CustomerName.Split(" ");
                            //we pass genderID property value to existing user object returned from database.
                            existingUser.GenderID = GenderID;
                            if (customerNames.Length > 0 && customerNames.Length < 2)
                            {
                                existingUser.FirstName = customerNames[0];
                            }
                            else if (customerNames.Length > 0 && customerNames.Length < 3)
                            {
                                existingUser.FirstName = customerNames[0];
                                existingUser.Surname = customerNames[1];
                            }
                            else if (customerNames.Length > 2)
                            {
                                existingUser.FirstName = customerNames[0];
                                existingUser.MiddleName = customerNames[1];
                                existingUser.Surname = customerNames[2];
                            }

                            existingUser.CustomerID = customerUpdateResult.CustomerID;

                            //var updateResult = await ConData.UpdateAspNetUser(createUserResult.Id, existingUser);
                            var connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["ConDataConnection"];
                            await ConData.UpdateAspNetUserV2(createUserResult.Id, existingUser,connectionString);

                            UriHelper.NavigateTo("customers", true);//navigate to list page

                        }
                        else
                        {
                            NotificationService.Notify(NotificationSeverity.Error, "Customer Update Error!", "An Error occurred while updating customer details!", 7000);
                        }

                    }
                    else
                    {
                        NotificationService.Notify(NotificationSeverity.Error, "Customer Update Error!", "An Error occurred while updating customer details!", 7000);
                    }
                }
                else
                {
                    NotificationService.Notify(NotificationSeverity.Error, "Customer Update Error!", "An Error occurred while updating customer details!", 7000);
                }
            }
            catch(Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "Customer Update Error!", ex.Message, 7000);
            }
        }
    }
}
