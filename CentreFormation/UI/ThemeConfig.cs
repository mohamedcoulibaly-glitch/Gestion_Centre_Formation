using System.Drawing;
using System.Windows.Forms;

namespace CentreFormation.UI
{
    /// <summary>
    /// Configuration centralisée du thème et du design de l'application
    /// Respecte la charte graphique moderne définie par les spécifications
    /// </summary>
    public static class ThemeConfig
    {
        // === PALETTE DE COULEURS MODERNE (2026) ===
        public static class Colors
        {
            // PRIMARY - Bleu professionnel saturé (Confiance, Technologie)
            public static readonly Color Primary = ColorTranslator.FromHtml("#0066CC");
            public static readonly Color PrimaryHover = ColorTranslator.FromHtml("#0052A3");
            public static readonly Color PrimaryLight = ColorTranslator.FromHtml("#E8F4FF");
            
            // SECONDARY - Violet moderne (Créativité, Innovation)
            public static readonly Color Secondary = ColorTranslator.FromHtml("#6C63FF");
            public static readonly Color SecondaryLight = ColorTranslator.FromHtml("#F0ECFF");
            
            // SEMANTIC COLORS
            public static readonly Color Success = ColorTranslator.FromHtml("#06A77D");          // Vert professionnel
            public static readonly Color SuccessLight = ColorTranslator.FromHtml("#E3F9F0");
            public static readonly Color SuccessHover = ColorTranslator.FromHtml("#058966");
            
            public static readonly Color Warning = ColorTranslator.FromHtml("#FF8C00");          // Orange moderne
            public static readonly Color WarningLight = ColorTranslator.FromHtml("#FFF4E6");
            public static readonly Color WarningHover = ColorTranslator.FromHtml("#E67E00");
            
            public static readonly Color Error = ColorTranslator.FromHtml("#E63946");            // Rouge moderne
            public static readonly Color ErrorLight = ColorTranslator.FromHtml("#FFE8EC");
            public static readonly Color ErrorHover = ColorTranslator.FromHtml("#D62828");
            
            public static readonly Color Info = ColorTranslator.FromHtml("#2196F3");             // Bleu info
            public static readonly Color InfoLight = ColorTranslator.FromHtml("#E3F2FD");
            
            // BACKGROUNDS & SURFACES
            public static readonly Color BackgroundMain = ColorTranslator.FromHtml("#F5F7FA");    // Gris pro
            public static readonly Color BackgroundSecondary = ColorTranslator.FromHtml("#FFFFFF");
            public static readonly Color CardBackground = ColorTranslator.FromHtml("#FFFFFF");
            public static readonly Color SidebarDark = ColorTranslator.FromHtml("#1A1F36");       // Noir-bleu
            public static readonly Color SidebarHover = ColorTranslator.FromHtml("#2A3450");
            
            // TEXT
            public static readonly Color TextDark = ColorTranslator.FromHtml("#1A1F36");          // Texte principal
            public static readonly Color TextSecondary = ColorTranslator.FromHtml("#6B7280");     // Texte secondaire
            public static readonly Color TextLight = ColorTranslator.FromHtml("#FFFFFF");         // Texte sur couleur
            public static readonly Color TextDisabled = ColorTranslator.FromHtml("#D1D5DB");      // Désactivé
            
            // BORDERS & DIVIDERS
            public static readonly Color BorderLight = ColorTranslator.FromHtml("#E5E7EB");
            public static readonly Color BorderMedium = ColorTranslator.FromHtml("#D1D5DB");
            public static readonly Color DividerColor = ColorTranslator.FromHtml("#E5E7EB");
            
            // SHADOWS (Pour utilisation avec transparency)
            public static readonly Color ShadowColor = Color.Black;                        // Opacity: 0.08-0.15
        }

        // === POLICES ===
        public static class Fonts
        {
            public static string DefaultFontName = "Segoe UI";
            public static float DefaultFontSize = 10f;
            public static float TitleFontSize = 14f;
            public static float HeaderFontSize = 12f;

            public static Font DefaultFont => new Font(DefaultFontName, DefaultFontSize);
            public static Font BoldFont => new Font(DefaultFontName, DefaultFontSize, FontStyle.Bold);
            public static Font TitleFont => new Font(DefaultFontName, TitleFontSize, FontStyle.Bold);
            public static Font HeaderFont => new Font(DefaultFontName, HeaderFontSize, FontStyle.Bold);
        }

        // === DIMENSIONS ===
        public static class Sizes
        {
            public const int DefaultMargin = 15;
            public const int ControlHeight = 35;
            public const int ButtonHeight = 40;
            public const int DataGridRowHeight = 40;
            public const int CornerRadius = 8;
            public const int BorderWidth = 1;
        }

        // === FONCTIONS UTILITAIRES POUR CONFIGURER LES CONTRÔLES ===

        /// <summary>
        /// Configure un bouton avec le style primaire (Bleu Ciel)
        /// </summary>
        public static void StyleButtonPrimary(Button button)
        {
            button.BackColor = Colors.Primary;
            button.ForeColor = Colors.TextLight;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = Fonts.BoldFont;
            button.Height = Sizes.ButtonHeight;
            button.Cursor = Cursors.Hand;
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(100, Colors.Primary);
            button.MouseLeave += (s, e) => button.BackColor = Colors.Primary;
        }

        /// <summary>
        /// Configure un bouton avec le style succès (Vert)
        /// </summary>
        public static void StyleButtonSuccess(Button button)
        {
            button.BackColor = Colors.Success;
            button.ForeColor = Colors.TextLight;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = Fonts.BoldFont;
            button.Height = Sizes.ButtonHeight;
            button.Cursor = Cursors.Hand;
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(100, Colors.Success);
            button.MouseLeave += (s, e) => button.BackColor = Colors.Success;
        }

        /// <summary>
        /// Configure un bouton avec le style avertissement (Jaune)
        /// </summary>
        public static void StyleButtonWarning(Button button)
        {
            button.BackColor = Colors.Warning;
            button.ForeColor = Colors.TextDark;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = Fonts.BoldFont;
            button.Height = Sizes.ButtonHeight;
            button.Cursor = Cursors.Hand;
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(200, Colors.Warning);
            button.MouseLeave += (s, e) => button.BackColor = Colors.Warning;
        }

        /// <summary>
        /// Configure un bouton avec le style secondaire (Gris clair)
        /// </summary>
        public static void StyleButtonSecondary(Button button)
        {
            button.BackColor = Color.LightGray;
            button.ForeColor = Colors.TextDark;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = Fonts.BoldFont;
            button.Height = Sizes.ButtonHeight;
            button.Cursor = Cursors.Hand;
            button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(200, Color.LightGray);
            button.MouseLeave += (s, e) => button.BackColor = Color.LightGray;
        }

        /// <summary>
        /// Configure un DataGridView avec le style moderne et professionnel
        /// </summary>
        public static void StyleDataGridView(DataGridView dgv)
        {
            // Couleurs générales
            dgv.BackgroundColor = Colors.CardBackground;
            dgv.ForeColor = Colors.TextDark;
            dgv.GridColor = Colors.BorderLight;

            // Headers élégants
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Colors.Primary,
                ForeColor = Colors.TextLight,
                Font = Fonts.HeaderFont,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(10, 5, 10, 5),
                SelectionBackColor = Colors.PrimaryHover
            };

            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Lignes alternées pour meilleure lisibilité
            dgv.RowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Colors.CardBackground,
                ForeColor = Colors.TextDark,
                Font = Fonts.DefaultFont,
                SelectionBackColor = Colors.PrimaryLight,
                SelectionForeColor = Colors.TextDark,
                Padding = new Padding(5)
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Colors.BackgroundMain,
                ForeColor = Colors.TextDark,
                Font = Fonts.DefaultFont,
                SelectionBackColor = Colors.PrimaryLight,
                SelectionForeColor = Colors.TextDark
            };

            // Hauteur des lignes
            dgv.RowTemplate.Height = 40;

            // Bordures minimalistes
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.AdvancedCellBorderStyle.Left = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedCellBorderStyle.Right = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedCellBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            dgv.AdvancedCellBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.Single;

            // Configuration générale
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Font = Fonts.DefaultFont;
        }

        /// <summary>
        /// Configure un GroupBox avec le style moderne
        /// </summary>
        public static void StyleGroupBox(GroupBox groupBox)
        {
            groupBox.ForeColor = Colors.Primary;
            groupBox.Font = Fonts.HeaderFont;
            groupBox.Padding = new Padding(Sizes.DefaultMargin);
        }

        /// <summary>
        /// Configure un TextBox avec les styles de focus
        /// </summary>
        public static void StyleTextBox(TextBox textBox)
        {
            textBox.Font = Fonts.DefaultFont;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Height = Sizes.ControlHeight;
        }

        /// <summary>
        /// Configure un Label comme titre
        /// </summary>
        public static void StyleLabelTitle(Label label)
        {
            label.Font = Fonts.TitleFont;
            label.ForeColor = Colors.TextDark;
            label.AutoSize = true;
        }

        /// <summary>
        /// Configure un Label comme titre de section
        /// </summary>
        public static void StyleLabelSubtitle(Label label)
        {
            label.Font = Fonts.HeaderFont;
            label.ForeColor = Colors.Primary;
            label.AutoSize = true;
        }

        /// <summary>
        /// Configure un Panel pour le design "Card"
        /// </summary>
        public static void StyleCardPanel(Panel panel)
        {
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Padding = new Padding(Sizes.DefaultMargin);
        }
    }
}
