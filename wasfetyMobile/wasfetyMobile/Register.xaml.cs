using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wasfetyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
        }

        private async void RegisterBtnClicked(object sender, EventArgs e)
        {
            Errors.IsVisible = false;
            string Pass = PasswordEntry.Text;
            string PassConf = ConfirmPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text) || string.IsNullOrWhiteSpace(ConfirmPasswordEntry.Text))
            {
                if( Pass != PassConf)
                {
                    return;
                }
                Errors.IsVisible = true;
                Errors.Text = "Enter All Fields";
                return;
            }

            LoginButton.IsEnabled = false;
            EmailEntry.IsEnabled = false;
            PasswordEntry.IsEnabled = false;
            ConfirmPasswordEntry.IsEnabled = false;
            var res = await new NetGate().Register(EmailEntry.Text.Trim(), PasswordEntry.Text.Trim(), ConfirmPasswordEntry.Text.Trim());

            if (res.First )
            {
                var loginRes = await new NetGate().Login(EmailEntry.Text.Trim(), PasswordEntry.Text.Trim());

                if (loginRes.First)
                {
                    // await DisplayAlert("Success", "Yaay", "Ok");
                    await Navigation.PushAsync(new AppBody());
                    //await Navigation.PushAsync(new CameraPage());
                    //Navigation.RemovePage(this);
                }
                else
                {
                    throw new Exception($"Login failed after a successful register, reason = {loginRes.Second}");
                }
            }
            else
            {
                Errors.IsVisible = true;
                Errors.Text = res.Second;

                //await DisplayAlert("Error", res.Second, "Ok");
                LoginButton.IsEnabled = true;
                EmailEntry.IsEnabled = true;
                PasswordEntry.IsEnabled = true;
                ConfirmPasswordEntry.IsEnabled = true;
            }
        }

        private void EmailEntry_Completed(object sender, EventArgs e)
        {
            PasswordEntry.Focus();
        }

        private void PasswordEntry_Completed(object sender, EventArgs e)
        {
            ConfirmPasswordEntry.Focus();
        }

        private void ConfirmPasswordEntry_Completed(object sender, EventArgs e)
        {
            LoginButton.Focus();
        }
    }
}