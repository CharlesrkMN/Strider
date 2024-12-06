   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.RazorPages;
   using Microsoft.AspNetCore.Mvc.Rendering;
   using Microsoft.Data.Sqlite;
   using System.Collections.Generic;

   public class MonstersModel : PageModel
   {
       public List<SelectListItem> MonsterList { get; set; }
       public Monster SelectedMonster { get; set; }

       public void OnGet()
       {
           LoadMonsterList();
       }

       public void OnPost(string selectedMonster)
       {
           LoadMonsterList();
           if (!string.IsNullOrEmpty(selectedMonster))
           {
               SelectedMonster = GetMonsterByMon_ID(int.Parse(selectedMonster));
           }
       }

       private void LoadMonsterList()
       {
           MonsterList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT Mon_ID, Mon_Type FROM Monsters";
               using (var reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       MonsterList.Add(new SelectListItem
                       {
                           Value = reader.GetInt32(0).ToString(),
                           Text = reader.GetString(1)
                       });
                   }
               }
           }
       }

       private Monster GetMonsterByMon_ID(int id)
       {
           using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT * FROM Monsters WHERE Mon_ID = @Mon_ID";
               command.Parameters.AddWithValue("@Mon_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new Monster
                       {
                           Mon_ID = reader.GetInt32(0),
                           Mon_Type = reader.GetString(1),
                           Mon_Description = reader.GetString(2),
                           ImageFileName = reader.GetString(3)
                       };
                   }
               }
           }
           return null;
       }
   }

   public class Monster
   {
       public int Mon_ID { get; set; }
       public string Mon_Type { get; set; }
       public string Mon_Description { get; set; }
       public string ImageFileName { get; set; }
   }