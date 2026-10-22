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

string playerName = Console.ReadLine();

Game game = new Game();
game.Start(playerName);

class Game
{
    private Random random = new Random();
    public int Total;
    private int[] x = new int[] { 10 };
    private int[] y = new int[] { 10 };
    private List<Vegetable> vegetables = new List<Vegetable>();
    private List<Enemy> enemies = new List<Enemy>();
    

    public void Start(string name)
    {
        MakePerson();
        CreateVeg();
        CreateEnemies();
        GameLoop();
    }

    public void GameLoop() //Her kører vores metoder konstant
    {
        while (true) 
        {
            MovePerson();
            PickVeg();
            MoveVeg();
            CheckEnemies();
        }
    }

    public void MakePerson()
    {
        Console.Clear();
        Console.SetCursorPosition(x[0], y[0]);
        Console.Write("O");
        DrawPoints();
    }

    public void DrawPoints()
    {
        Console.SetCursorPosition(0, 22);
        Console.WriteLine("Points: " + Total);
    }

    public void CreateVeg()
    {
        Vegetable carrot = new Vegetable();
        carrot.Name = "Carrot";
        carrot.Symbol = "C";
        carrot.Time = 5000;
        carrot.x = 8;
        carrot.y = 8;
        carrot.Point = 1;
        vegetables.Add(carrot);

        Vegetable salad = new Vegetable();
        salad.Name = "Salad";
        salad.Symbol = "S";
        salad.Time = 3000;
        salad.x = 20;
        salad.y = 5;
        salad.Point = 2;
        vegetables.Add(salad);

        foreach (Vegetable veg in vegetables)
        {
            veg.Watch.Start();
        }

        DrawVeg();
    }

    public void DrawVeg()
    {
        //Foreach-Løkke går igennem den liste man har lavet, og tjekker hvor mange ting der er i den. Løkken stopper af sig selv, når den har været hele listen igennem.
        foreach (Vegetable veg in vegetables)
        {
            Console.SetCursorPosition(veg.x, veg.y);
            Console.Write(veg.Symbol);
        }
    }

    public void PickVeg()
    {
        foreach (Vegetable veg in vegetables)
        {
            if (x[0] == veg.x && y[0] == veg.y)
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

    public void MoveVeg()
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

    public void CreateEnemies()
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

    public void DrawEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy.Visible)
            {
                Console.SetCursorPosition(enemy.x, enemy.y);
                Console.Write(enemy.Symbol);
            }
        }
    }

    public void CheckEnemies()
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

    public void ScareEnemies(ConsoleKey key)
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

    public void MovePerson()
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