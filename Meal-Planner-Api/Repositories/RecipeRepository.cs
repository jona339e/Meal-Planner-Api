using Meal_Planner_Api.Data;
using Meal_Planner_Api.Dto;
using Meal_Planner_Api.Interfaces;
using Meal_Planner_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Meal_Planner_Api.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly DataContext _context;

        public RecipeRepository(DataContext context)
        {
            _context = context;
        }

        public Recipe GetRecipe(int id)
        {
            return _context.Recipes
                            .Include(r => r.Ingredients)
                                .ThenInclude(i => i.Amounts)
                            .Include(r => r.Instructions)
                            .SingleOrDefault(r => r.Id == id);
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return _context.Recipes
                .Include(r => r.Ingredients)
                    .ThenInclude(i => i.Amounts) // Include Amounts within Ingredients
                .Include(r => r.Instructions)
                .ToList();
        }

        public Recipe AddRecipe(Recipe recipe) { 
            _context.Recipes.Add(recipe);
            _context.SaveChanges();
            return recipe;
        }

        public void UpdateRecipe(Recipe recipe)
        {
            _context.Entry(recipe).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteRecipe(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                _context.SaveChanges();
            }
        }


    }
}
