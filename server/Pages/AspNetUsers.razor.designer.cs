using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using IyaElepoApp.Models.ConData;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IyaElepoApp.Models;

namespace IyaElepoApp.Pages
{
    public partial class AspNetUsersComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        protected AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        protected ConDataService ConData { get; set; }
        protected RadzenDataGrid<IyaElepoApp.Models.ConData.AspNetUser> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<IyaElepoApp.Models.ConData.AspNetUser> _getAspNetUsersResult;
        protected IEnumerable<IyaElepoApp.Models.ConData.AspNetUser> getAspNetUsersResult
        {
            get
            {
                return _getAspNetUsersResult;
            }
            set
            {
                if (!object.Equals(_getAspNetUsersResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAspNetUsersResult", NewValue = value, OldValue = _getAspNetUsersResult };
                    _getAspNetUsersResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Security.InitializeAsync(AuthenticationStateProvider);
            if (!Security.IsAuthenticated())
            {
                UriHelper.NavigateTo("Login", true);
            }
            else
            {
                await Load();
            }
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var conDataGetAspNetUsersResult = await ConData.GetAspNetUsers(new Query() { Filter = $@"i => i.Id.Contains(@0) || i.ConcurrencyStamp.Contains(@1) || i.Email.Contains(@2) || i.NormalizedEmail.Contains(@3) || i.NormalizedUserName.Contains(@4) || i.PasswordHash.Contains(@5) || i.PhoneNumber.Contains(@6) || i.SecurityStamp.Contains(@7) || i.UserName.Contains(@8) || i.FirstName.Contains(@9) || i.MiddleName.Contains(@10) || i.Surname.Contains(@11)", FilterParameters = new object[] { search, search, search, search, search, search, search, search, search, search, search, search } });
            getAspNetUsersResult = conDataGetAspNetUsersResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAspNetUser>("Add Asp Net User", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConData.ExportAspNetUsersToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,FirstName,MiddleName,Surname,CustomerID,SupplierID,GenderID" }, $"Asp Net Users");

            }

            if (args == null || args.Value == "xlsx")
            {
                await ConData.ExportAspNetUsersToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "Id,AccessFailedCount,ConcurrencyStamp,Email,EmailConfirmed,LockoutEnabled,LockoutEnd,NormalizedEmail,NormalizedUserName,PasswordHash,PhoneNumber,PhoneNumberConfirmed,SecurityStamp,TwoFactorEnabled,UserName,FirstName,MiddleName,Surname,CustomerID,SupplierID,GenderID" }, $"Asp Net Users");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(IyaElepoApp.Models.ConData.AspNetUser args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAspNetUser>("Edit Asp Net User", new Dictionary<string, object>() { {"Id", args.Id} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var conDataDeleteAspNetUserResult = await ConData.DeleteAspNetUser($"{data.Id}");
                    if (conDataDeleteAspNetUserResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception conDataDeleteAspNetUserException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete AspNetUser" });
            }
        }
    }
}
