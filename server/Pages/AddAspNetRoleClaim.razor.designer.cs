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
    public partial class AddAspNetRoleClaimComponent : ComponentBase
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

        IEnumerable<IyaElepoApp.Models.ConData.AspNetRole> _getAspNetRolesResult;
        protected IEnumerable<IyaElepoApp.Models.ConData.AspNetRole> getAspNetRolesResult
        {
            get
            {
                return _getAspNetRolesResult;
            }
            set
            {
                if (!object.Equals(_getAspNetRolesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAspNetRolesResult", NewValue = value, OldValue = _getAspNetRolesResult };
                    _getAspNetRolesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IyaElepoApp.Models.ConData.AspNetRoleClaim _aspnetroleclaim;
        protected IyaElepoApp.Models.ConData.AspNetRoleClaim aspnetroleclaim
        {
            get
            {
                return _aspnetroleclaim;
            }
            set
            {
                if (!object.Equals(_aspnetroleclaim, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "aspnetroleclaim", NewValue = value, OldValue = _aspnetroleclaim };
                    _aspnetroleclaim = value;
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
            var conDataGetAspNetRolesResult = await ConData.GetAspNetRoles();
            getAspNetRolesResult = conDataGetAspNetRolesResult;

            aspnetroleclaim = new IyaElepoApp.Models.ConData.AspNetRoleClaim(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(IyaElepoApp.Models.ConData.AspNetRoleClaim args)
        {
            try
            {
                var conDataCreateAspNetRoleClaimResult = await ConData.CreateAspNetRoleClaim(aspnetroleclaim);
                DialogService.Close(aspnetroleclaim);
            }
            catch (System.Exception conDataCreateAspNetRoleClaimException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new AspNetRoleClaim!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
