using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAppGUI
{//class to store and work with recipe details
    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public Recipe(string name, List<Ingredient> ingredients, List<string> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }

        public double GetTotalCalories()
        {
            return Ingredients.Sum(i => i.Calories);
        }

        public override string ToString()
        {
            string ingredientsText = string.Join("\n", Ingredients.Select(i => i.ToString()));
            string stepsText = string.Join("\n", Steps.Select((step, index) => $"{index + 1}. {step}"));

            return $"Recipe: {Name}\n\nIngredients:\n{ingredientsText}\n\nSteps:\n{stepsText}\n\nTotal Calories: {GetTotalCalories()}";
        }
    }

}