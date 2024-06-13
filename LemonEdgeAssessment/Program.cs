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

        public PhoneNumberGenerator(Keypad keypad, RookMovement rookMovement)
        {
            this.keypad = keypad;
            this.rookMovement = rookMovement;
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
        static void Main()
        {
            Keypad keypad = new Keypad();
            RookMovement rookMovement = new RookMovement();
            PhoneNumberGenerator generator = new PhoneNumberGenerator(keypad, rookMovement);

            for (int i = 1; i <= 7; i++)
            {
                Console.WriteLine($"Count of valid {i}-digit phone numbers: {generator.CountValidNumbers(i)}");
            }
        }
    }
}
