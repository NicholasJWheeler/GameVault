using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameVault;

[SQLite.Table("backloggame")]
public class BacklogGame {
    // PrimaryKey is typically numeric 
    [PrimaryKey, AutoIncrement, SQLite.Column("_id")]
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageSource { get; set; }
    public string Release { get; set; }
    public int Metacritic { get; set; }
    public string ESRBRating { get; set; }
    public string Platforms { get; set; }
    public string Genres { get; set; }
    public int Playtime { get; set; }
    public override string ToString()
    {
        return string.Format("[{0}], Name: {1}", Id, Name);
    }
}

