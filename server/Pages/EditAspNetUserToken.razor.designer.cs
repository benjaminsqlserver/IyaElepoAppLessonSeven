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
    public partial class EditAspNetUserTokenComponent : ComponentBase
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
        public dynamic UserId { get; set; }

        [Parameter]
        public dynamic LoginProvider { get; set; }

        [Parameter]
        public dynamic Name { get; set; }

        IyaElepoApp.Models.ConData.AspNetUserToken _aspnetusertoken;
        protected IyaElepoApp.Models.ConData.AspNetUserToken aspnetusertoken
        {
            get
            {
                return _aspnetusertoken;
            }
            set
            {
                if (!object.Equals(_aspnetusertoken, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "aspnetusertoken", NewValue = value, OldValue = _aspnetusertoken };
                    _aspnetusertoken = value;
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
            var conDataGetAspNetUserTokenByUserIdAndLoginProviderAndNameResult = await ConData.GetAspNetUserTokenByUserIdAndLoginProviderAndName($"{UserId}", $"{LoginProvider}", $"{Name}");
            aspnetusertoken = conDataGetAspNetUserTokenByUserIdAndLoginProviderAndNameResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(IyaElepoApp.Models.ConData.AspNetUserToken args)
        {
            try
            {
                var conDataUpdateAspNetUserTokenResult = await ConData.UpdateAspNetUserToken($"{UserId}", $"{LoginProvider}", $"{Name}", aspnetusertoken);
                DialogService.Close(aspnetusertoken);
            }
            catch (System.Exception conDataUpdateAspNetUserTokenException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update AspNetUserToken" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
