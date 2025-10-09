using System;
using System.Collections.Generic;

namespace RookKeypad
{
    /// <summary>
    /// This class performs all key related operations.Contains the keypad array and methods to get key positions and validate start keys.
    /// It applies encapsulation rule with variables and key related operations.
    /// </summary>
    class Keypad
    {
        private char[][] keys = new char[][]
        {
            new char[] { '1', '2', '3' },
            new char[] { '4', '5', '6' },
            new char[] { '7', '8', '9' },
            new char[] { '*', '0', '#' }
        };

        private HashSet<char> validStartKeys = new HashSet<char> { '2', '3', '4', '5', '6', '7', '8', '9' };

        public (int, int) GetPosition(char key)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                for (int j = 0; j < keys[i].Length; j++)
                {
                    if (keys[i][j] == key)
                        return (i, j);
                }
            }
            return (-1, -1); // Key not found
        }

        public char GetKeyAtPosition((int, int) position)
        {
            return keys[position.Item1][position.Item2];
        }

        public bool IsValidStartKey(char key)
        {
            return validStartKeys.Contains(key);
        }
    }

    /// <summary>
    /// This class performs all Rook's movement operations.And checks if a move is valid.
    /// It encapsulates the rook's movement logic
    /// </summary>
    class RookMovement
    {
        private (int, int)[] rookMoves = new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

        public List<(int, int)> GetValidMoves(Keypad keypad, (int, int) position)
        {
            List<(int, int)> validMoves = new List<(int, int)>();

            foreach (var move in rookMoves)
            {
                int steps = 1;
                while (true)
                {
                    var newPos = Move(position, move, steps);
                    if (IsWithinBounds(newPos) && IsKeyValid(keypad, newPos))
                    {
                        validMoves.Add(newPos);
                        steps++;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return validMoves;
        }

        private (int, int) Move((int, int) pos, (int, int) move, int steps)
        {
            int newRow = pos.Item1 + move.Item1 * steps;
            int newCol = pos.Item2 + move.Item2 * steps;
            return (newRow, newCol);
        }

        private bool IsWithinBounds((int, int) pos)
        {
            return pos.Item1 >= 0 && pos.Item1 < 4 && pos.Item2 >= 0 && pos.Item2 < 3;
        }

        private bool IsKeyValid(Keypad keypad, (int, int) pos)
        {
            char key = keypad.GetKeyAtPosition(pos);
            return key != '*' && key != '#';
        }
    }

    // PhoneNumberGenerator class generates valid phone numbers based on keypad and rook movement
    class PhoneNumberGenerator
    {
        private Keypad keypad;
        private RookMovement rookMovement;
        private Dictionary<char, List<char>> adjacency;

        public PhoneNumberGenerator(Keypad keypad, RookMovement rookMovement)
        {
            this.keypad = keypad;
            this.rookMovement = rookMovement;
            BuildAdjacency();
        }

        public int CountValidNumbers(int digitLength)
        {
            List<string> validNumbers = new List<string>();

            foreach (char key in "1234567890#*")
            {
                if (keypad.IsValidStartKey(key))
                {
                    var startPos = keypad.GetPosition(key);
                    validNumbers.AddRange(GenerateNumbers(startPos, key.ToString(), digitLength));
                }
            }

            return validNumbers.Count;
        }

        private void BuildAdjacency()
        {
            adjacency = new Dictionary<char, List<char>>();

            foreach (char key in "1234567890")
            {
                var pos = keypad.GetPosition(key);
                if (pos.Item1 == -1)
                    continue;

                var validMoves = rookMovement.GetValidMoves(keypad, pos);
                List<char> neighbors = new List<char>();
                foreach (var newPos in validMoves)
                {
                    char newKey = keypad.GetKeyAtPosition(newPos);
                    neighbors.Add(newKey);
                }
                adjacency[key] = neighbors;
            }
        }

        public long CountValidNumbersDP(int digitLength)
        {
            if (digitLength <= 0)
                return 0;

            Dictionary<char, long> counts = new Dictionary<char, long>();
            foreach (char key in "1234567890")
            {
                counts[key] = keypad.IsValidStartKey(key) ? 1L : 0L;
            }

            for (int length = 2; length <= digitLength; length++)
            {
                Dictionary<char, long> nextCounts = new Dictionary<char, long>();
                foreach (char key in "1234567890")
                {
                    nextCounts[key] = 0L;
                }

                foreach (char fromKey in "1234567890")
                {
                    long ways = counts[fromKey];
                    if (ways == 0)
                        continue;

                    foreach (char toKey in adjacency[fromKey])
                    {
                        nextCounts[toKey] += ways;
                    }
                }

                counts = nextCounts;
            }

            long total = 0;
            foreach (var kv in counts)
            {
                total += kv.Value;
            }
            return total;
        }

        private List<string> GenerateNumbers((int, int) pos, string current, int digitLength)
        {
            if (current.Length == digitLength)
                return new List<string> { current };

            List<string> validNumbers = new List<string>();
            var validMoves = rookMovement.GetValidMoves(keypad, pos);

            foreach (var newPos in validMoves)
            {
                char newKey = keypad.GetKeyAtPosition(newPos);
                validNumbers.AddRange(GenerateNumbers(newPos, current + newKey, digitLength));
            }

            return validNumbers;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Keypad keypad = new Keypad();
            RookMovement rookMovement = new RookMovement();
            PhoneNumberGenerator generator = new PhoneNumberGenerator(keypad, rookMovement);

            // Defaults
            int minLen = 1;
            int maxLen = 7;
            string algo = "dp"; // dp or dfs

            // Simple CLI parsing
            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLowerInvariant();
                if ((arg == "-l" || arg == "--length") && i + 1 < args.Length && int.TryParse(args[i + 1], out int len))
                {
                    minLen = len;
                    maxLen = len;
                    i++;
                }
                else if ((arg == "-m" || arg == "--min") && i + 1 < args.Length && int.TryParse(args[i + 1], out int min))
                {
                    minLen = min;
                    i++;
                }
                else if ((arg == "--max") && i + 1 < args.Length && int.TryParse(args[i + 1], out int max))
                {
                    maxLen = max;
                    i++;
                }
                else if ((arg == "-r" || arg == "--range") && i + 1 < args.Length)
                {
                    string[] parts = args[i + 1].Split('-', '–');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int a) && int.TryParse(parts[1], out int b))
                    {
                        minLen = Math.Min(a, b);
                        maxLen = Math.Max(a, b);
                    }
                    i++;
                }
                else if ((arg == "-a" || arg == "--algo") && i + 1 < args.Length)
                {
                    string value = args[i + 1].ToLowerInvariant();
                    if (value == "dp" || value == "dfs")
                        algo = value;
                    i++;
                }
            }

            if (minLen < 1)
                minLen = 1;
            if (maxLen < minLen)
                maxLen = minLen;

            for (int i = minLen; i <= maxLen; i++)
            {
                if (algo == "dp")
                {
                    long count = generator.CountValidNumbersDP(i);
                    Console.WriteLine($"Count of valid {i}-digit phone numbers (dp): {count}");
                }
                else
                {
                    int count = generator.CountValidNumbers(i);
                    Console.WriteLine($"Count of valid {i}-digit phone numbers (dfs): {count}");
                }
            }
        }
    }
}
