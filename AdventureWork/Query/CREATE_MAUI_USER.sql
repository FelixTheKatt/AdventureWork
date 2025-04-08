-- Créer le login au niveau du serveur avec un mot de passe
CREATE LOGIN maui_user WITH PASSWORD = 'MauiPass123!', CHECK_POLICY=OFF;

-- Se connecter à la base de données AdventureWorks2022
USE AdventureWorks2022;

-- Créer l'utilisateur dans la base de données lié au login
CREATE USER maui_user FOR LOGIN maui_user;

-- Accorder des permissions de lecture et d'écriture dans la base de données
EXEC sp_addrolemember 'db_datareader', 'maui_user';
EXEC sp_addrolemember 'db_datawriter', 'maui_user';