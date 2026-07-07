using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace Calculator_NETFramework_
{
    public partial class Form1 : Form
    {
        private CalculatorEngine engine;
        private ThemeManager themeManager;

        public Form1()
        {
            InitializeComponent();
            engine = new CalculatorEngine();
            themeManager = new ThemeManager();
        }

        #region Display
        private void Form1_Load(object sender, EventArgs e)
        {
            // Устанавливаем тему по умолчанию
            comboBoxTheme.SelectedIndex = 0;
            themeManager.ApplyTheme(this, comboBoxTheme.Text);

        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // Скрываем фокус с текстового поля при запуске формы
            ActiveControl = null;
        }

        #endregion

        #region Numbers
        private void NumberButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            engine.EnterDigit(button.Text);
            textDisplay.Text = engine.GetDisplay();
            textDisplay.SelectionStart = textDisplay.Text.Length;
            textDisplay.ScrollToCaret();
        }

        #endregion

        #region Operations


        private void buttonEqual_Click(object sender, EventArgs e)
        {
            if (!engine.CalculateEqual(out string display))
            {
                // либо нет операции, либо некорректное число
                if (engine.HasInvalidNumber)
                {
                    MessageBox.Show("Некорректное число");
                }
                else if (engine.DivisionByZero)
                {
                    MessageBox.Show("Деление на ноль невозможно!");
                }
                return;
            }

            textDisplay.Text = display;
            textDisplay.SelectionStart = textDisplay.Text.Length;
            textDisplay.ScrollToCaret();
        }

        private void OperationButton_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (!engine.SetOperation(button.Text, out string display))
            {
                if (engine.DivisionByZero)
                {
                    MessageBox.Show("Деление на ноль невозможно!");
                }
                else
                {
                    MessageBox.Show("Некорректное число");
                }
                return;
            }

            textDisplay.Text = display;
            textDisplay.SelectionStart = textDisplay.Text.Length;
            textDisplay.ScrollToCaret();
        }

        private void buttonDot_Click(object sender, EventArgs e)
        {
            engine.EnterDot();
            textDisplay.Text = engine.GetDisplay();
        }
        #endregion

        #region Other
        private void buttonClear_Click(object sender, EventArgs e)
        {
            engine.Clear();
            textDisplay.Text = "0";
        }

        // Сохраняем только результат вычислений
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.Filter = "Текстовый файл|*.txt";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(
                    saveDialog.FileName,
                    engine.GetResult().ToString(CultureInfo.InvariantCulture)
                );
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.Filter = "Текстовый файл|*.txt";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                string content = File.ReadAllText(openDialog.FileName);
                textDisplay.Text = content;

                if (!engine.LoadFromString(content))
                {
                    MessageBox.Show("Файл содержит некорректные данные");
                }
            }
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            string display = engine.Backspace();
            textDisplay.Text = display;
            textDisplay.SelectionStart = textDisplay.Text.Length;
            textDisplay.ScrollToCaret();
        }

        private void buttonPrecent_Click(object sender, EventArgs e)
        {
            engine.Percent();
            textDisplay.Text = engine.GetDisplay();
            textDisplay.SelectionStart = textDisplay.Text.Length;
            textDisplay.ScrollToCaret();
        }

        private void comboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            themeManager.ApplyTheme(this, comboBoxTheme.Text);
        }
        #endregion
    }
}
