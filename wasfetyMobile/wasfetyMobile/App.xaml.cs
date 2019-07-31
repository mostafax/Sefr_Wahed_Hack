using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using wasfetyMobile.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace wasfetyMobile
{
    public partial class App : Application
    {
        public static string AndroidPath { get; set; }
        public static string LocalDirectory { get; private set; }
        public static LandingPage LandingPage { get; private set; }
        public static HistoryMealsPage HistoryMealsPage { get; set; }
        static DbContext db;
        public static DbContext Db
        {
            get
            {
                if (db == null)
                {
                    db = new DbContext(
                      Path.Combine(LocalDirectory, "AppMealHistory.db3"));
                }
                return db;
            }
        }

        public App()
        {
            InitializeComponent();
            LocalDirectory = FileSystem.AppDataDirectory;
            if(NetGate.TryLoadCachedAccessToken())
            {
                MainPage = new NavigationPage(new AppBody());
            }
            else
            {
                LandingPage = new LandingPage();
                MainPage = new NavigationPage(LandingPage);
            }
        }

        protected override void OnStart()
        {
           // // this code opens the camera when the app starts like snapchat
           //     var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
           //     var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
           //  
           //     if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
           //     {
           //         cameraStatus = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
           //         storageStatus = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
           //     }
           //  
           //     if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
           //     {
           //         var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
           //         {
           //             Directory = "Sample",
           //             Name = "test.jpg"
           //         });
           //     }
        }

        protected override void OnSleep()
        { 
        }

        protected override void OnResume()
        {
        }
    }
}
