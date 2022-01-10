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
    public partial class PeriodicProductSupplyReportComponent : ComponentBase
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
        protected RadzenDataGrid<IyaElepoApp.Models.ConData.ProductSupplyReportView> grid0;

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

        IEnumerable<IyaElepoApp.Models.ConData.ProductSupplyReportView> _getProductSupplyReportViewsResult;
        protected IEnumerable<IyaElepoApp.Models.ConData.ProductSupplyReportView> getProductSupplyReportViewsResult
        {
            get
            {
                return _getProductSupplyReportViewsResult;
            }
            set
            {
                if (!object.Equals(_getProductSupplyReportViewsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getProductSupplyReportViewsResult", NewValue = value, OldValue = _getProductSupplyReportViewsResult };
                    _getProductSupplyReportViewsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        DateTime _startDate;
        protected DateTime startDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                if (!object.Equals(_startDate, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "startDate", NewValue = value, OldValue = _startDate };
                    _startDate = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        DateTime _endDate;
        protected DateTime endDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                if (!object.Equals(_endDate, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "endDate", NewValue = value, OldValue = _endDate };
                    _endDate = value;
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

            var conDataGetProductSupplyReportViewsResult = await ConData.GetProductSupplyReportViews(new Query() { Filter = $@"i => i.Supplier.Contains(@0) || i.Product.Contains(@1)", FilterParameters = new object[] { search, search } });
            getProductSupplyReportViewsResult = conDataGetProductSupplyReportViewsResult;

            startDate = new System.DateTime(1970,1,1);

            endDate = new System.DateTime(1970,1,1);

            await UpdateInitialReportStartAndEndDatesAsync();
        }

        protected async System.Threading.Tasks.Task BtnDisplayReportClick(MouseEventArgs args)
        {
            await FetchPeriodicProductSupplyReportAsync();
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await ConData.ExportProductSupplyReportViewsToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "SupplyID,SupplyDate,Supplier,Product,QuantityInLitres" }, $"Periodic Product Supply Report");

            }

            if (args == null || args.Value == "xlsx")
            {
                await ConData.ExportProductSupplyReportViewsToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "SupplyID,SupplyDate,Supplier,Product,QuantityInLitres" }, $"Periodic Product Supply Report");

            }
        }
    }
}
