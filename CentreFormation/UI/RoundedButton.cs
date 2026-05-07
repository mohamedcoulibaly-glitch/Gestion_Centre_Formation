using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace CentreFormation.UI
{
    /// <summary>
    /// Bouton custom avec coins arrondis et animations
    /// Remplace les buttons carrés standards WinForms
    /// </summary>
    public class RoundedButton : Button
    {
        // Propriétés privées
        private int _borderRadius = 8;
        private Color _normalColor = Color.FromArgb(0, 102, 204);      // Bleu primaire
        private Color _hoverColor = Color.FromArgb(0, 82, 163);        // Bleu hover
        private Color _pressedColor = Color.FromArgb(0, 52, 103);      // Bleu pressed
        private Color _borderColor = Color.Transparent;
        private int _borderWidth = 0;
        private bool _isHovering = false;
        private bool _isPressed = false;

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public Color NormalColor
        {
            get => _normalColor;
            set { _normalColor = value; Invalidate(); }
        }

        public Color HoverColor
        {
            get => _hoverColor;
            set { _hoverColor = value; Invalidate(); }
        }

        public Color PressedColor
        {
            get => _pressedColor;
            set { _pressedColor = value; Invalidate(); }
        }

        public Color BorderColor
        {
            get => _borderColor;
            set { _borderColor = value; Invalidate(); }
        }

        public int BorderWidth
        {
            get => _borderWidth;
            set { _borderWidth = value; Invalidate(); }
        }

        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            ForeColor = Color.White;
            Height = 40;
            Cursor = Cursors.Hand;

            MouseEnter += (s, e) => { _isHovering = true; Invalidate(); };
            MouseLeave += (s, e) => { _isHovering = false; Invalidate(); };
            MouseDown += (s, e) => { _isPressed = true; Invalidate(); };
            MouseUp += (s, e) => { _isPressed = false; Invalidate(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Déterminer la couleur actuelle
            Color currentColor = _isPressed ? _pressedColor : (_isHovering ? _hoverColor : _normalColor);

            // Créer le chemin arrondi
            GraphicsPath path = GetRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _borderRadius);

            // Remplir le bouton
            using (SolidBrush brush = new SolidBrush(currentColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            // Dessiner la bordure si spécifiée
            if (_borderWidth > 0)
            {
                using (Pen pen = new Pen(_borderColor, _borderWidth))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }

            // Dessiner le texte
            StringFormat sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            using (SolidBrush textBrush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, textBrush, new Rectangle(0, 0, Width, Height), sf);
            }

            path.Dispose();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Ne pas dessiner le background par défaut
        }

        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.X + rect.Width - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.X + rect.Width - diameter, rect.Y + rect.Height - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Y + rect.Height - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();

            return path;
        }
    }
}
