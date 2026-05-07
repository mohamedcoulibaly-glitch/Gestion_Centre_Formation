/// <summary>
/// Classe Module : représente un module de formation du centre
/// Contient les informations pédagogiques : Code, Libellé, Durée et Coefficient
/// Le coefficient est utilisé pour le calcul de la moyenne pondérée des étudiants
/// </summary>
using System;

namespace CentreFormation.Models
{
    public class Module
    {
        /// <summary>
        /// Override du ToString pour un affichage court et lisible
        /// Format: "Libellé (Code)"
        /// </summary>
        public override string ToString()
        {
            return $"{Libelle} ({Code})";
        }

        /// <summary>
        /// Durée du module en heures avec validation
        /// Doit être positive (> 0)
        /// </summary>
        private int _duree;

        /// <summary>
        /// Compteur statique : suivi du nombre total de modules créés
        /// </summary>
        private static int _nombreModules = 0;

        /// <summary>
        /// Code unique du module
        /// Exemple: "WEB101", "BDD202", etc.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Libellé/nom complet du module
        /// Exemple: "Développement Web Avancé"
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Durée du module en heures avec validation
        /// Ne peut pas être zéro ou négatif
        /// </summary>
        public int Duree
        {
            get { return _duree; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La durée doit être supérieure à 0.");
                }

                _duree = value;
            }
        }

        /// <summary>
        /// Coefficient du module pour le calcul de moyenne pondérée
        /// Plus le coefficient est élevé, plus la note compte dans la moyenne
        /// Exemple: coefficient 3 pour un module important, 1 pour un module optionnel
        /// </summary>
        public int Coefficient { get; set; }

        /// <summary>
        /// Propriété statique : retourne le nombre total de modules créés
        /// </summary>
        public static int NombreModules
        {
            get { return _nombreModules; }
        }

        /// <summary>
        /// Constructeur de la classe Module
        /// Initialise tous les champs et incrémente le compteur total
        /// </summary>
        public Module(string code, string libelle, int duree, int coefficient)
        {
            Code = code;
            Libelle = libelle;
            Duree = duree;
            Coefficient = coefficient;

            _nombreModules++;
        }
    }
}