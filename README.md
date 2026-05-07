# 📚 Gestion Centre de Formation

## 1. 👥 Qui sommes-nous ?

Ce projet a été réalisé par :
- **Mohamed COULIBALY**
- **Babacar SENE**

Étudiants en classe de **STIC2**

---

## 2. 🚀 Comment lancer le projet ?

### Prérequis
- **.NET 8.0** ou supérieur installé
- **Visual Studio 2022** ou **Visual Studio Code** avec les extensions C# nécessaires

### Étapes pour lancer l'application

1. **Ouvrir le fichier solution** :
   - Naviguez vers le dossier `CentreFormation/`
   - Ouvrez le fichier **`CentreFormation.sln`** avec Visual Studio

2. **Démarrer l'application** :
   - Appuyez sur **`F5`** ou cliquez sur le bouton **"Démarrer"** (Play)
   - Alternative : `Ctrl + F5` pour lancer sans le débogage

3. **Via la ligne de commande** :
   ```bash
   dotnet run
   ```

---

## 3. ✨ Fonctionnalités implémentées

L'application de gestion d'un centre de formation offre les fonctionnalités suivantes :

### 📋 Gestion des Étudiants
- Ajouter un nouvel étudiant
- Afficher la liste des étudiants
- Modifier les informations d'un étudiant
- Supprimer un étudiant
- Gestion complète avec persistance des données

### 👨‍🏫 Gestion des Formateurs
- Ajouter un formateur
- Afficher la liste des formateurs
- Modifier les informations des formateurs
- Supprimer un formateur
- Conservation des données en base

### 📖 Gestion des Modules
- Ajouter un module de formation
- Gérer les modules disponibles
- Modifier les détails des modules
- Supprimer des modules

### 📊 Gestion des Notes
- Enregistrer les notes des étudiants
- Consulter les performances des étudiants
- Générer des statistiques et rapports

### 📈 Statistiques et Rapports
- Visualiser les statistiques avec des graphiques interactifs (LiveChartsCore)
- Générer des rapports en PDF
- Exporter les données vers Excel

### 💾 Sauvegarde et Persistance
- Sauvegarde automatique des données en JSON
- Chargement des données au démarrage
- Gestion sécurisée de la persistance des données

### 🎨 Interface Utilisateur moderne
- Design moderne avec thème personnalisé
- Boutons personnalisés avec coins arrondis
- Champs de texte stylisés
- Notifications de type "Toast"

---

## 4. 📦 Dépendances

L'application utilise les packages NuGet suivants :

| Package | Version | Description |
|---------|---------|-------------|
| **LiveChartsCore.SkiaSharpView.WinForms** | 2.0.0-rc2 | Bibliothèque pour créer des graphiques interactifs modernes |
| **SkiaSharp.Views.WindowsForms** | 2.88.8 | Support du rendu graphique SkiaSharp pour WinForms |
| **PdfSharp.MigraDoc.Standard** | 1.51.15 | Génération de documents PDF |
| **ClosedXML** | 0.104.2 | Création et manipulation de fichiers Excel |

### Framework
- **.NET 8.0 (Windows Forms)** : Framework moderne et performant pour les applications desktop

---

## 5. 📝 Remarques particulières

Ce projet a été **passionnant à créer** ! C'était notre **première expérience** avec une **application desktop complète en C# WinForms**, et nous avons beaucoup appris lors de son développement :

✅ Maîtrise des concepts fondamentaux de la POO avec C#  
✅ Création d'interfaces graphiques avec Windows Forms  
✅ Gestion de la persistance des données en JSON  
✅ Intégration de bibliothèques externes (graphiques, PDF, Excel)  
✅ Création d'une application professionnelle et fonctionnelle  

Cette expérience nous a permis de :
- Comprendre l'architecture d'une application desktop
- Mettre en pratique les bonnes pratiques de programmation
- Découvrir l'écosystème .NET et ses outils puissants
- Collaborer efficacement en équipe

Le projet démontre une bonne compréhension des principes de base du développement d'applications et constitue une base solide pour des projets plus complexes à l'avenir. 🎓

---

## 📂 Structure du projet

```
CentreFormation/
├── Forms/              # Formulaires WinForms
├── Models/             # Modèles de données
├── Services/           # Services métier
├── UI/                 # Composants UI personnalisés
├── Data/               # Fichiers de données JSON
└── Program.cs          # Point d'entrée de l'application
```

---

**Merci de votre intérêt pour ce projet ! 🙏**
