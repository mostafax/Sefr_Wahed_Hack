using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SharedModels;
using wasfetyMobile.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wasfetyMobile
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraPage : ContentPage
    {
        byte[] image;
        public CameraPage()
        {
            InitializeComponent();
            string path = "";
            switch(Device.RuntimePlatform)
            {
                case Device.Android:
                    path = Path.Combine(App.AndroidPath, "Pictures/temp/");
                    break;
                default:
                    throw new NotImplementedException("This platform is not yet supported");
            }
            /*
            bool imageLoaded = false;
            foreach(string file in Directory.EnumerateFiles(path))
            {
                if (!imageLoaded)
                {
                    image = File.ReadAllBytes(file);
                    ImageViewer.Source = ImageSource.FromStream(() => new MemoryStream(image));
                    imageLoaded = true;
                }
                File.Delete(file);
            }
            */
        }

        private void TakePhoto_Clicked(object sender, EventArgs e)
        {
            Initialize();
        }
        private void Upload_image(object sender, EventArgs e)
        {
            InitializeUpload();
        }
        private async void InitializeUpload()
        {
            MediaFile file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                CompressionQuality = 30,
                PhotoSize = PhotoSize.Small,
            });
            if (file != null)
            {
                MealStackLayout.IsVisible = false;
                ImageViewer.IsVisible = true;
                Stream s = file.GetStream();
                image = new byte[s.Length];
                s.Read(image, 0, (int)s.Length);
                ImageViewer.Source = ImageSource.FromStream(() => new MemoryStream(image));
            }
        }
        private async void Initialize()
        {
            SubmitButton.IsEnabled = false;
            /*
            foreach (string filePath in Directory.EnumerateFiles(Path.Combine(App.AndroidPath, "Pictures/temp/")))
            {
                File.Delete(filePath);
            }
            */
            MediaFile file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Name = "shot.jpg",
                SaveToAlbum = true,
                AllowCropping = true,
                CompressionQuality = 30,
                PhotoSize = PhotoSize.Small
            });
            //string path = file.Path;
            if (file != null)
            {
                MealStackLayout.IsVisible = false;
                ImageViewer.IsVisible = true;
                Stream s = file.GetStream();
                image = new byte[s.Length];
                s.Read(image, 0, (int)s.Length);
                s.Close();
                ImageViewer.Source = ImageSource.FromStream(() => new MemoryStream(image));
                /*
                foreach (string fileName in Directory.EnumerateFiles(Path.Combine(App.AndroidPath, "Pictures/temp/")))
                {
                    File.Delete(fileName);
                };
                */
            }
            SubmitButton.IsEnabled = true;
        }

        private async void SubmitClicked(object sender, EventArgs e)
        {
            if(image == null)
            {
                return;
            }
            SubmitButton.IsEnabled = false;
            TakePhoto.IsEnabled = false;
            Upload_Image.IsEnabled = false;
            Meal meal = null;
            try
            {
                ProgressReport progress = new ProgressReport();
                UploadBar.IsVisible = true;
                UploadBar.BindingContext = UploadLabel.BindingContext = progress;
                NetGate netGate = new NetGate(progress);
                await Task.Run(async () =>
                {
                    meal = await netGate.GetMeal(image);
                });
                UploadBar.IsVisible = false;
                progress.Percentage = 0;
            }
            catch (InvalidDataException ee)
            {
                await DisplayAlert("Error", ee.Message, "Ok");
                SubmitButton.IsEnabled = true;
                return;
            }
            if(meal == null)
            {
                await DisplayAlert("Error", "Couldn't find food in the picture", "Ok");
                SubmitButton.IsEnabled = true;
                TakePhoto.IsEnabled = true;
                Upload_Image.IsEnabled = true;
                return;
            }
            App.HistoryMealsPage.AddToHistory(new AppMealHistory(meal, Convert.ToBase64String(image)));
            ImageViewer.IsVisible = false;
            MealStackLayout.IsVisible = true;
            MealImage.Source = ImageSource.FromStream(() => new MemoryStream(image));
            MealName.Text = meal.Title;
            StepsStackLayout.Children.Clear();
            foreach (var i in meal.Instructions)
            {
                StepsStackLayout.Children.Add(new Label
                {
                    TextColor = Color.FromHex("#d47691"),
                    Margin = new Thickness(10, 5, 10, 5),
                    FontSize = 16,
                    Text = i
                });
                StepsStackLayout.Children.Add(new BoxView
                {
                    WidthRequest = 0.1,
                    HeightRequest = 0.5,
                    Color = Color.LightGray
                });
            }
            IngredientsListView.ItemsSource = meal.Ingredients;
            IngredientsListView.HeightRequest = meal.Ingredients.Count * 45;
            SubmitButton.IsEnabled = true;
            TakePhoto.IsEnabled = true;
            Upload_Image.IsEnabled = true;
        }

    }
}