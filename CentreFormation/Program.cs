/// <summary>
/// Point d'entrée de l'application de gestion du centre de formation
/// Initialise le contexte WinForms et lance la fenêtre principale FormMain
/// </summary>
using System;
using System.Windows.Forms;
using CentreFormation.Forms;

namespace CentreFormation
{
    internal static class Program
    {
        /// <summary>
        /// Méthode Main : point d'entrée de l'application
        /// [STAThread] : Single-Threaded Apartment, requis pour Windows Forms
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Active les styles visuels modernes du système d'exploitation
            Application.EnableVisualStyles();

            // Utilise le rendu de texte par défaut pour plus de compatibilité
            Application.SetCompatibleTextRenderingDefault(false);

            // Lance la boucle d'événements de l'application avec FormMain comme fenêtre principale
            Application.Run(new FormMain());
        }
    }
}