using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Recipe_Keeper.Classes
{
    public class Ingredient
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public int RecipeId { get; set; }

    }
}
