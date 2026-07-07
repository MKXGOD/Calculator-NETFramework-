using System.Drawing;
using System.Windows.Forms;

namespace Calculator_NETFramework_
{
    internal class ThemeManager
    {
        public void ApplyTheme(Form form, string theme)
        {
            switch (theme)
            {
                case "Темная":
                    SetDarkTheme(form);
                    break;
                case "Светлая":
                    SetLightTheme(form);
                    break;
                default:
                    form.BackColor = Color.White;
                    form.ForeColor = Color.Black;
                    break;
            }
        }

        private Control FindControlRecursive(Control parent, string name)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Name == name) return c;
                Control found = FindControlRecursive(c, name);
                if (found != null) return found;
            }

            return null;
        }

        private void SetLightTheme(Form form)
        {
            form.BackColor = SystemColors.ControlLight;

            var textDisplay = FindControlRecursive(form, "textDisplay");
            if (textDisplay != null)
            {
                textDisplay.BackColor = SystemColors.ControlLight;
                textDisplay.ForeColor = SystemColors.ControlText;
            }

            var comboBox = FindControlRecursive(form, "comboBoxTheme");
            if (comboBox != null)
            {
                comboBox.BackColor = SystemColors.ControlLight;
                comboBox.ForeColor = SystemColors.ControlText;
            }

            var label = FindControlRecursive(form, "label1");
            if (label != null) label.ForeColor = SystemColors.ControlText;

            foreach (Control control in form.Controls)
            {
                if (control is Button button)
                {
                    try { button.FlatAppearance.BorderColor = SystemColors.ActiveBorder; } catch { }
                    button.ForeColor = SystemColors.ControlText;

                    if (button.Text == "=")
                    {
                        button.BackColor = SystemColors.ActiveCaption;
                    }
                    else if (IsOperationButton(button))
                    {
                        button.BackColor = SystemColors.ScrollBar;
                    }
                    else
                    {
                        button.BackColor = SystemColors.Control;
                    }
                }
            }
        }

        private void SetDarkTheme(Form form)
        {
            form.BackColor = SystemColors.WindowFrame;

            var textDisplay = FindControlRecursive(form, "textDisplay");
            if (textDisplay != null)
            {
                textDisplay.BackColor = SystemColors.WindowFrame;
                textDisplay.ForeColor = SystemColors.Control;
            }

            var comboBox = FindControlRecursive(form, "comboBoxTheme");
            if (comboBox != null)
            {
                comboBox.BackColor = SystemColors.WindowFrame;
                comboBox.ForeColor = SystemColors.Control;
            }

            var label = FindControlRecursive(form, "label1");
            if (label != null) label.ForeColor = SystemColors.Control;

            foreach (Control control in form.Controls)
            {
                if (control is Button button)
                {
                    button.ForeColor = SystemColors.Control;
                    try { button.FlatAppearance.BorderColor = SystemColors.GrayText; } catch { }

                    if (button.Text == "=")
                    {
                        button.BackColor = Color.Goldenrod;
                    }
                    else if (IsOperationButton(button))
                    {
                        button.BackColor = SystemColors.ControlDarkDark;
                    }
                    else
                    {
                        button.BackColor = SystemColors.ControlDark;
                    }
                }
            }
        }

        private bool IsOperationButton(Button button)
        {
            return button.Text == "+"
                || button.Text == "-"
                || button.Text == "*"
                || button.Text == "/"
                || button.Text == "C"
                || button.Text == "<-"
                || button.Text == "%"
                || button.Text == ".";
        }
    }
}
