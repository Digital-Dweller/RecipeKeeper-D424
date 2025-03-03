using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Keeper.Classes
{
    internal class Ingredient
    {
        required public int Id { get; set; }
        required public string Title { get; set; }
        required public double Quantity { get; set; }
        required public int UnitId { get; set; }
        required public int RecipeId { get; set; }
    }
}
