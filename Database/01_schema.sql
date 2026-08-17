-- ProManLog : création des tables (SQLite)
-- Ce script est exécuté automatiquement par l'API à chaque démarrage
-- (CREATE TABLE IF NOT EXISTS : sans danger sur une base déjà existante).

CREATE TABLE IF NOT EXISTS Utilisateur (
    IdUtilisateur   INTEGER PRIMARY KEY AUTOINCREMENT,
    NomUtilisateur  TEXT NOT NULL UNIQUE,
    MotDePasseHash  TEXT NOT NULL,
    NomComplet      TEXT NOT NULL,
    Role            TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Client (
    IdClient    INTEGER PRIMARY KEY AUTOINCREMENT,
    TypeClient  TEXT NOT NULL,
    Nom         TEXT NOT NULL,
    Contact     TEXT NOT NULL DEFAULT '',
    Email       TEXT NOT NULL DEFAULT '',
    Telephone   TEXT NOT NULL DEFAULT '',
    Adresse     TEXT NOT NULL DEFAULT ''
);

CREATE TABLE IF NOT EXISTS Vehicule (
    IdVehicule                INTEGER PRIMARY KEY AUTOINCREMENT,
    Immatriculation           TEXT NOT NULL UNIQUE,
    TypeVehicule              TEXT NOT NULL,
    CapaciteKg                INTEGER NOT NULL,
    Kilometrage               INTEGER NOT NULL,
    Statut                    TEXT NOT NULL,
    DernierEntretien          DATE NULL,
    ProchainEntretien         DATE NULL,
    ProchainControleTechnique DATE NULL,
    FinAssurance              DATE NULL
);

CREATE TABLE IF NOT EXISTS Employe (
    IdEmploye        INTEGER PRIMARY KEY AUTOINCREMENT,
    Nom              TEXT NOT NULL,
    Prenom           TEXT NOT NULL,
    Poste            TEXT NOT NULL,
    DateEmbauche     DATE NOT NULL,
    Email            TEXT NOT NULL DEFAULT '',
    Telephone        TEXT NOT NULL DEFAULT '',
    Statut           TEXT NOT NULL,
    Disponibilite    TEXT NOT NULL,
    NumeroPermis     TEXT NOT NULL DEFAULT '',
    CategoriesPermis TEXT NOT NULL DEFAULT '',
    ExpirationPermis DATE NULL
);

CREATE TABLE IF NOT EXISTS Prestation (
    IdPrestation   INTEGER PRIMARY KEY AUTOINCREMENT,
    IdClient       INTEGER NOT NULL,
    IdVehicule     INTEGER NULL,
    IdEmploye      INTEGER NULL,
    AdresseDepart  TEXT NOT NULL,
    AdresseArrivee TEXT NOT NULL,
    DateHeure      DATETIME NOT NULL,
    TypeService    TEXT NOT NULL,
    Prix           DECIMAL NOT NULL,
    Statut         TEXT NOT NULL,
    FOREIGN KEY (IdClient)   REFERENCES Client(IdClient),
    FOREIGN KEY (IdVehicule) REFERENCES Vehicule(IdVehicule),
    FOREIGN KEY (IdEmploye)  REFERENCES Employe(IdEmploye)
);

CREATE TABLE IF NOT EXISTS Historique (
    IdHistorique INTEGER PRIMARY KEY AUTOINCREMENT,
    DateAction   DATETIME NOT NULL,
    TypeAction   TEXT NOT NULL,
    TypeEntite   TEXT NOT NULL,
    IdEntite     INTEGER NULL,
    Description  TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Charge (
    IdCharge   INTEGER PRIMARY KEY AUTOINCREMENT,
    Periode    DATE NOT NULL,
    TypeCharge TEXT NOT NULL,
    Montant    DECIMAL NOT NULL
);

CREATE INDEX IF NOT EXISTS IX_Prestation_Statut ON Prestation(Statut);
CREATE INDEX IF NOT EXISTS IX_Prestation_Client ON Prestation(IdClient);
CREATE INDEX IF NOT EXISTS IX_Historique_Date   ON Historique(DateAction);
CREATE INDEX IF NOT EXISTS IX_Historique_Type   ON Historique(TypeAction);
CREATE INDEX IF NOT EXISTS IX_Charge_Periode    ON Charge(Periode);
