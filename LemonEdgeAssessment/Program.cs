using System;
using System.Collections.Generic;

namespace RookKeypad
{
    /// <summary>
    /// This class performs all key-related operations.
    /// Contains the keypad array and methods to get key positions and validate start keys.
    /// Demonstrates encapsulation with private members.
    /// </summary>
    class Keypad
    {
        // 2D array representing the keypad layout
        private char[][] keys = new char[][]
        {
            new char[] { '1', '2', '3' },
            new char[] { '4', '5', '6' },
            new char[] { '7', '8', '9' },
            new char[] { '*', '0', '#' }
        };

        // Valid keys from which a phone number can start
        private HashSet<char> validStartKeys = new HashSet<char> { '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Returns the position (row, column) of the given key in the keypad.
        /// </summary>
        public (int, int) GetPosition(char key)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                for (int j = 0; j < keys[i].Length; j++)
                {
                    if (keys[i][j] == key)
                        return (i, j); // Return position when key is found
                }
            }
            return (-1, -1); // Return invalid position if key not found
        }

        /// <summary>
        /// Returns the key character at a given position (row, column).
        /// </summary>
        public char GetKeyAtPosition((int, int) position)
        {
            return keys[position.Item1][position.Item2];
        }

        /// <summary>
        /// Checks whether a given key is a valid starting key.
        /// </summary>
        public bool IsValidStartKey(char key)
        {
            return validStartKeys.Contains(key);
        }
    }

    /// <summary>
    /// This class encapsulates the Rook’s movement logic on the keypad.
    /// It checks and returns all valid rook moves from a given position.
    /// </summary>
    class RookMovement
    {
        // Possible rook movements: up, down, left, right
        private (int, int)[] rookMoves = new (int, int)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };

        /// <summary>
        /// Returns all valid positions the rook can move to from the current position.
        /// </summary>
        public List<(int, int)> GetValidMoves(Keypad keypad, (int, int) position)
        {
            List<(int, int)> validMoves = new List<(int, int)>();

            foreach (var move in rookMoves)
            {
                int steps = 1;
                while (true)
                {
                    var newPos = Move(position, move, steps); // Move rook in given direction by 'steps'
                    if (IsWithinBounds(newPos) && IsKeyValid(keypad, newPos))
                    {
                        validMoves.Add(newPos); // Add valid move to list
                        steps++; // Continue moving in the same direction
                    }
                    else
                    {
                        break; // Stop when move goes out of bounds or hits invalid key
                    }
                }
            }
            return validMoves;
        }

        /// <summary>
        /// Calculates new position after moving certain steps in a given direction.
        /// </summary>
        private (int, int) Move((int, int) pos, (int, int) move, int steps)
        {
            int newRow = pos.Item1 + move.Item1 * steps;
            int newCol = pos.Item2 + move.Item2 * steps;
            return (newRow, newCol);
        }

        /// <summary>
        /// Checks if the position is within the 4x3 keypad grid.
        /// </summary>
        private bool IsWithinBounds((int, int) pos)
        {
            return pos.Item1 >= 0 && pos.Item1 < 4 && pos.Item2 >= 0 && pos.Item2 < 3;
        }

        /// <summary>
        /// Checks if the key at a given position is valid (not '*' or '#').
        /// </summary>
        private bool IsKeyValid(Keypad keypad, (int, int) pos)
        {
            char key = keypad.GetKeyAtPosition(pos);
            return key != '*' && key != '#';
        }
    }

    /// <summary>
    /// Generates all valid phone numbers based on rook movement rules.
    /// Combines logic from Keypad and RookMovement classes.
    /// </summary>
    class PhoneNumberGenerator
    {
        private Keypad keypad;
        private RookMovement rookMovement;

        /// <summary>
        /// Constructor initializing keypad and rook movement references.
        /// </summary>
        public PhoneNumberGenerator(Keypad keypad, RookMovement rookMovement)
        {
            this.keypad = keypad;
            this.rookMovement = rookMovement;
        }

        /// <summary>
        /// Counts the number of valid phone numbers of a given length.
        /// </summary>
        public int CountValidNumbers(int digitLength)
        {
            List<string> validNumbers = new List<string>();

            // Iterate over all keys on keypad
            foreach (char key in "1234567890#*")
            {
                if (keypad.IsValidStartKey(key)) // Check if key can start a phone number
                {
                    var startPos = keypad.GetPosition(key);
                    // Recursively generate valid numbers of given length
                    validNumbers.AddRange(GenerateNumbers(startPos, key.ToString(), digitLength));
                }
            }

            return validNumbers.Count;
        }

        /// <summary>
        /// Recursively generates all valid phone numbers starting from a given position.
        /// </summary>
        private List<string> GenerateNumbers((int, int) pos, string current, int digitLength)
        {
            // Base case: if desired length reached, return current number
            if (current.Length == digitLength)
                return new List<string> { current };

            List<string> validNumbers = new List<string>();
            var validMoves = rookMovement.GetValidMoves(keypad, pos); // Get next valid moves

            // Explore each valid next move recursively
            foreach (var newPos in validMoves)
            {
                char newKey = keypad.GetKeyAtPosition(newPos);
                validNumbers.AddRange(GenerateNumbers(newPos, current + newKey, digitLength));
            }

            return validNumbers;
        }
    }

    /// <summary>
    /// Main program entry point — executes rook keypad logic and displays results.
    /// </summary>
    class Program
    {
        static void Main()
        {
            // Create instances of the required classes
            Keypad keypad = new Keypad();
            RookMovement rookMovement = new RookMovement();
            PhoneNumberGenerator generator = new PhoneNumberGenerator(keypad, rookMovement);

            // Print counts of all valid numbers from 1 to 7 digits
            for (int i = 1; i <= 7; i++)
            {
                Console.WriteLine($"Count of valid {i}-digit phone numbers: {generator.CountValidNumbers(i)}");
            }
        }
    }
}
