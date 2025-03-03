using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Classes
{
    internal class Recipe
    {
        required public int Id { get; set; }
        required public string Title { get; set; }
        required public string Author { get; set; }
        required public string Description { get; set; }
        required public string Category { get; set; }
        required public bool Favorited { get; set; }
        public string? ImagePath { get; set; }
        required public List<Ingredient> Ingredients { get; set; }
        required public List<dbDirection> Directions { get; set; }


    }
}
