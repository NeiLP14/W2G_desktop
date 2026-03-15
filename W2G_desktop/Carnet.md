# Carnet de bord - Projet W2G Desktop

## 1. Initialisation du projet

* Création d’un projet **C# WPF** nommé `W2G_desktop`.
* Mise en place de la base de données **MySQL** avec les tables essentielles : `user`, `bay`, `offer`, `reservation`.
* Ajout des références nécessaires pour l'accès à MySQL via `MySql.Data`.
* Organisation des dossiers et namespaces : `Models`, `Services`, `Pages`.

---

## 2. Modèle de données

* **User**

  * `Id` (int)
  * `Email` (string)
  * `Username` (string)
  * `Role` (string, JSON)
  * `Discr` (string, discriminer type utilisateur : `admin`, `technician`, `accountant`, `customer`, `company`)

* **Bay**

  * `Id` (int)
  * `Label` (string, généré automatiquement : `Bay 1`, `Bay 2`, …)
  * `Size` (int)

* **Offer**

  * `Id` (int)
  * `Label` (string)
  * `NbUnit` (int)
  * `Price` (decimal)
  * `Reduction` (int, pourcentage)

* **Reservation**

  * `Id` (int)
  * `UserId` (int)
  * `OffreId` (int)
  * `DateDeb` (DateTime)
  * `DateFin` (DateTime)

---

## 3. Services métier

* **UserService**

  * Authentification sécurisée avec hash BCrypt.
  * Vérification de l’existence d’un email.
  * Création d’un utilisateur avec hashage du mot de passe et assignation automatique du `discr` selon le rôle.
  * Récupération des clients (`customer` ou `company`) ou tous les utilisateurs (admin).
  * Mise à jour d’un utilisateur existant.
  * Suppression d’un utilisateur et de ses réservations associées (transaction sécurisée).

* **BayService**

  * Récupération des baies depuis la base.
  * Création d’une baie avec label automatique.
  * Mise à jour et suppression d’une baie.

* **OfferService**

  * Récupération des offres existantes.
  * Création d’une offre via formulaire admin.
  * Mise à jour et suppression d’offres.

* **ReservationService**

  * Récupération de toutes les réservations.
  * Récupération des réservations par utilisateur.
  * Création, modification et suppression de réservations.
  * Suppression en cascade lors de la suppression d’un utilisateur.

---

## 4. Architecture de l’interface

* **MainWindow** avec un `Frame` pour naviguer entre les pages.

* Navigation centralisée avec boutons :

  * `Créer un utilisateur` (admin)
  * `Liste des clients`
  * `Réservations`
  * `Offres` (création visible seulement pour admin)
  * `Baies` (création visible seulement pour admin)

* Pages principales :

  * `LoginPage` – connexion utilisateur
  * `UsersPage` – liste et gestion des utilisateurs (Create/Edit/Delete)
  * `CreateUserPage` – formulaire de création utilisateur
  * `EditUserPage` – modification des informations utilisateur
  * `CustomersPage` – liste des clients uniquement
  * `ReservationsPage` – gestion des réservations
  * `OffersPage` – liste et gestion des offres
  * `CreateOffrePage` – formulaire de création d’offre
  * `BaysPage` – liste et gestion des baies
  * `CreateBayPage` – création d’une baie avec label automatique

* **MainWindow** :

  * Mise à jour dynamique des boutons selon le rôle via `UpdateMenu()`
  * Titre de la fenêtre indiquant l’utilisateur connecté et son rôle

---

## 5. Flux utilisateur

* **Connexion** :

  * Page `LoginPage` affichée si aucun utilisateur connecté.
  * Vérification des identifiants avec hash BCrypt.
  * Mise à jour de la navigation et du titre après connexion.

* **Actions utilisateur** :

  * Tous les utilisateurs connectés peuvent accéder aux clients, offres, baies et réservations (lecture).
  * L’admin peut créer, modifier et supprimer des utilisateurs, offres et baies.
  * L’admin peut visualiser tous les utilisateurs, pas seulement les clients.

* **Navigation fluide** via le `Frame` principal pour changer de page.

---

## 6. Gestion des utilisateurs

* **Liste des utilisateurs** (`UsersPage`) :

  * Affiche tous les utilisateurs (admin, technician, accountant, customer, company)
  * Boutons : `Créer`, `Modifier`, `Supprimer`
  * Sélection dans le `DataGrid` active `Modifier` et `Supprimer`

* **Création utilisateur** (`CreateUserPage`) :

  * Rôle sélectionnable via `ComboBox`
  * Discrim automatique selon rôle
  * MessageBox de confirmation ou TextBlock d’erreur

* **Modification utilisateur** (`EditUserPage`) :

  * Modification des informations et rôle
  * Mise à jour automatique de la base

* **Suppression utilisateur** :

  * Confirmation via MessageBox
  * Suppression en cascade des réservations associées

---

## 7. Gestion des offres

* **OffersPage** :

  * DataGrid avec `Id`, `Label`, `NbUnit`, `Price`, `Reduction`
  * Création, modification et suppression d’offres pour admin
  * Validation des champs avec messages d’erreur

---

## 8. Gestion des baies

* **BaysPage** :

  * DataGrid avec `Id`, `Label`, `Size`
  * Création d’une baie avec label automatique
  * Modification et suppression pour admin

---

## 9. Gestion des réservations

* Liste des réservations pour tous les utilisateurs.
* Possibilité de filtrer par utilisateur.
* Création, modification et suppression de réservations.
* Suppression en cascade si l’utilisateur est supprimé.

---

## 10. Gestion des erreurs et messages

* Messages d’erreur clairs dans les formulaires :

  * Login : mauvais identifiants
  * Création/édition utilisateur, offre, baie : champs vides ou invalides
* MessageBox pour confirmation et succès.
* Validation côté client avant insertion dans la base.

---

## 11. Points techniques clés

* Séparation logique métier / interface utilisateur (`Services` vs `Pages`).
* Navigation centralisée via `Frame` dans `MainWindow`.
* Gestion des rôles et permissions côté UI et service.
* Uniformisation des `DataGrid` pour tous les modules (colonnes et marges).
* Transactions pour suppression cascade sécurisée.
* Génération automatique des labels pour les baies.
* Hashage des mots de passe avec BCrypt.

---

## 12. Améliorations futures possibles

* Validation plus poussée des champs (regex email, mot de passe fort).
* Pagination et recherche dans les DataGrid pour grandes tables.
* Dashboard et statistiques après connexion.
* Gestion multi-langue / localisation.
* Gestion avancée des logs et suivi des actions.
* Gestion dynamique des rôles et permissions (table séparée).
* Notifications ou alertes pour certaines actions (réservations proches, etc.).
* Export CSV / PDF des listes (utilisateurs, offres, baies, réservations).

---

## 13. Gestion avancée des baies et unités

* **Création d’une baie** (`CreateBayPage`) :

  * Génération automatique du `Label` (`B01`, `B02`, …) basé sur le nombre de baies existantes.
  * Création automatique des **unités** (`Unit`) associées à la baie, numérotées de `U01` jusqu’à la taille totale de la baie.
  * Chaque unité possède : `Position`, `BayId`, `IsOccupied` (booléen), `StateId`, `TypeId`, et optionnel `ReservationId`.

* **Modification d’une baie** (`EditBayPage`) :

  * Possibilité d’augmenter ou de réduire la taille.
  * Si la taille augmente : création automatique des unités manquantes.
  * Si la taille diminue : suppression impossible des unités occupées, sinon suppression des unités vides en trop.
  * Validation et messages clairs si l’action est bloquée (MessageBox).

* **Affichage des unités** (`UnitsPage`) :

  * Liste les unités d’une baie sélectionnée avec :

    * `Position`
    * `Label` (ex : U01, U02…)
    * `IsOccupied` (booléen, lecture seule)
    * Nom de l’utilisateur qui occupe l’unité si applicable.
  * Possibilité de **scroll** vertical dans le tableau pour baies de grande taille.
  * Bouton “Voir les unités” déplacé **en bas** du tableau pour simplifier l’interface.
  * Activation du bouton uniquement si une baie est sélectionnée.

* **BaysPage** :

  * Boutons de modification et de visualisation des unités visibles uniquement pour les **admins**.
  * Boutons désactivés ou masqués pour les utilisateurs standards.
  * Sélection dans le `DataGrid` active dynamiquement les boutons `Modifier` et `Voir les unités`.

---

## 14. Gestion des offres (permissions)

* **OffresPage** (`OffresPage`) :

  * Boutons `Créer`, `Modifier`, `Supprimer` visibles uniquement pour les **admins**.
  * Les boutons `Modifier` et `Supprimer` sont activés uniquement si une offre est sélectionnée.
  * Correction des appels vers `CreateOffrePage` et `EditOfferPage` avec passage du `User` courant pour les permissions.
  * Les offres sont récupérées via `OfferService.GetAllOffers()`.
  * Validation côté interface avant création ou modification d’une offre.
  * MessageBox de confirmation lors de la suppression.

---

## 15. Permissions et UI dynamique

* Tous les boutons liés aux actions sensibles (Créer/Modifier/Supprimer) **s’adaptent automatiquement** au rôle de l’utilisateur (`ROLE_ADMIN`).
* Les utilisateurs standards peuvent seulement **consulter** les listes :

  * Baies (sans modifier ni supprimer)
  * Offres (sans modifier ni supprimer)
  * Réservations
  * Clients
* Les admins voient et peuvent interagir avec tous les boutons disponibles.