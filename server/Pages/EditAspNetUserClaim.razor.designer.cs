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
    public partial class EditAspNetUserClaimComponent : ComponentBase
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

        [Parameter]
        public dynamic Id { get; set; }

        IyaElepoApp.Models.ConData.AspNetUserClaim _aspnetuserclaim;
        protected IyaElepoApp.Models.ConData.AspNetUserClaim aspnetuserclaim
        {
            get
            {
                return _aspnetuserclaim;
            }
            set
            {
                if (!object.Equals(_aspnetuserclaim, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "aspnetuserclaim", NewValue = value, OldValue = _aspnetuserclaim };
                    _aspnetuserclaim = value;
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
            var conDataGetAspNetUserClaimByIdResult = await ConData.GetAspNetUserClaimById(Id);
            aspnetuserclaim = conDataGetAspNetUserClaimByIdResult;

            var conDataGetAspNetUsersResult = await ConData.GetAspNetUsers();
            getAspNetUsersResult = conDataGetAspNetUsersResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(IyaElepoApp.Models.ConData.AspNetUserClaim args)
        {
            try
            {
                var conDataUpdateAspNetUserClaimResult = await ConData.UpdateAspNetUserClaim(Id, aspnetuserclaim);
                DialogService.Close(aspnetuserclaim);
            }
            catch (System.Exception conDataUpdateAspNetUserClaimException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update AspNetUserClaim" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
