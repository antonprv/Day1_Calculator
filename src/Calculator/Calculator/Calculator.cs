// Created by Anton Piruev in 2026. 
// Any direct commercial use of derivative work is strictly prohibited.

using System;
using System.Data;
using System.Linq;

namespace Calculator
{
  internal class Calculator
  {
    public string LastCalculation { get; private set; }

    public string CalculationStack { get; private set; }

    public Calculator() => CalculationStack = string.Empty;

    public void AddOperation(string operation) => CalculationStack += operation;

    public void SetCalculationStack(string newStack) => CalculationStack = newStack;

    public void ClearLastCalculation() => LastCalculation = string.Empty;

    public void ClearCalculationStack() => CalculationStack = string.Empty;

    public string PerformCalculation()
    {
      string result;

      if (string.IsNullOrWhiteSpace(CalculationStack))
      {
        ClearLastCalculation();
        ClearCalculationStack();
        return "0";
      }

      try
      {
        result = new DataTable().Compute(CalculationStack, null).ToString();
      }
      catch (Exception)
      {
        result = "0";
        ClearLastCalculation();
        ClearCalculationStack();

        return result;
      }

      LastCalculation = $"{CalculationStack} = {result}";

      ClearCalculationStack();

      return result;
    }
  }
}
