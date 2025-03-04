using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Recipe_Keeper.Database
{
    internal class dbIngredient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public double Quantity { get; set; }
        public int UnitId { get; set; }
        [Indexed]
        public int RecipeId { get; set; }
    }
}
