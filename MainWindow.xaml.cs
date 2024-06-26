using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeAppGUI
{
    //all processes for each button are under each button
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Recipe> _recipes;
        private List<Ingredient> _ingredients;
        private List<string> _steps;

        private string _recipeName;
        private string _ingredientName;
        private string _ingredientQuantity;
        private string _ingredientUnit;
        private string _ingredientCalories;
        private string _ingredientFoodGroup;
        private string _stepDescription;

        public event PropertyChangedEventHandler PropertyChanged;
        //event handler created manually
        public string RecipeName
        {
            get { return _recipeName; }
            set { _recipeName = value; NotifyPropertyChanged(nameof(RecipeName)); }
        }

        public string IngredientName
        {
            get { return _ingredientName; }
            set { _ingredientName = value; NotifyPropertyChanged(nameof(IngredientName)); }
        }

        public string IngredientQuantity
        {
            get { return _ingredientQuantity; }
            set { _ingredientQuantity = value; NotifyPropertyChanged(nameof(IngredientQuantity)); }
        }

        public string IngredientUnit
        {
            get { return _ingredientUnit; }
            set { _ingredientUnit = value; NotifyPropertyChanged(nameof(IngredientUnit)); }
        }

        public string IngredientCalories
        {
            get { return _ingredientCalories; }
            set { _ingredientCalories = value; NotifyPropertyChanged(nameof(IngredientCalories)); }
        }

        public string IngredientFoodGroup
        {
            get { return _ingredientFoodGroup; }
            set { _ingredientFoodGroup = value; NotifyPropertyChanged(nameof(IngredientFoodGroup)); }
        }

        public string StepDescription
        {
            get { return _stepDescription; }
            set { _stepDescription = value; NotifyPropertyChanged(nameof(StepDescription)); }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _recipes = new List<Recipe>();
            _ingredients = new List<Ingredient>();
            _steps = new List<string>();

          
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate and convert input values
            if (string.IsNullOrEmpty(IngredientName) || string.IsNullOrEmpty(IngredientQuantity) || string.IsNullOrEmpty(IngredientUnit) || string.IsNullOrEmpty(IngredientCalories) || string.IsNullOrEmpty(IngredientFoodGroup))
            {
                MessageBox.Show("Please fill in all fields for the ingredient.");
                return;
            }

            if (!double.TryParse(IngredientQuantity, out double quantity))
            {
                MessageBox.Show("Invalid quantity format. Please enter a valid number.");
                return;
            }

            if (!double.TryParse(IngredientCalories, out double calories))
            {
                MessageBox.Show("Invalid calories format. Please enter a valid number.");
                return;
            }

            // Create a new Ingredient object
            Ingredient newIngredient = new Ingredient(IngredientName, quantity, IngredientUnit, calories, IngredientFoodGroup);

            // Add ingredient to _ingredients list
            _ingredients.Add(newIngredient);

            // Clear input fields
            IngredientName = string.Empty;
            IngredientQuantity = string.Empty;
            IngredientUnit = string.Empty;
            IngredientCalories = string.Empty;
            IngredientFoodGroup = string.Empty;
        }


        private void AddStepButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrEmpty(StepDescription))
            {
                MessageBox.Show("Please enter a step description.");
                return;
            }

            // Add step to list
            _steps.Add(StepDescription);

            // Clear input field
            StepDescription = string.Empty;
        }

        private void SaveRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate input (e.g., RecipeName, _ingredients, _steps)
            if (string.IsNullOrEmpty(RecipeName) || _ingredients.Count == 0 || _steps.Count == 0)
            {
                MessageBox.Show("Please fill in all fields for the recipe.");
                return;
            }

            // Create a new Recipe object
            Recipe newRecipe = new Recipe(RecipeName, new List<Ingredient>(_ingredients), new List<string>(_steps));

            // Add recipe to _recipes list
            _recipes.Add(newRecipe);

            // Clear input fields
            RecipeName = string.Empty;
            _ingredients.Clear();
            _steps.Clear();

            // Update UI
            MessageBox.Show("Recipe saved successfully!");
            LoadRecipesToListBox();
        }

        private void LoadRecipesToListBox()
        {
            RecipeListBox.Items.Clear();
            foreach (var recipe in _recipes)
            {
                RecipeListBox.Items.Add(recipe.Name);
            }
        }

        private void RecipeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Display recipe details when selection changes
            if (RecipeListBox.SelectedIndex != -1)
            {
                string selectedRecipeName = RecipeListBox.SelectedItem.ToString();
                Recipe selectedRecipe = _recipes.FirstOrDefault(r => r.Name == selectedRecipeName);

                if (selectedRecipe != null)
                {
                    DisplayRecipeDetails(selectedRecipe);
                }
            }
        }

        private void DisplayRecipeDetails(Recipe recipe)
        {
            // Clear existing text
            RecipeDetailsTextBlock.Text = string.Empty;

            // Display recipe details
            RecipeDetailsTextBlock.Inlines.Add(new Run($"Recipe: {recipe.Name}\n\n") { FontWeight = FontWeights.Bold, FontSize = 16 });

            RecipeDetailsTextBlock.Inlines.Add("Ingredients:\n");
            foreach (var ingredient in recipe.Ingredients)
            {
                RecipeDetailsTextBlock.Inlines.Add($"- {ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories)\n");
                RecipeDetailsTextBlock.Inlines.Add($"  Food Group: {ingredient.FoodGroup}\n");
            }

            RecipeDetailsTextBlock.Inlines.Add("\nSteps:\n");
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                RecipeDetailsTextBlock.Inlines.Add($"{i + 1}. {recipe.Steps[i]}\n");
            }
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            string ingredientFilter = IngredientFilterTextBox.Text.Trim().ToLower();
            string foodGroupFilter = (FoodGroupComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (!int.TryParse(MaxCaloriesTextBox.Text, out int maxCalories))
            {
                maxCalories = int.MaxValue; // If invalid input, don't filter by calories
            }

            var filteredRecipes = _recipes.Where(recipe =>
            {
                bool ingredientMatch = string.IsNullOrEmpty(ingredientFilter) ||
                    recipe.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientFilter));

                bool foodGroupMatch = foodGroupFilter == "All" || string.IsNullOrEmpty(foodGroupFilter) ||
                    recipe.Ingredients.Any(i => i.FoodGroup.Equals(foodGroupFilter, StringComparison.OrdinalIgnoreCase));

                bool caloriesMatch = recipe.Ingredients.Sum(i => i.Calories) <= maxCalories;

                return ingredientMatch && foodGroupMatch && caloriesMatch;
            }).ToList();

            UpdateRecipeListBox(filteredRecipes);
        }
        //method to update list box
        private void UpdateRecipeListBox(List<Recipe> recipes)
        {
            RecipeListBox.Items.Clear();
            foreach (var recipe in recipes)
            {
                RecipeListBox.Items.Add(recipe.Name);
            }

            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes match the current filters.", "Filter Results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
