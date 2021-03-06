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
    public partial class EditAspNetUserComponent : ComponentBase
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

        IyaElepoApp.Models.ConData.AspNetUser _aspnetuser;
        protected IyaElepoApp.Models.ConData.AspNetUser aspnetuser
        {
            get
            {
                return _aspnetuser;
            }
            set
            {
                if (!object.Equals(_aspnetuser, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "aspnetuser", NewValue = value, OldValue = _aspnetuser };
                    _aspnetuser = value;
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
            var conDataGetAspNetUserByIdResult = await ConData.GetAspNetUserById($"{Id}");
            aspnetuser = conDataGetAspNetUserByIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(IyaElepoApp.Models.ConData.AspNetUser args)
        {
            try
            {
                var conDataUpdateAspNetUserResult = await ConData.UpdateAspNetUser($"{Id}", aspnetuser);
                DialogService.Close(aspnetuser);
            }
            catch (System.Exception conDataUpdateAspNetUserException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update AspNetUser" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
