using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Radzen;
using Radzen.Blazor;

namespace IyaElepoApp.Pages
{
    public partial class PeriodicProductSupplyReportComponent
    {
        private async Task UpdateInitialReportStartAndEndDatesAsync()
        {
            try
            {
                //set report initial report start date to 30 days ago
                startDate = DateTime.Today.AddDays(-30);
                //set initial report end date to current date and time
                endDate = DateTime.Now;

               await FetchPeriodicProductSupplyReportAsync();
            }
            catch(Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "App Error!", ex.Message, 7000);
            }
        }
    

        //method to reset the report page

        private async Task ResetPageAsync()
        {
            try
            {
                await UpdateInitialReportStartAndEndDatesAsync();
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "App Error!", ex.Message, 7000);
            }
        }

        //method to fetch report
        private async Task FetchPeriodicProductSupplyReportAsync()
        {
            try
            {
                getProductSupplyReportViewsResult = await ConData.FetchPeriodicProductSupplyAsync(startDate, endDate);
            }
            catch (Exception ex)
            {
                NotificationService.Notify(NotificationSeverity.Error, "App Error!", ex.Message, 7000);
            }
        }
    
    }
}
