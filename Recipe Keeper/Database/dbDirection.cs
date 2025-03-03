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
        required public int Id { get; set; }
        required public string Title { get; set; }
        required public string Description { get; set; }
        required public int OrderPosition { get; set; }
        [Indexed]
        required public int RecipeId { get; set; }
    }
}
