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
    public partial class AddAspNetUserRoleComponent : ComponentBase
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

        IyaElepoApp.Models.ConData.AspNetUserRole _aspnetuserrole;
        protected IyaElepoApp.Models.ConData.AspNetUserRole aspnetuserrole
        {
            get
            {
                return _aspnetuserrole;
            }
            set
            {
                if (!object.Equals(_aspnetuserrole, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "aspnetuserrole", NewValue = value, OldValue = _aspnetuserrole };
                    _aspnetuserrole = value;
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
            var conDataGetAspNetUsersResult = await ConData.GetAspNetUsers();
            getAspNetUsersResult = conDataGetAspNetUsersResult;

            var conDataGetAspNetRolesResult = await ConData.GetAspNetRoles();
            getAspNetRolesResult = conDataGetAspNetRolesResult;

            aspnetuserrole = new IyaElepoApp.Models.ConData.AspNetUserRole(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(IyaElepoApp.Models.ConData.AspNetUserRole args)
        {
            try
            {
                var conDataCreateAspNetUserRoleResult = await ConData.CreateAspNetUserRole(aspnetuserrole);
                DialogService.Close(aspnetuserrole);
            }
            catch (System.Exception conDataCreateAspNetUserRoleException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new AspNetUserRole!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
