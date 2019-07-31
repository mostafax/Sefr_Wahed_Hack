using System.Collections.Generic;

namespace SharedModels
{
    public class ImageRequest
    {
        public string Base64Image { get; set; }
    }
    public class Meal
    {
        public List<string> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
        public string Title { get; set; }
    }
    public class MealHistory
    {
        public virtual int Id { get; set; }
        public string Base64Image { get; set; }
        public virtual List<string> Ingredients { get; set; }
        public virtual List<string> Instructions { get; set; }
        public string Title { get; set; }
        public MealHistory() { }
        public MealHistory(Meal meal, string Base64Image)
        {
            Title = meal.Title;
            Ingredients = new List<string>(meal.Ingredients);
            Instructions = new List<string>(meal.Instructions);
            this.Base64Image = Base64Image;
        }
    }
}