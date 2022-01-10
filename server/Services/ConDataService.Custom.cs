using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using IyaElepoApp.Data;
using IyaElepoApp.Models.ConData;

namespace IyaElepoApp
{
    public partial class ConDataService
    {
        public async Task<Models.ConData.AspNetUser> FetchAspNetUserByEmail(string email)
        {
            var returnedUser=new Models.ConData.AspNetUser();

            try
            {
                var returnedUser1 = context.AspNetUsers.AsNoTracking().FirstOrDefault(p => p.Email == email);
                if(returnedUser1!=null)
                {
                    returnedUser = returnedUser1;
                }

            }
            catch(Exception ex)
            {
                throw;
            }

            return await Task.FromResult(returnedUser);
        }

        public async Task DeleteAllRolesForUser(string id)
        {
            try
            {
                var existingUserRoles = context.AspNetUserRoles.Where(p => p.UserId == id).ToList();
                if(existingUserRoles.Any())
                {
                    context.AspNetUserRoles.RemoveRange(existingUserRoles);
                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Models.ConData.ProductSupplyReportView>> FetchPeriodicProductSupplyAsync(DateTime startDate, DateTime endDate)
        {
            List<Models.ConData.ProductSupplyReportView> reportData = new List<Models.ConData.ProductSupplyReportView>();

            try
            {
                var reportData1 = context.ProductSupplyReportViews.Where(p => p.SupplyDate >= startDate && p.SupplyDate <= endDate).AsQueryable();

                if(reportData1.Any())
                {
                    reportData = reportData1.ToList();
                }


            }
            catch(Exception ex)
            {
                throw;
            }

            return await Task.FromResult(reportData);
        }

        public async Task UpdateAspNetUserV2(string id, AspNetUser existingUser2,string connectionString)
        {
            var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            try
            {
                var command = new Microsoft.Data.SqlClient.SqlCommand();
                command.Connection = connection;
                command.CommandText = "UpdateApplicationUserV2";
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 1000000;
                command.Parameters.AddWithValue("@UserID", id);
                command.Parameters.AddWithValue("@FirstName", existingUser2.FirstName);
                if(string.IsNullOrEmpty(existingUser2.MiddleName))
                {
                    command.Parameters.AddWithValue("@MiddleName", "");
                }
                else
                {
                    command.Parameters.AddWithValue("@MiddleName", existingUser2.MiddleName);
                }
                
                command.Parameters.AddWithValue("@Surname", existingUser2.Surname);
                command.Parameters.AddWithValue("@CustomerID", Convert.ToInt64(existingUser2.CustomerID));
                command.Parameters.AddWithValue("@GenderID", Convert.ToInt64(existingUser2.GenderID));

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();



            }
            catch(Exception ex)
            {
                throw;
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
    }
}
