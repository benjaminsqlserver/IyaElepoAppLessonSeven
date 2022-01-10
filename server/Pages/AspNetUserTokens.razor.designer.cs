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
    public partial class AspNetUserTokensComponent : ComponentBase
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
        protected RadzenDataGrid<IyaElepoApp.Models.ConData.AspNetUserToken> grid0;

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

        IEnumerable<IyaElepoApp.Models.ConData.AspNetUserToken> _getAspNetUserTokensResult;
        protected IEnumerable<IyaElepoApp.Models.ConData.AspNetUserToken> getAspNetUserTokensResult
        {
            get
            {
                return _getAspNetUserTokensResult;
            }
            set
            {
                if (!object.Equals(_getAspNetUserTokensResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAspNetUserTokensResult", NewValue = value, OldValue = _getAspNetUserTokensResult };
                    _getAspNetUserTokensResult = value;
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

            var conDataGetAspNetUserTokensResult = await ConData.GetAspNetUserTokens(new Query() { Filter = $@"i => i.UserId.Contains(@0) || i.LoginProvider.Contains(@1) || i.Name.Contains(@2) || i.Value.Contains(@3)", FilterParameters = new object[] { search, search, search, search } });
            getAspNetUserTokensResult = conDataGetAspNetUserTokensResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAspNetUserToken>("Add Asp Net User Token", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConData.ExportAspNetUserTokensToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "UserId,LoginProvider,Name,Value" }, $"Asp Net User Tokens");

            }

            if (args == null || args.Value == "xlsx")
            {
                await ConData.ExportAspNetUserTokensToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "UserId,LoginProvider,Name,Value" }, $"Asp Net User Tokens");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(IyaElepoApp.Models.ConData.AspNetUserToken args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAspNetUserToken>("Edit Asp Net User Token", new Dictionary<string, object>() { {"UserId", args.UserId}, {"LoginProvider", args.LoginProvider}, {"Name", args.Name} });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var conDataDeleteAspNetUserTokenResult = await ConData.DeleteAspNetUserToken($"{data.UserId}", $"{data.LoginProvider}", $"{data.Name}");
                    if (conDataDeleteAspNetUserTokenResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception conDataDeleteAspNetUserTokenException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete AspNetUserToken" });
            }
        }
    }
}
