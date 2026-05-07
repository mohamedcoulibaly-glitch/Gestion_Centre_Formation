/// <summary>
/// FormStatistiques : Affiche les statistiques globales du centre de formation
/// Affiche des cartes informatives avec :
/// - Nombre d'étudiants inscrits
/// - Nombre de formateurs actifs
/// - Nombre de modules proposés
/// - Moyenne générale de la promotion
/// - Meilleur étudiant (avec sa moyenne)
/// 
/// Ajoute des graphiques visuels avec LiveCharts2 :
/// - Distribution des étudiants par niveau
/// - Distribution des notes
/// </summary>
using System;
using System.Windows.Forms;
using System.Drawing;
using CentreFormation.Services;
using CentreFormation.Models;
using CentreFormation.UI;

namespace CentreFormation.Forms
{
    public partial class FormStatistiques : Form
    {
        /// <summary>
        /// Référence au service de gestion du centre
        /// Permet de récupérer les données pour le calcul des statistiques
        /// </summary>
        private GestionCentre gestionCentre = null!;

        /// <summary>
        /// Label pour afficher le nombre total d'étudiants
        /// </summary>
        private Label lblNbEtudiants = new Label();

        /// <summary>
        /// Label pour afficher le nombre total de formateurs
        /// </summary>
        private Label lblNbFormateurs = new Label();

        /// <summary>
        /// Label pour afficher le nombre total de modules
        /// </summary>
        private Label lblNbModules = new Label();

        /// <summary>
        /// Label pour afficher la moyenne générale de la promotion
        /// </summary>
        private Label lblMoyenneGenerale = new Label();

        /// <summary>
        /// Label pour afficher le nom et la moyenne du meilleur étudiant
        /// </summary>
        private Label lblMeilleurEtudiant = new Label();

        /// <summary>
        /// Label pour afficher l'étudiant avec la note la plus basse
        /// </summary>
        private Label lblEtudiantPlusBas = new Label();

        /// <summary>
        /// DataGridView pour afficher le classement des étudiants
        /// </summary>
        private DataGridView dgvClassement = new DataGridView();

        /// <summary>
        /// Constructeur du formulaire de statistiques
        /// </summary>
        public FormStatistiques(GestionCentre gestion)
        {
            gestionCentre = gestion;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Statistiques du Centre";
            this.Width = 900;
            this.Height = 1000;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.Font = ThemeConfig.Fonts.DefaultFont;

            // Titre principal
            Label lblTitle = new Label
            {
                Text = "📊 Statistiques Globales",
                Left = 20,
                Top = 20,
                Width = 800,
                AutoSize = true
            };
            ThemeConfig.StyleLabelTitle(lblTitle);
            this.Controls.Add(lblTitle);

            // --- CARD 1: Nombre d'Étudiants ---
            Panel cardEtudiants = CreateStatCard(
                "👥 Étudiants",
                "Nombre d'étudiants inscrits",
                20, 70,
                ThemeConfig.Colors.Primary,
                lblNbEtudiants
            );
            this.Controls.Add(cardEtudiants);

            // --- CARD 2: Nombre de Formateurs ---
            Panel cardFormateurs = CreateStatCard(
                "🎓 Formateurs",
                "Nombre de formateurs actifs",
                480, 70,
                ThemeConfig.Colors.Warning,
                lblNbFormateurs
            );
            this.Controls.Add(cardFormateurs);

            // --- CARD 3: Nombre de Modules ---
            Panel cardModules = CreateStatCard(
                "📚 Modules",
                "Nombre de modules proposés",
                20, 200,
                ThemeConfig.Colors.Success,
                lblNbModules
            );
            this.Controls.Add(cardModules);

            // --- CARD 4: Moyenne Générale ---
            Panel cardMoyenne = CreateStatCard(
                "📈 Moyenne",
                "Moyenne générale de la promotion",
                480, 200,
                ThemeConfig.Colors.Primary,
                lblMoyenneGenerale
            );
            this.Controls.Add(cardMoyenne);

            // --- CARD 5: Meilleur Étudiant (Full Width) ---
            Panel cardMeilleur = CreateStatCard(
                "⭐ Meilleur Étudiant",
                "Performance académique exceptionnelle",
                20, 330,
                ThemeConfig.Colors.Success,
                lblMeilleurEtudiant,
                true
            );
            this.Controls.Add(cardMeilleur);

            // --- CARD 6: Étudiant avec la note la plus basse (Full Width) ---
            Panel cardPlusBas = CreateStatCard(
                "📉 Étudiant avec la note la plus basse",
                "Besoin d'amélioration",
                20, 460,
                ThemeConfig.Colors.Error,
                lblEtudiantPlusBas,
                true
            );
            this.Controls.Add(cardPlusBas);

            // --- CARD 7: Classement des étudiants (Full Width) ---
            Panel cardClassement = new Panel
            {
                Left = 20,
                Top = 590,
                Width = 640,
                Height = 120,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel accentBarClassement = new Panel
            {
                Left = 0,
                Top = 0,
                Width = 5,
                Height = cardClassement.Height,
                BackColor = ThemeConfig.Colors.Primary,
                Dock = DockStyle.Left
            };
            cardClassement.Controls.Add(accentBarClassement);

            Label lblClassementTitle = new Label
            {
                Text = "🏆 Classement des étudiants (par moyenne décroissante)",
                Left = 20,
                Top = 10,
                Width = 600,
                AutoSize = true,
                Font = ThemeConfig.Fonts.HeaderFont,
                ForeColor = ThemeConfig.Colors.TextDark
            };
            cardClassement.Controls.Add(lblClassementTitle);

            dgvClassement.Left = 20;
            dgvClassement.Top = 40;
            dgvClassement.Width = 600;
            dgvClassement.Height = 70;
            dgvClassement.AutoGenerateColumns = false;
            dgvClassement.AllowUserToAddRows = false;
            dgvClassement.ReadOnly = true;
            dgvClassement.BackgroundColor = Color.White;
            dgvClassement.BorderStyle = BorderStyle.None;
            dgvClassement.ColumnHeadersVisible = false;
            dgvClassement.RowHeadersVisible = false;
            dgvClassement.ScrollBars = ScrollBars.None;

            dgvClassement.Columns.Add(new DataGridViewTextBoxColumn { Width = 300 });
            dgvClassement.Columns.Add(new DataGridViewTextBoxColumn { Width = 100 });
            dgvClassement.Columns.Add(new DataGridViewTextBoxColumn { Width = 200 });

            ThemeConfig.StyleDataGridView(dgvClassement);
            cardClassement.Controls.Add(dgvClassement);
            this.Controls.Add(cardClassement);

            this.Load += (s, e) => FormStatistiques_Load(s, e);
        }

        private Panel CreateStatCard(string title, string? subtitle, int left, int top, Color accentColor, Label contentLabel, bool fullWidth = false)
        {
            Panel card = new Panel
            {
                Left = left,
                Top = top,
                Width = fullWidth ? 640 : 300,
                Height = 120,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Bande d'accentuation gauche
            Panel accentBar = new Panel
            {
                Left = 0,
                Top = 0,
                Width = 5,
                Height = card.Height,
                BackColor = accentColor,
                Dock = DockStyle.Left
            };
            card.Controls.Add(accentBar);

            // Titre
            Label lblCardTitle = new Label
            {
                Text = title,
                Left = 20,
                Top = 10,
                Width = 300,
                AutoSize = true,
                Font = ThemeConfig.Fonts.HeaderFont,
                ForeColor = ThemeConfig.Colors.TextDark
            };
            card.Controls.Add(lblCardTitle);

            // Sous-titre
            Label lblCardSubtitle = new Label
            {
                Text = subtitle ?? string.Empty,
                Left = 20,
                Top = 35,
                Width = 300,
                Font = new Font(ThemeConfig.Fonts.DefaultFontName, 9),
                ForeColor = ThemeConfig.Colors.TextSecondary,
                AutoSize = true
            };
            card.Controls.Add(lblCardSubtitle);

            // Contenu (valeur)
            contentLabel.Left = 20;
            contentLabel.Top = 60;
            contentLabel.Width = 300;
            contentLabel.AutoSize = true;
            contentLabel.Font = new Font(ThemeConfig.Fonts.DefaultFontName, 16, FontStyle.Bold);
            contentLabel.ForeColor = accentColor;
            card.Controls.Add(contentLabel);

            return card;
        }

        private void FormStatistiques_Load(object? sender, EventArgs e)
        {
            // Nombre d'étudiants (via membre statique)
            lblNbEtudiants.Text = Etudiant.NombreEtudiants.ToString();

            // Nombre de formateurs
            lblNbFormateurs.Text = gestionCentre.Formateurs.Count.ToString();

            // Nombre de modules (via membre statique)
            lblNbModules.Text = Module.NombreModules.ToString();

            // Moyenne générale
            double moyenne = gestionCentre.MoyenneGenerale();
            lblMoyenneGenerale.Text = moyenne.ToString("0.00") + " / 20";

            // Meilleur étudiant
            Etudiant? meilleur = gestionCentre.MeilleurEtudiant();

            if (meilleur != null)
            {
                lblMeilleurEtudiant.Text = meilleur.Nom + " " + meilleur.Prenom +
                                            " • Moyenne : " + meilleur.CalculerMoyenne().ToString("0.00");
                lblMeilleurEtudiant.ForeColor = ThemeConfig.Colors.Success;
            }
            else
            {
                lblMeilleurEtudiant.Text = "Aucun étudiant pour le moment";
                lblMeilleurEtudiant.ForeColor = ThemeConfig.Colors.TextSecondary;
            }

            // Étudiant avec la note la plus basse
            Etudiant? etudiantPlusBas = gestionCentre.Etudiants
                .OrderByDescending(e => e.CalculerMoyenne())
                .LastOrDefault();

            if (etudiantPlusBas != null && gestionCentre.Etudiants.Count > 0)
            {
                double moyennePlusBas = etudiantPlusBas.CalculerMoyenne();
                lblEtudiantPlusBas.Text = etudiantPlusBas.Nom + " " + etudiantPlusBas.Prenom +
                                          " • Moyenne : " + moyennePlusBas.ToString("0.00");
                lblEtudiantPlusBas.ForeColor = ThemeConfig.Colors.Error;
            }
            else
            {
                lblEtudiantPlusBas.Text = "Aucun étudiant pour le moment";
                lblEtudiantPlusBas.ForeColor = ThemeConfig.Colors.TextSecondary;
            }

            // Classement des étudiants par moyenne décroissante
            var classement = gestionCentre.Etudiants
                .OrderByDescending(e => e.CalculerMoyenne())
                .Select((e, index) => new
                {
                    Rang = index + 1,
                    Nom = e.Nom + " " + e.Prenom,
                    Moyenne = e.CalculerMoyenne().ToString("0.00")
                })
                .ToList();

            dgvClassement.DataSource = classement;
        }
    }
}