/// <summary>
/// FormMain : Fenêtre principale de l'application
/// Interface centrale pour gérer:
/// - Les étudiants (voir, ajouter, modifier, supprimer)
/// - Les formateurs (voir, ajouter, modifier, supprimer)  
/// - Les modules (voir, ajouter, modifier, supprimer)
/// - Les notes (enregistrer les évaluations)
/// - Les statistiques (moyenne générale, meilleur étudiant, etc.)
/// 
/// Architecture:
/// - Panneau latéral (Sidebar) : navigation entre les sections
/// - Barre de titre personnalisée : avec bouton fermer
/// - TabControl : onglets cachés pour chaque section
/// - DataGridView : affichage des données tabulaires
/// - Fonctions de recherche pour filtrer les données
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentreFormation.Services;
using CentreFormation.Forms;
using CentreFormation.UI;
using CentreFormation.Models;
using System.Drawing;
using System.IO;
using MigraDocDocument = MigraDoc.DocumentObjectModel.Document;
using MigraDocSection = MigraDoc.DocumentObjectModel.Section;
using MigraDocParagraph = MigraDoc.DocumentObjectModel.Paragraph;
using MigraDocTable = MigraDoc.DocumentObjectModel.Tables.Table;
using MigraDocRow = MigraDoc.DocumentObjectModel.Tables.Row;
using MigraDocCell = MigraDoc.DocumentObjectModel.Tables.Cell;
using MigraDocUnit = MigraDoc.DocumentObjectModel.Unit;
using MigraDocPageFormat = MigraDoc.DocumentObjectModel.PageFormat;
using MigraDocParagraphAlignment = MigraDoc.DocumentObjectModel.ParagraphAlignment;
using MigraDocTextFormat = MigraDoc.DocumentObjectModel.TextFormat;
using MigraDocColors = MigraDoc.DocumentObjectModel.Colors;
using PdfDocumentRenderer = MigraDoc.Rendering.PdfDocumentRenderer;
using ClosedXML.Excel;

namespace CentreFormation.Forms
{
    public partial class FormMain : Form
    {
        /// <summary>
        /// Objet principal qui contient toutes les données du centre
        /// Gère la logique métier et la persistance
        /// </summary>
        private GestionCentre _gestionCentre = new GestionCentre();

        /// <summary>
        /// Panneau latéral gauche contenant les boutons de navigation
        /// </summary>
        private Panel? _sidebarPanel = null;

        /// <summary>
        /// Barre de titre personnalisée en haut de la fenêtre
        /// </summary>
        private Panel? _titleBarPanel = null;

        /// <summary>
        /// Variables pour le déplacement de la fenêtre
        /// </summary>
        private bool _isDragging = false;
        private Point _dragCursorPoint;
        private Point _dragFormPoint;

        /// <summary>
        /// Contrôle d'onglets principal contenant 5 onglets
        /// (Étudiants, Formateurs, Modules, Notes, Statistiques)
        /// </summary>
        private TabControl? _tabControl = null;

        /// <summary>
        /// Références aux DataGridView pour actualiser les données
        /// </summary>
        private DataGridView? _dgvEtudiants = null;
        private DataGridView? _dgvFormateurs = null;
        private DataGridView? _dgvModules = null;

        public FormMain()
        {
            InitializeComponent();
            ApplyModernTheme();
        }

        /// <summary>
        /// Initialise tous les composants visuels de la fenêtre principale
        /// Crée la barre de titre, le sidebar et le contenu central
        /// L'ordre d'ajout est important pour le z-order (étagement)
        /// </summary>
        private void InitializeComponent()
        {
            // Configuration de la fenêtre
            this.Text = "Gestion Centre Formation";
            this.Width = 1200;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.AutoScaleMode = AutoScaleMode.Font;

            // Panneau principal avec TabControl (doit être ajouté en premier pour le DockStyle.Fill)
            CreateMainContent();

            // Panneau latéral de navigation (Sidebar)
            CreateSidebar();

            // Barre de titre personnalisée (doit être ajoutée en dernier pour être en haut)
            CreateTitleBar();
        }

        /// <summary>
        /// Crée la barre de titre personnalisée en haut de la fenêtre
        /// Contient le titre de l'application et le bouton fermer
        /// </summary>
        private void CreateTitleBar()
        {
            _titleBarPanel = new Panel
            {
                BackColor = ThemeConfig.Colors.Primary,
                Height = 60,
                Dock = DockStyle.Top
            };

            Label titleLabel = new Label
            {
                Text = "🏫 Gestion Centre Formation",
                ForeColor = ThemeConfig.Colors.TextLight,
                Font = ThemeConfig.Fonts.TitleFont,
                Left = 20,
                Top = 18,
                AutoSize = true
            };
            _titleBarPanel.Controls.Add(titleLabel);

            // Bouton Fermer
            Button closeBtn = new Button
            {
                Text = "✕",
                BackColor = ThemeConfig.Colors.Primary,
                ForeColor = ThemeConfig.Colors.TextLight,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Width = 50,
                Height = 60,
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Font = new Font("Arial", 14, FontStyle.Bold)
            };
            closeBtn.Click += (s, e) => this.Close();
            closeBtn.MouseEnter += (s, e) => closeBtn.BackColor = Color.FromArgb(200, ThemeConfig.Colors.Primary.R, ThemeConfig.Colors.Primary.G, ThemeConfig.Colors.Primary.B);
            closeBtn.MouseLeave += (s, e) => closeBtn.BackColor = ThemeConfig.Colors.Primary;
            _titleBarPanel.Controls.Add(closeBtn);

            // Add drag functionality to title bar
            _titleBarPanel.MouseDown += TitleBar_MouseDown;
            _titleBarPanel.MouseMove += TitleBar_MouseMove;
            _titleBarPanel.MouseUp += TitleBar_MouseUp;

            this.Controls.Add(_titleBarPanel);
        }

        private void TitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _dragCursorPoint = Cursor.Position;
                _dragFormPoint = this.Location;
            }
        }

        private void TitleBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (_isDragging && e.Button == MouseButtons.Left)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(_dragCursorPoint));
                this.Location = Point.Add(_dragFormPoint, new Size(diff));
            }
        }

        private void TitleBar_MouseUp(object? sender, MouseEventArgs e)
        {
            _isDragging = false;
        }

        /// <summary>
        /// Crée la barre latérale (Sidebar) avec les boutons de navigation
        /// Permet de basculer entre les différentes sections (Étudiants, Formateurs, etc.)
        /// </summary>
        private void CreateSidebar()
        {
            _sidebarPanel = new Panel
            {
                BackColor = ThemeConfig.Colors.SidebarDark,
                Width = 220,
                Dock = DockStyle.Left
            };

            // Titre du sidebar
            Label sidebarTitle = new Label
            {
                Text = "📋 MENU",
                ForeColor = ThemeConfig.Colors.TextLight,
                Font = ThemeConfig.Fonts.BoldFont,
                Left = 20,
                Top = 20,
                AutoSize = true
            };
            _sidebarPanel.Controls.Add(sidebarTitle);

            // Boutons de navigation
            int btnY = 70;
            const int btnHeight = 45;
            const int spacing = 10;

            Button btnEtudiants = CreateSidebarButton("👥 Étudiants", 20, btnY, "etudiants");
            btnEtudiants.Click += (s, e) => _tabControl!.SelectedIndex = 0;
            _sidebarPanel!.Controls.Add(btnEtudiants);
            btnY += btnHeight + spacing;

            Button btnFormateurs = CreateSidebarButton("👨‍🏫 Formateurs", 20, btnY, "formateurs");
            btnFormateurs.Click += (s, e) => _tabControl!.SelectedIndex = 1;
            _sidebarPanel!.Controls.Add(btnFormateurs);
            btnY += btnHeight + spacing;

            Button btnModules = CreateSidebarButton("📚 Modules", 20, btnY, "modules");
            btnModules.Click += (s, e) => _tabControl!.SelectedIndex = 2;
            _sidebarPanel!.Controls.Add(btnModules);
            btnY += btnHeight + spacing;

            Button btnNotes = CreateSidebarButton("📝 Notes", 20, btnY, "notes");
            btnNotes.Click += (s, e) => _tabControl!.SelectedIndex = 3;
            _sidebarPanel!.Controls.Add(btnNotes);
            btnY += btnHeight + spacing;

            Button btnStats = CreateSidebarButton("📊 Statistiques", 20, btnY, "stats");
            btnStats.Click += (s, e) => _tabControl!.SelectedIndex = 4;
            _sidebarPanel!.Controls.Add(btnStats);

            this.Controls.Add(_sidebarPanel!);
        }

        /// <summary>
        /// Crée le contenu principal avec le TabControl
        /// Contient 5 onglets : Étudiants, Formateurs, Modules, Notes, Statistiques
        /// Les onglets sont cachés (ItemSize.Height = 1) car on utilise le Sidebar pour la navigation
        /// </summary>
        private void CreateMainContent()
        {
            _tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                ItemSize = new Size(0, 1),  // Masquer les onglets (on utilise le sidebar)
                SizeMode = TabSizeMode.Fixed,
                Appearance = TabAppearance.FlatButtons,
                Multiline = false
            };

            // Onglet 1: Étudiants
            _tabControl.TabPages.Add(CreateStudentsTab());

            // Onglet 2: Formateurs
            _tabControl.TabPages.Add(CreateTrainersTab());

            // Onglet 3: Modules
            _tabControl.TabPages.Add(CreateModulesTab());

            // Onglet 4: Notes
            _tabControl.TabPages.Add(CreateGradesTab());

            // Onglet 5: Statistiques
            _tabControl.TabPages.Add(CreateStatsTab());

            this.Controls.Add(_tabControl);
        }

        private TabPage CreateStudentsTab()
        {
            TabPage tab = new TabPage { BackColor = ThemeConfig.Colors.BackgroundMain };

            // Top bar avec actions
            Panel topBar = new Panel
            {
                Height = 70,
                Dock = DockStyle.Top,
                BackColor = ThemeConfig.Colors.CardBackground,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblTitle = new Label
            {
                Text = "📚 Liste des Étudiants",
                Font = ThemeConfig.Fonts.TitleFont,
                ForeColor = ThemeConfig.Colors.TextDark,
                Left = 20,
                Top = 12,
                AutoSize = true
            };
            topBar.Controls.Add(lblTitle);

            RoundedButton btnAddStudent = new RoundedButton
            {
                Text = "➕ Ajouter Étudiant",
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover,
                Width = 150,
                Height = 40,
                Left = 20,
                Top = 40
            };
            btnAddStudent.Click += (s, e) =>
            {
                FormAjoutEtudiant form = new FormAjoutEtudiant(_gestionCentre);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshStudentsDataGrid();
                }
            };
            topBar.Controls.Add(btnAddStudent);

            RoundedButton btnDeleteStudent = new RoundedButton
            {
                Text = "🗑️ Supprimer",
                NormalColor = ThemeConfig.Colors.Error,
                HoverColor = ThemeConfig.Colors.ErrorHover,
                Width = 120,
                Height = 40,
                Left = 180,
                Top = 40
            };
            btnDeleteStudent.Click += (s, e) => DeleteSelectedStudent();
            topBar.Controls.Add(btnDeleteStudent);

            RoundedButton btnExportPDF = new RoundedButton
            {
                Text = "📄 Export PDF",
                NormalColor = ThemeConfig.Colors.Secondary,
                HoverColor = ThemeConfig.Colors.Primary,
                Width = 120,
                Height = 40,
                Left = 310,
                Top = 40
            };
            btnExportPDF.Click += (s, e) => ExportStudentPDF();
            topBar.Controls.Add(btnExportPDF);

            RoundedButton btnExportExcel = new RoundedButton
            {
                Text = "📊 Export Excel",
                NormalColor = ThemeConfig.Colors.Info,
                HoverColor = ThemeConfig.Colors.PrimaryHover,
                Width = 130,
                Height = 40,
                Left = 440,
                Top = 40
            };
            btnExportExcel.Click += (s, e) => ExportStudentsExcel();
            topBar.Controls.Add(btnExportExcel);

            tab.Controls.Add(topBar);

            // Barre de recherche
            Panel searchBar = CreateSearchBar("Rechercher étudiant par nom ou niveau...", (searchTerm) =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    RefreshStudentsDataGrid();
                }
                else
                {
                    var filtered = _gestionCentre.Etudiants
                        .Where(e => e.Nom.ToLower().Contains(searchTerm.ToLower()) ||
                                   e.Prenom.ToLower().Contains(searchTerm.ToLower()) ||
                                   e.Niveau.ToLower().Contains(searchTerm.ToLower()))
                        .ToList();
                    PopulateStudentsDataGrid(filtered);
                }
            });
            searchBar.Height = 50;
            searchBar.Dock = DockStyle.Top;
            tab.Controls.Add(searchBar);

            // DataGridView
            _dgvEtudiants = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                BackgroundColor = ThemeConfig.Colors.CardBackground
            };

            // Colonnes
            _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 60 });
            _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nom", HeaderText = "Nom", Width = 120 });
            _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Prenom", HeaderText = "Prénom", Width = 120 });
            _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Age", HeaderText = "Âge", Width = 60 });
            _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Matricule", HeaderText = "Matricule", Width = 100 });
            _dgvEtudiants.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Niveau", HeaderText = "Niveau", Width = 100 });

            // Appliquer le style moderne
            ThemeConfig.StyleDataGridView(_dgvEtudiants);

            // Double-click pour éditer
            _dgvEtudiants.DoubleClick += (s, e) =>
            {
                if (_dgvEtudiants.CurrentRow != null)
                {
                    int id = (int)_dgvEtudiants.CurrentRow.Cells[0].Value;
                    var student = _gestionCentre.Etudiants.FirstOrDefault(x => x.Id == id);
                    if (student != null)
                    {
                        FormAjoutEtudiant form = new FormAjoutEtudiant(_gestionCentre, student);
                        if (form.ShowDialog() == DialogResult.OK)
                        {
                            RefreshStudentsDataGrid();
                        }
                    }
                }
            };

            tab.Controls.Add(_dgvEtudiants);

            // Charger les données au démarrage
            RefreshStudentsDataGrid();

            return tab;
        }

        private TabPage CreateTrainersTab()
        {
            TabPage tab = new TabPage { BackColor = ThemeConfig.Colors.BackgroundMain };

            Panel topBar = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = ThemeConfig.Colors.CardBackground,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            Label lblTitle = new Label
            {
                Text = "👨‍🏫 Liste des Formateurs",
                Font = ThemeConfig.Fonts.TitleFont,
                ForeColor = ThemeConfig.Colors.TextDark,
                Left = 0,
                Top = 5,
                AutoSize = true
            };
            topBar.Controls.Add(lblTitle);

            RoundedButton btnAddTrainer = new RoundedButton
            {
                Text = "➕ Ajouter Formateur",
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover,
                Width = 170,
                Height = 40,
                Left = 0,
                Top = 35
            };
            btnAddTrainer.Click += (s, e) =>
            {
                FormAjoutFormateur form = new FormAjoutFormateur(_gestionCentre);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshTrainersDataGrid();
                }
            };
            topBar.Controls.Add(btnAddTrainer);

            RoundedButton btnDeleteTrainer = new RoundedButton
            {
                Text = "🗑️ Supprimer",
                NormalColor = ThemeConfig.Colors.Error,
                HoverColor = ThemeConfig.Colors.ErrorHover,
                Width = 120,
                Height = 40,
                Left = 180,
                Top = 35
            };
            btnDeleteTrainer.Click += (s, e) => DeleteSelectedTrainer();
            topBar.Controls.Add(btnDeleteTrainer);

            tab.Controls.Add(topBar);

            Panel searchBar = CreateSearchBar("Rechercher formateur...", (searchTerm) =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    RefreshTrainersDataGrid();
                }
                else
                {
                    var filtered = _gestionCentre.Formateurs
                        .Where(f => f.Nom.ToLower().Contains(searchTerm.ToLower()) ||
                                   f.Prenom.ToLower().Contains(searchTerm.ToLower()))
                        .ToList();
                    PopulateTrainersDataGrid(filtered);
                }
            });
            searchBar.Height = 60;
            searchBar.Dock = DockStyle.Top;
            tab.Controls.Add(searchBar);

            _dgvFormateurs = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            _dgvFormateurs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "ID", Width = 60 });
            _dgvFormateurs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Nom", HeaderText = "Nom", Width = 120 });
            _dgvFormateurs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Prenom", HeaderText = "Prénom", Width = 120 });
            _dgvFormateurs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Age", HeaderText = "Âge", Width = 60 });
            _dgvFormateurs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Specialite", HeaderText = "Spécialité", Width = 150 });
            _dgvFormateurs.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Salaire", HeaderText = "Salaire", Width = 100 });

            ThemeConfig.StyleDataGridView(_dgvFormateurs);
            _dgvFormateurs.CellDoubleClick += (s, e) => EditSelectedTrainer();
            tab.Controls.Add(_dgvFormateurs);

            RefreshTrainersDataGrid();
            return tab;
        }

        private TabPage CreateModulesTab()
        {
            TabPage tab = new TabPage { BackColor = ThemeConfig.Colors.BackgroundMain };

            Panel topBar = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = ThemeConfig.Colors.CardBackground,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            Label lblTitle = new Label
            {
                Text = "📚 Liste des Modules",
                Font = ThemeConfig.Fonts.TitleFont,
                ForeColor = ThemeConfig.Colors.TextDark,
                Left = 0,
                Top = 5,
                AutoSize = true
            };
            topBar.Controls.Add(lblTitle);

            RoundedButton btnAddModule = new RoundedButton
            {
                Text = "➕ Ajouter Module",
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover,
                Width = 150,
                Height = 40,
                Left = 0,
                Top = 35
            };
            btnAddModule.Click += (s, e) =>
            {
                FormAjoutModule form = new FormAjoutModule(_gestionCentre);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshModulesDataGrid();
                }
            };
            topBar.Controls.Add(btnAddModule);

            RoundedButton btnDeleteModule = new RoundedButton
            {
                Text = "🗑️ Supprimer",
                NormalColor = ThemeConfig.Colors.Error,
                HoverColor = ThemeConfig.Colors.ErrorHover,
                Width = 120,
                Height = 40,
                Left = 160,
                Top = 35
            };
            btnDeleteModule.Click += (s, e) => DeleteSelectedModule();
            topBar.Controls.Add(btnDeleteModule);

            tab.Controls.Add(topBar);

            Panel searchBar = CreateSearchBar("Rechercher module...", (searchTerm) =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    RefreshModulesDataGrid();
                }
                else
                {
                    var filtered = _gestionCentre.Modules
                        .Where(m => m.Libelle.ToLower().Contains(searchTerm.ToLower()) ||
                                   m.Code.ToLower().Contains(searchTerm.ToLower()))
                        .ToList();
                    PopulateModulesDataGrid(filtered);
                }
            });
            searchBar.Height = 60;
            searchBar.Dock = DockStyle.Top;
            tab.Controls.Add(searchBar);

            _dgvModules = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            _dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Code", HeaderText = "Code", Width = 100 });
            _dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Libelle", HeaderText = "Libellé", Width = 250 });
            _dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Duree", HeaderText = "Durée (h)", Width = 80 });
            _dgvModules.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Coefficient", HeaderText = "Coefficient", Width = 100 });

            ThemeConfig.StyleDataGridView(_dgvModules);
            _dgvModules.CellDoubleClick += (s, e) => EditSelectedModule();
            tab.Controls.Add(_dgvModules);

            RefreshModulesDataGrid();
            return tab;
        }

        private TabPage CreateGradesTab()
        {
            TabPage tab = new TabPage { BackColor = ThemeConfig.Colors.BackgroundMain };

            Panel topBar = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = ThemeConfig.Colors.CardBackground,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            Label lblTitle = new Label
            {
                Text = "📝 Gestion des Notes",
                Font = ThemeConfig.Fonts.TitleFont,
                ForeColor = ThemeConfig.Colors.TextDark,
                Left = 0,
                Top = 5,
                AutoSize = true
            };
            topBar.Controls.Add(lblTitle);

            RoundedButton btnAddGrade = new RoundedButton
            {
                Text = "➕ Ajouter Note",
                NormalColor = ThemeConfig.Colors.Success,
                HoverColor = ThemeConfig.Colors.SuccessHover,
                Width = 140,
                Height = 40,
                Left = 0,
                Top = 35
            };
            btnAddGrade.Click += (s, e) =>
            {
                FormNotes form = new FormNotes(_gestionCentre);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    // Refresh notes table
                }
            };
            topBar.Controls.Add(btnAddGrade);

            tab.Controls.Add(topBar);

            Label lblNotes = new Label
            {
                Text = "Veuillez utiliser le formulaire 'Ajouter Note' pour gérer les notes des étudiants.",
                Font = ThemeConfig.Fonts.DefaultFont,
                ForeColor = ThemeConfig.Colors.TextSecondary,
                Left = 15,
                Top = 100,
                AutoSize = true
            };
            tab.Controls.Add(lblNotes);

            return tab;
        }

        private TabPage CreateStatsTab()
        {
            TabPage tab = new TabPage { BackColor = ThemeConfig.Colors.BackgroundMain };

            Panel topBar = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = ThemeConfig.Colors.CardBackground,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(15)
            };

            Label lblTitle = new Label
            {
                Text = "📊 Statistiques",
                Font = ThemeConfig.Fonts.TitleFont,
                ForeColor = ThemeConfig.Colors.TextDark,
                Left = 0,
                Top = 5,
                AutoSize = true
            };
            topBar.Controls.Add(lblTitle);

            tab.Controls.Add(topBar);

            RoundedButton btnViewStats = new RoundedButton
            {
                Text = "📊 Voir Statistiques Complètes",
                NormalColor = ThemeConfig.Colors.Secondary,
                HoverColor = ThemeConfig.Colors.Primary,
                Width = 220,
                Height = 40,
                Left = 15,
                Top = 100
            };
            btnViewStats.Click += (s, e) =>
            {
                FormStatistiques form = new FormStatistiques(_gestionCentre);
                form.ShowDialog();
            };
            tab.Controls.Add(btnViewStats);

            return tab;
        }

        private Panel CreateSearchBar(string placeholder, Action<string> onSearch)
        {
            Panel searchPanel = new Panel
            {
                BackColor = ThemeConfig.Colors.CardBackground,
                Padding = new Padding(15)
            };

            TextBox txtSearch = new TextBox
            {
                PlaceholderText = "🔍 " + placeholder,
                Width = 400,
                Height = 35,
                Font = ThemeConfig.Fonts.DefaultFont,
                Left = 0,
                Top = 12
            };
            ThemeConfig.StyleTextBox(txtSearch);

            txtSearch.TextChanged += (s, e) => onSearch?.Invoke(txtSearch.Text);

            searchPanel.Controls.Add(txtSearch);
            return searchPanel;
        }

        private void RefreshStudentsDataGrid()
        {
            PopulateStudentsDataGrid(_gestionCentre.Etudiants);
        }

        private void PopulateStudentsDataGrid(List<Etudiant> students)
        {
            _dgvEtudiants!.DataSource = null;
            _dgvEtudiants!.DataSource = students.Select(e => new
            {
                e.Id,
                e.Nom,
                e.Prenom,
                e.Age,
                e.Matricule,
                e.Niveau
            }).ToList();
        }

        private void RefreshTrainersDataGrid()
        {
            PopulateTrainersDataGrid(_gestionCentre.Formateurs);
        }

        private void PopulateTrainersDataGrid(List<Formateur> trainers)
        {
            _dgvFormateurs!.DataSource = null;
            _dgvFormateurs!.DataSource = trainers.Select(f => new
            {
                f.Id,
                f.Nom,
                f.Prenom,
                f.Age,
                f.Specialite,
                Salaire = $"{f.Salaire:N0} FCFA"
            }).ToList();
        }

        private void RefreshModulesDataGrid()
        {
            PopulateModulesDataGrid(_gestionCentre.Modules ?? new List<Module>());
        }

        private void PopulateModulesDataGrid(List<Module> modules)
        {
            _dgvModules!.DataSource = null;
            _dgvModules!.DataSource = modules.Select(m => new
            {
                m.Code,
                m.Libelle,
                m.Duree,
                m.Coefficient
            }).ToList();
        }

        private void DeleteSelectedStudent()
        {
            if (_dgvEtudiants!.CurrentRow == null)
            {
                Toast.ShowWarning("Veuillez sélectionner un étudiant à supprimer");
                return;
            }

            int id = (int)_dgvEtudiants!.CurrentRow.Cells[0].Value;
            var student = _gestionCentre.Etudiants.FirstOrDefault(x => x.Id == id);

            if (student != null)
            {
                var result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer '{student.Nom} {student.Prenom}' ?\n\nCette action est irréversible.",
                    "⚠️ Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    _gestionCentre.Etudiants.Remove(student);
                    Sauvegarde.SauvegarderEtudiants(_gestionCentre.Etudiants);
                    Toast.ShowSuccess($"Étudiant '{student.Nom}' supprimé avec succès");
                    RefreshStudentsDataGrid();
                }
            }
        }

        private void DeleteSelectedTrainer()
        {
            if (_dgvFormateurs!.CurrentRow == null)
            {
                Toast.ShowWarning("Veuillez sélectionner un formateur à supprimer");
                return;
            }

            int id = (int)_dgvFormateurs!.CurrentRow.Cells[0].Value;
            var trainer = _gestionCentre.Formateurs.FirstOrDefault(x => x.Id == id);

            if (trainer != null)
            {
                var result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer '{trainer.Nom} {trainer.Prenom}' ?\n\nCette action est irréversible.",
                    "⚠️ Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    _gestionCentre.Formateurs.Remove(trainer);
                    Sauvegarde.SauvegarderFormateurs(_gestionCentre.Formateurs);
                    Toast.ShowSuccess($"Formateur '{trainer.Nom}' supprimé avec succès");
                    RefreshTrainersDataGrid();
                }
            }
        }

        private void DeleteSelectedModule()
        {
            if (_dgvModules!.CurrentRow == null)
            {
                Toast.ShowWarning("Veuillez sélectionner un module à supprimer");
                return;
            }

            string code = _dgvModules!.CurrentRow.Cells[0].Value?.ToString() ?? string.Empty;
            var module = _gestionCentre.Modules.FirstOrDefault(x => x.Code == code);

            if (module != null)
            {
                var result = MessageBox.Show(
                    $"Êtes-vous sûr de vouloir supprimer le module '{module.Code} - {module.Libelle}' ?\n\nCette action est irréversible.",
                    "⚠️ Confirmation de suppression",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.Yes)
                {
                    _gestionCentre.Modules.Remove(module);
                    Sauvegarde.SauvegarderModules(_gestionCentre.Modules);
                    Toast.ShowSuccess($"Module '{module.Code}' supprimé avec succès");
                    RefreshModulesDataGrid();
                }
            }
        }

        private void EditSelectedTrainer()
        {
            if (_dgvFormateurs!.CurrentRow == null)
            {
                Toast.ShowWarning("Veuillez sélectionner un formateur à modifier");
                return;
            }

            int id = (int)_dgvFormateurs!.CurrentRow.Cells[0].Value;
            var trainer = _gestionCentre.Formateurs.FirstOrDefault(x => x.Id == id);

            if (trainer != null)
            {
                FormAjoutFormateur form = new FormAjoutFormateur(_gestionCentre, trainer);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshTrainersDataGrid();
                    Toast.ShowSuccess($"Formateur '{trainer.Nom}' modifié avec succès");
                }
            }
        }

        private void EditSelectedModule()
        {
            if (_dgvModules!.CurrentRow == null)
            {
                Toast.ShowWarning("Veuillez sélectionner un module à modifier");
                return;
            }

            string code = _dgvModules!.CurrentRow.Cells[0].Value?.ToString() ?? string.Empty;
            var module = _gestionCentre.Modules.FirstOrDefault(x => x.Code == code);

            if (module != null)
            {
                FormAjoutModule form = new FormAjoutModule(_gestionCentre, module);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshModulesDataGrid();
                    Toast.ShowSuccess($"Module '{module.Code}' modifié avec succès");
                }
            }
        }

        private void ExportStudentPDF()
        {
            if (_dgvEtudiants!.CurrentRow == null)
            {
                Toast.ShowWarning("Veuillez sélectionner un étudiant pour exporter son bulletin");
                return;
            }

            int id = (int)_dgvEtudiants.CurrentRow.Cells[0].Value;
            var student = _gestionCentre.Etudiants.FirstOrDefault(x => x.Id == id);

            if (student != null)
            {
                try
                {
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        Title = "Sauvegarder le bulletin de notes",
                        FileName = $"Bulletin_{student.Nom}_{student.Prenom}.pdf"
                    };

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        MigraDocDocument document = new MigraDocDocument();
                        MigraDocSection section = document.AddSection();
                        section.PageSetup = document.DefaultPageSetup.Clone();
                        section.PageSetup.PageFormat = MigraDocPageFormat.A4;

                        MigraDocParagraph title = section.AddParagraph($"Bulletin de Notes");
                        title.Format.Font.Size = 20;
                        title.Format.Font.Bold = true;
                        title.Format.Alignment = MigraDocParagraphAlignment.Center;
                        title.Format.SpaceAfter = "1cm";

                        MigraDocParagraph studentInfo = section.AddParagraph();
                        studentInfo.AddFormattedText("Étudiant: ", MigraDocTextFormat.Bold);
                        studentInfo.AddText($"{student.Nom} {student.Prenom}\n");
                        studentInfo.AddFormattedText("Matricule: ", MigraDocTextFormat.Bold);
                        studentInfo.AddText($"{student.Matricule}\n");
                        studentInfo.AddFormattedText("Niveau: ", MigraDocTextFormat.Bold);
                        studentInfo.AddText($"{student.Niveau}\n");
                        studentInfo.AddFormattedText("Moyenne Générale: ", MigraDocTextFormat.Bold);
                        studentInfo.AddText($"{student.CalculerMoyenne():F2}/20\n");
                        studentInfo.Format.SpaceAfter = "0.5cm";

                        if (student.Notes.Count > 0)
                        {
                            MigraDocTable table = section.AddTable();
                            table.Borders.Width = 0.5;
                            table.AddColumn(MigraDocUnit.FromCentimeter(5));
                            table.AddColumn(MigraDocUnit.FromCentimeter(5));
                            table.AddColumn(MigraDocUnit.FromCentimeter(3));
                            table.AddColumn(MigraDocUnit.FromCentimeter(3));

                            MigraDocRow headerRow = table.AddRow();
                            headerRow.Shading.Color = MigraDocColors.LightBlue;
                            headerRow.Cells[0].AddParagraph("Module").Format.Font.Bold = true;
                            headerRow.Cells[1].AddParagraph("Code").Format.Font.Bold = true;
                            headerRow.Cells[2].AddParagraph("Note").Format.Font.Bold = true;
                            headerRow.Cells[3].AddParagraph("Coefficient").Format.Font.Bold = true;

                            foreach (var note in student.Notes)
                            {
                                MigraDocRow row = table.AddRow();
                                row.Cells[0].AddParagraph(note.Module.Libelle);
                                row.Cells[1].AddParagraph(note.Module.Code);
                                row.Cells[2].AddParagraph($"{note.Valeur:F2}");
                                row.Cells[3].AddParagraph(note.Module.Coefficient.ToString());
                            }
                        }
                        else
                        {
                            section.AddParagraph("Aucune note enregistrée pour cet étudiant.");
                        }

                        PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);
                        renderer.Document = document;
                        renderer.RenderDocument();
                        renderer.PdfDocument.Save(saveDialog.FileName);

                        Toast.ShowSuccess($"Bulletin exporté avec succès: {saveDialog.FileName}");
                    }
                }
                catch (Exception ex)
                {
                    Toast.ShowError($"Erreur lors de l'export PDF: {ex.Message}");
                }
            }
        }

        private void ExportStudentsExcel()
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    Title = "Sauvegarder la liste des étudiants",
                    FileName = "Liste_Etudiants.xlsx"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Étudiants");

                        worksheet.Cell("A1").Value = "ID";
                        worksheet.Cell("B1").Value = "Nom";
                        worksheet.Cell("C1").Value = "Prénom";
                        worksheet.Cell("D1").Value = "Âge";
                        worksheet.Cell("E1").Value = "Matricule";
                        worksheet.Cell("F1").Value = "Niveau";
                        worksheet.Cell("G1").Value = "Moyenne";

                        var headerRange = worksheet.Range("A1:G1");
                        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                        headerRange.Style.Font.Bold = true;

                        int row = 2;
                        foreach (var student in _gestionCentre.Etudiants)
                        {
                            worksheet.Cell(row, 1).Value = student.Id;
                            worksheet.Cell(row, 2).Value = student.Nom;
                            worksheet.Cell(row, 3).Value = student.Prenom;
                            worksheet.Cell(row, 4).Value = student.Age;
                            worksheet.Cell(row, 5).Value = student.Matricule;
                            worksheet.Cell(row, 6).Value = student.Niveau;
                            worksheet.Cell(row, 7).Value = student.CalculerMoyenne();
                            row++;
                        }

                        worksheet.Columns().AdjustToContents();
                        workbook.SaveAs(saveDialog.FileName);

                        Toast.ShowSuccess($"Liste exportée avec succès: {saveDialog.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.ShowError($"Erreur lors de l'export Excel: {ex.Message}");
            }
        }

        private Button CreateSidebarButton(string text, int left, int top, string tag)
        {
            Button btn = new Button
            {
                Text = text,
                Left = left,
                Top = top,
                Width = 180,
                Height = 45,
                BackColor = ThemeConfig.Colors.SidebarDark,
                ForeColor = ThemeConfig.Colors.TextLight,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Font = ThemeConfig.Fonts.BoldFont,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0),
                Tag = tag
            };

            btn.MouseEnter += (s, e) => btn.BackColor = ThemeConfig.Colors.Primary;
            btn.MouseLeave += (s, e) => btn.BackColor = ThemeConfig.Colors.SidebarDark;

            return btn;
        }

        private void ApplyModernTheme()
        {
            this.BackColor = ThemeConfig.Colors.BackgroundMain;
            this.Font = ThemeConfig.Fonts.DefaultFont;
        }
    }
}
