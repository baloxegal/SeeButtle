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
            gameMap.CombatFormation();
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
        public bool[] State { get; set; }
        public List<KeyValuePair<char, int>> CoordList;
        
        public Ship()
        {            
        }
        public Ship(char x, int y, char dir, int len)
        {
            X = x;
            Y = y;
            Dir = dir;
            Len = len;
            State = new bool[Len];            
        }
        public bool this[int i]
        {
            get
            {
                return State[i];
            }
            set
            {
                State[i] = value;
            }
        }
    }

    class Map
    {        
        const int WIDTH = 10;
        const int HEIGHT = 10;
        const string X_COORD = "abcdefghijklmnopqrstuvwxyz";        
        Navy myNavy = new Navy();
        enum Direction
        {
            NORTH = 'N', SOUTH = 'S', WEST = 'W', EAST = 'E'
        }
        List<KeyValuePair<char, int>> AllCoordinatesList = new List<KeyValuePair<char, int>>();
        public void Render()
        {
            var b = "";
            for (int i = 0; i < WIDTH; i++)
            {
                b += X_COORD[i] + " ";
            }
            Console.WriteLine(String.Format("{0, 4}{1, 20}", " ", b));
            for (int y = 0; y < HEIGHT; y++)
            {
                Console.Write(String.Format("{0, 3}{1, 1}", y + 1, " "));
                for (int x = 0; x < WIDTH; x++)
                {
                    if(AllCoordinatesList.Contains(KeyValuePair.Create(X_COORD[x], y + 1)))
                        Console.Write("# ");                        
                    else
                        Console.Write(". ");                   
                }
                Console.WriteLine();
            }
        }
        public void CombatFormation()
        {
            for (int i = 0; i < myNavy.navy.Length; i++)
            {
                for (int j = 0; j < myNavy.navy[i].Length; j++)
                {
                    Console.WriteLine($"Entering data for {myNavy.navy[i][j].Len} deck ship");
                        
                    while (true)
                    {
                        Console.WriteLine($"Enter from \"{Map.X_COORD[0]}\" to \"{Map.X_COORD[Map.WIDTH - 1]}\" character for X coordinate");
                        var x = Console.ReadLine();
                        if (string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x) || int.TryParse(x, out int d) || x.Length > 1 || !Map.X_COORD.Substring(0, WIDTH).Contains(x))
                        {
                            Console.WriteLine("You enter wrong character for X coordinate");
                            continue;
                        }
                        myNavy.navy[i][j].X = x.ToCharArray()[0];
                        break;
                    }
                    while (true)
                    {
                        Console.WriteLine($"Enter from \"1\" to \"{Map.HEIGHT}\" integer for Y coordinate");
                        var y = Console.ReadLine();
                        if (!int.TryParse(y, out int d) || d > 10 || d < 1)
                        {
                            Console.WriteLine("You enter wrong character for Y coordinate");
                            continue;
                        }
                        myNavy.navy[i][j].Y = d;
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
                        myNavy.navy[i][j].Dir = d.ToCharArray()[0];
                        break;
                    }
                    myNavy.navy[i][j].CoordList = new List<KeyValuePair<char, int>>();
                    myNavy.navy[i][j].CoordList.Add(KeyValuePair.Create(myNavy.navy[i][j].X, myNavy.navy[i][j].Y));                    
                    for (int l = 1; l < myNavy.navy[i][j].Len; l++)
                    {
                        var key = myNavy.navy[i][j].CoordList.Last().Key;
                        var value = myNavy.navy[i][j].CoordList.Last().Value;
                        switch (myNavy.navy[i][j].Dir)
                        {
                             
                            case 'N' : 
                                myNavy.navy[i][j].CoordList.Add(KeyValuePair.Create(key, value++));
                                break;
                            case 'S':
                                myNavy.navy[i][j].CoordList.Add(KeyValuePair.Create(key, value--));
                                break;
                            case 'W':
                                myNavy.navy[i][j].CoordList.Add(KeyValuePair.Create(key--, value));
                                break;
                            case 'E':
                                myNavy.navy[i][j].CoordList.Add(KeyValuePair.Create(key++, value));
                                break;
                        }
                    }
                    List<KeyValuePair<char, int>> AdditionalCoordinates = new List<KeyValuePair<char, int>>();
                    for(int a = 0; a < myNavy.navy[i][j].CoordList.Count; a++)
                    {
                        var key = myNavy.navy[i][j].CoordList[a].Key;
                        var value = myNavy.navy[i][j].CoordList[a].Value;
                        for (char w = key--; w <= key++; w++)
                        {
                            for(int h = value--; h < value++; h++)
                            {
                                if ((w == key && h == value) || AdditionalCoordinates.Contains(KeyValuePair.Create(w, h)))
                                    continue;
                                AdditionalCoordinates.Add(KeyValuePair.Create(w, h));
                            }
                        }
                    }
                    for (int c = 0; c < myNavy.navy[i][j].CoordList.Count; c++)
                    {
                        if(myNavy.navy[i][j].CoordList[c].Key < 'a' || myNavy.navy[i][j].CoordList[c].Key > Map.X_COORD[WIDTH - 1] || myNavy.navy[i][j].CoordList[c].Value < 0 || myNavy.navy[i][j].CoordList[c].Value > HEIGHT - 1)
                        {
                            Console.WriteLine("You entered the coordinates at which the ship is out of the battlefield");
                            myNavy.navy[i][j].CoordList.Clear();
                            break;
                        }                        
                    }
                    if (myNavy.navy[i][j].CoordList.Count == 0)
                    {
                        j--;
                        continue;
                    }
                    if (AllCoordinatesList.Any(coord => myNavy.navy[i][j].CoordList.Contains(coord)))
                    {                        
                        Console.WriteLine("You have mistakenly entered coordinates that have already been used");
                        myNavy.navy[i][j].CoordList.Clear();
                    }
                    else if (AllCoordinatesList.Any(coord => AdditionalCoordinates.Contains(coord)))
                    {
                        Console.WriteLine("You entered the wrong coordinates, the ships are touching");
                        myNavy.navy[i][j].CoordList.Clear();
                    }
                    else
                        AllCoordinatesList.AddRange(myNavy.navy[i][j].CoordList);
                    if (myNavy.navy[i][j].CoordList.Count == 0)
                    {
                        j--;
                        continue;
                    }                   
                    Console.Clear();
                    Render();
                }
            }
        }
    }
    class Navy
    {
        enum DeckQuant
        {
            ONE_DECK = 1, TWO_DECK, THREE_DECK, FOUR_DECK
        }
        enum ShipQuant
        {
            ONE_DECK = 4, TWO_DECK = 3, THREE_DECK = 2, FOUR_DECK = 1
        }        
        public Ship[][] navy;         
        public Navy()
        {
            ShipQuant[] ArrayShipQuant = (ShipQuant[])Enum.GetValues(typeof(ShipQuant));
            DeckQuant[] ArrayDeckQuant = (DeckQuant[])Enum.GetValues(typeof(DeckQuant));            
            navy = new Ship[ArrayShipQuant.Length][];            
            for (int i = 0; i < navy.Length; i++)
            {
                navy[i] = new Ship[(int)ArrayShipQuant[i]];                
                for (int j = 0; j <  navy[i].Length; j++)
                {   
                    navy[i][j] = new Ship { Len = (int)ArrayDeckQuant[i], State = new bool[(int)ArrayDeckQuant[i]] };
                }
            }           
        }        
    }
}