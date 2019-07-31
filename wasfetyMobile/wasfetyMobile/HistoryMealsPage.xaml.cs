using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wasfetyMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wasfetyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryMealsPage : ContentPage
    {
        private readonly ObservableCollection<AppMealHistory> Histories = new ObservableCollection<AppMealHistory>();
        public HistoryMealsPage()
        {
            InitializeComponent();
            App.HistoryMealsPage = this;
            LoadHistoryPage();
        }
        public void AddToHistory(AppMealHistory history)
        {
            Histories.Insert(0, history);
        }
        private async void LoadHistoryPage()
        {
            ItemsListView.ItemsSource = Histories;
            foreach (var item in await App.Db.GetMeals())
            {
                Histories.Add(item);
            }
        }
        protected override bool OnBackButtonPressed()
        {
            if(Frame.IsVisible == false)
            {
                return base.OnBackButtonPressed();
            }   
            Frame.IsVisible = false;
            ItemsListView.IsVisible = true;
            return true;
        }
        private void CloseFrame(object sender, EventArgs e)
        {
            Frame.IsVisible = false;
            ItemsListView.IsVisible = true;
        }
        private void ItemsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Frame.IsVisible = true;
            ItemsListView.IsVisible = false;
            FameMealImage.Source = Histories[e.ItemIndex].Image;
            FrameMealName.Text = Histories[e.ItemIndex].Title;
            FrameStepsStackLayout.Children.Clear();
            foreach(var i in Histories[e.ItemIndex].Instructions)
            {
                FrameStepsStackLayout.Children.Add(new Label
                {
                    TextColor = Color.FromHex("#d47691"),
                    Margin = new Thickness(10,5,10,5),
                    FontSize = 16,
                    Text = i
                });
                FrameStepsStackLayout.Children.Add(new BoxView
                {
                    WidthRequest = 0.1,
                    HeightRequest = 0.5,
                    Color = Color.LightGray
                });
            }

            FrameIngredientsListView.ItemsSource = Histories[e.ItemIndex].Ingredients;
            FrameIngredientsListView.HeightRequest = Histories[e.ItemIndex].Ingredients.Count * 45;
        }
    }
}