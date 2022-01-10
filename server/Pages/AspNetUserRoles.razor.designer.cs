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
    public partial class AspNetUserRolesComponent : ComponentBase
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
        protected RadzenDataGrid<IyaElepoApp.Models.ConData.AspNetUserRole> grid0;

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

        IEnumerable<IyaElepoApp.Models.ConData.AspNetUserRole> _getAspNetUserRolesResult;
        protected IEnumerable<IyaElepoApp.Models.ConData.AspNetUserRole> getAspNetUserRolesResult
        {
            get
            {
                return _getAspNetUserRolesResult;
            }
            set
            {
                if (!object.Equals(_getAspNetUserRolesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAspNetUserRolesResult", NewValue = value, OldValue = _getAspNetUserRolesResult };
                    _getAspNetUserRolesResult = value;
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

            var conDataGetAspNetUserRolesResult = await ConData.GetAspNetUserRoles(new Query() { Filter = $@"i => i.UserId.Contains(@0) || i.RoleId.Contains(@1)", FilterParameters = new object[] { search, search }, Expand = "AspNetUser,AspNetRole" });
            getAspNetUserRolesResult = conDataGetAspNetUserRolesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAspNetUserRole>("Add Asp Net User Role", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConData.ExportAspNetUserRolesToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "AspNetUser,AspNetRole", Select = "AspNetUser.ConcurrencyStamp as AspNetUserConcurrencyStamp,AspNetRole.ConcurrencyStamp as AspNetRoleConcurrencyStamp" }, $"Asp Net User Roles");

            }

            if (args == null || args.Value == "xlsx")
            {
                await ConData.ExportAspNetUserRolesToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "AspNetUser,AspNetRole", Select = "AspNetUser.ConcurrencyStamp as AspNetUserConcurrencyStamp,AspNetRole.ConcurrencyStamp as AspNetRoleConcurrencyStamp" }, $"Asp Net User Roles");

            }
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var conDataDeleteAspNetUserRoleResult = await ConData.DeleteAspNetUserRole($"{data.UserId}", $"{data.RoleId}");
                    if (conDataDeleteAspNetUserRoleResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception conDataDeleteAspNetUserRoleException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete AspNetUserRole" });
            }
        }
    }
}
