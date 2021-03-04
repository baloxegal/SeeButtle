using System;

namespace SeeButtle
{
    using Things;
    class Program
    {
        static void Main(string[] args)
        {
            new Ship('e', 5, 's', 3);
            new Ship('a', 9, 's', 2);
            new Ship('c', 3, 'w', 3);
            new Ship('f', 5, 'n', 1);

            Map gameMap = new Map();
            gameMap.Render();
        }
    }    
}
namespace Things
{
    class Ship
    {
        public char X {get; set;}
        public int Y { get; set; }
        public char Dir { get; set; }
        public int Len { get; set; }
        bool[] segs;
        public static int count = 0;        

        public Ship()
        {
            ++count;
            Map.things[count - 1] = this;
        }
        public Ship(char x, int y, char dir, int len)
        {
            X = x;
            Y = y - 1;
            Dir = dir;
            Len = len;
            ++count;
            Map.things[count - 1] = this; 
        }
    }

    class Map
    {
        int width = 10;
        int height = 10;
        string xCoord = "abcdefghij";

        public static Ship[] things = new Ship[10];

        public void Render()
        {
            var b = "";
            for(int i = 0; i < xCoord.Length; i++)
            {
                b = b + xCoord[i] + " ";
            }            
            Console.WriteLine(String.Format("{0, 4} {1, 20}", " ", b));
            for(int y = 0; y < height; y++)
            {
                Console.Write(String.Format("{0, 3} {1, 1} ", y + 1, ""));
                for(int x = 0; x < width; x++)
                {
                    for(int i = 0; i < 3; i++)
                    {
                        if(things[i].Y == y && things[i].X == xCoord[x])
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                            break;
                        }
                    }                    
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}