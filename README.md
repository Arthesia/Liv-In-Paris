### **Liv’In Paris - Optimisation des Livraisons avec C# et Graphes** 🚀  

📍 **Projet universitaire** en **C#**, intégrant **POO, algorithmique des graphes et bases de données** pour optimiser un service de livraison de repas à Paris via le **réseau de métro**.  

---

## **🛠️ Fonctionnalités principales**  
✔️ **Gestion des utilisateurs** : Création, modification et suppression des **clients** et **cuisiniers**  
✔️ **Système de commandes** : Gestion des transactions avec suivi des historiques  
✔️ **Optimisation des livraisons** : Calcul des **itinéraires les plus courts** entre cuisinier et client  
✔️ **Algorithmes de graphes** :  
   - 🔍 **Dijkstra**  
   - 🔍 **Bellman-Ford**  
   - 🔍 **Floyd-Warshall**  
✔️ **Statistiques avancées** : Suivi des commandes, livraisons et performances des cuisiniers  
✔️ **Interface console et visualisation** : Affichage interactif des **graphes et trajets**  

---

## **🧰 Technologies utilisées**  
🖥 **Langage** : C# (.NET)  
📊 **Base de données** : SQL (MySQL ou SQLite)  
📈 **Graphes & Algorithmes** :  
- **Modélisation des itinéraires**  
- **Calcul du plus court chemin**  
- **Coloration et couverture de graphes**  
🎨 **Visualisation graphique** : System.Drawing, SkiaSharp  

---

## **🎯 Objectif du projet**  
**Développer une application robuste** permettant d’optimiser les trajets de livraison entre cuisiniers et clients en exploitant **les graphes urbains** du réseau de métro parisien.  

🔍 **Analyse avancée des parcours**  
🚀 **Optimisation des coûts et délais de livraison**  
📍 **Modélisation réaliste des itinéraires**  

---

## **📁 Structure du projet**  
```
LivinParis/
├── Program.cs               # Point d’entrée principal
├── Models/                  # Classes principales
│   ├── Client.cs
│   ├── Cuisinier.cs
│   ├── Commande.cs
│   ├── Plat.cs
│   ├── Trajet.cs
│   ├── Statistiques.cs
├── Services/                # Logique métier
│   ├── ClientService.cs
│   ├── CuisinierService.cs
│   ├── CommandeService.cs
│   ├── TrajetService.cs
│   ├── StatistiqueService.cs
│   ├── GrapheService.cs
├── Data/                    # Gestion de la base de données
│   ├── Database.cs
│   ├── Clients.csv
│   ├── Cuisiniers.csv
│   ├── Commandes.csv
│   ├── MetroLines.json
└── Tests/                   # Tests unitaires
    ├── UnitTests.cs
    ├── IntegrationTests.cs
```

---

## **💻 Installation & Exécution**
### **1️⃣ Cloner le dépôt**
```sh
git clone https://github.com/votre-repo/LivinParis.git
cd LivinParis
```
### **2️⃣ Configurer la base de données**
📌 **Option 1 : MySQL**
```sql
CREATE DATABASE livinparis;
USE livinparis;
SOURCE schema.sql;
```
📌 **Option 2 : SQLite**
```sh
sqlite3 livinparis.db < schema.sql
```

### **3️⃣ Compiler et Exécuter**
```sh
dotnet build
dotnet run
```

---

## **📝 Roadmap**
✅ **Étape 1** : Mise en place de la base de données et des modèles (clients, cuisiniers, commandes)  
🔄 **Étape 2** : Développement des services et algorithmes de graphes  
🚀 **Étape 3** : Implémentation de l’optimisation des trajets et visualisation  
🎨 **Étape 4** : Ajout d’une interface graphique (WPF ou OpenStreetMap)  

---

## **🤝 Contribuer**
🚀 **Pull requests et suggestions bienvenues !**  
📌 Ouvrez une **issue** pour proposer des améliorations.  

---

## **📜 Licence**
📄 Ce projet est sous licence **MIT** – [Voir le fichier LICENSE](LICENSE).

---

✨ **Liv’In Paris** : Une solution intelligente et optimisée pour la livraison de repas en milieu urbain ! 🚇🍽
