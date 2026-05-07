/// <summary>
/// FormAjoutFormateur : Formulaire pour ajouter ou modifier un formateur
/// Peut fonctionner en deux modes :
/// - Mode Création : ajouter un nouveau formateur
/// - Mode Édition : modifier un formateur existant
/// 
/// Champs gérés :
/// - Informations personnelles : ID, Nom, Prénom, Âge
/// - Informations professionnelles : Spécialité, Module enseigné, Salaire
/// </summary>
using System;
using System.Windows.Forms;
using System.Drawing;
using CentreFormation.Services;
using CentreFormation.Models;
using CentreFormation.UI;

namespace CentreFormation.Forms
{
    public partial class FormAjoutFormateur : Form
    {
        /// <summary>
        /// Référence au service de gestion du centre
        /// Permet de sauvegarder le formateur dans les données persistantes
        /// </summary>
        private GestionCentre _gestion;

        /// <summary>
        /// Formateur créé ou modifié
        /// Null pour les nouveaux formateurs, contient l'objet pour l'édition
        /// </summary>
        public Formateur? FormateurCree { get; private set; } = null;

        /// <summary>
        /// Référence au formateur en cours de modification (mode édition)
        /// </summary>
        private Formateur? _formateurEdite = null;

        /// <summary>
        /// Indicateur du mode de fonctionnement
        /// true = mode édition, false = mode création
        /// </summary>
        private bool _modeEdition = false;

        // === CONTRÔLES MODERNES ===
        /// <summary>
        /// TextBox pour l'ID (auto-généré et lecture seule en création)
        /// </summary>
        private ModernTextBox txtId = new ModernTextBox();

        /// <summary>
        /// TextBox moderne pour le nom du formateur
        /// </summary>
        private ModernTextBox txtNom = new ModernTextBox();

        /// <summary>
        /// TextBox moderne pour le prénom
        /// </summary>
        private ModernTextBox txtPrenom = new ModernTextBox();

        /// <summary>
        /// Contrôle numérique pour l'âge
        /// </summary>
        private NumericUpDown nudAge = new NumericUpDown();

        /// <summary>
        /// TextBox pour la spécialité professionnelle
        /// Exemple: "Développement Web", "Bases de Données"
        /// </summary>
        private ModernTextBox txtSpecialite = new ModernTextBox();

        /// <summary>
        /// TextBox pour le module enseigné
        /// </summary>
        private ModernTextBox txtModule = new ModernTextBox();

        /// <summary>
        /// Contrôle numérique pour le salaire mensuel
        /// Accepte les décimales (ex: 1500.50)
        /// </summary>
        private NumericUpDown nudSalaire = new NumericUpDown();

        /// <summary>
        /// Constructeur mode création
        /// Lance le formulaire pour créer un nouveau formateur
        /// Auto-génère un nouvel ID
        /// </summary>
        public FormAjoutFormateur(GestionCentre gestion)
        {
            _gestion = gestion;
            _modeEdition = false;
            InitializeComponent();
            txtId.Text = _gestion.NouvelIdFormateur().ToString();
            txtId.ReadOnly = true;
        }

        /// <summary>
        /// Constructeur mode édition
        /// Charge un formateur existant et permet sa modification
        /// </summary>
        public FormAjoutFormateur(GestionCentre gestion, Formateur formateur)
        {
            _gestion = gestion;
            _formateurEdite = formateur;
            _modeEdition = true;
            InitializeComponent();
            ChargerDonnees();
        }

        /// <summary>
        /// Charge les données du formateur dans les champs du formulaire
        /// Utilisé en mode édition
        /// </summary>
        private void ChargerDonnees()
        {
            if (_modeEdition && _formateurEdite != null)
            {
                this.Text = "Modifier un formateur";
                txtId.Text = _formateurEdite.Id.ToString();
                txtNom.Text = _formateurEdite.Nom;
                txtPrenom.Text = _formateurEdite.Prenom;
                nudAge.Value = _formateurEdite.Age;
                txtSpecialite.Text = _formateurEdite.Specialite;
                txtModule.Text = _formateurEdite.ModuleEnseigne;
                nudSalaire.Value = (decimal)_formateurEdite.Salaire;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Ajouter un formateur";
            this.Width = 550;
            this.Height = 550;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.Font = ThemeConfig.Fonts.DefaultFont;

            // Titre
            Label lblTitle = new Label
            {
                Text = "Enregistrer un Nouveau Formateur",
                Left = 20,
                Top = 20,
                Width = 400,
                AutoSize = true
            };
            ThemeConfig.StyleLabelTitle(lblTitle);
            this.Controls.Add(lblTitle);

            // GroupBox pour les informations personnelles
            GroupBox gbPersonnelles = new GroupBox
            {
                Text = "Informations Personnelles",
                Left = 20,
                Top = 60,
                Width = 500,
                Height = 180
            };
            ThemeConfig.StyleGroupBox(gbPersonnelles);

            // ID
            Label lblId = new Label { Text = "ID :", Left = 20, Top = 35, Width = 80, AutoSize = true };
            lblId.Font = ThemeConfig.Fonts.DefaultFont;
            txtId.Left = 130;
            txtId.Top = 32;
            txtId.Width = 340;

            // Nom
            Label lblNom = new Label { Text = "Nom :", Left = 20, Top = 75, Width = 80, AutoSize = true };
            lblNom.Font = ThemeConfig.Fonts.DefaultFont;
            txtNom.Left = 130;
            txtNom.Top = 72;
            txtNom.Width = 340;

            // Prénom
            Label lblPrenom = new Label { Text = "Prénom :", Left = 20, Top = 115, Width = 80, AutoSize = true };
            lblPrenom.Font = ThemeConfig.Fonts.DefaultFont;
            txtPrenom.Left = 130;
            txtPrenom.Top = 112;
            txtPrenom.Width = 340;

            // Âge
            Label lblAge = new Label { Text = "Âge :", Left = 20, Top = 155, Width = 80, AutoSize = true };
            lblAge.Font = ThemeConfig.Fonts.DefaultFont;
            nudAge.Left = 130;
            nudAge.Top = 152;
            nudAge.Width = 340;
            nudAge.Font = ThemeConfig.Fonts.DefaultFont;

            gbPersonnelles.Controls.Add(lblId);
            gbPersonnelles.Controls.Add(txtId);
            gbPersonnelles.Controls.Add(lblNom);
            gbPersonnelles.Controls.Add(txtNom);
            gbPersonnelles.Controls.Add(lblPrenom);
            gbPersonnelles.Controls.Add(txtPrenom);
            gbPersonnelles.Controls.Add(lblAge);
            gbPersonnelles.Controls.Add(nudAge);
            this.Controls.Add(gbPersonnelles);

            // GroupBox pour les informations professionnelles
            GroupBox gbProfessionnelles = new GroupBox
            {
                Text = "Informations Professionnelles",
                Left = 20,
                Top = 250,
                Width = 500,
                Height = 180
            };
            ThemeConfig.StyleGroupBox(gbProfessionnelles);

            // Spécialité
            Label lblSpecialite = new Label { Text = "Spécialité :", Left = 20, Top = 35, Width = 80, AutoSize = true };
            lblSpecialite.Font = ThemeConfig.Fonts.DefaultFont;
            txtSpecialite.Left = 130;
            txtSpecialite.Top = 32;
            txtSpecialite.Width = 340;

            // Module
            Label lblModule = new Label { Text = "Module :", Left = 20, Top = 75, Width = 80, AutoSize = true };
            lblModule.Font = ThemeConfig.Fonts.DefaultFont;
            txtModule.Left = 130;
            txtModule.Top = 72;
            txtModule.Width = 340;

            // Salaire
            Label lblSalaire = new Label { Text = "Salaire :", Left = 20, Top = 115, Width = 80, AutoSize = true };
            lblSalaire.Font = ThemeConfig.Fonts.DefaultFont;
            nudSalaire.Left = 130;
            nudSalaire.Top = 112;
            nudSalaire.Width = 340;
            nudSalaire.Font = ThemeConfig.Fonts.DefaultFont;
            nudSalaire.DecimalPlaces = 2;
            nudSalaire.Maximum = 1000000;

            gbProfessionnelles.Controls.Add(lblSpecialite);
            gbProfessionnelles.Controls.Add(txtSpecialite);
            gbProfessionnelles.Controls.Add(lblModule);
            gbProfessionnelles.Controls.Add(txtModule);
            gbProfessionnelles.Controls.Add(lblSalaire);
            gbProfessionnelles.Controls.Add(nudSalaire);
            this.Controls.Add(gbProfessionnelles);

            // Boutons
            RoundedButton btnValider = new RoundedButton 
            { 
                Text = "✓ Enregistrer", 
                Left = 150, 
                Top = 445, 
                Width = 140,
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover
            };
            btnValider.Click += btnValider_Click;

            RoundedButton btnAnnuler = new RoundedButton 
            { 
                Text = "✕ Annuler", 
                Left = 310, 
                Top = 445, 
                Width = 140,
                NormalColor = ThemeConfig.Colors.TextSecondary,
                HoverColor = ThemeConfig.Colors.BorderMedium
            };
            btnAnnuler.Click += (s, e) => this.Close();

            this.Controls.Add(btnValider);
            this.Controls.Add(btnAnnuler);
        }

        private void btnValider_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validation du nom
                if (string.IsNullOrWhiteSpace(txtNom.Text))
                {
                    Toast.ShowWarning("Le nom ne peut pas être vide");
                    return;
                }

                // Validation du prénom
                if (string.IsNullOrWhiteSpace(txtPrenom.Text))
                {
                    Toast.ShowWarning("Le prénom ne peut pas être vide");
                    return;
                }

                // Validation de l'âge
                if (nudAge.Value < 0)
                {
                    Toast.ShowWarning("L'âge ne peut pas être négatif");
                    return;
                }

                // Validation de la spécialité
                if (string.IsNullOrWhiteSpace(txtSpecialite.Text))
                {
                    Toast.ShowWarning("La spécialité ne peut pas être vide");
                    return;
                }

                // Validation du module
                if (string.IsNullOrWhiteSpace(txtModule.Text))
                {
                    Toast.ShowWarning("Le module ne peut pas être vide");
                    return;
                }

                if (_modeEdition && _formateurEdite != null)
                {
                    // Mode édition
                    _formateurEdite.Nom = txtNom.Text;
                    _formateurEdite.Prenom = txtPrenom.Text;
                    _formateurEdite.Age = (int)nudAge.Value;
                    _formateurEdite.Specialite = txtSpecialite.Text;
                    _formateurEdite.ModuleEnseigne = txtModule.Text;
                    _formateurEdite.Salaire = nudSalaire.Value;

                    _gestion.ModifierFormateur(_formateurEdite);
                    Toast.ShowSuccess($"Formateur '{_formateurEdite.Nom}' modifié avec succès!");
                }
                else
                {
                    // Mode création
                    Formateur nouveauFormateur = new Formateur(
                        _gestion.NouvelIdFormateur(),
                        txtNom.Text,
                        txtPrenom.Text,
                        (int)nudAge.Value,
                        txtSpecialite.Text,
                        txtModule.Text,
                        nudSalaire.Value
                    );

                    _gestion.AjouterFormateur(nouveauFormateur);
                    FormateurCree = nouveauFormateur;
                    Toast.ShowSuccess($"Formateur '{nouveauFormateur.Nom}' enregistré avec succès!");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Erreur: {ex.Message}");
            }
        }

        private void HighlightError(Control control, string message)
        {
            if (control is ModernTextBox mtb)
            {
                mtb.ShowInvalid(message);
            }
            else
            {
                control.BackColor = ThemeConfig.Colors.Warning;
            }
            Toast.ShowWarning(message);
            if (control is ModernTextBox)
            {
                control.BackColor = ThemeConfig.Colors.CardBackground;
            }
            else
            {
                control.BackColor = Color.White;
            }
            control.Focus();
        }
    }
}