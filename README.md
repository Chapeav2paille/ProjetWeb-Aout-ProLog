# ProManLog — Gestion d'une entreprise de transport

Application de gestion (prestations, clients, flotte, ressources humaines) développée
en **ASP.NET Core 10** (backend) et **Angular 21** (frontend), suivant les principes
de la **Clean Architecture**, avec **Dapper** pour l'accès aux données et **SQLite**
comme SGBD.

---

## Fonctionnalités

Le projet propose **8 fonctionnalités distinctes** :

1. **Authentification** : connexion par JWT, protection de toutes les routes de
   l'API et du frontend, déconnexion.
2. **Tableau de bord** : indicateurs clés du mois (prestations en cours et
   terminées, chiffre d'affaires, état de la flotte), chacun détaillé dans un
   tableau, graphique du chiffre d'affaires sur six mois, alertes d'échéances
   (entretiens, contrôles techniques, assurances et permis expirant sous
   30 jours) et cinq dernières activités.
3. **Prestations** : CRUD complet, filtre par statut et changement de statut
   directement depuis la liste.
4. **Clients** : CRUD complet (entreprise ou particulier) et fiche client
   affichant l'historique de ses prestations.
5. **Flotte** : CRUD complet des véhicules avec suivi du kilométrage, des
   entretiens, des contrôles techniques et des assurances.
6. **Ressources humaines** : CRUD complet des employés, avec informations de
   permis pour les chauffeurs, statut et disponibilité.
7. **Chiffres** : récapitulatif comptable sur six mois sous forme de tableau :
   chiffre d'affaires, charges fixes détaillées (salaire, frais, loyer, facture,
   essence), TVA encaissée, TVA payée, TVA à reverser et résultat avant impôt,
   avec une ligne de total.
8. **Historique** : journalisation automatique des opérations, filtrable par
   période et par type d'action, avec un code couleur : ajout en vert,
   modification en bleu, suppression en rouge et changement de statut en orange.

---

## Prérequis

| Outil | Version utilisée | Vérification |
|---|---|---|
| .NET SDK | **10.0.300** | `dotnet --version` |
| Node.js | **24.15.0** | `node --version` |
| npm | **11.12.1** | `npm --version` |
| Angular (framework et CLI) | **21.2** | `ng version` |

> **Base de données** : SQLite — aucune installation de serveur n'est nécessaire.
> Le fichier `Database/promanlog.db` est créé et rempli automatiquement au premier
> démarrage de l'API.

---

## Installation

### 1. Restaurer les packages .NET

```bash
dotnet restore ProManLog.slnx
```

### 2. Installer les dépendances Angular

```bash
cd front
```

```bash
npm install
```

---

## Base de données

La base **SQLite** ne nécessite aucune installation ni aucune commande manuelle :
au premier démarrage de l'API, `Database/01_schema.sql` est exécuté automatiquement
pour créer le fichier `Database/promanlog.db` et ses tables (`CREATE TABLE IF NOT
EXISTS`, sans danger sur une base déjà existante). Si la table `Utilisateur` est
vide, `Database/02_seed.sql` est ensuite exécuté pour insérer le jeu de données de
démonstration (clients, véhicules, employés, prestations, charges, historique et
les deux comptes administrateurs).

Les deux scripts SQL restent disponibles dans `Database/` pour consultation ou
exécution manuelle (par exemple avec la CLI `sqlite3` ou un outil comme
DB Browser for SQLite) :

```bash
sqlite3 Database/promanlog.db < Database/01_schema.sql
```

```bash
sqlite3 Database/promanlog.db < Database/02_seed.sql
```

> Pour repartir d'une base vierge avec de nouvelles données de démonstration,
> supprimer le fichier `Database/promanlog.db` : il sera recréé et réensemencé
> automatiquement au prochain lancement de l'API.

### Configuration de la chaîne de connexion

La chaîne de connexion se trouve dans `api/appsettings.json`, clé
`ConnectionStrings:Default`. Valeur par défaut :

```json
"ConnectionStrings": {
  "Default": "Data Source=../Database/promanlog.db"
}
```

Ce chemin est relatif au dossier `api/` (celui depuis lequel `dotnet run` doit
être lancé) et n'a normalement pas besoin d'être modifié.

### Variables d'environnement

Aucune variable d'environnement n'est nécessaire. Tous les paramètres
(chaîne de connexion, clé JWT, durée du token) sont dans
`api/appsettings.json`.

---

## Lancement du backend

> **Attention** : la commande `dotnet run` doit être exécutée **depuis le dossier
> `api`**, pas depuis la racine du dépôt.

```bash
cd api
```

```bash
dotnet run
```

L'API démarre sur `http://localhost:5287`.
La documentation interactive (Scalar) est accessible à l'adresse
`http://localhost:5287/scalar/v1`.

---

## Lancement du frontend

> **Attention** : la commande doit être exécutée **depuis le dossier
> `front`**, pas depuis la racine du dépôt.

```bash
cd front
```

```bash
npm start
```

L'application Angular est accessible à l'adresse `http://localhost:4200`.

> Le backend doit être démarré **avant** le frontend pour que les appels API
> fonctionnent.

---

## Comptes de test

L'application nécessite une authentification. Deux comptes administrateurs sont
créés automatiquement au premier démarrage :

| Nom d'utilisateur | Mot de passe | Rôle |
|---|---|---|
| `admin` | `admin123` | Admin |
| `admin2` | `admin123` | Admin |

Seule la page de connexion est accessible sans authentification.

---

## Architecture du projet

```
ProManLog_WEB/
│
├── core/                          <- Couche métier (aucune dépendance externe)
│   ├── Entities/                  <- Entités du domaine
│   ├── DTOs/                      <- Objets de transfert de données
│   ├── Interfaces/                <- Contrats (IXRepository, IXService)
│   └── Services/                  <- Logique métier, mapping Entité vers DTO
│
├── infra/                         <- Couche d'accès aux données
│   ├── Data/                      <- Factory de connexion SQLite
│   ├── Repositories/              <- Implémentations Dapper (SQL paramétré)
│   └── Security/                  <- Hachage des mots de passe, génération du JWT
│
├── api/                           <- Couche présentation (HTTP)
│   ├── Controllers/               <- Points d'entrée REST, aucune logique métier
│   ├── Program.cs                 <- Injection de dépendances, JWT, CORS,
│   │                                  création/peuplement automatique de la base
│   └── appsettings.json           <- Configuration
│
├── front/                         <- Frontend Angular 21
│   └── src/app/
│       ├── components/            <- Un composant par rubrique (signals, @if/@for/@switch)
│       ├── Services/              <- Services HTTP (HttpClient + Observables)
│       ├── models/                <- Interfaces TypeScript
│       ├── guards/                <- Garde de route (authentification)
│       ├── interceptors/          <- Ajout du token JWT aux requêtes
│       ├── app.routes.ts          <- Routage Angular
│       └── app.html               <- Layout principal (menu + router-outlet)
│
└── Database/
    ├── 01_schema.sql              <- Création des tables (exécuté automatiquement)
    ├── 02_seed.sql                <- Jeu de données de démonstration
    └── promanlog.db                <- Fichier SQLite (généré, absent du dépôt)
```

Les dossiers des couches portent uniquement le nom de la couche : `api`, `core`
et `infra`. Le frontend Angular est dans `front`.

---

## Stack technique

| Couche | Technologie |
|---|---|
| Backend | ASP.NET Core 10 |
| ORM | Dapper 2.x |
| Base de données | SQLite (Microsoft.Data.Sqlite) |
| Authentification | JWT (JwtBearer) |
| Documentation API | Scalar (`/scalar/v1`) |
| Frontend | Angular 21.2 |
| Langage frontend | TypeScript 5.9 |
| Formulaires | ReactiveFormsModule |
| HTTP | HttpClient (Observables RxJS) |
| État des composants | Angular Signals (natif, pas de bibliothèque externe) |

---

## Routes de l'API

Toutes les routes, sauf `POST /api/Auth/connexion`, exigent l'en-tête
`Authorization: Bearer <token>`.

| Méthode | Route | Description |
|---|---|---|
| POST | `/api/Auth/connexion` | Connexion, renvoie le token JWT |
| GET | `/api/TableauBord` | Données du tableau de bord |
| GET / POST | `/api/Prestation` | Liste (`?statut=`) / création |
| GET / PUT / DELETE | `/api/Prestation/{id}` | Détail / modification / suppression |
| PATCH | `/api/Prestation/{id}/statut` | Changement de statut |
| GET / POST | `/api/Client` | Liste / création |
| GET / PUT / DELETE | `/api/Client/{id}` | Détail / modification / suppression |
| GET | `/api/Client/{id}/prestations` | Prestations d'un client |
| GET / POST | `/api/Vehicule` | Liste / création |
| GET / PUT / DELETE | `/api/Vehicule/{id}` | Détail / modification / suppression |
| GET / POST | `/api/Employe` | Liste / création |
| GET / PUT / DELETE | `/api/Employe/{id}` | Détail / modification / suppression |
| GET | `/api/Historique` | Historique (`?du=&au=&typeAction=`) |
| GET | `/api/Chiffres` | Récapitulatif comptable sur six mois |

---

## Principes respectés

- **Clean Architecture** : dépendances orientées vers le centre
  (Core <- Infrastructure, Core <- API). Le projet `core` ne référence
  aucun autre projet ni aucune bibliothèque d'accès aux données.
- **Repository Pattern** : les repositories manipulent uniquement des Entités ;
  les Services mappent vers des DTOs.
- **Injection de dépendances** : toutes les classes reçoivent leurs dépendances
  via le constructeur, enregistrées dans `Program.cs`.
- **Séparation des responsabilités** : les contrôleurs ne contiennent aucune
  logique métier ni SQL.
- **Nouvelle syntaxe Angular** : `@if`, `@for`, `@empty` et `@switch` utilisés
  dans les templates.
- **Aucune bibliothèque d'état externe** : l'état est géré uniquement par des
  Signals Angular natifs et des Services.
- **Entity Framework n'est pas utilisé** : l'accès aux données passe
  exclusivement par Dapper avec du SQL paramétré.

---

## Remarques

- Les mots de passe sont stockés hachés en SHA-256. Une fonction dédiée
  (bcrypt, Argon2) serait utilisée en production.
- La clé JWT présente dans `api/appsettings.json` est une clé de
  démonstration.
- Le CORS n'autorise que l'origine `http://localhost:4200`.
- Le graphique du chiffre d'affaires est dessiné en SVG directement dans le
  template Angular : aucune bibliothèque de graphiques n'est utilisée.
- La rubrique Chiffres applique une TVA de 20 %. Les salaires ne sont pas
  déductibles de la TVA : la TVA payée est calculée sur les frais, le loyer,
  les factures et l'essence.
- Les clés étrangères sont activées explicitement à chaque connexion
  (`PRAGMA foreign_keys = ON`, désactivées par défaut sous SQLite) : elles
  empêchent la suppression d'un client, d'un véhicule ou d'un employé encore
  référencé par une prestation. L'API renvoie alors `409 Conflict` et le
  message est affiché dans l'interface.
- Le fichier `Database/promanlog.db` n'est pas versionné (voir `.gitignore`) :
  chaque clone du dépôt régénère sa propre base au premier démarrage de l'API.
