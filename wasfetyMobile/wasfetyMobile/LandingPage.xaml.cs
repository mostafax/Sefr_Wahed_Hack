using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wasfetyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : CarouselPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }
        public async void Jump()
        {
            await Navigation.PushAsync(new AppBody());
            Navigation.RemovePage(this);
        }
        private void CarouselPage_CurrentPageChanged(object sender, EventArgs e)
        {
            TitleBarLabel.Text = CurrentPage.Title;
        }
    }
}