# Technical Design Document (TDD) pour C# Unity
## Document d'en tête 
- **Titre de Projet:** Endless War
- **Studio:** Axentra Games
- **Version:** 1.0
- **Date:** 2025-02-21
- **Auteurs:** [[GORSSE Camille](https://github.com/Jehyann)] [[MONTES Julen](https://github.com/JulenMYT)] [[DUBOIS Florentin](https://github.com/fduboisCG)] [[CARDOSO Florian](https://github.com/Portos2004)] [[FERRANDEZ-TARTARIN Noémie](https://github.com/Nonorrs)]
- **Contact:** [cgorsse@gaming.tech] [jmontes@gaming.tech] [fdubois@gaming.tech] [fcardoso@gaming.tech] [nferrandez-tartarin@gaming.tech]

## Historique des versions

| Date          | Version | Description                | Auteurs       |
|---------------|---------|----------------------------|--------------|
| 2025-02-21    | 1.0     | Technical Design Document | Axentra Games |

## Sommaire
1. [Introduction](#1-introduction)
2. [Outils, environnement et déploiement](#2-outils-environnement-et-déploiement)
3. [Architecture et conception du système](#3-architecture-et-conception-du-système)
4. [Performances et optimisation](#4-performances-et-optimisation)




---
## 1. Introduction
### 1.1 Intention
Ce document décrit la conception technique du projet en C# Unity, détaillant son architecture, ses modules et son approche de développement piloté par les tests (TDD).

### 1.2 Étendue
- **Objectif :** Développer un jeu mobile pour le rendu, la physique, l'audio et la gestion des entrées.
- **Application :** Développement de jeux en temps réel et projets académiques.

### 1.3 Définitions, Acronymes et Abbréviations
- **TDD:** Test-Driven Development (Développement piloté par les tests)
- **API:** Application Programming Interface (Interface de programmation d'applications)
- **FPS:** Frames Per Second (Images par seconde)
- **IDE:** Integrated Development Environment (Environnement de développement intégré)


### 1.4 Références
- [Documentation Unity](https://docs.unity3d.com/2022.3/Documentation/Manual/index.html)
- [Documentation Visual Studio](https://learn.microsoft.com/fr-fr/visualstudio/windows/?view=vs-2022)
- [Documentation Github](https://docs.github.com/en)

### 1.5 Aperçu du document 
Ce TDD détaille la conception, les interactions des modules et les stratégies de test du jeu, garantissant ainsi la clarté de l'architecture de haut niveau aux détails d'implémentation de bas niveau.
---
## 2. Outils, environnement et déploiement
### 2.1 Outils de développement et IDE
- Éditeurs de code prenant en charge les fonctionnalités C#.
- **IDE recommandés**: Visual Studio or VSCode.
- **Moteur de jeu**: Unity 2022.3
- **Processeur**: Intel Core i5 10ᵉ génération / AMD Ryzen 5 ou supérieur
- **Nombre de cœurs CPU**: 4 minimum (8 recommandé)
- **Mémoire RAM**: 8 Go minimum (16 Go recommandé)
- **Carte Graphique(GPU)**: Minimum : Nvidia GTX 1050 / AMD Radeon RX 560
- **Stockage**: 10 Go d’espace libre (SSD recommandé pour de meilleures performances)
- **Système d’exploitation**: Windows 10/11
### 2.2 Logiciels et API nécessaires
- **Moteur de jeu**: Unity (version LTS recommandée, ex. Unity 2022 LTS)
- **SDK Android**: Android SDK installé via Unity Hub
- **API graphique**: OpenGL ES 3.1 / Vulkan 1.1
- **Outil de développement**: Unity Remote
### 2.3 Contrôle des versions
- Utilisez Github pour le contrôle de version.
- Adoptez une stratégie de branchement claire pour le développement de fonctionnalités.
### 2.4 Environnement de déploiement
- **Plateformes cibles**: Android, IOS.
- **Système d’exploitation**: Android 7.0 (Nougat) ou supérieur
- **Processeur**: Qualcomm Snapdragon 665 / Mediatek Helio P60 ou équivalent
- **Nombre de cœurs CPU**: 4 minimum
- **Mémoire RAM**: 3 Go
- **GPU**: Adreno 610 / Mali-G72 MP3 ou équivalent
- **Stockage libre**: 1,5 Go
- **Écran**: 720p minimum
- ---
## 3. Architecture et conception du système
### 3.1 Répartition des modules
- **Rendering Module:** Le moteur de rendu de Unity sera utilisé pour afficher les éléments du jeu.
- **Physics Module:** Le moteur physique de Unity sera utilisé pour gérer les collisions et les interactions entre les objets du jeu.
- **Audio Module:** Le jeu utilisera différents formats audio pour optimiser la qualité et la performance.
- **Input Module:** Le système d’inputs utilisé sera l’Input System de Unity, qui offre une gestion plus flexible et moderne des contrôles par rapport à l'ancien système (Input Manager). Il permet de mieux gérer les         entrées multi-dispositifs, comme l’écran tactile pour les mobiles, les manettes ou encore le clavier et la souris pour d’éventuelles adaptations futures.
### 3.2 Diagrammes d'interaction
#### Main Loop
```mermaid
graph TD;
    A[Lancer le jeu] --> B[Choisir un personnage];
    B --> C[Choisir la classe du personnage];
    C --> D[GameLoop];
    D --> E[Fin de partie quand le joueur n’a plus de point de vie ou qu’il bat l’ennemi final];
    E --> F[Écran de récompense];
    F --> G[Relancer ou quitter];
```
#### Game Loop
```mermaid
graph TD;
    A[Éliminer des ennemis] --> B[Récupérer de l’expérience];
    B --> C[Gagner en niveau];
    C --> D[Amélioration];
```

### 3.3 Pourquoi Unity
- **Compatibilité multiplateforme**: Unity permet d’exporter facilement un jeu sur Android, iOS et d’autres plateformes sans devoir réécrire le code. Cela facilite le développement cross-platform, réduisant le temps et les coûts.
- **Optimisation des performances**: Unity propose des outils avancés comme Adaptive Performance pour ajuster la qualité graphique en fonction du matériel.
Le Scriptable Render Pipeline (URP) permet d’optimiser les graphismes pour les appareils mobiles.
Support de Vulkan et OpenGL ES 3.1, offrant de meilleures performances graphiques sur Android.
- **Taille et gestion des ressources**: Unity propose des compressions avancées (ex. ASTC, ETC2) pour réduire la taille du jeu sans trop impacter la qualité.
Le Addressables System optimise le chargement des assets pour éviter une consommation excessive de mémoire.
- **Large choix d’outils et de plugins**: Unity dispose du Unity Asset Store, qui propose des plugins pour l'UI, l'animation, l’IA et bien plus encore.
Intégration facile avec des SDK populaires comme Google Play Services, Firebase, AdMob et Facebook SDK pour la monétisation et l’analyse.
- **Facilité de développement**: C# est un langage accessible et performant pour les jeux mobiles.
Unity propose un éditeur intuitif avec du drag & drop et des outils comme Timeline pour l’animation et Cinemachine pour les caméras.
- **Support et communauté**: Grande communauté avec beaucoup de tutoriels et forums d’entraide.
Documentation bien fournie et mises à jour régulières avec les dernières technologies mobiles.
- **Gestion de la monétisation**: Unity Ads et Unity IAP (In-App Purchases) sont directement intégrés, facilitant la monétisation des jeux mobiles.
- ### 3.4 Pourquoi pas Unreal
Ce moteur est conçu pour les jeux avec un rendu ultra détaillé ce qui le rend moins optimisé pour le marché du jeu mobile à cause des performances plus basses des téléphones.

Exemple différence de graphisme sur un jeu (Infinity Nikki) développé sur Unreal entre le mobile et le pc:
![Image d'Infinity Nikki pour démontrer la différence de graphisme entre mobile et pc](/docs/image.png)
---
## 4. Performances et optimisation
### 4.1 Performance Goals
Étant donné que le jeu est destiné aux appareils mobiles, plusieurs optimisations sont nécessaires pour garantir une fluidité à 60 FPS, une consommation minimale de batterie, et une stabilité sur une large gamme de téléphones (entrée de gamme à haut de gamme).
### 4.2 Profiling and Benchmarking
- Integrate profiling tools such as Valgrind or Visual Studio Profiler.
- Include benchmarking tests as part of the TDD suite.
### 4.3 Optimization Techniques
- Use object pooling and memory management best practices.
- Implement batching and frustum culling in the rendering process.
---

