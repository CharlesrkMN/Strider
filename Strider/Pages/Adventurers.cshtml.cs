   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.RazorPages;
   using Microsoft.AspNetCore.Mvc.Rendering;
   using Microsoft.Data.Sqlite;
   using System.Collections.Generic;

   public class AdventurersModel : PageModel
   {
       public List<SelectListItem> AdventurerList { get; set; }
       public Adventurer SelectedAdventurer { get; set; }

       public void OnGet()
       {
           LoadAdventurerList();
       }

       public void OnPost(string selectedAdventurer)
       {
           LoadAdventurerList();
           if (!string.IsNullOrEmpty(selectedAdventurer))
           {
               SelectedAdventurer = GetAdventurerByadv_ID(int.Parse(selectedAdventurer));
           }
       }

       public void LoadAdventurerList()
       {
           AdventurerList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT adv_ID, adv_Type FROM Adventurers";
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

       public Adventurer GetAdventurerByadv_ID(int id)
       {
           using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT * FROM Adventurers WHERE adv_ID = @adv_ID";
               command.Parameters.AddWithValue("@adv_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new Adventurer
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

   public class Adventurer
   {
       public int adv_ID { get; set; }
       public string adv_Type { get; set; }
       public string adv_Race { get; set; }
       public string ImageFileName { get; set; }
   }

       public class Stat
   {
       public int Stat_ID { get; set; }
       public int Health { get; set; }
       public int Speed { get; set; }
       public int Exp { get; set; }
       public int adv_ID {get; set; }
       public int Mon_ID {get; set; }
    }