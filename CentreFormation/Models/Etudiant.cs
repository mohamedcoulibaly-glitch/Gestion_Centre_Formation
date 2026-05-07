/// <summary>
/// Classe Etudiant : représente un étudiant du centre de formation
/// Hérite de Personne et ajoute les propriétés académiques :
/// - Matricule (numéro d'immatriculation unique)
/// - Niveau (L1, L2, L3, M1, M2)
/// - Gestion des notes (moyenne pondérée par les coefficients)
/// </summary>
using System.Text.Json.Serialization;
using CentreFormation.Models;

namespace CentreFormation.Models
{
    public class Etudiant : Personne  // hérite de Personne pour réutiliser Id, Nom, Prénom, Age
    {
        /// <summary>
        /// Override du ToString pour un affichage court et lisible
        /// Format: "Nom Prénom (Matricule)"
        /// </summary>
        public override string ToString()
        {
            return $"{Nom} {Prenom} ({Matricule})";
        }

        // ============================================
        // CHAMPS PRIVÉS
        // ============================================

        /// <summary>
        /// Numéro d'immatriculation unique de l'étudiant
        /// Identifie l'étudiant dans les registres du centre
        /// </summary>
        private string _matricule = string.Empty;

        /// <summary>
        /// Niveau d'étude : L1, L2, L3, M1, M2
        /// L = Licence, M = Master
        /// </summary>
        private string _niveau = string.Empty;

        /// <summary>
        /// Collection des notes obtenues par cet étudiant
        /// Permet de calculer sa moyenne générale
        /// </summary>
        private List<Note> _notes;

        /// <summary>
        /// Compteur statique : suivi du nombre total d'étudiants créés
        /// </summary>
        private static int _nombreEtudiants = 0;

        // ============================================
        // CONSTRUCTEUR
        // ============================================

        /// <summary>
        /// Constructeur de la classe Etudiant
        /// Initialise toutes les propriétés
        /// Appelle le constructeur parent Personne via base()
        /// Crée une liste vide de notes au démarrage
        /// Incrémente le compteur total d'étudiants
        /// </summary>
        public Etudiant(int id, string nom, string prenom, int age, string matricule, string niveau)
            : base(id, nom, prenom, age)  // appelle le constructeur de Personne
        {
            Matricule = matricule;
            Niveau = niveau;
            _notes    = new List<Note>(); // liste vide au départ
            _nombreEtudiants++; // incrémente le compteur
        }

        // ============================================
        // PROPRIÉTÉS
        // ============================================

        /// <summary>
        /// Propriété Matricule avec validation
        /// Ne peut pas être vide ou null
        /// </summary>
        public string Matricule
        {
            get => _matricule;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le matricule ne peut pas être vide.");
                _matricule = value;
            }
        }

        /// <summary>
        /// Propriété Niveau avec validation
        /// Doit être un niveau d'études valide (L1, L2, L3, M1, M2)
        /// Ne peut pas être vide
        /// </summary>
        public string Niveau
        {
            get => _niveau;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Le niveau ne peut pas être vide.");
                _niveau = value;
            }
        }

        /// <summary>
        /// Collection des notes de l'étudiant
        /// [JsonIgnore] : exclut cette collection de la sérialisation JSON
        /// pour éviter les références circulaires
        /// </summary>
        [JsonIgnore]
        public List<Note> Notes => _notes;

        /// <summary>
        /// Propriété statique : retourne le nombre total d'étudiants créés
        /// </summary>
        public static int NombreEtudiants
        {
            get { return _nombreEtudiants; }
        }

        // ============================================
        // MÉTHODES
        // ============================================

        /// <summary>
        /// Ajoute une note à la liste des notes de l'étudiant
        /// Utilisé lors de l'enregistrement d'une nouvelle évaluation
        /// </summary>
        public void AjouterNote(Note note)
        {
            _notes.Add(note);
        }

        /// <summary>
        /// Calcule la moyenne pondérée des notes de l'étudiant
        /// La moyenne tient compte du coefficient de chaque module
        /// Formule: (Σ note × coefficient) / Σ coefficient
        /// Retourne 0 si aucune note n'existe
        /// </summary>
        public double CalculerMoyenne()
        {
            if (_notes == null || _notes.Count == 0) return 0;

            double totalPoints      = 0;
            double totalCoefficient = 0;

            foreach (Note note in _notes)
            {
                totalPoints      += note.Valeur * note.Module.Coefficient;
                totalCoefficient += note.Module.Coefficient;
            }

            if (totalCoefficient == 0) return 0;

            return Math.Round(totalPoints / totalCoefficient, 2);
        }

        /// <summary>
        /// Redéfinit la méthode AfficherDetails() de la classe parent Personne
        /// Ajoute les informations académiques : Matricule, Niveau et Moyenne générale
        /// </summary>
        public override string AfficherDetails()
        {
            return base.AfficherDetails() 
                + $" | Matricule: {_matricule} | Niveau: {_niveau} | Moyenne: {CalculerMoyenne()}/20";
        }
    }
}