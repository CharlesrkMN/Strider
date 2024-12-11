   using Microsoft.AspNetCore.Mvc;
   using Microsoft.AspNetCore.Mvc.RazorPages;
   using Microsoft.AspNetCore.Mvc.Rendering;
   using Microsoft.Data.Sqlite;
   using System.Collections.Generic;

    //creates monstersmodel class, containing functions for the monsters page
   public class MonstersModel : PageModel
   {
       public List<SelectListItem> MonsterList { get; set; } //declare the monsterlist to select from
       public Monster SelectedMonster { get; set; } // gets  and sets selected monster

       public void OnGet() //calls to load monster options when drop-down is clicked
       {
           LoadMonsterList();
       }

       public void OnPost(string selectedMonster) //sets Selected monster value based on monster clicked from monster list
       {
           LoadMonsterList();
           if (!string.IsNullOrEmpty(selectedMonster))
           {
               SelectedMonster = GetMonsterByMon_ID(int.Parse(selectedMonster));
           }
       }

       public void LoadMonsterList() //creates a list of monsters by monster id and type from the monsters table in the strider database
       {
           MonsterList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=Strider.db")) //uses sqlite to connect to strider database
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT Mon_ID, Mon_Type FROM Monsters";//select statement
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

       public Monster GetMonsterByMon_ID(int id)//gets selected monster info based off of unique monster id
       {
           using (var connection = new SqliteConnection("Data Source=Strider.db"))//connects to the strider database using sqlite
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT * FROM Monsters WHERE Mon_ID = @Mon_ID";//sqlite select statement
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

   public class Monster //declares monster class
   {
        //declares monster properties from the strider database monsters table
       public int Mon_ID { get; set; }
       public string Mon_Type { get; set; }
       public string Mon_Description { get; set; }
       public string ImageFileName { get; set; }
   }