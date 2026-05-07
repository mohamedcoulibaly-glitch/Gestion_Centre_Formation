/// <summary>
/// FormAjoutModule : Formulaire pour ajouter ou modifier un module de formation
/// Peut fonctionner en deux modes :
/// - Mode Création : ajouter un nouveau module
/// - Mode Édition : modifier un module existant
/// 
/// Champs gérés :
/// - Code : identifiant unique du module (ex: WEB101)
/// - Libellé : nom complet du module
/// - Durée : nombre d'heures de formation
/// - Coefficient : poids dans le calcul de la moyenne pondérée
/// </summary>
using System;
using System.Windows.Forms;
using CentreFormation.Services;
using System.Drawing;
using CentreFormation.Models;
using CentreFormation.UI;

namespace CentreFormation.Forms
{
    public partial class FormAjoutModule : Form
    {
        /// <summary>
        /// Référence au service de gestion du centre
        /// Permet de sauvegarder le module dans les données persistantes
        /// </summary>
        private GestionCentre _gestion;

        /// <summary>
        /// Module créé
        /// Null pour les nouveaux modules, contient l'objet pour l'édition
        /// </summary>
        public Module? ModuleCree { get; private set; } = null;

        /// <summary>
        /// Référence au module en cours de modification (mode édition)
        /// </summary>
        private Module? _moduleEdite = null;

        /// <summary>
        /// Indicateur du mode de fonctionnement
        /// true = mode édition, false = mode création
        /// </summary>
        private bool _modeEdition = false;

        // === CONTRÔLES MODERNES ===
        /// <summary>
        /// TextBox pour le code unique du module
        /// Exemple: WEB101, BDD202, etc.
        /// </summary>
        private ModernTextBox txtCode = new ModernTextBox();

        /// <summary>
        /// TextBox pour le libellé/nom du module
        /// </summary>
        private ModernTextBox txtLibelle = new ModernTextBox();

        /// <summary>
        /// Contrôle numérique pour la durée en heures
        /// </summary>
        private NumericUpDown nudDuree = new NumericUpDown();

        /// <summary>
        /// Contrôle numérique pour le coefficient
        /// Plus élevé = plus important pour la moyenne
        /// </summary>
        private NumericUpDown nudCoefficient = new NumericUpDown();

        /// <summary>
        /// Constructeur mode création
        /// Lance le formulaire pour créer un nouveau module
        /// </summary>
        public FormAjoutModule(GestionCentre gestion)
        {
            _gestion = gestion;
            _modeEdition = false;
            InitializeComponent();
        }

        /// <summary>
        /// Constructeur mode édition
        /// Charge un module existant et permet sa modification
        /// </summary>
        public FormAjoutModule(GestionCentre gestion, Module module)
        {
            _gestion = gestion;
            _moduleEdite = module;
            _modeEdition = true;
            InitializeComponent();
            ChargerDonnees();
        }

        /// <summary>
        /// Charge les données du module dans les champs du formulaire
        /// Utilisé en mode édition
        /// Le code ne peut pas être modifié (lecture seule)
        /// </summary>
        private void ChargerDonnees()
        {
            if (_modeEdition && _moduleEdite != null)
            {
                this.Text = "Modifier un module";
                txtCode.Text = _moduleEdite.Code;
                txtCode.ReadOnly = true;
                txtLibelle.Text = _moduleEdite.Libelle;
                nudDuree.Value = _moduleEdite.Duree;
                nudCoefficient.Value = _moduleEdite.Coefficient;
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Ajouter un module";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.Font = ThemeConfig.Fonts.DefaultFont;

            // Titre
            Label lblTitle = new Label
            {
                Text = "Créer un Nouveau Module",
                Left = 20,
                Top = 20,
                Width = 400,
                AutoSize = true
            };
            ThemeConfig.StyleLabelTitle(lblTitle);
            this.Controls.Add(lblTitle);

            // GroupBox pour les informations du module
            GroupBox gbModule = new GroupBox
            {
                Text = "Informations du Module",
                Left = 20,
                Top = 60,
                Width = 450,
                Height = 220
            };
            ThemeConfig.StyleGroupBox(gbModule);

            // Code
            Label lblCode = new Label { Text = "Code :", Left = 20, Top = 35, Width = 80, AutoSize = true };
            lblCode.Font = ThemeConfig.Fonts.DefaultFont;
            txtCode.Left = 130;
            txtCode.Top = 32;
            txtCode.Width = 280;

            // Libellé
            Label lblLibelle = new Label { Text = "Libellé :", Left = 20, Top = 75, Width = 80, AutoSize = true };
            lblLibelle.Font = ThemeConfig.Fonts.DefaultFont;
            txtLibelle.Left = 130;
            txtLibelle.Top = 72;
            txtLibelle.Width = 280;

            // Durée
            Label lblDuree = new Label { Text = "Durée (h) :", Left = 20, Top = 115, Width = 80, AutoSize = true };
            lblDuree.Font = ThemeConfig.Fonts.DefaultFont;
            nudDuree.Left = 130;
            nudDuree.Top = 112;
            nudDuree.Width = 280;
            nudDuree.Font = ThemeConfig.Fonts.DefaultFont;

            // Coefficient
            Label lblCoefficient = new Label { Text = "Coefficient :", Left = 20, Top = 155, Width = 80, AutoSize = true };
            lblCoefficient.Font = ThemeConfig.Fonts.DefaultFont;
            nudCoefficient.Left = 130;
            nudCoefficient.Top = 152;
            nudCoefficient.Width = 280;
            nudCoefficient.Font = ThemeConfig.Fonts.DefaultFont;

            gbModule.Controls.Add(lblCode);
            gbModule.Controls.Add(txtCode);
            gbModule.Controls.Add(lblLibelle);
            gbModule.Controls.Add(txtLibelle);
            gbModule.Controls.Add(lblDuree);
            gbModule.Controls.Add(nudDuree);
            gbModule.Controls.Add(lblCoefficient);
            gbModule.Controls.Add(nudCoefficient);
            this.Controls.Add(gbModule);

            // Boutons
            RoundedButton btnValider = new RoundedButton 
            { 
                Text = "✓ Créer le Module", 
                Left = 130, 
                Top = 305, 
                Width = 150,
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover
            };
            btnValider.Click += btnValider_Click;

            RoundedButton btnAnnuler = new RoundedButton 
            { 
                Text = "✕ Annuler", 
                Left = 300, 
                Top = 305, 
                Width = 150,
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
                // Validation du code
                if (string.IsNullOrWhiteSpace(txtCode.Text))
                {
                    Toast.ShowWarning("Le code ne peut pas être vide");
                    return;
                }

                // Validation du libellé
                if (string.IsNullOrWhiteSpace(txtLibelle.Text))
                {
                    Toast.ShowWarning("Le libellé ne peut pas être vide");
                    return;
                }

                // Validation de la durée
                if (nudDuree.Value <= 0)
                {
                    Toast.ShowWarning("La durée doit être positive");
                    return;
                }

                if (_modeEdition && _moduleEdite != null)
                {
                    // Mode édition
                    _moduleEdite.Libelle = txtLibelle.Text;
                    _moduleEdite.Duree = (int)nudDuree.Value;
                    _moduleEdite.Coefficient = (int)nudCoefficient.Value;

                    _gestion.ModifierModule(_moduleEdite);
                    Toast.ShowSuccess($"Module '{_moduleEdite.Code}' modifié avec succès!");
                }
                else
                {
                    // Mode création
                    Module nouveauModule = new Module(
                        txtCode.Text,
                        txtLibelle.Text,
                        (int)nudDuree.Value,
                        (int)nudCoefficient.Value
                    );

                    _gestion.AjouterModule(nouveauModule);
                    ModuleCree = nouveauModule;
                    Toast.ShowSuccess($"Module '{nouveauModule.Code}' créé avec succès!");
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