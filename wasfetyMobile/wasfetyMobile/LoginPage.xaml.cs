using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wasfetyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void LoginButtonClicked(object sender, EventArgs e)
        {
            Errors.IsVisible = false;
            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                Errors.IsVisible = true;
                Errors.Text = "Enter All Fields";
                return ;
            }
            LoginButton.IsEnabled = false;
            EmailEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = false;

            Pair<bool, string> res = null;
            await Task.Run(async () =>
            {
                res = await new NetGate().Login(EmailEntry.Text.Trim(), PasswordEntry.Text.Trim());
            });
            if (res.First)
            {
                App.LandingPage.Jump();
            }
            else
            {
                uint timeout = 50;
                WrongEntryAnimation(EmailEntry, timeout);
                WrongEntryAnimation(PasswordEntry, timeout);

                Errors.IsVisible = true;
                Errors.Text = res.Second;

                // await DisplayAlert("Error", res.Second, "Ok");
                LoginButton.IsEnabled = true;
                EmailEntry.IsEnabled = true;
                PasswordEntry.IsEnabled = true;
                return;
            }

        }

        private async void WrongEntryAnimation(Entry entry, uint timeout)
        {
            await entry.TranslateTo(-15, 0, timeout);
            await entry.TranslateTo(15, 0, timeout);
            await entry.TranslateTo(-10, 0, timeout);
            await entry.TranslateTo(10, 0, timeout);
            await entry.TranslateTo(-5, 0, timeout);
            await entry.TranslateTo(5, 0, timeout);
            entry.TranslationX = 0;

        }

        private void EmailEntry_Completed(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            LoginButton.Focus();
        }
    }
}