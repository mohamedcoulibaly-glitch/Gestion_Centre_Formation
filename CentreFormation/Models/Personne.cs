/// <summary>
/// Classe de base représentant une personne au sein du centre de formation.
/// C'est la classe parente pour Etudiant et Formateur.
/// Elle contient les propriétés communes : Id, Nom, Prénom et Age
/// </summary>
using System;

namespace CentreFormation.Models
{
    public class Personne
    {
        private int _age;

        /// <summary>
        /// Compteur statique : suivi du nombre total de personnes créées
        /// </summary>
        private static int _nombreTotal = 0;

        /// <summary>
        /// Identifiant unique de la personne
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nom de la personne
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prénom de la personne
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Age de la personne avec validation
        /// Ne peut pas être négatif
        /// </summary>
        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("L'âge ne peut pas être négatif.");
                }

                _age = value;
            }
        }

        /// <summary>
        /// Propriété statique : retourne le nombre total de personnes créées
        /// Utile pour les statistiques du centre
        /// </summary>
        public static int NombreTotal
        {
            get { return _nombreTotal; }
        }

        /// <summary>
        /// Constructeur de la classe Personne
        /// Initialise tous les champs et incrémente le compteur total
        /// </summary>
        public Personne(int id, string nom, string prenom, int age)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Age = age;

            _nombreTotal++;
        }

        /// <summary>
        /// Méthode virtuelle pour afficher les détails de la personne
        /// Peut être surchargée par les classes dérivées (Etudiant, Formateur)
        /// </summary>
        public virtual string AfficherDetails()
        {
            return $"ID : {Id} - {Nom} {Prenom} - Age : {Age}";
        }
    }
}