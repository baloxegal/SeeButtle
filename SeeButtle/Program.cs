using System;
using System.Collections.Generic;
using System.Linq;

namespace SeeButtle
{
    using Things;
    class Program
    {
        static void Main(string[] args)
        {
            Map gameMap = new Map();
            gameMap.Render();

            gameMap.myNavy.CombatFormation(gameMap);

        }
    }    
}
namespace Things
{
    class Ship
    {
        public char X { get; set; }
        public int Y { get; set; }
        public char Dir { get; set; }
        public int Len { get; set; }
        bool[] state;
        public static int count = 0;

        public Ship()
        {
            ++count;
        }
        public Ship(char x, int y, char dir, int len)
        {
            X = x;
            Y = y - 1;
            Dir = dir;
            Len = len;
            state = new bool[Len];
            ++count;
        }
        public bool this[int i]
        {
            get
            {
                return state[i];
            }
            set
            {
                state[i] = value;
            }
        }
    }

    class Map
    {
        public const int SIZE = 10;        
        public const string X_Coord = "abcdefghijklmnopqrstuvwxyz";        
        public Navy myNavy = new Navy();

        public void Render()
        {
            var b = "";
            for (int i = 0; i < SIZE; i++)
            {
                b += X_Coord[i] + " ";
            }
            Console.WriteLine(String.Format("{0, 4}{1, 20}", " ", b));
            for (int y = 0; y < SIZE; y++)
            {
                Console.Write(String.Format("{0, 3}{1, 1}", y + 1, " "));
                for (int x = 0; x < SIZE; x++)
                {
                    if(myNavy.CoordinatesList.Contains(KeyValuePair.Create(X_Coord[x], y)))
                        Console.Write("# ");                        
                    else
                        Console.Write(". ");                   
                }
                Console.WriteLine();
            }
        }
    }
    class Navy
    {
        protected enum DeckQuant
        {
            ONE_DECK = 1, TWO_DECK//, THREE_DECK, FOUR_DECK
        }
        protected enum ShipQuant
        {
            /*FOUR_DECK = 1, THREE_DECK,*/ TWO_DECK = 1, ONE_DECK
        }
        Dictionary<ShipQuant, int> ShipDictionary = new Dictionary<ShipQuant, int>();
        public List<KeyValuePair<char, int>> CoordinatesList = new List<KeyValuePair<char, int>>();
        protected enum Direction
        {
            NORTH = 'N', SOUTH = 'S', WEST = 'W', EAST = 'E'
        }
        public Ship[][] myNavy;        
        public Navy()
        {
            ShipQuant[] ArrayShipQuant = (ShipQuant[])Enum.GetValues(typeof(ShipQuant));
            DeckQuant[] ArrayDeckQuant = (DeckQuant[])Enum.GetValues(typeof(DeckQuant));            
            for(int i = 0; i < ArrayDeckQuant.Length; i++)
            {
                ShipDictionary.Add(ArrayShipQuant[ArrayShipQuant.Length - 1 - i], (int)ArrayDeckQuant[i]);
            }
            
            myNavy = new Ship[ArrayShipQuant.Length][];
            for (int i = 0; i < myNavy.Length; i++)
            {
                myNavy[i] = new Ship[(int)ArrayShipQuant[i]];
                for(int j = 0; j <  myNavy[i].Length; j++)
                {   
                    ShipDictionary.TryGetValue(ArrayShipQuant[i], out int l);
                    myNavy[i][j] = new Ship { Len = l };
                }
            }           
        }
        public void CombatFormation(Map map)
        {            
            for (int i = 0; i < myNavy.Length; i++)
            {
                for (int j = 0; j < myNavy[i].Length; j++)
                {
                    while (true)
                    {
                        Console.WriteLine($"Enter from \"{Map.X_Coord[0]}\" to \"{Map.X_Coord[Map.SIZE - 1]}\" character for X coordinate");
                        var x = Console.ReadLine();
                        if (string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x) || int.TryParse(x, out int d) || x.Length > 1 || !Map.X_Coord.Substring(0, 10).Contains(x))
                        {
                            Console.WriteLine("You enter wrong character for X coordinate");
                            continue;
                        }
                        myNavy[i][j].X = x.ToCharArray()[0];
                        break;
                    }
                    while (true)
                    {
                        Console.WriteLine($"Enter from \"1\" to \"{Map.SIZE}\" integer for Y coordinate");
                        var y = Console.ReadLine();
                        if (!int.TryParse(y, out int d) || d > 10 || d < 1)
                        {
                            Console.WriteLine("You enter wrong character for Y coordinate");
                            continue;
                        }
                        myNavy[i][j].Y = d;
                        break;
                    }
                    while (true)
                    {
                        Console.WriteLine($"Enter one of the following characters \"{(char)Direction.NORTH}\", \"{(char)Direction.SOUTH}\", \"{(char)Direction.WEST}\" or \"{(char)Direction.EAST}\" for initialization of ship direction field");
                        var d = Console.ReadLine();
                        if (string.IsNullOrEmpty(d) || string.IsNullOrWhiteSpace(d) || int.TryParse(d, out int dir) || d.Length > 1 || !Enum.IsDefined(typeof(Direction), (int)d.ToCharArray()[0]))
                        {                           
                            Console.WriteLine("You enter wrong character for ship direction");
                            continue;
                        }
                        myNavy[i][j].Dir = d.ToCharArray()[0];
                        break;
                    }
                    KeyValuePair<char, int> pair = KeyValuePair.Create(myNavy[i][j].X, myNavy[i][j].Y);
                    CoordinatesList.Add(pair);
                    Console.Clear();
                    map.Render();
                }               
            }
        }
    }
}