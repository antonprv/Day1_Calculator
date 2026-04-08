// Created by Anton Piruev in 2026. 
// Any direct commercial use of derivative work is strictly prohibited.

using System.Windows.Forms;

namespace Calculator
{
  public partial class UI : Form
  {
    private readonly Calculator calculator;

    private bool _isUpdatingText = false;

    public UI()
    {
      InitializeComponent();
      calculator = new Calculator();
      InitializeContextMenu();
    }

    #region Perform Calculation | Clear history

    private void KeyEquals_Click(object sender, System.EventArgs e)
    {
      string calcResult;

      calcResult = calculator.PerformCalculation();
      calculator.AddOperation(calcResult);

      UpdateCalcBox(calcResult, true);

      if (string.IsNullOrWhiteSpace(calculator.LastCalculation))
        return;

      AppendHistory(calculator.LastCalculation);
      calculator.ClearLastCalculation();
    }

    private void KeyClear_Click(object sender, System.EventArgs e)
    {
      UpdateCalcBox(OperationNames.Zero);
      calculator.ClearLastCalculation();
      calculator.ClearCalculationStack();
    }

    private void KeyClearErase_Click(object sender, System.EventArgs e)
    {
      calculator.ClearLastCalculation();
      historyRichTextBox.Clear();
    }
    #endregion

    #region Add Operations To Stack 

    private void AddCalcOperation(string op)
    {
      calculator.AddOperation(op);
      UpdateCalcBox(ParseCalc(calculator.CalculationStack));
    }

    private void Key1_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.One);

    private void Key2_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Two);

    private void Key3_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Three);

    private void Key4_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Four);

    private void Key5_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Five);

    private void Key6_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Six);

    private void Key7_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Seven);

    private void Key8_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Eight);

    private void Key9_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Nine);

    private void Key0_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Zero);

    private void KeyDot_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Dot);

    private void KeyOpenBracket_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.LeftBracket);

    private void KeyCloseBracket_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.RightBracket);

    private void KeyDivide_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Divide);

    private void KeyMultiply_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Multiply);

    private void KeySubtract_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Subtract);

    private void KeyAdd_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(OperationNames.Add);

    #endregion

    #region Visual Draw Instructions

    private string ParseCalc(string calculation)
    {
      return calculation
        .Replace(OperationNames.Divide, "÷")
        .Replace(OperationNames.Multiply, "×");
    }

    private string UnParseCalc(string calculation)
    {
      return calculation
        .Replace("÷", OperationNames.Divide)
        .Replace("×", OperationNames.Multiply);
    }

    private void AppendHistory(string text)
    {
      historyRichTextBox.AppendText(ParseCalc(text) + "\n");
      historyRichTextBox.SelectAll();
      historyRichTextBox.SelectionAlignment = HorizontalAlignment.Right;
    }

    private void UpdateCalcBox(string text, bool resetSelection = false)
    {
      _isUpdatingText = true;

      int caretPos = calcTextBox.SelectionStart;
      calcTextBox.Text = text;
      calcTextBox.SelectionStart = caretPos;

      if (resetSelection)
        calcTextBox.SelectionStart = calcTextBox.TextLength;

      _isUpdatingText = false;
    }

    #endregion

    #region Manual Input

    private void CalcTextBox_TextChanged(object sender, System.EventArgs e)
    {
      if (_isUpdatingText) return;

      FilterInput();

      calculator.SetCalculationStack(UnParseCalc(calcTextBox.Text));
    }

    private void FilterInput()
    {
      string parsed = ParseCalc(calcTextBox.Text);

      if (parsed != calcTextBox.Text)
      {
        UpdateCalcBox(parsed);
      }
    }

    private void calcTextBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
        KeyEquals_Click(sender, e);
    }

    #endregion

    #region ContextMenu

    private void InitializeContextMenu()
    {
      var contextMenu = new ContextMenuStrip();

      bool isRussian = System
        .Threading
        .Thread
        .CurrentThread
        .CurrentUICulture
        .TwoLetterISOLanguageName == "ru";

      var copyItem = new ToolStripMenuItem(
        isRussian ? "Копировать" : "Copy");

      var clearItem = new ToolStripMenuItem(
        isRussian ? "Очистить историю" : "Clear history");

      copyItem.Click += (s, e) =>
      {
        if (!string.IsNullOrEmpty(historyRichTextBox.SelectedText))
          Clipboard.SetText(historyRichTextBox.SelectedText);
      };

      clearItem.Click += (s, e) =>
      {
        historyRichTextBox.Clear();
      };

      contextMenu.Items.Add(copyItem);
      contextMenu.Items.Add(clearItem);

      historyRichTextBox.ContextMenuStrip = contextMenu;
    }

    #endregion
  }
}
