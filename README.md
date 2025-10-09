# LemonEdge Assessment - Rook Keypad Phone Number Generator

A C# console application that generates valid phone numbers by simulating rook chess piece movement on a standard phone keypad.

## Problem Description

This project solves the following challenge: Given a standard phone keypad layout, generate all possible valid phone numbers of various lengths where each subsequent digit can only be reached by moving like a rook in chess (horizontally or vertically) from the current position.

### Keypad Layout
```
1 2 3
4 5 6
7 8 9
* 0 #
```

### Rules
- **Valid starting keys**: 2, 3, 4, 5, 6, 7, 8, 9 (excludes 1, 0, *, #)
- **Movement**: Rook-style movement only (horizontal and vertical)
- **Forbidden keys**: Cannot land on `*` or `#` during movement
- **Phone number lengths**: Generates numbers from 1 to 7 digits

## Features

- 🎯 **Object-Oriented Design**: Clean separation of concerns with dedicated classes
- ♜ **Chess Logic**: Implements rook movement patterns accurately
- 📱 **Keypad Simulation**: Models a standard phone keypad with position tracking
- 🔢 **Number Generation**: Recursively generates all valid phone number combinations
- 📊 **Statistics**: Counts and displays valid numbers for each length

## Project Structure

```
LemonEdgeAssessment/
├── LemonEdgeAssessment.csproj    # Project configuration
├── Program.cs                    # Main application with all classes
└── README.md                     # This file
```

### Classes

- **`Keypad`**: Manages the keypad layout, key positions, and validation
- **`RookMovement`**: Handles rook chess piece movement logic
- **`PhoneNumberGenerator`**: Generates valid phone numbers using keypad and movement rules
- **`Program`**: Entry point that demonstrates the functionality

## Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later

### Installation & Running

1. **Clone the repository**:
   ```bash
   git clone <repository-url>
   cd LemonEdgeAssessment
   ```

2. **Navigate to the project directory**:
   ```bash
   cd LemonEdgeAssessment
   ```

3. **Build the project**:
   ```bash
   dotnet build
   ```

4. **Run the application**:
   ```bash
   dotnet run --project LemonEdgeAssessment
   ```

## Sample Output

```
Count of valid 1-digit phone numbers: 8
Count of valid 2-digit phone numbers: 24
Count of valid 3-digit phone numbers: 56
Count of valid 4-digit phone numbers: 120
Count of valid 5-digit phone numbers: 240
Count of valid 6-digit phone numbers: 472
Count of valid 7-digit phone numbers: 904
```

## CLI Usage

The app now supports CLI options to control the computation:

- `-l | --length <n>`: Compute count for a single length n
- `-m | --min <n>` and `--max <n>`: Compute counts for the range [min, max]
- `-r | --range <a-b>`: Shorthand range, e.g., `-r 3-7`
- `-a | --algo <dp|dfs>`: Choose algorithm; `dp` (default) is faster for counting

Examples:

```bash
dotnet run --project LemonEdgeAssessment -- -l 5
dotnet run --project LemonEdgeAssessment -- -r 3-7
dotnet run --project LemonEdgeAssessment -- -m 2 --max 6 -a dfs
```

Notes:
- Use `--` to separate `dotnet run` arguments from app arguments.
- `dp` uses dynamic programming over a precomputed adjacency map for speed.

## Algorithm Explanation

1. **Initialization**: Create keypad with valid starting positions
2. **Starting Points**: Begin from each valid starting key (2-9)
3. **Movement Generation**: For each position, calculate all valid rook moves
4. **Recursive Building**: Recursively build phone numbers by exploring valid moves
5. **Validation**: Ensure moves stay within bounds and avoid forbidden keys
6. **Counting**: Count all valid combinations for each target length

### Time Complexity
- **Worst Case**: O(8 × 4^n) where n is the phone number length
- **Space Complexity**: O(n) for recursion stack depth

## Performance Notes

- The original recursive DFS enumerates all combinations; correct but slower for larger n.
- The new DP method counts paths without enumerating them, typically much faster for larger n.

## Technical Details

- **Target Framework**: .NET 8.0
- **Language**: C# 12.0
- **Architecture**: Console Application
- **Design Patterns**: Object-Oriented Programming with encapsulation

## Contributing

This is an assessment project, but suggestions and improvements are welcome:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/improvement`)
3. Commit your changes (`git commit -am 'Add improvement'`)
4. Push to the branch (`git push origin feature/improvement`)
5. Create a Pull Request

## License

This project is created as part of a technical assessment for LemonEdge.

## Author

Created as part of LemonEdge technical assessment - October 2025