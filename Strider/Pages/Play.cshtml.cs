using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
public class PlayModel : PageModel {
    public Player Pchoice {get; set;}
    //public Enemy
    protected void mWarrior()
    {
        //hero.src = "/aImages/@Model.Pchoice.ImageFileName";
    }
    public async Task OnPostmWarrior()
    {
            Pchoice = GetPlayerByadv_ID(1);
    }

    public async Task OnPostfWarrior()
    {
            Pchoice = GetPlayerByadv_ID(2);
    }

    public async Task OnPostmArcher()
    {
            Pchoice = GetPlayerByadv_ID(3);
    }

    public async Task OnPostfArcher()
    {
            Pchoice = GetPlayerByadv_ID(4);
    }

    public async Task OnPostmMage()
    {
            Pchoice = GetPlayerByadv_ID(5);
    }

    public async Task OnPostfMage()
    {
            Pchoice = GetPlayerByadv_ID(6);
    }
    public Player GetPlayerByadv_ID(int id)
    {
        using (var connection = new SqliteConnection("Data Source=Strider.db"))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Adventurers WHERE adv_ID = id";
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                   {
                       return new Player
                       {
                           adv_ID = reader.GetInt32(0),
                           adv_Type = reader.GetString(1),
                           adv_Race = reader.GetString(2),
                           ImageFileName = reader.GetString(3)
                       };
                   }
               }
           }
           return null;
       }
    }

   public class Player
   {
       public int adv_ID { get; set; }
       public string adv_Type { get; set; }
       public string adv_Race { get; set; }
       public string ImageFileName { get; set; }
   }