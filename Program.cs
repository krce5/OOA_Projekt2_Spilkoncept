using System.Diagnostics;
using System.Timers;
using System.Xml.Linq;

Console.WriteLine("Welcome to the Bondemand Larsens Vegetable field!");
Console.WriteLine("You have been hired to protect the veggies. ");
Console.WriteLine("You are supposed to harvest the veggetables and");
Console.WriteLine("..scare the enemies away!");
Console.WriteLine("Use the arrow keys to move around and walk in the vegetable patch to harvest them.");
Console.WriteLine("Press A to scare the rabbit and D to scare the pig.");
Console.WriteLine("Write your name below and press ENTER to start the game.");

string playerName = Console.ReadLine(); // Venter på, at spilleren skriver sit navn og trykker enter

Game game = new Game();
game.Start(playerName); //Starter spiller og sender spillerens navn med

class Game //Klassen indeholder spillets funktioner
{
    private Random random = new Random(); //Laver random som bruges til at lave tilfældige posistioner
    public int Total; //Gemmer spillerens samlede point
    private int[] x = new int[] { 10 }; //Gemmer spillerens x pos
    private int[] y = new int[] { 10 }; //Gemmer spillerens y pos
    private List<Vegetable> vegetables = new List<Vegetable>(); //Liste som indeholder alle grøntsager
    private List<Enemy> enemies = new List<Enemy>(); //Liste som indeholder alle fjender
    

    public void Start(string name) //Starter spillet ved at oprette og flytte spilleren
    {
        MakePerson(); //Tegner spilleren på skærmen
        CreateVeg(); //Opretter grøntsagerne
        CreateEnemies(); //Opretter fjenderne
        GameLoop(); //Starter spillets hovedløkke
    }

    public void GameLoop() //Her kører vores metoder konstant
    {
        while (true) 
        {
            MovePerson(); //Gør det muligt at flytte spilleren
            PickVeg(); //Tjekker om spilleren samler en grøntsag op 
            MoveVeg(); //Flytter grøntsagerne
            CheckEnemies(); //Tjekker om fjenden skal opstå eller forsvinde
        }
    }

    public void MakePerson() //Opretter og tegner spilleren på skærmen
    {
        Console.Clear(); //Ryder konsolskærmen
        Console.SetCursorPosition(x[0], y[0]); //Flytter cursor til spillerens startpos
        Console.Write("O"); //Tegner spilleren som O
        DrawPoints(); //Viser spillerens point på skærmen
    }

    public void DrawPoints() //Viser spillerens point på skærmen
    {
        Console.SetCursorPosition(0, 22);
        Console.WriteLine("Points: " + Total);
    }

    public void CreateVeg() //Grøntsager oprettes og dets egenskaber
    {
        Vegetable carrot = new Vegetable();
        carrot.Name = "Carrot";
        carrot.Symbol = "C"; //Carrot vises som C på skærmen
        carrot.Time = 5000; //Ms for gullerod til at flytte sig
        carrot.x = 8;
        carrot.y = 8;
        carrot.Point = 1;
        vegetables.Add(carrot); //Tilføjes til listen

        Vegetable salad = new Vegetable();
        salad.Name = "Salad";
        salad.Symbol = "S";
        salad.Time = 3000;
        salad.x = 20;
        salad.y = 5;
        salad.Point = 2;
        vegetables.Add(salad);

        foreach (Vegetable veg in vegetables) //Gennemgår alle grøntsager i listen
        {
            veg.Watch.Start(); //Starter stopur for grøntsag
        }

        DrawVeg();
    }

    public void DrawVeg() //Tegner grøntsagerne på skærmen
    {
        //Foreach-Løkke går igennem den liste man har lavet, og tjekker hvor mange ting der er i den. Løkken stopper af sig selv, når den har været hele listen igennem.
        foreach (Vegetable veg in vegetables)
        {
            Console.SetCursorPosition(veg.x, veg.y);
            ConsoleColor previousColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(veg.Symbol);
            Console.ForegroundColor = previousColor;
        }
    }

    public void PickVeg() //Tjekker om spiller står på en grøntsag
    {
        foreach (Vegetable veg in vegetables)
        {
            if (x[0] == veg.x && y[0] == veg.y) //Tjekker om spillerens pos er samme som grøntsagens pos
            {
                Total = Total + veg.Point;
                veg.x = random.Next(1, 31);
                veg.y = random.Next(1, 21);

                while (veg.x == x[0] && veg.y == y[0])
                {
                    veg.x = random.Next(1, 31);
                    veg.y = random.Next(1, 21);
                }
                veg.Watch.Restart();
                MakePerson();
                DrawVeg();
                DrawEnemies();
            }
        }
    }

    public void MoveVeg() //Tjekker om grøntsagerne skal flyttes
    {
        foreach (Vegetable veg in vegetables)
        {
            if (veg.Watch.ElapsedMilliseconds >= veg.Time)
            {
                veg.x = random.Next(1, 31);
                veg.y = random.Next(1, 21);

                while (veg.x == x[0] && veg.y == y[0])
                {
                    veg.x = random.Next(1, 31);
                    veg.y = random.Next(1, 21);
                }
                veg.Watch.Restart();
                MakePerson();
                DrawVeg();
                DrawEnemies();
            }
        }
    }

    public void CreateEnemies() //Opretter fjenderne og deres egenskaber
    {
        Enemy pig = new Enemy();
        pig.Name = "Cow";
        pig.Symbol = "P";
        pig.Time = 3000;        // Er på marken i 3 sek
        pig.WaitTime = 7000;    // Kommer hver 7. sek
        pig.Key = ConsoleKey.D;
        pig.Visible = false;
        enemies.Add(pig);

        Enemy rabbit = new Enemy();
        rabbit.Name = "Rabbit";
        rabbit.Symbol = "R";
        rabbit.Time = 4000;
        rabbit.WaitTime = 8000;
        rabbit.Key = ConsoleKey.A;
        rabbit.Visible = false;
        enemies.Add(rabbit);

        //Foreach-Løkke går igennem den liste man har lavet, og tjekker hvor mange ting der er i den. Løkken stopper af sig selv, når den har været hele listen igennem.
        foreach (Enemy enemy in enemies) 
        {
            enemy.Watch.Start();
        }
    }

    public void DrawEnemies() //Tegner fjenderne på skærmen
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Visible) //Tjekker om fjenden er synlig
            {
                Console.SetCursorPosition(enemy.x, enemy.y);
                ConsoleColor previousColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(enemy.Symbol);
                Console.ForegroundColor = previousColor;
            }
        }
    }

    public void CheckEnemies() //Tjekker om fjender skal dukke op eller forsvinde
    {
        foreach (Enemy enemy in enemies)
        {
            // Fjenden er skjult, og ventetiden er gået: den kommer ind
            if (enemy.Visible == false && enemy.Watch.ElapsedMilliseconds >= enemy.WaitTime)
            {
                enemy.x = random.Next(1, 31);
                enemy.y = random.Next(1, 21);

                while (enemy.x == x[0] && enemy.y == y[0])
                {
                    enemy.x = random.Next(1, 31);
                    enemy.y = random.Next(1, 21);
                }
                enemy.Visible = true;
                enemy.Watch.Restart();
                MakePerson();
                DrawVeg();
                DrawEnemies();
            }
            // Fjenden er synlig, og man nåede ikke at skræmme den: minus et point
            else if (enemy.Visible == true && enemy.Watch.ElapsedMilliseconds >= enemy.Time)
            {
                enemy.Visible = false;
                Total = Total - 1;
                enemy.Watch.Restart();
                MakePerson();
                DrawVeg();
                DrawEnemies();
            }
        }
    }

    public void ScareEnemies(ConsoleKey key) //Om spilleren trykker på den rigtige tast vil fjenden skræmmes væk
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Visible == true && key == enemy.Key)
            {
                enemy.Visible = false;
                enemy.Watch.Restart();   // Ventetiden starter forfra
            }
        }
    }

    public void MovePerson() //Spilleren kan bævege sig med arrow keys
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.UpArrow) y[0]--;
            if (key.Key == ConsoleKey.DownArrow) y[0]++;
            if (key.Key == ConsoleKey.LeftArrow) x[0]--;
            if (key.Key == ConsoleKey.RightArrow) x[0]++;

            ScareEnemies(key.Key);   // Tjek om der blev trykket A eller D

            if (x[0] < 1)
            {
                x[0] = 1;
            }
            if (x[0] > 30)
            {
                x[0] = 30;
            }
            if (y[0] < 1)
            {
                y[0] = 1;
            }
            if (y[0] > 20)
            {
                y[0] = 20;
            }
            MakePerson();
            DrawVeg();
            DrawEnemies();
        }
    } 
}

class Person
{
    public string Name;
    public int x;
    public int y;
}

class Vegetable
{
    public int x;
    public int y;
    public string Name;
    public string Symbol;
    public int Point;
    public int Time;
    public Stopwatch Watch = new Stopwatch();
}

class Enemy
{
    public int x;
    public int y;
    public string Name;
    public string Symbol;
    public int Time;
    public int WaitTime;
    public ConsoleKey Key;
    public bool Visible;
    public Stopwatch Watch = new Stopwatch();
}