using System;
using System.Collections.Generic;

namespace RookKeypad
{
    class Program
    {
        static char[][] keypad = {
            new char[] { '1', '2', '3' },
            new char[] { '4', '5', '6' },
            new char[] { '7', '8', '9' },
            new char[] { '*', '0', '#' }
        };

        static HashSet<char> validStartKeys = new HashSet<char> { '2', '3', '4', '5', '6', '7', '8', '9' };

        static (int, int)[] rookMoves = { (1, 0), (-1, 0), (0, 1), (0, -1) };

        static (int, int) GetPosition(char key)
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                    if (keypad[i][j] == key)
                        return (i, j);
            return (-1, -1);
        }

        static (int, int) Move((int, int) pos, (int, int) move, int steps)
        {
            int newRow = pos.Item1 + move.Item1 * steps;
            int newCol = pos.Item2 + move.Item2 * steps;
            if (newRow >= 0 
                && newRow < 4 && 
                newCol >= 0 && 
                newCol < 3 && 
                keypad[newRow][newCol] != '*' && 
                keypad[newRow][newCol] != '#')
                return (newRow, newCol);
            return (-1, -1);
        }

        static List<string> GenerateNumbers((int, int) pos, string current, int length,int digitLength)
        {
            if (length == digitLength)
                return new List<string> { current };

            List<string> validNumbers = new List<string>();
            foreach (var move in rookMoves)
            {
                int steps = 1;
                while (true)
                {
                    var newPos = Move(pos, move, steps);
                    if (newPos.Item1 != -1)
                    {
                        char newKey = keypad[newPos.Item1][newPos.Item2];
                        validNumbers.AddRange(GenerateNumbers(newPos, current + newKey, length + 1, digitLength));
                        steps++;
                    }
                    else
                        break;
                }
            }
            return validNumbers;
        }

        static int CountValidNumbers(int digitLength)
        {
            List<string> validNumbers = new List<string>();
            foreach (var key in validStartKeys)
            {
                var startPos = GetPosition(key);
                validNumbers.AddRange(GenerateNumbers(startPos, key.ToString(), 1, digitLength));
            }
            return validNumbers.Count;
        }

        static void Main()
        {
            Console.WriteLine($"Count of valid 1-digit phone numbers: {CountValidNumbers(1)}");
            Console.WriteLine($"Count of valid 2-digit phone numbers: {CountValidNumbers(2)}");
            Console.WriteLine($"Count of valid 3-digit phone numbers: {CountValidNumbers(3)}");
            Console.WriteLine($"Count of valid 4-digit phone numbers: {CountValidNumbers(4)}");
            Console.WriteLine($"Count of valid 5-digit phone numbers: {CountValidNumbers(5)}");
            Console.WriteLine($"Count of valid 6-digit phone numbers: {CountValidNumbers(6)}");
            Console.WriteLine($"Count of valid 7-digit phone numbers: {CountValidNumbers(7)}");
        }
    }
}
