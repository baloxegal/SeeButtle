using System;

namespace SeeButtle
{
    using Things;
    class Program
    {
        static void Main(string[] args)
        {
            Map gameMap = new Map();
            gameMap.Render();

            gameMap.myNavy.CombatFormation();

            gameMap.Render();
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
        static public int width = 10;
        static public int height = 10;
        static public string xCoord = "abcdefghijklmnopqrstuvwxyz";
        public Navy myNavy = new Navy();

        public void Render()
        {
            var b = "";
            for (int i = 0; i < width; i++)
            {
                b += xCoord[i] + " ";
            }
            Console.WriteLine(String.Format("{0, 4} {1, 20}", " ", b));
            for (int y = 0; y < height; y++)
            {
                Console.Write(String.Format("{0, 3} {1, 1} ", y + 1, " "));
                for (int x = 0; x < width; x++)
                {                    
                    for (int i = 0; i < myNavy.oneDeck.Length; i++)
                    {
                        if (myNavy.oneDeck[i].X == xCoord[x] && myNavy.oneDeck[i].Y == y)
                        {
                            Console.Write("# ");
                        }
                        else
                        {
                            if(i == myNavy.oneDeck.Length - 1)
                                Console.Write(".");                            
                        }
                    }
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
    class Navy
    {
        protected enum DeckQuant
        {
            ONE_DECK = 1, TWO_DECK, THREE_DECK, FOUR_DECK
        }
        protected enum ShipQuant
        {
            FOUR_DECK = 1, THREE_DECK, TWO_DECK, ONE_DECK
        }
        protected enum Direction
        {
            NORTH = 'N', SOUTH = 'S', WEST = 'W', EAST = 'E'
        }
        public Ship[] oneDeck = new Ship[(int)ShipQuant.ONE_DECK];
        public Ship[] twoDeck = new Ship[(int)ShipQuant.TWO_DECK];
        public Ship[] threeDeck = new Ship[(int)ShipQuant.THREE_DECK];
        public Ship[] fourDeck = new Ship[(int)ShipQuant.FOUR_DECK];

        public Navy()
        {
            for (int i = 0; i < oneDeck.Length; i++)
            {
                oneDeck[i] = new Ship { Len = (int)DeckQuant.ONE_DECK };
            }
            for (int i = 0; i < twoDeck.Length; i++)
            {
                twoDeck[i] = new Ship { Len = (int)DeckQuant.TWO_DECK };
            }
            for (int i = 0; i < threeDeck.Length; i++)
            {
                threeDeck[i] = new Ship { Len = (int)DeckQuant.THREE_DECK };
            }
            for (int i = 0; i < fourDeck.Length; i++)
            {
                fourDeck[i] = new Ship { Len = (int)DeckQuant.FOUR_DECK };
            }
        }
        public void CombatFormation()
        {
            for (int i = 0; i < oneDeck.Length; i++)
            {
                while (true)
                {
                    Console.WriteLine($"Enter from \"{Map.xCoord[0]}\" to \"{Map.xCoord[Map.width - 1]}\" character for X coordinate");
                    var x = Console.ReadLine();
                    if (string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x) || int.TryParse(x, out int d) || x.Length > 1 || !Map.xCoord.Contains(x))
                    {
                        Console.WriteLine("You enter wrong character for X coordinate");
                        continue;
                    }
                    oneDeck[i].X = x.ToCharArray()[0];
                    break;                   
                }
                while (true)
                {
                    Console.WriteLine($"Enter from \"1\" to \"{Map.height}\" integer for Y coordinate");
                    var y = Console.ReadLine();
                    if (!int.TryParse(y, out int d) || d > 10 || d < 1)
                    {
                        Console.WriteLine("You enter wrong character for Y coordinate");
                        continue;
                    }
                    oneDeck[i].Y = d;
                    break;                    
                }
                while (true)
                {
                    Console.WriteLine($"Enter one of the following characters \"{(char)Direction.NORTH}\", \"{(char)Direction.SOUTH}\", \"{(char)Direction.WEST}\" or \"{(char)Direction.EAST}\" for initialization of ship direction field");
                    var d = Console.ReadLine();
                    if (string.IsNullOrEmpty(d) || string.IsNullOrWhiteSpace(d) || int.TryParse(d, out int dir) || d.Length > 1 || Enum.IsDefined(typeof(Direction), d))
                    {
                        Console.WriteLine("You enter wrong character for ship direction");
                        continue;
                    }
                    oneDeck[i].Dir = d.ToCharArray()[0];
                    break;
                }
            }            
        }
    }
}