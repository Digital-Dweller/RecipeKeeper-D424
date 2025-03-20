using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Recipe_Keeper.Database;

namespace Recipe_Keeper.Classes
{
    public class Recipe : dbRecipe
    {
        required public List<dbIngredient> Ingredients { get; set; }
        required public List<dbDirection> Directions { get; set; }
    }
}
