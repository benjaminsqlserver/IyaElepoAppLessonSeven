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
    public partial class AspNetUserLoginsComponent : ComponentBase
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
        protected RadzenDataGrid<IyaElepoApp.Models.ConData.AspNetUserLogin> grid0;

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

        IEnumerable<IyaElepoApp.Models.ConData.AspNetUserLogin> _getAspNetUserLoginsResult;
        protected IEnumerable<IyaElepoApp.Models.ConData.AspNetUserLogin> getAspNetUserLoginsResult
        {
            get
            {
                return _getAspNetUserLoginsResult;
            }
            set
            {
                if (!object.Equals(_getAspNetUserLoginsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAspNetUserLoginsResult", NewValue = value, OldValue = _getAspNetUserLoginsResult };
                    _getAspNetUserLoginsResult = value;
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

            var conDataGetAspNetUserLoginsResult = await ConData.GetAspNetUserLogins(new Query() { Filter = $@"i => i.LoginProvider.Contains(@0) || i.ProviderKey.Contains(@1) || i.ProviderDisplayName.Contains(@2) || i.UserId.Contains(@3)", FilterParameters = new object[] { search, search, search, search }, Expand = "AspNetUser" });
            getAspNetUserLoginsResult = conDataGetAspNetUserLoginsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAspNetUserLogin>("Add Asp Net User Login", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConData.ExportAspNetUserLoginsToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "AspNetUser", Select = "LoginProvider,ProviderKey,ProviderDisplayName,AspNetUser.ConcurrencyStamp as AspNetUserConcurrencyStamp" }, $"Asp Net User Logins");

            }

            if (args == null || args.Value == "xlsx")
            {
                await ConData.ExportAspNetUserLoginsToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "AspNetUser", Select = "LoginProvider,ProviderKey,ProviderDisplayName,AspNetUser.ConcurrencyStamp as AspNetUserConcurrencyStamp" }, $"Asp Net User Logins");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(IyaElepoApp.Models.ConData.AspNetUserLogin args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAspNetUserLogin>("Edit Asp Net User Login", new Dictionary<string, object>() { {"LoginProvider", args.LoginProvider}, {"ProviderKey", args.ProviderKey} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var conDataDeleteAspNetUserLoginResult = await ConData.DeleteAspNetUserLogin($"{data.LoginProvider}", $"{data.ProviderKey}");
                    if (conDataDeleteAspNetUserLoginResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception conDataDeleteAspNetUserLoginException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete AspNetUserLogin" });
            }
        }
    }
}
