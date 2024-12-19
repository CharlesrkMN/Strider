using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Security.Cryptography;

public class PlayModel : PageModel //set playmodel class
   {
    //creates instances of classes
       public List<SelectListItem> PlayerList { get; set; }
       public List<SelectListItem> EnemyList { get; set; }
       public Player SelectedPlayer { get; set; }
       public Enemy SelectedEnemy {get; set;}
       public pAbility SelectedAbility {get; set;}
       public pAbility SelectedAbility2 {get; set;}
       public mAbility SelectedmAbility {get; set;}
       public mAbility SelectedmAbility2 {get; set;}
       public pStat SelectedpStat {get; set;}
       public mStat SelectedmStat {get; set;}
       public int pDmg {get; set;}
       public int pHealth {get; set;}
       public int dmgDone {get; set;}
       public int mDmgDone {get; set;}
       public int mDmg {get; set;}
       public int mHealth {get; set;}
       

       public void Versus() //generates a random enemy opponent 
       {
            Random rnd = new Random();
            int x = rnd.Next(1,8); //set variable x = random number from 1-8
            SelectedEnemy = GetEnemyByMon_ID(x); //calls function using x as id value parameter
            SelectedmStat = GetmStatByadv_ID(x); //calls function to set monster stats based off of random id#
            SelectedmAbility = GetmAbilityByadv_ID(x); //calls function to set monsters first ability based off of the random id#
            SelectedmAbility2 = GetmAbility2Byadv_ID(x); //calls function to set monsters second ability based off of the random id#
            mHealth = SelectedmStat.Health; //sets mHealth value to equal the monsters health stat from the Stats table
       }

        public void LoadEnemyList() //loads enemies from the monsters table on strider database
       {
           EnemyList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT Mon_ID, Mon_Type FROM Monsters";
               using (var reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       EnemyList.Add(new SelectListItem
                       {
                           Value = reader.GetInt32(0).ToString(),
                           Text = reader.GetString(1)
                       });
                   }
               }
           }
       }

    public Enemy GetEnemyByMon_ID(int id) //gets enemy info based on int value id
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
                       return new Enemy
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
   

       public void OnGet() //used to load class options
       {
           LoadPlayerList();
       }

       public void OnPost(string selectedPlayer) //run when player chooses a class
       {
           LoadPlayerList();
           if (!string.IsNullOrEmpty(selectedPlayer))
           {
               SelectedPlayer = GetPlayerByadv_ID(int.Parse(selectedPlayer)); //set Selected player instance to chosen class
               SelectedAbility = GetAbilityByadv_ID(int.Parse(selectedPlayer)); //set ability to chosen class
               SelectedAbility2 = GetAbility2Byadv_ID(int.Parse(selectedPlayer)); //set ability2 to chosen class
               SelectedpStat = GetpStatByadv_ID(int.Parse(selectedPlayer)); //set player stats for chosen class
               pHealth = SelectedpStat.Health; //sets pHealth value to equal adventurers health stat from the Stats table
           }
           Versus(); //run versus function to populate opponent
       }

       public void LoadPlayerList() //loads class options for player from Adventurers table in the strider database
       {
           PlayerList = new List<SelectListItem>();
           using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT adv_ID, adv_Type FROM Adventurers";
               using (var reader = command.ExecuteReader())
               {
                   while (reader.Read())
                   {
                       PlayerList.Add(new SelectListItem //creates a list of class options in order by adv_id and named by adv_type
                       {
                           Value = reader.GetInt32(0).ToString(),
                           Text = reader.GetString(1)
                       });
                   }
               }
           }
       }

       public Player GetPlayerByadv_ID(int id) //gets the class info based on unique adventurer id found from player list
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
       public pAbility GetAbilityByadv_ID(int id) //sets players first ability
       {
            using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               //inner join statement using adventurer ids from both tables
               command.CommandText = "SELECT a.* FROM ABILITIES AS a INNER JOIN Adventurers as t ON a.adv_ID = t.adv_ID WHERE t.adv_ID = @adv_ID";
               command.Parameters.AddWithValue("@adv_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new pAbility
                       {
                           Ability_ID = reader.GetInt32(0),
                           Action = reader.GetString(1),
                           Damage = reader.GetInt32(2),
                           adv_ID = reader.GetInt32(3),
                           Mon_ID = reader.GetInt32(4)
                       };
                   }
               }
           }
           return null;
       }

        public pAbility GetAbility2Byadv_ID(int id) //sets players second ability
       {
            using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               //uses inner join to select records based off equal adv_IDs, then orders by descending to get the second ability.
               command.CommandText = "SELECT a.* FROM ABILITIES AS a INNER JOIN Adventurers as t ON a.adv_ID = t.adv_ID WHERE t.adv_ID = @adv_ID ORDER BY a.Ability_ID DESC";
               command.Parameters.AddWithValue("@adv_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new pAbility
                       {
                           Ability_ID = reader.GetInt32(0),
                           Action = reader.GetString(1),
                           Damage = reader.GetInt32(2),
                           adv_ID = reader.GetInt32(3),
                           Mon_ID = reader.GetInt32(4)
                       };
                   }
               }
           }
           return null;
       }

    public mAbility GetmAbilityByadv_ID(int id) //sets monster ability
       {
            using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               //inner join to get matching mon_ids from both tables
               command.CommandText = "SELECT a.* FROM ABILITIES AS a INNER JOIN Monsters as m ON a.Mon_ID = m.Mon_ID WHERE m.Mon_ID = @adv_ID";
               command.Parameters.AddWithValue("@adv_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new mAbility
                       {
                           Ability_ID = reader.GetInt32(0),
                           Action = reader.GetString(1),
                           Damage = reader.GetInt32(2),
                           adv_ID = reader.GetInt32(3),
                           Mon_ID = reader.GetInt32(4)
                       };
                   }
               }
           }
           return null;
       }

    public mAbility GetmAbility2Byadv_ID(int id) //sets monster second ability
       {
            using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               //order by desc to get second record with same mon_id
               command.CommandText = "SELECT a.* FROM ABILITIES AS a INNER JOIN Monsters as m ON a.Mon_ID = m.Mon_ID WHERE m.Mon_ID = @adv_ID ORDER BY a.Ability_ID DESC";
               command.Parameters.AddWithValue("@adv_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new mAbility
                       {
                           Ability_ID = reader.GetInt32(0),
                           Action = reader.GetString(1),
                           Damage = reader.GetInt32(2),
                           adv_ID = reader.GetInt32(3),
                           Mon_ID = reader.GetInt32(4)
                       };
                   }
               }
           }
           return null;
       }



    public pStat GetpStatByadv_ID(int id) //sets players stats
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
                       return new pStat
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

    public mStat GetmStatByadv_ID(int id) //sets monsters stats
       {
            using (var connection = new SqliteConnection("Data Source=Strider.db"))
           {
               connection.Open();
               var command = connection.CreateCommand();
               command.CommandText = "SELECT s.* FROM Stats AS s INNER JOIN Monsters as m ON s.Mon_ID = m.Mon_ID WHERE m.Mon_ID = @Mon_ID";
               command.Parameters.AddWithValue("@Mon_ID", id);
               using (var reader = command.ExecuteReader())
               {
                   if (reader.Read())
                   {
                       return new mStat
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
    public async Task<IActionResult> OnPostButtonClickAsync()
    {
        // runs when an ability to perform is clicked on
        pDmg = SelectedAbility.Damage;
        Combat(); //calls the combat method
        return RedirectToPage();
    }

    public void Combat() //calculates player damage to the monster
    {
        Random val = new Random();
        int dmgDone = val.Next(1, pDmg); //sets dmgdone to a random int value between 1 and the maximum amount of dmg the ability can do determined in the abilities table
        mHealth = mHealth - dmgDone; //calculate monsters health after subtracting dmgdone
        if (mHealth > 0) //if the monsters health value is greater than 0, call on the monster turn method
        {
            MonTurn();
        }
    }

    public void MonTurn()
    {
        Random val = new Random();
        int randAbility = val.Next(1,2); //randomly choose the ability the monster uses. only 2 choices
        if (randAbility == 1) //if random # equals 1 then use first monster ability
        {
            int mDmg = SelectedmAbility.Damage;
        }
        if (randAbility == 2) //if random # equals 2 then use second monster ability
        {

            int mDmg = SelectedmAbility2.Damage;
        }
        int mDmgDone = val.Next(1,mDmg); //sets monsters dmgdone to player equal to a random int between 1 and the maximum damage the selected ability can perform
        pHealth = pHealth - mDmgDone; //subtract monster dmgdone from player health
    }


   }

   public class Player //global instance of the player class, with values determined by the adventurers table
   {
       public int adv_ID { get; set; }
       public string adv_Type { get; set; }
       public string adv_Race { get; set; }
       public string ImageFileName { get; set; }
   }

   public class Enemy //global instance of the Enemy class, with values determined by the monsters table
   {
        public int Mon_ID { get; set; }
       public string Mon_Type { get; set; }
       public string Mon_Description { get; set; }
       public string ImageFileName { get; set; }
   }

   public class pAbility //global instance of the player Ability class, with values determined by the abilities table
   {
        public int Ability_ID {get; set;}
        public string Action {get; set;}
        public int Damage {get; set;}
        public int adv_ID {get;set;}
        public int Mon_ID {get; set;}
   }

      public class mAbility //global instance of the monster ability class, with values determined by the abilities table
   {
        public int Ability_ID {get; set;}
        public string Action {get; set;}
        public int Damage {get; set;}
        public int adv_ID {get;set;}
        public int Mon_ID {get; set;}
   }

   public class pStat //global instance of the player stat class, with values determined by the stats table
   {
        public int Stat_ID {get; set;}
        public int Health {get; set;}
        public int Speed {get; set;}
        public int Exp {get; set;}
        public int adv_ID {get; set;}
        public int Mon_ID {get; set;}
   }

   public class mStat //global instance of the monsters stat class, with values determined by the stats table
   {
        public int Stat_ID {get; set;}
        public int Health {get; set;}
        public int Speed {get; set;}
        public int Exp {get; set;}
        public int adv_ID {get; set;}
        public int Mon_ID {get; set;}
   }

