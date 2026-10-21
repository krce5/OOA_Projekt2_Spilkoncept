//vores dejlige projekt
//Hej
//kaj kage
//Se kaj kagen
//Cool

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
    public int Total;
    private int[] x = new int[] { 10 };
    private int[] y = new int[] { 10 };

    //private int[] Vegx = new int[] { 8 };
    //private int[] Vegy = new int[] { 8 };



    public void Start(string name)
    {
        MakePerson(name);
        MovePerson();
        CreateVeg();
    }

    public void MakePerson(string name)
    {
        Console.Clear();
        Console.SetCursorPosition(x[0], y[0]);
        Console.Write("O");

    }

    public void MovePerson()
    {
        while (true)
        {
            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow) y[0]--;
                if (key.Key == ConsoleKey.DownArrow) y[0]++;
                if (key.Key == ConsoleKey.LeftArrow) x[0]--;
                if (key.Key == ConsoleKey.RightArrow) x[0]++;

                Console.Clear();
                Console.SetCursorPosition(x[0], y[0]);
                Console.Write("O");
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
            }
        }
    }
    public void CreateVeg()
    {
        Vegetable veg1 = new Vegetable();

        veg1.Name = "Carrot";
        veg1.Time = 3;
        

        Console.Clear();
        Console.SetCursorPosition(8, 8);
        Console.Write("C");
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