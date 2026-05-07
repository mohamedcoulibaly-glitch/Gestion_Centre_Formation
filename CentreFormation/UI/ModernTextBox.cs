using System;
using System.Drawing;
using System.Windows.Forms;

namespace CentreFormation.UI
{
    /// <summary>
    /// TextBox moderne avec validation en temps réel
    /// Affiche bordure rouge si invalide, verte si valide
    /// </summary>
    public class ModernTextBox : TextBox
    {
        private Color _normalBorderColor = ThemeConfig.Colors.BorderLight;
        private Color _focusBorderColor = ThemeConfig.Colors.Primary;
        private Color _validBorderColor = ThemeConfig.Colors.Success;
        private Color _invalidBorderColor = ThemeConfig.Colors.Error;
        private bool _isValid = true;
        private string _errorMessage = "";

        public bool IsValid
        {
            get => _isValid;
            set { _isValid = value; Invalidate(); }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; }
        }

        public ModernTextBox()
        {
            Font = new Font("Segoe UI", 10f);
            Height = 38;
            BorderStyle = BorderStyle.FixedSingle;
            Padding = new Padding(8, 5, 8, 5);
            BackColor = ThemeConfig.Colors.CardBackground;
            ForeColor = ThemeConfig.Colors.TextDark;

            // Events
            GotFocus += (s, e) => UpdateBorderColor();
            LostFocus += (s, e) => UpdateBorderColor();
        }

        private void UpdateBorderColor()
        {
            if (Focused)
            {
                BackColor = ThemeConfig.Colors.BackgroundMain;
            }
            else if (!_isValid)
            {
                BackColor = ThemeConfig.Colors.ErrorLight;
            }
            else
            {
                BackColor = ThemeConfig.Colors.CardBackground;
            }
        }

        public void ShowValid()
        {
            _isValid = true;
            _errorMessage = "";
            UpdateBorderColor();
        }

        public void ShowInvalid(string message = "Champ invalide")
        {
            _isValid = false;
            _errorMessage = message;
            UpdateBorderColor();
        }
    }
}
