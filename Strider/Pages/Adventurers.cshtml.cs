   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.RazorPages;
   using Microsoft.AspNetCore.Mvc.Rendering;
   using Microsoft.Data.Sqlite;
   using System.Collections.Generic;

    //creates adventurersmodel class, containing functions for the adventurers page
   public class AdventurersModel : PageModel
   {
       public List<SelectListItem> AdventurerList { get; set; } //declare the adventurerlist to select from
       public Adventurer SelectedAdventurer { get; set; } // gets  and sets selected adventurer
       public Stat SelectedStats {get; set;}

       public void OnGet() //calls to load adventurer options when drop-down is clicked
       {
           LoadAdventurerList();
       }

       public void OnPost(string selectedAdventurer) //sets Selected Adventurer value based on adventurer clicked from adventurer list
       {
           LoadAdventurerList();
           if (!string.IsNullOrEmpty(selectedAdventurer))
           {
               SelectedAdventurer = GetAdventurerByadv_ID(int.Parse(selectedAdventurer));
               SelectedStats = GetStatByadv_ID(int.Parse(selectedAdventurer));
           }
       }

       public void LoadAdventurerList() //creates a list of adventurers by adventurer id and type from the Adventurers table in the strider database
       {
           AdventurerList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=Strider.db")) //uses sqlite to connect to the strider database
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT adv_ID, adv_Type FROM Adventurers"; //select statement
               using (var reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       AdventurerList.Add(new SelectListItem
                       {
                           Value = reader.GetInt32(0).ToString(),
                           Text = reader.GetString(1)
                       });
                   }
               }
           }
       }

       public Adventurer GetAdventurerByadv_ID(int id) //gets selected adventurer info based off of unique adventurer id
       {
           using (var connection = new SqliteConnection("Data Source=Strider.db")) //uses sqlite to connect to the strider database
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT * FROM Adventurers WHERE adv_ID = @adv_ID"; //select statement
               command.Parameters.AddWithValue("@adv_ID", id); //sets id as adventurer id
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new Adventurer //returns selected adventurer vlaues
                       {
                           adv_ID = reader.GetInt32(0),
                           adv_Type = reader.GetString(1),
                           adv_Race = reader.GetString(2),
                           ImageFileName = reader.GetString(3),
                           Desc = reader.GetString(4)
                       };
                   }
               }
           }
           return null;
       }

    public Stat GetStatByadv_ID(int id) //sets players stats
       {
            using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT s.* FROM Stats AS s INNER JOIN Adventurers as t ON s.adv_ID = t.adv_ID WHERE t.adv_ID = @adv_ID";
               command.Parameters.AddWithValue("@adv_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new Stat
                       {
                           Stat_ID = reader.GetInt32(0),
                           Health = reader.GetInt32(1),
                           Speed = reader.GetInt32(2),
                           Exp = reader.GetInt32(3),
                           adv_ID = reader.GetInt32(4),
                           Mon_ID = reader.GetInt32(5)
                       };
                   }
               }
           }
           return null;
       }
   }

   public class Adventurer //declares the adventurer class
   {
        //declaring variables to be populated by the adventurers table
       public int adv_ID { get; set; } 
       public string adv_Type { get; set; }
       public string adv_Race { get; set; }
       public string ImageFileName { get; set; }
       public string Desc {get; set;}
   }

       public class Stat //declares the stat class
   {
        //declaring variables to be populated by the stats table
       public int Stat_ID { get; set; }
       public int Health { get; set; }
       public int Speed { get; set; }
       public int Exp { get; set; }
       public int adv_ID {get; set; }
       public int Mon_ID {get; set; }
    }