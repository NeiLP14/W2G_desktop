# Carnet de bord - Projet W2G Desktop

## 1. Initialisation du projet

* Création d’un projet **C# WPF** nommé `W2G_desktop`.
* Mise en place de la base de données **MySQL** avec les tables essentielles : `user` et `bay`.
* Ajout des références nécessaires pour l'accès à MySQL via `MySql.Data`.
* Organisation des dossiers et namespaces : `Models`, `Services`, `Pages`.

---

## 2. Modèle de données

* Modèle `User` avec propriétés :

  * `Id` (int)
  * `Email` (string)
  * `Username` (string)
  * `Role` (string)
  * `Discr` (string, pour discriminer type utilisateur)

* Modèle `Bay` (non détaillé ici) pour gérer les emplacements (bays) affichés dans un DataGrid.

---

## 3. Services métier

* **UserService**

  * Authentification sécurisée avec comparaison du hash BCrypt du mot de passe.
  * Vérification de l’existence d’un email.
  * Création d’un utilisateur avec hashage du mot de passe.
  * Récupération des clients (rôle `customer` ou `company`).

* **BayService**

  * Récupération des bays depuis la base (implémentation simple).

---

## 4. Architecture de l’interface

* Passage d’une architecture basée sur plusieurs **Windows** vers une interface moderne basée sur une seule `MainWindow` avec un **Frame** pour naviguer entre différentes **Pages**.
* Pages principales développées :

  * `LoginPage` (page de connexion)
  * `CreateUserPage` (formulaire création utilisateur)
  * `CustomersPage` (liste clients)
* `MainWindow` :

  * Contient le frame de navigation (`MainFrame`).
  * Gère l’état utilisateur connecté.
  * Affiche ou cache les boutons "Créer un utilisateur" et "Liste des clients" selon le rôle (bouton "Créer un utilisateur" visible uniquement pour `ROLE_ADMIN`).
  * Met à jour le titre de la fenêtre en fonction de l’utilisateur connecté.

---

## 5. Flux utilisateur

* Au lancement, `MainWindow` affiche la page `LoginPage` si aucun utilisateur connecté.
* Après connexion valide (seuls rôles `ADMIN`, `TECHNICIAN`, `ACCOUNTANT` autorisés), la fenêtre met à jour :

  * Boutons visibles/cachés selon rôle.
  * Affiche par défaut la page `CustomersPage`.
* Navigation possible via les boutons pour aller vers la liste des clients ou vers la page de création d’utilisateur.
* La création d’un utilisateur ne peut être réalisée que par un admin.

---

## 6. Gestion des erreurs et messages

* Messages d’erreur clairs dans le formulaire de connexion : mauvais identifiants, accès refusé.
* Messages d’erreur dans la création utilisateur (ex : email déjà utilisé, champs vides).
* Utilisation de `MessageBox` pour confirmation de création réussie.

---

## 7. Problèmes rencontrés et solutions

* **Erreur `InitializeComponent` non trouvé** → vérification des espaces de noms, des noms de classes et du XAML liés.
* **Ambiguïté sur la classe `User`** → utilisation stricte des namespaces, import de `W2G_desktop.Models`.
* **Erreur de ressource XAML introuvable** → vérification de la structure du projet et propriétés des fichiers `.xaml` (Build Action = `Page`).
* **Erreur constructeur manquant sur `MainWindow`** → ajout d’un constructeur sans paramètre obligatoire pour le XAML.
* **Gestion de la visibilité des boutons selon rôle** dans `MainWindow_Loaded` et méthode publique `SetCurrentUser`.
* **Migration de `Window` vers `Page` pour login et création utilisateur** pour faciliter la navigation dans le frame principal.

---

## 8. Points techniques clés et bonnes pratiques

* Séparation claire de la logique métier (Services) et UI (Pages/Windows).
* Utilisation du pattern navigation dans un `Frame` pour une UI fluide et modulaire.
* Gestion explicite des rôles et permissions côté UI.
* Sécurisation des mots de passe avec BCrypt.
* Passage d’informations entre pages/fenêtres via constructeur ou méthodes publiques.
* Gestion des erreurs côté client pour améliorer l’expérience utilisateur.
* Mise en place d’un système de navigation cohérent et évolutif.

---

## 9. Améliorations futures possibles

* Ajout de validation plus poussée côté formulaire (ex : regex email, force mot de passe).
* Implémentation de la gestion des sessions ou tokens pour améliorer la sécurité.
* Ajout d’une page d’accueil après connexion avec résumé/dashboards.
* Gestion des rôles plus dynamique (ex : interface admin pour modifier les rôles).
* Ajout de logs d’activité et gestion des erreurs plus avancée.
* Support multi-langue/localisation.