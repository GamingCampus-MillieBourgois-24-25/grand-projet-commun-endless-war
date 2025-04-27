# Technical Design Document (TDD) pour C# Unity
## Document d'en tête 
- **Titre de Projet:** Endless War
- **Studio:** Axentra Games
- **Version:** 2.0
- **Date:** 2025-04-27
- **Auteurs:** [[GORSSE Camille](https://github.com/Jehyann)] [[MONTES Julen](https://github.com/JulenMYT)] [[DUBOIS Florentin](https://github.com/fduboisCG)] [[CARDOSO Florian](https://github.com/Portos2004)] [[FERRANDEZ-TARTARIN Noémie](https://github.com/Nonorrs)]
- **Contact:** [cgorsse@gaming.tech] [jmontes@gaming.tech] [fdubois@gaming.tech] [fcardoso@gaming.tech] [nferrandez-tartarin@gaming.tech]

## Historique des versions

| Date          | Version | Description                | Auteurs       |
|---------------|---------|----------------------------|--------------|
| 2025-02-21    | 1.0     | Technical Design Document | Axentra Games |
| 2025-04-27    | 2.0     | Technical Design Document | Axentra Games |

## Sommaire
1. [Introduction](#1-introduction)
2. [Outils, environnement et déploiement](#2-outils-environnement-et-déploiement)
3. [Architecture et conception du système](#3-architecture-et-conception-du-système)
4. [Performances et optimisation](#4-performances-et-optimisation)
5. [Gestion des Erreurs et Stabilité](#5-gestion-des-erreurs-et-stabilité)
6. [Interface Design](#6-interface-design)
7. [Features](#7-features)
8. [Technique](#8-technique)
9. [Timeline du projet](9#-timeline-du-projet)

---
## 1. Introduction
### 1.1 Intention
Ce document décrit la conception technique du projet en C# Unity, détaillant son architecture, ses modules et son approche de développement piloté par les tests (TDD).

### 1.2 Étendue
- **Objectif :** Développer un jeu mobile pour le rendu, la physique, l'audio et la gestion des entrées.
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
- **IDE recommandés**: Visual Studio.
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
### 2.3 Contrôle des versions
- Utilisez Github pour le contrôle de version.
- Utilisation de Github Desktop pour gérer le projet plus facilement
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
- **Facilité de développement**: C# est un langage accessible et performant pour les jeux mobiles.
Unity propose un éditeur intuitif avec du drag & drop et des outils comme Timeline pour l’animation et Cinemachine pour les caméras.
- **Support et communauté**: Grande communauté avec beaucoup de tutoriels et forums d’entraide.
Documentation bien fournie et mises à jour régulières avec les dernières technologies mobiles.
- ### 3.4 Pourquoi pas Unreal
Ce moteur est conçu pour les jeux avec un rendu ultra détaillé ce qui le rend moins optimisé pour le marché du jeu mobile à cause des performances plus basses des téléphones.
Notre équipe étant aussi beaucoup plus à l'aise sur Unity, a fait que le choix d'utiliser ce moteur plutôt qu'Unreal plus évident.

Exemple différence de graphisme sur un jeu (Infinity Nikki) développé sur Unreal entre le mobile et le pc:
![Image d'Infinity Nikki pour démontrer la différence de graphisme entre mobile et pc](/docs/image.png)
---
## 4. Performances et optimisation
### 4.1 Objectifs de performance
Étant donné que le jeu est destiné aux appareils mobiles, plusieurs optimisations sont nécessaires pour garantir une fluidité à 60 FPS, une consommation minimale de batterie, et une stabilité sur une large gamme de téléphones (entrée de gamme à haut de gamme).
### 4.2 Optimisation Graphique
- **Sprites et UI optimisés**: Utilisation de Sprite Atlases pour limiter les appels de rendu.
- **Shaders mobiles**: Privilégier des shaders simples et optimisés pour réduire l’impact sur le GPU.
- **Pas d’ombres dynamiques sur mobile**: Utilisation de Lightmaps pour alléger le calcul d’éclairage.
- **Effets visuels légers**: Particle Systems limités en nombre et optimisés en durée de vie et nombre de particules.
### 4.3 Optimisation des Physiques et des Collisions
- Désactiver les collisions inutiles avec des Layers dédiés (éviter que les projectiles et ennemis testent les collisions entre eux).
- Rigidbody en mode Kinematic lorsque la physique n’est pas nécessaire (ex : ennemis qui se déplacent vers le joueur sans interaction avec le décor).
- Triggers plutôt que collisions physiques pour la détection des objets récupérables et attaques à distance.
### 4.4 Gestion des Spawns et Optimisation des Ennemis
- **Pooling des ennemis et projectiles**: Au lieu d’instancier et détruire en boucle, on réutilise les objets via un Object Pool pour éviter le garbage collection fréquent.
- **Suppression des ennemis hors écran**: Désactivation automatique des ennemis trop éloignés du joueur pour alléger le CPU.
- **IA simplifiée**: Pas de pathfinding complexe, les ennemis se contentent d’un déplacement basique vers le joueur.
- **Spawners du joueur**:
- **Spawners des objets**:
### 4.5 Optimisation des Scripts et du CPU
- **Moins d'Update()**: Éviter d’utiliser Update() dans chaque script. Favoriser les événements et coroutines pour exécuter du code seulement quand c’est nécessaire.
- **FixedUpdate pour la physique uniquement**: Réduire la fréquence de FixedUpdate (Time.fixedDeltaTime) pour économiser du CPU.
### 4.6 Optimisation Audio
- Limiter les AudioSources actives en simultané.
- **Compresser les sons**: Effets sonores en WAV.
---
## 5. Gestion des Erreurs et Stabilité
Un jeu mobile doit être robuste et éviter les crashs ou bugs frustrants.
- Pooling des objets pour éviter les allocations mémoire inutiles.
- Éviter les grosses allocations en temps réel :
- Privilégier les listes pré-allouées (List<T>.Capacity).
---
## 6. Interface Design
### 6.1 Interfaces internes
- Définissez des API claires entre les modules à l'aide de classes ou d'interfaces abstraites.
### 6.2 API externes et formats de fichiers
- Prise en charge des formats de fichiers standards: CS (scripts), FBX (models), PNG  (textures), WAV (audio).
---
## 7. Features
### 7.1 3C
- Le jeu adopte une vue du dessus, avec une caméra qui suit le joueur en permanence, le maintenant au centre de l’écran.
- Au début de chaque partie, le joueur choisit:
  * **Sa classe**: Les classes influencent le style de jeu avec des compétences passives uniques et des bonus spécifiques.
Chaque classe affecte les compétences disponibles lors des montées de niveau.
### 7.2 Terrain
- Le terrain est une surface texturée sur laquelle le joueur et les ennemis peuvent se déplacer librement.
### 7.3 Joueur
- Le joueur évolue en 3D sur un vaste terrain et peut se déplacer librement. Son attaque est automatique, au corps à corps ou à distance selon son équipement.
- Pour se déplacer, l’utilisateur pose son doigt sur l’écran à gauche, faisant apparaître un stick virtuel qu’il peut bouger à sa guise.
- Pour viser, l’utilisateur pose son doigt sur l’écran à droite, faisant apparaître un stick virtuel qu’il peut bouger à sa guise.
- En éliminant des ennemis, le joueur gagne de l’expérience. À chaque montée de niveau, il peut choisir parmi une liste de compétences, lui permettant d’améliorer ses statistiques ou d’obtenir de nouvelles armes pour diversifier son arsenal.
- La partie prend fin si les points de vie du joueur tombent à zéro ou si le joueur bat les 10 vagues d’ennemis.
### 7.4 Ennemis
- Les ennemis en 3D se déplacent vers le joueur et lui infligent des dégâts au contact ou à distance. Leur comportement est simple et prévisible, mais leur nombre croissant les rend rapidement menaçants. Leur résistance augmente progressivement, rendant leur élimination de plus en plus difficile.
- Ils apparaissent à une certaine distance autour du joueur, tentant de l’encercler pour limiter ses possibilités d’évasion.
- Au fil des vagues, le nombre d’ennemis augmente pour maintenir un niveau de défi constant.
- Périodiquement, un boss redoutable fait son apparition, intensifiant le combat et représentant une menace plus grande.
- Lorsqu’ils sont vaincus, les ennemis laissent derrière eux une gemme d’expérience. La valeur de cette gemme dépend de la dangerosité de l’ennemi éliminé.
### 7.5 HUD
- Barre de vie : Située sous le joueur, elle indique son état de santé. Si elle se vide, il perd la partie et doit recommencer.
- Barre d’expérience : Affichée en haut de l’écran, elle permet de visualiser la progression jusqu’au prochain niveau.
- Image du personnage : Affichée en haut de l'écran, elle représente le personnage choisi par le joueur.
- Bouton de paramètres : Affiché en haut de l'écran, permet de mettre en pause le jeu.
### 7.6 Attaques
- Les attaques sont des scriptable object.
- Elles sont dépendantes du personnage choisi lors du lancement d'une partie et sont déblocables au fur et  à mesure d'une partie. Elles peuvent être améliorées.
### 7.7 Tooltips
- Les Tooltips sont des scriptable object.
- Ils permettent d'expliquer les fonctionnalités du jeu (ex: les gemmes, combat, règle, etc...).
- Ils sont visibles une fois puis ne réapparaitront plus.
### 7.8 SkillTree
- Avant de lancer une partie et si on a suffisamment d'argent, on peut aller améliorer ses statistiques tel que:
  *  Dégâts
  * Vitesse
  * Soin
  * Portée de détection des gemmes
  * PV
 - On récupère de l'argent lors des parties, les ennemies ont une chance de lâcher de l'argent lorsqu'ils sont tués.
### 7.9 LevelUp
- Lorsque la barre d'expérience est remplie au maximum, un menu pour obtenir une nouvelle attaque apparait. 
- On a la possibilité d'en choisir une parmi 4.
- On peut voir ce que fait une attaque grâce à un bouton qui ouvre une fenêtre explicative.
- On peut aussi voir les noms et les images représentant les attaques. 
 ---
## 8. Technique
### 8.1 Inputs
- Le système d’inputs utilisé sera l’Input System de Unity, qui offre une gestion plus flexible et moderne des contrôles par rapport à l'ancien système (Input Manager). Il permet de mieux gérer les entrées multi-dispositifs, comme l’écran tactile pour les mobiles, les manettes ou encore le clavier et la souris pour d’éventuelles adaptations futures.
- Dans ce jeu, l’Input System sera principalement utilisé pour :
    * Gérer les sticks virtuels qui permettent au joueur de se déplacer et viser en touchant l’écran.
    * Détecter les interactions et les éventuels menus du jeu.
### 8.2 Audio
- Le jeu utilisera un format pour les fichiers audio : les fichiers .wav seront employés pour la musique du jeu, les effets sonores et les bruitages.
- Unity intégrera ces fichiers via AudioSource et AudioClip, avec un Volume Manager pour ajuster les volumes des différents éléments sonores (musique, effets, interface). 
### 8.3 Physique
- Le moteur physique de Unity sera utilisé pour gérer les collisions et les interactions entre les objets du jeu.
- **Rigidbody**: Chaque entité dynamique (joueur, ennemis, projectiles) possédera un Rigidbody pour être affectée par la physique du jeu.
- **Colliders**: Les objets auront des colliders adaptés (BoxCollider, SphereCollider, CapsuleCollider) afin de détecter les contacts et déclencher les interactions (dégâts, ramassage de gemmes).
- **Layers & Triggers**: Une optimisation sera faite en définissant des layers spécifiques pour limiter les calculs de collisions inutiles et en utilisant des triggers pour les interactions (comme ramasser une gemme sans collision physique réelle).
### 8.4 Rendering
- Le moteur de rendu de Unity sera utilisé pour afficher les éléments du jeu:
- **Modèles 3D**: Le joueur, les ennemis et le décor seront modélisés en 3D et rendus en temps réel.
- **Sprites & UI**: Les éléments de l’interface utilisateur (HUD, barres de vie et d’expérience, menus) seront affichés via le système UI Canvas de Unity.
- **Éclairage**: Un éclairage simple sera utilisé pour optimiser les performances sur mobile, avec éventuellement des Lightmaps pour pré-calculer certaines ombres et alléger le rendu.
- **Effets visuels**: Des Particle Systems pourront être ajoutés pour des effets comme des explosions, des impacts ou des auras d’expérience autour des gemmes.
### 8.5 Shader
- Dans notre jeu, nous avons 2 shaders:
   * Un shader pour une attaque (une boule de feu)
   * Un shader pour rendre la tilemap moins répétitive en créant des imperfections et en changeant la couleur de la tilemap.
### 8.6 Sauvegarde des Scores & Progression
- Fichiers JSON pour sauvegarder: 
    * Personnage et classe choisis.
    * Niveau atteint, compétences débloquées.
    * Tutoriels visionnés.
---
## 9. Timeline du projet
- **Étape 1:** Préprod
- **Étape 2:** Prototype du projet
- **Étape 3:** Alpha-test
- **Étape 4:** Amélioration des features
- **Étape 5:** Bêta-test

