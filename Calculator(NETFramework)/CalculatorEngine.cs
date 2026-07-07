using System;
using System.Globalization;

namespace Calculator_NETFramework_
{
    internal class CalculatorEngine
    {
        private double firstNumber = 0;
        private string operation = "";

        private string currentNumber = "";
        private string expression = "";

        private bool isNewNumber = true;
        private bool isResultShown = false;

        public bool HasInvalidNumber { get; private set; }
        public bool DivisionByZero { get; private set; }

        public string GetDisplay() => string.IsNullOrEmpty(expression) ? "0" : expression;

        public double GetResult() => firstNumber;

        public void EnterDigit(string digit)
        {
            if (isResultShown)
            {
                Clear();
            }

            if (isNewNumber)
            {
                currentNumber = "";
                isNewNumber = false;
            }

            currentNumber += digit;
            expression += digit;
        }

        public void EnterDot()
        {
            if (isNewNumber)
            {
                currentNumber = "";
                isNewNumber = false;
            }

            if (!currentNumber.Contains("."))
            {
                currentNumber += ".";
                expression += ".";
            }
        }

        private void Calculate(double secondNumber)
        {
            DivisionByZero = false;

            switch (operation)
            {
                case "+":
                    firstNumber += secondNumber;
                    break;
                case "-":
                    firstNumber -= secondNumber;
                    break;
                case "*":
                    firstNumber *= secondNumber;
                    break;
                case "/":
                    if (secondNumber == 0)
                    {
                        DivisionByZero = true;
                        Clear();
                        return;
                    }

                    firstNumber /= secondNumber;
                    break;
            }

            expression = firstNumber.ToString("G15", CultureInfo.InvariantCulture);
        }

        public bool SetOperation(string op, out string display)
        {
            HasInvalidNumber = false;
            DivisionByZero = false;
            display = GetDisplay();

            if (isNewNumber && operation != "")
            {
                operation = op;
                return true;
            }

            if (!double.TryParse(currentNumber, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                HasInvalidNumber = true;
                return false;
            }

            if (operation == "")
            {
                firstNumber = number;
            }
            else
            {
                Calculate(number);
                if (DivisionByZero)
                {
                    display = GetDisplay();
                    return false;
                }
            }

            operation = op;
            expression += op;
            currentNumber = "";
            isNewNumber = true;
            isResultShown = false;

            display = expression;
            return true;
        }

        public bool CalculateEqual(out string display)
        {
            HasInvalidNumber = false;
            DivisionByZero = false;
            display = GetDisplay();

            if (operation == "" || isNewNumber)
            {
                return false;
            }

            if (!double.TryParse(currentNumber, NumberStyles.Any, CultureInfo.InvariantCulture, out double secondNumber))
            {
                HasInvalidNumber = true;
                return false;
            }

            Calculate(secondNumber);
            if (DivisionByZero)
            {
                display = GetDisplay();
                return false;
            }

            currentNumber = "";
            isNewNumber = true;
            isResultShown = true;

            display = expression;
            return true;
        }

        public string Backspace()
        {
            // Если сейчас показывается результат и пользователь нажал <-,
            // можно очистить последний символ результата
            if (isNewNumber)
            {
                expression = GetDisplay();
                currentNumber = expression;
                isNewNumber = false;
            }

            if (string.IsNullOrEmpty(currentNumber))
            {
                isNewNumber = true;
                return "0";
            }

            char removedChar = currentNumber[currentNumber.Length - 1];
            currentNumber = currentNumber.Remove(currentNumber.Length - 1);

            if (expression.Length > 0)
            {
                expression = expression.Remove(expression.Length - 1);
            }

            if (removedChar == '+' || removedChar == '-' || removedChar == '*' || removedChar == '/')
            {
                operation = "";
                currentNumber = firstNumber.ToString(CultureInfo.InvariantCulture);
            }

            if (currentNumber.Length == 0)
            {
                isNewNumber = true;
                return "0";
            }

            return expression;
        }

        public void Percent()
        {
            if (!double.TryParse(currentNumber, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                return;
            }

            double percentValue;

            if (operation == "+" || operation == "-")
            {
                percentValue = firstNumber * number / 100.0;
            }
            else
            {
                percentValue = number / 100.0;
            }

            currentNumber = percentValue.ToString("G15", CultureInfo.InvariantCulture);

            string oldNumber = number.ToString(CultureInfo.InvariantCulture);
            int lastIndex = expression.Length - oldNumber.Length;

            if (lastIndex >= 0)
            {
                expression = expression.Substring(0, lastIndex) + currentNumber;
            }
        }

        public void Clear()
        {
            firstNumber = 0;
            operation = "";
            currentNumber = "";
            expression = "";
            isNewNumber = true;
            isResultShown = false;
            HasInvalidNumber = false;
            DivisionByZero = false;
        }

        public bool LoadFromString(string text)
        {
            if (double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            {
                Clear();
                firstNumber = result;
                expression = text;
                isNewNumber = true;
                return true;
            }

            return false;
        }
    }
}
