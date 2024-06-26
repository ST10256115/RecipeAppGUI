using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAppGUI
{//this class saves and works with ingredients
    public class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public double Calories { get; set; }
        public string FoodGroup { get; set; }

        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }
        //to string to display details contained in class
        public override string ToString()
        {
            return $"{Quantity} {Unit} of {Name} ({Calories} calories, {FoodGroup})";
        }
    }
}
