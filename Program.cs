//vores dejlige projekt
//Hej
//kaj kage
//Se kaj kagen
//Cool

using System.Diagnostics;
using System.Timers;
using System.Xml.Linq;

Console.WriteLine("Welcome to the Bondemand Larsens Vegetable field!");
Console.WriteLine("You have been hired to protect the veggies. ");
Console.WriteLine("You are supposed to harvest the veggetables and");
Console.WriteLine("..scare the rabbits away!");
Console.WriteLine("Use the arrow keys to move around and walk in the vegetable patch to harvest them.");
Console.WriteLine("Press R to scare the rabbit.");
Console.WriteLine("Write your name below and press ENTER to start the game.");

string playerName = Console.ReadLine();

Game game = new Game();
game.Start(playerName);

class Game
{
    private Random random = new Random();
    private Stopwatch stopwatch = new Stopwatch();
    public int Total;
    private int[] x = new int[] { 10 };
    private int[] y = new int[] { 10 };
    private Vegetable veg1;

    public void Start(string name)
    {
        MakePerson();
        CreateVeg();
        GameLoop();
    }

    public void GameLoop() //Her køre vores MovePerson og PickVeg konstant
    {
        while (true) 
        {
            MovePerson();
            PickVeg();
            MoveVeg();
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
        veg1 = new Vegetable();

        veg1.Name = "Carrot";
        veg1.Time = 5000;
        veg1.x = 8;
        veg1.y = 8;
        veg1.Point = 1;

        DrawVeg();
        stopwatch.Start();
    }

    public void DrawVeg()
    {
        Console.SetCursorPosition(veg1.x,veg1.y);
        Console.Write("C");
    }

    public void PickVeg()
    {
        if (x[0] == veg1.x && y[0] == veg1.y)
        {
            Total = Total + veg1.Point;
            veg1.x = random.Next(1, 31);
            veg1.y = random.Next(1, 21);

            while (veg1.x == x[0] && veg1.y == y[0])
            {
                veg1.x = random.Next(1, 31);
                veg1.y = random.Next(1, 21);
            }
            stopwatch.Restart();
            MakePerson();
            DrawVeg();
        }
    }

    public void MoveVeg()
    {
        if(stopwatch.ElapsedMilliseconds >= veg1.Time)
        {
            veg1.x = random.Next(1, 31);
            veg1.y = random.Next(1, 21);

            while (veg1.x == x[0] && veg1.y == y[0])
            {
                veg1.x = random.Next(1, 31);
                veg1.y = random.Next(1, 21);
            }
            stopwatch.Restart();
            MakePerson();
            DrawVeg();
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
        }
    } 
}

class Person
{
    public string Name;
    public int x;
    public int y;
    public Game MyGame;
}

class Vegetable
{
    public int x;
    public int y;
    public string Name;
    public int Point;
    public int Time;
    public Game MyGame2;
}