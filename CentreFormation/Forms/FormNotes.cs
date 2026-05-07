/// <summary>
/// FormNotes : Formulaire pour gérer les notes des étudiants
/// Permet d'associer un étudiant à un module et de lui attribuer une note (0-20)
/// Affiche toutes les notes d'un étudiant sélectionné avec sa moyenne en temps réel
/// Permet de modifier ou supprimer des notes existantes
/// 
/// Étapes :
/// 1. Sélectionner un étudiant dans le ComboBox (affiche ses notes automatiquement)
/// 2. Sélectionner un module dans le ComboBox
/// 3. Entrer la note (0-20)
/// 4. Cliquer sur "Enregistrer la Note" pour ajouter ou modifier
/// 5. Utiliser les boutons Modifier/Supprimer pour gérer les notes existantes
/// </summary>
using System;
using System.Windows.Forms;
using System.Drawing;
using CentreFormation.Models;
using CentreFormation.Services;
using CentreFormation.UI;

namespace CentreFormation.Forms
{
    public partial class FormNotes : Form
    {
        /// <summary>
        /// Référence au service de gestion du centre
        /// Permet d'accéder aux étudiants et modules disponibles
        /// </summary>
        private GestionCentre gestionCentre = null!;

        /// <summary>
        /// ComboBox pour sélectionner l'étudiant
        /// Remplie au chargement avec tous les étudiants enregistrés
        /// </summary>
        private ComboBox cmbEtudiants = new ComboBox();

        /// <summary>
        /// ComboBox pour sélectionner le module
        /// Remplie au chargement avec tous les modules enregistrés
        /// </summary>
        private ComboBox cmbModules = new ComboBox();

        /// <summary>
        /// Contrôle numérique pour la note
        /// Accepte les valeurs décimales entre 0 et 20
        /// </summary>
        private NumericUpDown nudNote = new NumericUpDown();

        /// <summary>
        /// DataGridView pour afficher toutes les notes de l'étudiant sélectionné
        /// </summary>
        private DataGridView dgvNotes = new DataGridView();

        /// <summary>
        /// Label pour afficher la moyenne de l'étudiant en temps réel
        /// </summary>
        private Label lblMoyenne = new Label();

        /// <summary>
        /// Constructeur du formulaire de gestion des notes
        /// </summary>
        public FormNotes(GestionCentre gestion)
        {
            gestionCentre = gestion;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestion des Notes";
            this.Width = 700;
            this.Height = 650;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.Font = ThemeConfig.Fonts.DefaultFont;

            // Titre
            Label lblTitle = new Label
            {
                Text = "📝 Gestion des Notes",
                Left = 20,
                Top = 20,
                Width = 600,
                AutoSize = true
            };
            ThemeConfig.StyleLabelTitle(lblTitle);
            this.Controls.Add(lblTitle);

            // GroupBox pour l'enregistrement de notes
            GroupBox gbNotes = new GroupBox
            {
                Text = "Enregistrer/Modifier une Note",
                Left = 20,
                Top = 60,
                Width = 650,
                Height = 160
            };
            ThemeConfig.StyleGroupBox(gbNotes);

            // Étudiant
            Label lblEtudiant = new Label { Text = "Étudiant :", Left = 20, Top = 35, Width = 100, AutoSize = true };
            lblEtudiant.Font = ThemeConfig.Fonts.DefaultFont;
            cmbEtudiants.Left = 140;
            cmbEtudiants.Top = 32;
            cmbEtudiants.Width = 280;
            cmbEtudiants.Font = ThemeConfig.Fonts.DefaultFont;
            cmbEtudiants.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEtudiants.SelectedIndexChanged += cmbEtudiants_SelectedIndexChanged;

            // Charger les étudiants
            foreach (Etudiant etudiant in gestionCentre.Etudiants)
            {
                cmbEtudiants.Items.Add(etudiant);
            }

            // Module
            Label lblModule = new Label { Text = "Module :", Left = 20, Top = 75, Width = 100, AutoSize = true };
            lblModule.Font = ThemeConfig.Fonts.DefaultFont;
            cmbModules.Left = 140;
            cmbModules.Top = 72;
            cmbModules.Width = 280;
            cmbModules.Font = ThemeConfig.Fonts.DefaultFont;
            cmbModules.DropDownStyle = ComboBoxStyle.DropDownList;

            // Charger les modules
            foreach (Module module in gestionCentre.Modules)
            {
                cmbModules.Items.Add(module);
            }

            // Note
            Label lblNote = new Label { Text = "Note (0-20) :", Left = 20, Top = 115, Width = 100, AutoSize = true };
            lblNote.Font = ThemeConfig.Fonts.DefaultFont;
            nudNote.Left = 140;
            nudNote.Top = 112;
            nudNote.Width = 280;
            nudNote.Font = ThemeConfig.Fonts.DefaultFont;
            nudNote.Minimum = 0;
            nudNote.Maximum = 20;
            nudNote.DecimalPlaces = 2;

            gbNotes.Controls.Add(lblEtudiant);
            gbNotes.Controls.Add(cmbEtudiants);
            gbNotes.Controls.Add(lblModule);
            gbNotes.Controls.Add(cmbModules);
            gbNotes.Controls.Add(lblNote);
            gbNotes.Controls.Add(nudNote);
            this.Controls.Add(gbNotes);

            // GroupBox pour afficher les notes de l'étudiant
            GroupBox gbListeNotes = new GroupBox
            {
                Text = "Notes de l'étudiant sélectionné",
                Left = 20,
                Top = 230,
                Width = 650,
                Height = 250
            };
            ThemeConfig.StyleGroupBox(gbListeNotes);

            // Label pour la moyenne
            lblMoyenne.Text = "Moyenne: --";
            lblMoyenne.Left = 20;
            lblMoyenne.Top = 20;
            lblMoyenne.Width = 200;
            lblMoyenne.Font = new Font(ThemeConfig.Fonts.DefaultFont.FontFamily, 11, FontStyle.Bold);
            lblMoyenne.ForeColor = ThemeConfig.Colors.Primary;
            gbListeNotes.Controls.Add(lblMoyenne);

            // DataGridView pour afficher les notes
            dgvNotes.Left = 20;
            dgvNotes.Top = 50;
            dgvNotes.Width = 610;
            dgvNotes.Height = 160;
            dgvNotes.AllowUserToAddRows = false;
            dgvNotes.AllowUserToDeleteRows = false;
            dgvNotes.ReadOnly = true;
            dgvNotes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvNotes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvNotes.BackgroundColor = Color.White;
            dgvNotes.BorderStyle = BorderStyle.FixedSingle;

            // Colonnes du DataGridView
            dgvNotes.Columns.Add("Module", "Module");
            dgvNotes.Columns.Add("Note", "Note");
            dgvNotes.Columns.Add("Coefficient", "Coefficient");
            dgvNotes.Columns.Add("NotePonderee", "Note Pondérée");

            gbListeNotes.Controls.Add(dgvNotes);
            this.Controls.Add(gbListeNotes);

            // Boutons
            RoundedButton btnAjouter = new RoundedButton 
            { 
                Text = "✓ Enregistrer", 
                Left = 450, 
                Top = 495, 
                Width = 120,
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover
            };
            btnAjouter.Click += btnAjouterNote_Click;

            RoundedButton btnModifier = new RoundedButton 
            { 
                Text = "✏️ Modifier", 
                Left = 450, 
                Top = 530, 
                Width = 120,
                NormalColor = ThemeConfig.Colors.Primary,
                HoverColor = ThemeConfig.Colors.PrimaryHover
            };
            btnModifier.Click += btnModifierNote_Click;

            RoundedButton btnSupprimer = new RoundedButton 
            { 
                Text = "🗑️ Supprimer", 
                Left = 580, 
                Top = 495, 
                Width = 120,
                NormalColor = ThemeConfig.Colors.Error,
                HoverColor = ThemeConfig.Colors.ErrorHover
            };
            btnSupprimer.Click += btnSupprimerNote_Click;

            RoundedButton btnAnnuler = new RoundedButton 
            { 
                Text = "✕ Fermer", 
                Left = 580, 
                Top = 530, 
                Width = 120,
                NormalColor = ThemeConfig.Colors.TextSecondary,
                HoverColor = ThemeConfig.Colors.BorderMedium
            };
            btnAnnuler.Click += (s, e) => this.Close();

            this.Controls.Add(btnAjouter);
            this.Controls.Add(btnModifier);
            this.Controls.Add(btnSupprimer);
            this.Controls.Add(btnAnnuler);
        }

        private void cmbEtudiants_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cmbEtudiants.SelectedItem == null)
            {
                dgvNotes.Rows.Clear();
                lblMoyenne.Text = "Moyenne: --";
                return;
            }

            Etudiant etudiantChoisi = (Etudiant)cmbEtudiants.SelectedItem;
            AfficherNotesEtudiant(etudiantChoisi);
        }

        private void AfficherNotesEtudiant(Etudiant etudiant)
        {
            dgvNotes.Rows.Clear();

            List<Note> notes = gestionCentre.GetNotesParEtudiant(etudiant.Id);

            foreach (Note note in notes)
            {
                dgvNotes.Rows.Add(
                    note.Module.Libelle,
                    note.Valeur.ToString("F2"),
                    note.Module.Coefficient,
                    (note.Valeur * note.Module.Coefficient).ToString("F2")
                );
            }

            double moyenne = etudiant.CalculerMoyenne();
            lblMoyenne.Text = $"Moyenne: {moyenne:F2}/20";
        }

        private void btnAjouterNote_Click(object? sender, EventArgs e)
        {
            try
            {
                // Validation de la sélection de l'étudiant
                if (cmbEtudiants.SelectedItem == null)
                {
                    Toast.ShowWarning("Veuillez sélectionner un étudiant");
                    cmbEtudiants.Focus();
                    return;
                }

                // Validation de la sélection du module
                if (cmbModules.SelectedItem == null)
                {
                    Toast.ShowWarning("Veuillez sélectionner un module");
                    cmbModules.Focus();
                    return;
                }

                // Validation de la note
                if (nudNote.Value < 0 || nudNote.Value > 20)
                {
                    Toast.ShowWarning("La note doit être entre 0 et 20");
                    nudNote.Focus();
                    return;
                }

                Etudiant etudiantChoisi = (Etudiant)cmbEtudiants.SelectedItem;
                Module moduleChoisi = (Module)cmbModules.SelectedItem;

                Note note = new Note(
                    etudiantChoisi,
                    moduleChoisi,
                    (double)nudNote.Value
                );

                gestionCentre.AjouterNote(note);

                Toast.ShowSuccess($"Note {nudNote.Value:F2}/20 enregistrée pour {etudiantChoisi.Nom}");

                // Rafraîchir l'affichage
                AfficherNotesEtudiant(etudiantChoisi);

                // Réinitialiser les champs
                cmbModules.SelectedIndex = -1;
                nudNote.Value = 0;
                cmbModules.Focus();
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Erreur: {ex.Message}");
            }
        }

        private void btnModifierNote_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cmbEtudiants.SelectedItem == null)
                {
                    Toast.ShowWarning("Veuillez sélectionner un étudiant");
                    return;
                }

                if (cmbModules.SelectedItem == null)
                {
                    Toast.ShowWarning("Veuillez sélectionner un module");
                    return;
                }

                if (dgvNotes.SelectedRows.Count == 0)
                {
                    Toast.ShowWarning("Veuillez sélectionner une note à modifier dans la liste");
                    return;
                }

                Etudiant etudiantChoisi = (Etudiant)cmbEtudiants.SelectedItem;
                Module moduleChoisi = (Module)cmbModules.SelectedItem;

                Note note = new Note(
                    etudiantChoisi,
                    moduleChoisi,
                    (double)nudNote.Value
                );

                gestionCentre.ModifierNote(note);

                Toast.ShowSuccess($"Note modifiée avec succès");

                // Rafraîchir l'affichage
                AfficherNotesEtudiant(etudiantChoisi);

                // Réinitialiser les champs
                cmbModules.SelectedIndex = -1;
                nudNote.Value = 0;
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Erreur: {ex.Message}");
            }
        }

        private void btnSupprimerNote_Click(object? sender, EventArgs e)
        {
            try
            {
                if (cmbEtudiants.SelectedItem == null)
                {
                    Toast.ShowWarning("Veuillez sélectionner un étudiant");
                    return;
                }

                if (dgvNotes.SelectedRows.Count == 0)
                {
                    Toast.ShowWarning("Veuillez sélectionner une note à supprimer dans la liste");
                    return;
                }

                Etudiant etudiantChoisi = (Etudiant)cmbEtudiants.SelectedItem;
                DataGridViewRow selectedRow = dgvNotes.SelectedRows[0];
                string moduleLibelle = selectedRow.Cells["Module"].Value?.ToString() ?? string.Empty;

                // Trouver le module correspondant
                Module? moduleToDelete = gestionCentre.Modules.FirstOrDefault(m => m.Libelle == moduleLibelle);
                if (moduleToDelete == null)
                {
                    Toast.ShowError("Module introuvable");
                    return;
                }

                gestionCentre.SupprimerNote(etudiantChoisi.Id, moduleToDelete.Code);

                Toast.ShowSuccess("Note supprimée avec succès");

                // Rafraîchir l'affichage
                AfficherNotesEtudiant(etudiantChoisi);
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Erreur: {ex.Message}");
            }
        }
    }
}