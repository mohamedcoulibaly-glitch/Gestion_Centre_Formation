/// <summary>
/// Classe Note : représente une évaluation attribuée à un étudiant pour un module
/// Établit le lien entre un étudiant, un module et sa note (0-20)
/// </summary>
using System;

namespace CentreFormation.Models
{
    public class Note
    {
        /// <summary>
        /// Valeur de la note avec validation
        /// Doit être comprise entre 0 et 20 (échelle standard française)
        /// </summary>
        private double _valeur;

        /// <summary>
        /// Référence à l'étudiant qui a reçu cette note
        /// </summary>
        public Etudiant Etudiant { get; set; }

        /// <summary>
        /// Référence au module pour lequel l'étudiant a été évalué
        /// </summary>
        public Module Module { get; set; }

        /// <summary>
        /// Valeur numérique de la note avec validation
        /// Validation: La note doit être entre 0 et 20 inclus
        /// </summary>
        public double Valeur
        {
            get { return _valeur; }
            set
            {
                if (value < 0 || value > 20)
                {
                    throw new ArgumentException("La note doit être comprise entre 0 et 20.");
                }

                _valeur = value;
            }
        }

        /// <summary>
        /// Constructeur de la classe Note
        /// Associe un étudiant à une note pour un module spécifique
        /// </summary>
        public Note(Etudiant etudiant, Module module, double valeur)
        {
            Etudiant = etudiant;
            Module = module;
            Valeur = valeur;
        }
    }
}