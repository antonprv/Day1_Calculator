// Created by Anton Piruev in 2026. 
// Any direct commercial use of derivative work is strictly prohibited.

using System.Windows.Forms;

namespace Calculator
{
  public partial class UI : Form
  {
    private readonly Logic.Calculator calculator;

    private bool _isUpdatingText = false;

    private string _lastAdded;

    public UI()
    {
      InitializeComponent();
      calculator = new Logic.Calculator();
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
      UpdateCalcBox(Names.Values.Zero);
      calculator.ClearLastCalculation();
      calculator.ClearCalculationStack();
    }

    private void KeyClearErase_Click(object sender, System.EventArgs e)
    {
      calculator.ClearLastCalculation();
      historyRichTextBox.Clear();
    }

    #endregion

    #region Add Names.Values To Stack 

    private void AddCalcOperation(string op)
    {
      calculator.AddOperation(op);
      UpdateCalcBox(ParseCalc(calculator.CalculationStack));
    }

    private void Key1_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.One);

    private void Key2_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Two);

    private void Key3_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Three);

    private void Key4_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Four);

    private void Key5_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Five);

    private void Key6_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Six);

    private void Key7_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Seven);

    private void Key8_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Eight);

    private void Key9_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Nine);

    private void Key0_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Values.Zero);

    #endregion

    #region Add Names.Symbols to Stack

    private void KeyDot_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Symbols.Dot);

    private void KeyOpenBracket_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Symbols.LeftBracket);

    private void KeyCloseBracket_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Symbols.RightBracket);

    #endregion

    #region Add Names.Operations To Stack

    private void KeyDivide_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Operations.Divide);

    private void KeyMultiply_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Operations.Multiply);

    private void KeySubtract_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Operations.Subtract);

    private void KeyAdd_Click(object sender, System.EventArgs e) =>
      AddCalcOperation(Names.Operations.Add);

    #endregion

    #region Visual Draw Instructions

    private string ParseCalc(string calculation)
    {
      return calculation
        .Replace(Names.Operations.Divide, "÷")
        .Replace(Names.Operations.Multiply, "×")
        .Replace(Names.Symbols.Dot, ",");
    }

    private string UnParseCalc(string calculation)
    {
      return calculation
        .Replace("÷", Names.Operations.Divide)
        .Replace("×", Names.Operations.Multiply)
        .Replace(",", Names.Symbols.Dot);
    }

    private void AppendHistory(string text)
    {
      string parsed = ParseCalc(text);

      if (parsed == _lastAdded) return;

      if (!Names.Operations.HasOperations(text)) return;

      historyRichTextBox.AppendText(parsed + "\n");
      historyRichTextBox.SelectAll();
      historyRichTextBox.SelectionAlignment = HorizontalAlignment.Right;

      _lastAdded = parsed;
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
