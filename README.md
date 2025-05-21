

# README - Projet wfaCVV (Gestion Canoës Vallée Vézère) - Version Détaillée

## 1. Objectif du Projet

`wfaCVV` est une application de bureau Windows Forms (WinForms) développée en C# avec le .NET Framework 4.5.2. Elle a pour but de fournir une interface graphique pour la gestion des activités de location de l'entreprise "Canoës Vallée Vézère". Les fonctionnalités principales incluent :

*   La gestion (Ajout, Modification, Suppression, Consultation) des différents parcours de canoë/kayak proposés.
*   La gestion (Ajout, Modification, Suppression, Consultation) des types d'embarcations disponibles.
*   La consultation des tarifs basés sur la combinaison d'un parcours et d'une embarcation.

## 2. Technologies Utilisées

*   **Langage de Programmation :** C#
*   **Framework :** .NET Framework 4.5.2
*   **Interface Utilisateur (UI) :** Windows Forms (WinForms)
*   **Système de Gestion de Base de Données (SGBD) :** MariaDB (Version 10.4.32) - Entièrement compatible avec MySQL.
*   **Bibliothèque d'Accès aux Données :** `MySql.Data.dll` (Connecteur officiel MySQL pour ADO.NET)
*   **Environnement de Développement Intégré (IDE) :** Visual Studio (présumé)

## 3. Structure Détaillée de la Base de Données (`bddcvv`)

La base de données, nommée `bddcvv`, est le cœur du stockage des informations de l'application. Elle est définie dans le fichier `bddcvv.sql`.

**Schéma SQL:** (Référence au fichier `bddcvv.sql` dans le projet ou dépôt)

**Tables :**

1.  **`embarcation`**: Répertorie les types d'embarcations.
    *   `idEmbarcation` (INT(11), PK, NOT NULL, AUTO_INCREMENT): Clé primaire unique, auto-incrémentée. Identifie chaque type d'embarcation. *Exemple: 1*
    *   `typeEmbarcation` (VARCHAR(50), NOT NULL): Nom descriptif de l'embarcation. *Exemple: "Canoë"*
    *   `photoEmbarcation` (VARCHAR(50), NOT NULL): Nom du fichier image (supposé être stocké dans un dossier accessible par l'application, par exemple un sous-dossier `Images`). *Exemple: "canoe.jpeg"*

2.  **`parcours`**: Contient les détails des différents parcours proposés.
    *   `idParcours` (INT(11), PK, NOT NULL, AUTO_INCREMENT): Clé primaire unique, auto-incrémentée. Identifie chaque parcours. *Exemple: 1*
    *   `nomParcours` (VARCHAR(50), NOT NULL): Nom commercial du parcours. *Exemple: "Les 3 châteaux"*
    *   `distanceParcours` (SMALLINT(6), NOT NULL): Distance du parcours en kilomètres. *Exemple: 26*
    *   `dureeParcours` (VARCHAR(5), NOT NULL): Durée estimée, formatée en chaîne (HH'h'MM). *Exemple: "5h00"*
    *   `descParcours` (VARCHAR(250), NOT NULL): Description textuelle fournissant des détails logistiques ou touristiques. *Exemple: "Départ en minibus et arrivée aux Eyzies"*

3.  **`tarif`**: Table associative qui définit le prix pour chaque combinaison possible d'une embarcation et d'un parcours.
    *   `idEmbarcation` (INT(11), PK, FK, NOT NULL): Clé étrangère référençant `embarcation.idEmbarcation`. Fait partie de la clé primaire composite.
    *   `idParcours` (INT(11), PK, FK, NOT NULL): Clé étrangère référençant `parcours.idParcours`. Fait partie de la clé primaire composite.
    *   `prix` (FLOAT, NOT NULL): Le prix en euros pour la location de l'`idEmbarcation` sur l'`idParcours` spécifié. *Exemple: 22.0*

**Index et Contraintes :**

*   Chaque table (`embarcation`, `parcours`) a une clé primaire sur son champ `id...`.
*   La table `tarif` a une clé primaire composite (`idEmbarcation`, `idParcours`), garantissant qu'il ne peut y avoir qu'un seul prix pour une combinaison donnée.
*   Des contraintes de clé étrangère (`tarif_ibfk_1`, `tarif_ibfk_2`) sont définies sur la table `tarif` pour assurer l'intégrité référentielle avec les tables `embarcation` et `parcours`.
*   Les tables `embarcation` et `parcours` utilisent `AUTO_INCREMENT` pour générer automatiquement de nouveaux IDs.

## 4. Structure Détaillée de l'Application C# (wfaCVV)

L'application WinForms est structurée autour de formulaires pour l'interface utilisateur et de classes pour la logique métier et l'accès aux données.

**Espace de Noms Principal :** `wfaCVV`

**Fichiers et Classes Clés :**

*   **`Program.cs`:**
    *   Contient le point d'entrée `Main()`.
    *   Définit des listes statiques publiques :
        *   `public static List<Embarcation> lesEmbarcation;`
        *   `public static List<Parcours> lesParcours;`
        Ces listes sont peuplées au démarrage de l'application en lisant les données depuis les tables `embarcation` et `parcours` et servent de cache de données en mémoire pour l'application.
    *   Contient la méthode statique `calculTarif(int idEmbarcation, int idParcours)` qui interroge la table `tarif` pour obtenir le prix correspondant aux IDs fournis.

*   **Classes Métier :**
    *   **`Parcours.cs`:** Modélise un parcours.
        *   Propriétés : `id`, `nom`, `distance`, `duree`, `description` (correspondant aux colonnes de la table `parcours`).
        *   Méthodes CRUD : `Ajouter()`, `modifier()`, `Supprimer(int id)`, `GetAllParcours()`. Ces méthodes encapsulent les requêtes SQL (INSERT, UPDATE, DELETE, SELECT) pour interagir avec la table `parcours`. Elles utilisent la chaîne de connexion stockée dans les ressources.
    *   **`Embarcation.cs`:** Modélise une embarcation.
        *   Propriétés : `idEmb`, `nomEmb`, `photoEmb` (correspondant aux colonnes de la table `embarcation`).
        *   Méthodes CRUD : Similaires à `Parcours.cs`, mais pour la table `embarcation`. Ex: `modifier()`, `GetAllEmbarcations()`.

*   **Formulaires (Interface Utilisateur) :**
    *   **`FrmAccueil.cs`:** Le formulaire principal qui s'affiche au lancement. Contient des boutons ou des éléments de menu pour ouvrir les autres formulaires de gestion.
    *   **`FrmParcours.cs`:** Affiche la liste des parcours (provenant de `Program.lesParcours`) dans un contrôle `DataGridView`. Permet à l'utilisateur de sélectionner un parcours pour le modifier ou le supprimer, et fournit un bouton pour ouvrir le formulaire d'ajout.
    *   **`FrmAjoutParcours.cs`:** Contient des champs de saisie (TextBox, NumericUpDown, etc.) pour entrer les détails d'un nouveau parcours. Lors de la validation, crée une instance de `Parcours`, appelle sa méthode `Ajouter()`, puis met à jour la liste `Program.lesParcours` et rafraîchit l'affichage dans `FrmParcours`.
    *   **`FrmModifParcours.cs`:** Similaire à `FrmAjoutParcours`, mais est initialisé avec les données d'un objet `Parcours` existant (passé en paramètre lors de son ouverture depuis `FrmParcours`). Lors de la validation, appelle la méthode `modifier()` de l'objet `Parcours`, met à jour `Program.lesParcours` et rafraîchit `FrmParcours`.
    *   **`FrmEmbarcation.cs` / `FrmAjoutEmbarcation.cs` / `FrmUneEmbarcation.cs`:** Ensemble de formulaires pour la gestion CRUD des embarcations, fonctionnant sur le même principe que ceux des parcours, mais utilisant la classe `Embarcation` et la liste `Program.lesEmbarcation`. `FrmUneEmbarcation` est probablement utilisé pour l'affichage/modification d'une seule embarcation.
    *   **`FrmTarifs.cs`:** Permet à l'utilisateur de consulter un tarif.
        *   Utilise deux contrôles `ComboBox` (`CboParcours`, `CboEmbarcations`).
        *   Dans l'événement `FrmTarifs_Load`, les `DataSource` des ComboBox sont liés aux listes statiques `Program.lesParcours` et `Program.lesEmbarcation`. `DisplayMember` est défini sur "nom" (ou `nomEmb`) et `ValueMember` sur "id" (ou `idEmb`).
        *   Un bouton (`btnVoirTarif`) déclenche l'événement `btnVoirTarif_Click`. Cet événement récupère les `SelectedValue` (qui correspondent aux IDs) des deux ComboBox, appelle `Program.calculTarif()` avec ces IDs, et affiche le résultat dans un `Label` (`Lblarif`).

## 5. Accès aux Données et Connexion

*   **Bibliothèque :** `MySql.Data.MySqlClient` est utilisé pour toutes les interactions avec la base de données MariaDB.
*   **Chaîne de Connexion :** La chaîne de connexion est centralisée dans les ressources du projet (`Properties.Resources.chaineCnxCVV`) pour faciliter la configuration.
    ```csharp
    // Exemple de récupération dans le code :
    string connectionString = wfaCVV.Properties.Resources.chaineCnxCVV;
    ```
*   **Gestion des Connexions :** L'instruction `using (MySqlConnection cnx = new MySqlConnection(...))` est systématiquement employée pour garantir que la connexion à la base de données est correctement ouverte et surtout fermée (même en cas d'erreur), libérant ainsi les ressources.
*   **Sécurité :** Les requêtes SQL sont paramétrées (utilisation de `@paramName` et `cmd.Parameters.AddWithValue()`) pour prévenir les risques d'injection SQL.
*   **Exemple (Récupération Tarif dans `Program.cs`) :**
    ```csharp
    public static float calculTarif(int idEmbarcation, int idParcours)
    {
        float tarif = 0.0f;
        using (MySqlConnection cnxCVV = new MySqlConnection(wfaCVV.Properties.Resources.chaineCnxCVV))
        {
            try
            {
                cnxCVV.Open();
                string strRequeteSelectTarif = "SELECT prix FROM tarif WHERE idEmbarcation = @idEmbarcation AND idParcours = @idParcours";
                using (MySqlCommand cmdTarif = new MySqlCommand(strRequeteSelectTarif, cnxCVV))
                {
                    cmdTarif.Parameters.AddWithValue("@idEmbarcation", idEmbarcation);
                    cmdTarif.Parameters.AddWithValue("@idParcours", idParcours);
                    object result = cmdTarif.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        tarif = Convert.ToSingle(result);
                    }
                    else
                    {
                         throw new Exception("Aucun tarif trouvé pour l'embarcation et le parcours spécifiés.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du calcul du tarif : {ex.Message}", "Erreur BDD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        return tarif;
    }
    ```

## 6. Flux de Fonctionnement Typique

1.  **Lancement :** L'utilisateur exécute `wfaCVV.exe`. `Program.Main()` est appelé.
2.  **Initialisation :** Les méthodes `Parcours.GetAllParcours()` et `Embarcation.GetAllEmbarcations()` sont appelées pour charger les données des tables `parcours` et `embarcation` dans les listes statiques `Program.lesParcours` et `Program.lesEmbarcation`.
3.  **Affichage Accueil :** `Application.Run(new FrmAccueil());` affiche le formulaire principal.
4.  **Navigation (Ex: Gestion Parcours) :** L'utilisateur clique sur le bouton "Gérer les Parcours" dans `FrmAccueil`.
5.  **Affichage Liste Parcours :** Une instance de `FrmParcours` est créée et affichée. Le `DataGridView` est peuplé à partir de `Program.lesParcours`.
6.  **Action (Ex: Ajouter Parcours) :** L'utilisateur clique sur "Ajouter".
7.  **Formulaire Ajout :** `FrmAjoutParcours` s'affiche. L'utilisateur remplit les champs et valide.
8.  **Enregistrement :** Le code de `FrmAjoutParcours` crée un objet `Parcours`, appelle sa méthode `Ajouter()`. Cette méthode exécute une requête `INSERT` dans la BDD.
9.  **Mise à Jour Cache :** La nouvelle instance de `Parcours` (avec son ID généré par la BDD) est ajoutée à la liste `Program.lesParcours`.
10. **Rafraîchissement UI :** `FrmAjoutParcours` se ferme. `FrmParcours` rafraîchit son `DataGridView` pour afficher le nouveau parcours.
11. **Action (Ex: Consulter Tarifs) :** L'utilisateur revient à `FrmAccueil` et clique sur "Voir les Tarifs".
12. **Formulaire Tarifs :** `FrmTarifs` s'affiche. Les `ComboBox` sont peuplées via Data Binding sur `Program.lesParcours` et `Program.lesEmbarcation`.
13. **Sélection :** L'utilisateur sélectionne un parcours et une embarcation dans les `ComboBox`.
14. **Calcul et Affichage :** L'utilisateur clique sur "Voir Tarifs". L'événement `btnVoirTarif_Click` récupère les IDs sélectionnés, appelle `Program.calculTarif()`, qui exécute un `SELECT` sur la table `tarif`, et affiche le prix retourné dans le `Label`.
