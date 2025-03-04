using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Recipe_Keeper.Database
{
    internal class dbDirection
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int OrderPosition { get; set; }
        [Indexed]
        public int RecipeId { get; set; }
    }
}
