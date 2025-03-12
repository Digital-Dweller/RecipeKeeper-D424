using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Keeper.Classes
{
    public class Direction
    {
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
    }
}
