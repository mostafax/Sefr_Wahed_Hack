using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedModels;
using SQLite;
using Xamarin.Forms;

namespace wasfetyMobile.Models
{
    public class AppMealHistory : MealHistory, INotifyPropertyChanged
    {
        [PrimaryKey]
        public override int Id { get => base.Id; set => base.Id = value; }
        [Ignore]
        public override List<string> Ingredients { get => base.Ingredients; set => base.Ingredients = value; }
        [Ignore]
        public override List<string> Instructions { get => base.Instructions; set => base.Instructions = value; }
        public string Email { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        private float percentage;
        private bool Loading;
        private bool barVisible = true;
        private readonly ProgressReport Progress = new ProgressReport();
        private void NotifyPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public AppMealHistory()
        {
            Loading = false;
            Progress.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if(e.PropertyName == "Percentage")
                {
                    Percentage = ((ProgressReport)sender).Percentage;
                }
            };
        }
        public AppMealHistory(Meal meal, string Base64Image) : base(meal, Base64Image)
        {
            Loading = true;
            barVisible = false;
        }
        private async void LazyLoad()
        {
            lock (this)
            {
                if (Loading)
                {
                    return;
                }
                Loading = true;
            }
            if(!string.IsNullOrEmpty(Base64Image))
            {
                BarVisible = false;
                return;
            }
            AppMealHistory fullData = await new NetGate(Progress).GetHistoryDetail(this);
            lock (this)
            {
                Base64Image = fullData.Base64Image;
                Ingredients = fullData.Ingredients;
                Instructions = fullData.Instructions;
            }
            BarVisible = false;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
            App.Db.UpdateMeals(this);
        }
        [Ignore]
        public ImageSource Image
        {
            get
            {
                if(!Loading)
                {
                    LazyLoad();
                }
                return ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(Base64Image)));
            }
        }
        [Ignore]
        public float Percentage
        {
            get => percentage;
            set { percentage = value; NotifyPropertyChanged(); }
        }
        [Ignore]
        public bool BarVisible
        {
            get => barVisible;
            set { barVisible = value; NotifyPropertyChanged(); }
        }
        public string CombinedInstruction
        {
            get
            {
                return string.Join("\n", Instructions);
            }
            set
            {
                Instructions = new List<string>(value.Split('\n'));
            }
        }
        public string CombinedIngredients
        {
            get
            {
                return string.Join("\n", Ingredients);
            }
            set
            {
                Ingredients = new List<string>(value.Split('\n'));
            }
        }
    }
}
