# Strider

Welcome to my simple rpg combat app!

In order to run the program
    change directories to the strider directory using:
        cd Strider
    in the terminal.
    Then, run the program by entering:
        dotnet run

You will be welcomed by the Strider page. Navigate at the top bar and choose between the pages: Play, Adventurers, and Monsters.

On the Play page you will see an image of the "Decrepit Dungeon." Choose an adventurer class, and engage in combat against a random monster foe!
Below the combat images you will find 2 buttons corresponding to the class chosen. Use these to attack the monster!
Below the ability buttons you will see a text box log. This log is where you will be informed of valuable combat info such as remaining health or damage done to the monster.

If you navigate to the Adventurer page you will be able to select each available adventurer in order to learn more about them.

Lastly, if you navigate to the Monsters page you will be able to select each available monster in order to learn more about them.

Goodluck!

This project was done in razor pages. Most all code worked on is in:
_Layout.cshtml
_Layout.cshtml.css
Adventurers.cshtml
Adventurers.cshtml.cs
Monsters.cshtml
Monsters.cshtml.cs
Play.cshtml
Play.cshtml.cs
wwwroot/aImages
wwwroot/mImages
wwwroot/dImages
wwwroot/site.css

The projects database was implemented with sqlite3.
 using: sqlite3 Strider.db will enter you into the sqlite3 terminal for the strider database
 
 The tables utilized are named Adventurers, Monsters, Stats, and ABILITIES
