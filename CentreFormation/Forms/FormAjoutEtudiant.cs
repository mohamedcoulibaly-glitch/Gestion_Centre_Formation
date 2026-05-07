/// <summary>
/// FormAjoutEtudiant : Formulaire pour ajouter ou modifier un étudiant
/// Peut fonctionner en deux modes :
/// - Mode Création : ajouter un nouvel étudiant
/// - Mode Édition : modifier un étudiant existant
/// 
/// Champs gérés :
/// - Informations personnelles : Nom, Prénom, Âge
/// - Informations académiques : Matricule, Niveau (L1-L3, M1-M2)
/// </summary>
using System;
using System.Windows.Forms;
using CentreFormation.Services;
using CentreFormation.Models;
using CentreFormation.UI;
using System.Drawing;

namespace CentreFormation.Forms
{
    public partial class FormAjoutEtudiant : Form
    {
        /// <summary>
        /// Référence au service de gestion du centre
        /// Permet de sauvegarder l'étudiant dans les données persistantes
        /// </summary>
        private GestionCentre _gestion;

        /// <summary>
        /// Étudiant créé ou modifié
        /// Null pour les nouveaux étudiants, contient l'objet pour l'édition
        /// </summary>
        public Etudiant? EtudiantCree { get; private set; } = null;

        /// <summary>
        /// Référence à l'étudiant en cours de modification (mode édition)
        /// </summary>
        private Etudiant? _etudiantEdite = null;

        /// <summary>
        /// Indicateur du mode de fonctionnement
        /// true = mode édition, false = mode création
        /// </summary>
        private bool _modeEdition = false;

        // === CONTRÔLES MODERNES ===
        /// <summary>
        /// TextBox moderne avec validation pour le nom
        /// </summary>
        private ModernTextBox txtNom = new ModernTextBox();

        /// <summary>
        /// TextBox moderne pour le prénom
        /// </summary>
        private ModernTextBox txtPrenom = new ModernTextBox();

        /// <summary>
        /// Contrôle numérique pour l'âge
        /// Accepte seulement des entiers positifs
        /// </summary>
        private NumericUpDown nudAge = new NumericUpDown();

        /// <summary>
        /// TextBox moderne pour le matricule
        /// </summary>
        private ModernTextBox txtMatricule = new ModernTextBox();

        /// <summary>
        /// ComboBox pour sélectionner le niveau d'étude
        /// Options : L1, L2, L3, M1, M2
        /// </summary>
        private ComboBox cboNiveau = new ComboBox();

        /// <summary>
        /// Constructeur mode création
        /// Lance le formulaire pour créer un nouvel étudiant
        /// </summary>
        public FormAjoutEtudiant(GestionCentre gestion)
        {
            _gestion = gestion;
            _modeEdition = false;
            InitializeComponent();
        }

        /// <summary>
        /// Constructeur mode édition
        /// Charge un étudiant existant et permet sa modification
        /// </summary>
        public FormAjoutEtudiant(GestionCentre gestion, Etudiant etudiant)
        {
            _gestion = gestion;
            _etudiantEdite = etudiant;
            _modeEdition = true;
            InitializeComponent();
            ChargerDonnees();
        }

        /// <summary>
        /// Charge les données de l'étudiant dans les champs du formulaire
        /// Utilisé en mode édition
        /// </summary>
        private void ChargerDonnees()
        {
            if (_modeEdition && _etudiantEdite != null)
            {
                this.Text = "Modifier un étudiant";
                txtNom.Text = _etudiantEdite.Nom;
                txtPrenom.Text = _etudiantEdite.Prenom;
                nudAge.Value = _etudiantEdite.Age;
                txtMatricule.Text = _etudiantEdite.Matricule;
                cboNiveau.SelectedItem = _etudiantEdite.Niveau;
            }
        }

        private void InitializeComponent()
        {
            // Setup form
            this.Text = "Ajouter un étudiant";
            this.Width = 500;
            this.Height = 450;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.Font = ThemeConfig.Fonts.DefaultFont;

            // Titre
            Label lblTitle = new Label
            {
                Text = "Informations de l'Étudiant",
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
                Width = 450,
                Height = 180
            };
            ThemeConfig.StyleGroupBox(gbPersonnelles);

            // Nom
            Label lblNom = new Label { Text = "Nom :", Left = 20, Top = 35, Width = 80, AutoSize = true };
            lblNom.Font = ThemeConfig.Fonts.DefaultFont;
            txtNom.Left = 130;
            txtNom.Top = 32;
            txtNom.Width = 280;

            // Prénom
            Label lblPrenom = new Label { Text = "Prénom :", Left = 20, Top = 75, Width = 80, AutoSize = true };
            lblPrenom.Font = ThemeConfig.Fonts.DefaultFont;
            txtPrenom.Left = 130;
            txtPrenom.Top = 72;
            txtPrenom.Width = 280;

            // Âge
            Label lblAge = new Label { Text = "Âge :", Left = 20, Top = 115, Width = 80, AutoSize = true };
            lblAge.Font = ThemeConfig.Fonts.DefaultFont;
            nudAge.Left = 130;
            nudAge.Top = 112;
            nudAge.Width = 280;
            nudAge.Font = ThemeConfig.Fonts.DefaultFont;

            gbPersonnelles.Controls.Add(lblNom);
            gbPersonnelles.Controls.Add(txtNom);
            gbPersonnelles.Controls.Add(lblPrenom);
            gbPersonnelles.Controls.Add(txtPrenom);
            gbPersonnelles.Controls.Add(lblAge);
            gbPersonnelles.Controls.Add(nudAge);
            this.Controls.Add(gbPersonnelles);

            // GroupBox pour les informations académiques
            GroupBox gbAcademiques = new GroupBox
            {
                Text = "Informations Académiques",
                Left = 20,
                Top = 250,
                Width = 450,
                Height = 110
            };
            ThemeConfig.StyleGroupBox(gbAcademiques);

            // Matricule
            Label lblMatricule = new Label { Text = "Matricule :", Left = 20, Top = 35, Width = 80, AutoSize = true };
            lblMatricule.Font = ThemeConfig.Fonts.DefaultFont;
            txtMatricule.Left = 130;
            txtMatricule.Top = 32;
            txtMatricule.Width = 280;

            // Niveau
            Label lblNiveau = new Label { Text = "Niveau :", Left = 20, Top = 75, Width = 80, AutoSize = true };
            lblNiveau.Font = ThemeConfig.Fonts.DefaultFont;
            cboNiveau.Left = 130;
            cboNiveau.Top = 72;
            cboNiveau.Width = 280;
            cboNiveau.Font = ThemeConfig.Fonts.DefaultFont;
            cboNiveau.Items.AddRange(new string[] { "L1", "L2", "L3", "M1", "M2" });
            cboNiveau.DropDownStyle = ComboBoxStyle.DropDownList;

            gbAcademiques.Controls.Add(lblMatricule);
            gbAcademiques.Controls.Add(txtMatricule);
            gbAcademiques.Controls.Add(lblNiveau);
            gbAcademiques.Controls.Add(cboNiveau);
            this.Controls.Add(gbAcademiques);

            // Boutons
            RoundedButton btnValider = new RoundedButton 
            { 
                Text = "✓ Valider", 
                Left = 130, 
                Top = 375, 
                Width = 150,
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover
            };
            btnValider.Click += btnValider_Click;

            RoundedButton btnAnnuler = new RoundedButton 
            { 
                Text = "✕ Annuler", 
                Left = 300, 
                Top = 375, 
                Width = 150,
                NormalColor = ThemeConfig.Colors.TextSecondary,
                HoverColor = ThemeConfig.Colors.BorderMedium
            };
            btnAnnuler.Click += btnAnnuler_Click;

            // Add controls to form
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
                    HighlightError(txtNom, "Le nom ne peut pas être vide");
                    return;
                }

                // Validation du prénom
                if (string.IsNullOrWhiteSpace(txtPrenom.Text))
                {
                    HighlightError(txtPrenom, "Le prénom ne peut pas être vide");
                    return;
                }

                // Validation de l'âge
                if (nudAge.Value < 0)
                {
                    HighlightError(nudAge, "L'âge ne peut pas être négatif");
                    return;
                }

                // Validation du matricule
                if (string.IsNullOrWhiteSpace(txtMatricule.Text))
                {
                    HighlightError(txtMatricule, "Le matricule ne peut pas être vide");
                    return;
                }

                // Validation du niveau
                if (cboNiveau.SelectedItem == null)
                {
                    Toast.ShowWarning("Veuillez sélectionner un niveau");
                    cboNiveau.Focus();
                    return;
                }

                if (_modeEdition && _etudiantEdite != null)
                {
                    // Mode édition: modifier l'étudiant existant
                    _etudiantEdite.Nom = txtNom.Text;
                    _etudiantEdite.Prenom = txtPrenom.Text;
                    _etudiantEdite.Age = (int)nudAge.Value;
                    _etudiantEdite.Matricule = txtMatricule.Text;
                    _etudiantEdite.Niveau = cboNiveau.SelectedItem?.ToString() ?? string.Empty;

                    _gestion.ModifierEtudiant(_etudiantEdite);
                    Toast.ShowSuccess($"Étudiant '{_etudiantEdite.Nom}' modifié avec succès!");
                }
                else
                {
                    // Mode création: créer un nouvel étudiant
                    string niveau = cboNiveau.SelectedItem?.ToString() ?? string.Empty;
                    Etudiant nouvelEtudiant = new Etudiant(
                        _gestion.NouvelIdEtudiant(),
                        txtNom.Text,
                        txtPrenom.Text,
                        (int)nudAge.Value,
                        txtMatricule.Text,
                        niveau
                    );

                    _gestion?.AjouterEtudiant(nouvelEtudiant);
                    EtudiantCree = nouvelEtudiant;
                    Toast.ShowSuccess($"Étudiant '{nouvelEtudiant.Nom}' ajouté avec succès!");
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

        private void btnAnnuler_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}