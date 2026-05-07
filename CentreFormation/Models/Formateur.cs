/// <summary>
/// Classe Formateur : représente un formateur du centre de formation
/// Hérite de Personne et ajoute les propriétés professionnelles :
/// - Spécialité (domaine d'expertise)
/// - ModuleEnseigne (module principal enseigné)
/// - Salaire mensuel
/// </summary>
using System;

namespace CentreFormation.Models
{
    public class Formateur : Personne
    {
        /// <summary>
        /// Salaire mensuel du formateur
        /// Stocké en format decimal pour la précision financière
        /// </summary>
        private decimal _salaire;

        /// <summary>
        /// Spécialité professionnelle du formateur
        /// Exemple: "Développement Web", "Bases de Données", etc.
        /// </summary>
        public string Specialite { get; set; }

        /// <summary>
        /// Module principal enseigné par ce formateur
        /// </summary>
        public string ModuleEnseigne { get; set; }

        /// <summary>
        /// Propriété Salaire avec validation
        /// Le salaire doit être strictement positif
        /// </summary>
        public decimal Salaire
        {
            get { return _salaire; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Le salaire doit être supérieur à 0.");
                }

                _salaire = value;
            }
        }

        /// <summary>
        /// Constructeur de la classe Formateur
        /// Initialise toutes les propriétés du formateur
        /// Appelle le constructeur parent Personne
        /// </summary>
        public Formateur(int id, string nom, string prenom, int age,
                          string specialite, string moduleEnseigne, decimal salaire)
            : base(id, nom, prenom, age)
        {
            Specialite = specialite;
            ModuleEnseigne = moduleEnseigne;
            Salaire = salaire;
        }

        /// <summary>
        /// Calcule le salaire annuel du formateur
        /// Basé sur un contrat de 12 mois
        /// </summary>
        public decimal CalculerSalaireAnnuel()
        {
            return Salaire * 12;
        }

        /// <summary>
        /// Redéfinit la méthode AfficherDetails() de la classe parent
        /// Ajoute les informations professionnelles : Spécialité, Module et Salaire
        /// </summary>
        public override string AfficherDetails()
        {
            return base.AfficherDetails() +
                   $" - Spécialité : {Specialite} - Module : {ModuleEnseigne} - Salaire : {Salaire}";
        }
    }
}