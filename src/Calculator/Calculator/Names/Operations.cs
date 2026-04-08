// Created by Anton Piruev in 2026. 
// Any direct commercial use of derivative work is strictly prohibited.

using System.Linq;

namespace Calculator.Names
{
  internal static class Operations
  {
    public const string Add = "+";
    public const string Subtract = "-";
    public const string Multiply = "*";
    public const string Divide = "/";

    public static string[] PossibleOperations =
      { Add, Subtract, Multiply, Divide };

    public static bool HasOperations(string line) =>
      PossibleOperations.Any(line.Contains);
  }
}
