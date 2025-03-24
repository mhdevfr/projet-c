# Application Client MySQL en C#

Cette application console C# permet de se connecter à une base de données MySQL (accessible via phpMyAdmin) et d'exécuter des requêtes SQL.

## Prérequis

- .NET SDK 9.0 ou supérieur
- Une base de données MySQL accessible (soit localement, soit sur un serveur distant)
- Un compte utilisateur avec les droits d'accès sur cette base de données

## Configuration de la base de données

1. Assurez-vous que votre serveur MySQL est en cours d'exécution
2. Si vous utilisez phpMyAdmin, connectez-vous à l'interface d'administration
3. Créez une base de données (si elle n'existe pas déjà)
4. Notez les informations de connexion (serveur, port, nom de la base de données, nom d'utilisateur, mot de passe)

## Exécution de l'application

Pour exécuter l'application, ouvrez un terminal dans le répertoire du projet et exécutez :

```bash
cd MySQLClientApp
dotnet run
```

L'application vous demandera :
- L'adresse du serveur (par défaut: localhost)
- Le port (par défaut: 3306)
- Le nom de la base de données
- Le nom d'utilisateur
- Le mot de passe

Une fois connecté, vous pourrez :
1. Exécuter des requêtes SELECT
2. Exécuter des commandes (INSERT, UPDATE, DELETE)

## Structure du projet

- `Program.cs` : Point d'entrée de l'application, interface utilisateur en ligne de commande
- `DatabaseManager.cs` : Classe qui gère les connexions et requêtes à la base de données

## Fonctionnalités

- Connexion sécurisée à une base de données MySQL
- Exécution de requêtes SELECT avec affichage des résultats en tableau
- Exécution de commandes INSERT, UPDATE, DELETE avec retour du nombre de lignes affectées
- Utilisation de requêtes paramétrées pour éviter les injections SQL

## Extension possible

Cette application pourrait être étendue pour inclure :
- Une interface graphique (Windows Forms ou WPF)
- Une gestion des transactions
- Des fonctionnalités de sauvegarde/restauration de base de données
- Des modèles d'entités pour manipuler les données de façon orientée objet

## Licence

Ce projet est distribué sous licence MIT. 