using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Recipe_Keeper.Database
{
    public class dbIngredient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        [Indexed]
        public int RecipeId { get; set; }
    }
}
