using System;
using System.Drawing;
using System.Windows.Forms;

namespace CentreFormation.UI
{
    /// <summary>
    /// Toast notifications (style Android/iOS)
    /// Apparaît en bas-droit, disparaît automatiquement après 3 secondes
    /// </summary>
    public partial class ToastNotification : Form
    {
        private System.Windows.Forms.Timer _dismissTimer;
        private NotificationType _type;

        public enum NotificationType
        {
            Success,
            Error,
            Warning,
            Info
        }

        public ToastNotification(string message, NotificationType type = NotificationType.Info)
        {
            _type = type;
            InitializeComponent();
            this.Text = "";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.Width = 350;
            this.Height = 80;
            this.TopMost = true;
            this.ShowInTaskbar = false;

            // Positionner en bas-droit
            Screen screen = Screen.PrimaryScreen ?? Screen.AllScreens[0];
            this.Left = screen.Bounds.Right - this.Width - 20;
            this.Top = screen.Bounds.Bottom - this.Height - 20;

            // Couleur de fond selon le type
            Color bgColor = GetBackgroundColor();
            Color textColor = GetTextColor();

            this.BackColor = bgColor;

            // Créer les contrôles
            Label iconLabel = new Label
            {
                Text = GetIcon(),
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = textColor,
                Left = 15,
                Top = 12,
                Width = 50,
                Height = 50,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label messageLabel = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 10),
                ForeColor = textColor,
                Left = 70,
                Top = 15,
                Width = 270,
                Height = 50,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft
            };

            this.Controls.Add(iconLabel);
            this.Controls.Add(messageLabel);

            // Timer de disparition automatique
            _dismissTimer = new System.Windows.Forms.Timer();
            _dismissTimer.Interval = 3000; // 3 secondes
            _dismissTimer.Tick += (s, e) =>
            {
                _dismissTimer.Stop();
                this.Close();
            };
            _dismissTimer.Start();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(350, 80);
            this.ResumeLayout(false);
        }

        private Color GetBackgroundColor()
        {
            return _type switch
            {
                NotificationType.Success => ThemeConfig.Colors.SuccessLight,
                NotificationType.Error => ThemeConfig.Colors.ErrorLight,
                NotificationType.Warning => ThemeConfig.Colors.WarningLight,
                _ => ThemeConfig.Colors.InfoLight
            };
        }

        private Color GetTextColor()
        {
            return _type switch
            {
                NotificationType.Success => ThemeConfig.Colors.Success,
                NotificationType.Error => ThemeConfig.Colors.Error,
                NotificationType.Warning => ThemeConfig.Colors.Warning,
                _ => ThemeConfig.Colors.Info
            };
        }

        private string GetIcon()
        {
            return _type switch
            {
                NotificationType.Success => "✓",
                NotificationType.Error => "✕",
                NotificationType.Warning => "⚠",
                _ => "ℹ"
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Ajouter une bordure subtile
            Color borderColor = GetTextColor();
            using (Pen pen = new Pen(borderColor, 2))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            }
        }
    }

    // Helper class pour afficher les toasts facilement
    public static class Toast
    {
        public static void ShowSuccess(string message)
        {
            var toast = new ToastNotification(message, ToastNotification.NotificationType.Success);
            toast.Show();
        }

        public static void ShowError(string message)
        {
            var toast = new ToastNotification(message, ToastNotification.NotificationType.Error);
            toast.Show();
        }

        public static void ShowWarning(string message)
        {
            var toast = new ToastNotification(message, ToastNotification.NotificationType.Warning);
            toast.Show();
        }

        public static void ShowInfo(string message)
        {
            var toast = new ToastNotification(message, ToastNotification.NotificationType.Info);
            toast.Show();
        }
    }
}
