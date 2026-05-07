// Placeholder: sauvegarde/lecture fichiers JSON
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using CentreFormation.Models;
using System.Linq;

namespace CentreFormation.Services
{
    /// <summary>
    /// Classe statique responsable de la persistance des données au format JSON.
    /// Gère l'écriture et la lecture des fichiers pour les différentes entités du système.
    /// </summary>
    public static class Sauvegarde
    {
        // Chemins absolus vers les fichiers de stockage JSON basés sur le répertoire de l'application
        private static string basePath = AppDomain.CurrentDomain.BaseDirectory;
        private static string cheminFormateurs = Path.Combine(basePath, "Data", "formateurs.json");
        private static string cheminModules = Path.Combine(basePath, "Data", "modules.json");
        private static string cheminEtudiants = Path.Combine(basePath, "Data", "etudiants.json");
        private static string cheminNotes = Path.Combine(basePath, "Data", "notes.json");

        // Configuration du sérialiseur : indentation, insensibilité à la casse et gestion des références circulaires
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            MaxDepth = 64
        };

        /// <summary>
        /// Sérialise et enregistre la liste des formateurs sur le disque.
        /// </summary>
        public static void SauvegarderFormateurs(List<Formateur> liste)
        {
            string json = JsonSerializer.Serialize(liste, _options);
            File.WriteAllText(cheminFormateurs, json);
        }

        /// <summary>
        /// Charge les formateurs depuis le fichier JSON. Retourne une liste vide si le fichier n'existe pas.
        /// </summary>
        public static List<Formateur> ChargerFormateurs()
        {
            if (!File.Exists(cheminFormateurs))
                return new List<Formateur>();

            string json = File.ReadAllText(cheminFormateurs);

            return JsonSerializer.Deserialize<List<Formateur>>(json, _options)
                   ?? new List<Formateur>();
        }

        /// <summary>
        /// Sérialise et enregistre la liste des modules.
        /// </summary>
        public static void SauvegarderModules(List<Module> liste)
        {
            string json = JsonSerializer.Serialize(liste, _options);
            File.WriteAllText(cheminModules, json);
        }

        /// <summary>
        /// Charge les modules depuis le stockage disque.
        /// </summary>
        public static List<Module> ChargerModules()
        {
            if (!File.Exists(cheminModules))
                return new List<Module>();

            string json = File.ReadAllText(cheminModules);

            return JsonSerializer.Deserialize<List<Module>>(json, _options)
                   ?? new List<Module>();
        }

        /// <summary>
        /// Sérialise et enregistre la liste des étudiants.
        /// </summary>
        public static void SauvegarderEtudiants(List<Etudiant> liste)
        {
            string json = JsonSerializer.Serialize(liste, _options);
            File.WriteAllText(cheminEtudiants, json);
        }

        /// <summary>
        /// Charge les étudiants depuis le fichier JSON.
        /// </summary>
        public static List<Etudiant> ChargerEtudiants()
        {
            if (!File.Exists(cheminEtudiants))
                return new List<Etudiant>();

            string json = File.ReadAllText(cheminEtudiants);

            return JsonSerializer.Deserialize<List<Etudiant>>(json, _options)
                   ?? new List<Etudiant>();
        }

        /// <summary>
        /// Objet de transfert de données (DTO) pour simplifier la structure des notes dans le fichier JSON
        /// en utilisant des identifiants plutôt que des objets complets.
        /// </summary>
        private class NoteDto
        {
            public int EtudiantId { get; set; }
            public string ModuleCode { get; set; } = string.Empty;
            public double Valeur { get; set; }
        }

        /// <summary>
        /// Convertit les objets Note en NoteDto pour une sauvegarde simplifiée.
        /// </summary>
        public static void SauvegarderNotes(List<Note> notes)
        {
            var dtos = notes.Select(n => new NoteDto
            {
                EtudiantId = n.Etudiant?.Id ?? 0,
                ModuleCode = n.Module?.Code ?? string.Empty,
                Valeur = n.Valeur
            }).ToList();

            string json = JsonSerializer.Serialize(dtos, _options);
            File.WriteAllText(cheminNotes, json);
        }

        /// <summary>
        /// Charge les notes et reconstruit les liens d'objets avec les étudiants et modules déjà chargés.
        /// </summary>
        public static List<Note> ChargerNotes(List<Etudiant> etudiants, List<Module> modules)
        {
            if (!File.Exists(cheminNotes))
                return new List<Note>();

            string json = File.ReadAllText(cheminNotes);

            List<NoteDto> dtos;
            try
            {
                dtos = JsonSerializer.Deserialize<List<NoteDto>>(json, _options) ?? new List<NoteDto>();
            }
            catch
            {
                return new List<Note>();
            }

            var notes = new List<Note>();

            // Reconstruction des associations (Relinking)
            foreach (var dto in dtos)
            {
                var et = etudiants.FirstOrDefault(e => e.Id == dto.EtudiantId);
                var mod = modules.FirstOrDefault(m => m.Code == dto.ModuleCode);
                if (et != null && mod != null)
                {
                    try
                    {
                        notes.Add(new Note(et, mod, dto.Valeur));
                        et.AjouterNote(notes.Last());
                    }
                    catch
                    {
                        // ignore les valeurs de notes invalides
                    }
                }
            }

            return notes;
        }
    }
}
