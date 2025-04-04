=========================================
          README - Projet LivinParis
=========================================

Ce projet utilise MySQL comme base de données.

Étapes pour lancer le projet :

1. Ouvrir MySQL Workbench
   ➤ Se connecter à votre instance MySQL (ex : root@localhost:3306)

2. Importer la base de données :
   - Aller dans : Fichier → Ouvrir un script SQL
   - Ouvrir le fichier "BDDLIV.sql" (fourni avec ce projet)
   - Cliquer sur ⚡ (Exécuter) pour créer la base "livinparis"

3. Ouvrir le projet dans Visual Studio :
   - Fichier solution : TESTLivINParis.sln

4. Adapter la ligne 6 du fichier "Admin.cs" pour correspondre à votre configuration MySQL :

   public static string ConnectionString = 
       "server=localhost;port=3306;user=root;password=VOTRE_MDP_MYSQL;database=livinparis;";

   ➤ Remplacez les paramètres (serveur, port, utilisateur, mot de passe) si votre configuration MySQL est différente.

🛡 Connexion Administrateur :
   Pour accéder à l'espace administrateur (choix 3 dans le menu), les identifiants sont :

   ➤ Nom d'utilisateur : admin
   ➤ Mot de passe      : admin

Tout est prêt ! Merci de votre attention.
