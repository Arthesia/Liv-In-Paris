CREATE DATABASE  IF NOT EXISTS `livinparis` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `livinparis`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: livinparis
-- ------------------------------------------------------
-- Server version	9.2.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `avis`
--

DROP TABLE IF EXISTS `avis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `avis` (
  `id_avis` int NOT NULL,
  `note` int DEFAULT NULL,
  `commentaire` varchar(50) DEFAULT NULL,
  `date_avis` date DEFAULT NULL,
  `id_cuisinier` int NOT NULL,
  `id_client` int NOT NULL,
  PRIMARY KEY (`id_avis`),
  KEY `id_cuisinier` (`id_cuisinier`),
  KEY `id_client` (`id_client`),
  CONSTRAINT `avis_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `utilisateur` (`id_utilisateur`),
  CONSTRAINT `avis_ibfk_2` FOREIGN KEY (`id_client`) REFERENCES `utilisateur` (`id_utilisateur`),
  CONSTRAINT `avis_chk_1` CHECK ((`note` between 1 and 5))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `avis`
--

LOCK TABLES `avis` WRITE;
/*!40000 ALTER TABLE `avis` DISABLE KEYS */;
INSERT INTO `avis` VALUES (1,5,'Excellent repas!','2025-03-15',2,1),(2,4,'Un peu épicé','2025-03-16',3,4),(3,3,'Moyen','2025-03-17',5,6),(4,5,'Service impeccable','2025-03-18',7,8),(5,4,'Plutôt bon','2025-03-19',9,10),(6,2,'Manque de saveur','2025-03-20',12,11),(7,5,'Délicieux!','2025-03-21',14,13),(8,3,'Correct','2025-03-22',16,15),(9,4,'Bon rapport qualité-prix','2025-03-23',18,17),(10,5,'Un régal','2025-03-24',2,19),(11,4,'Intéressant','2025-03-25',3,1),(12,3,'Peut mieux faire','2025-03-26',5,4),(13,5,'Super','2025-03-27',7,6),(14,4,'Bien','2025-03-28',9,8),(15,3,'Moyen','2025-03-29',12,10),(16,5,'Délicieux, rien à redire.','2025-03-30',22,21),(17,3,'Temps de livraison un peu long.','2025-03-31',23,24),(18,4,'Bon rapport qualité-prix !','2025-04-01',25,28),(19,2,'Trop salé à mon goût.','2025-04-02',26,27),(20,5,'Plat très généreux, merci !','2025-04-03',27,21),(21,3,'Correct mais sans plus.','2025-04-04',29,30),(22,4,'Super expérience.','2025-04-05',30,33),(23,1,'Très déçu du plat commandé.','2025-04-06',31,32),(24,5,'Excellent service client !','2025-04-06',34,33),(25,4,'Je recommanderai !','2025-04-07',35,35),(26,3,'Livraison en retard mais plat bon.','2025-04-08',36,37),(27,5,'Vraiment délicieux !','2025-04-09',38,39),(28,4,'Belle présentation du plat.','2025-04-09',40,38),(29,2,'Manque d’assaisonnement.','2025-04-10',22,22),(30,4,'Top comme toujours !','2025-04-10',25,28),(31,4,'Très bon plat, je recommande.','2025-04-02',26,2),(32,3,'Rien d’exceptionnel mais bon.','2025-04-03',23,3),(33,5,'Excellent, très copieux !','2025-04-04',25,5),(34,2,'Pas à mon goût.','2025-04-05',27,7),(35,4,'J’ai adoré, surtout les saveurs.','2025-04-06',29,9),(36,5,'Rapide et délicieux.','2025-04-06',30,12),(37,3,'Pas mal mais un peu cher.','2025-04-07',22,14),(38,4,'Très bon équilibre des saveurs.','2025-04-08',31,16),(39,5,'Je commande chaque semaine !','2025-04-08',34,18),(40,2,'Livraison en retard.','2025-04-09',36,20),(41,4,'Belle présentation.','2025-04-09',23,1),(42,3,'Un peu fade mais correct.','2025-04-10',26,4),(43,5,'Plat chaud et savoureux !','2025-04-10',28,6),(44,4,'Très bon rapport qualité/prix.','2025-04-11',30,8),(45,3,'Pas trop fan du dessert.','2025-04-11',31,10),(46,5,'Incroyable, comme à la maison.','2025-04-12',32,11),(47,4,'Vraiment sympa.','2025-04-13',33,13),(48,3,'Service correct.','2025-04-13',35,15),(49,5,'Un pur délice !','2025-04-14',38,17),(50,4,'Rien à redire.','2025-04-14',40,19);
/*!40000 ALTER TABLE `avis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `id_utilisateur` int NOT NULL,
  `type_client` varchar(50) DEFAULT NULL,
  `nom_referent` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`),
  CONSTRAINT `client_ibfk_1` FOREIGN KEY (`id_utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Particulier',NULL),(4,'Entreprise','Durand'),(6,'Particulier',NULL),(8,'Particulier',NULL),(10,'Entreprise','Bernard'),(11,'Particulier',NULL),(13,'Entreprise','Fournier'),(15,'Particulier',NULL),(17,'Particulier',NULL),(19,'Particulier',NULL),(20,'Entreprise','Boulangerie'),(21,'Particulier',NULL),(22,'Entreprise','Fernandez'),(24,'Particulier',NULL),(25,'Entreprise','Silva'),(27,'Particulier',NULL),(28,'Particulier',NULL),(30,'Entreprise','Gonzalez'),(32,'Particulier',NULL),(33,'Particulier',NULL),(35,'Entreprise','Rodriguez'),(37,'Particulier',NULL),(38,'Entreprise','Haddad'),(39,'Particulier',NULL);
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `id_commande` int NOT NULL,
  `statut_commande` varchar(10) DEFAULT NULL,
  `date_commande` date DEFAULT NULL,
  `montant_commande` int DEFAULT NULL,
  `paiement_effectue` tinyint(1) DEFAULT NULL,
  `id_itineraire` varchar(50) NOT NULL,
  `id_livraison` varchar(50) NOT NULL,
  `id_client` int NOT NULL,
  PRIMARY KEY (`id_commande`),
  KEY `id_itineraire` (`id_itineraire`),
  KEY `id_livraison` (`id_livraison`),
  KEY `id_client` (`id_client`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`id_itineraire`) REFERENCES `itinéraire` (`id_itineraire`),
  CONSTRAINT `commande_ibfk_2` FOREIGN KEY (`id_livraison`) REFERENCES `livraison` (`id_livraison`),
  CONSTRAINT `commande_ibfk_3` FOREIGN KEY (`id_client`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES (1,'validée','2025-03-18',30,1,'IT001','LIV001',1),(2,'en cours','2025-03-19',45,0,'IT002','LIV002',4),(3,'validée','2025-03-20',50,1,'IT003','LIV003',6),(4,'validée','2025-03-21',25,1,'IT004','LIV004',8),(5,'en cours','2025-03-22',60,0,'IT005','LIV005',10),(6,'validée','2025-03-23',35,1,'IT006','LIV006',11),(7,'validée','2025-03-24',40,1,'IT007','LIV007',13),(8,'en cours','2025-03-25',55,0,'IT008','LIV008',15),(9,'validée','2025-03-26',65,1,'IT009','LIV009',17),(10,'en cours','2025-03-27',70,0,'IT010','LIV010',19),(11,'validée','2025-03-28',38,1,'IT002','LIV003',1),(12,'validée','2025-03-29',42,1,'IT004','LIV005',4),(13,'en cours','2025-03-30',50,0,'IT006','LIV007',6),(14,'validée','2025-03-31',45,1,'IT008','LIV009',8),(15,'validée','2025-04-01',55,1,'IT010','LIV010',10),(16,'en cours','2025-04-02',33,0,'IT001','LIV002',11),(17,'validée','2025-04-03',48,1,'IT003','LIV004',13),(18,'validée','2025-04-04',52,1,'IT005','LIV006',15),(19,'en cours','2025-04-05',60,0,'IT007','LIV008',17),(20,'validée','2025-04-06',40,1,'IT009','LIV010',19),(21,'validée','2025-04-01',36,1,'IT003','LIV011',21),(22,'en cours','2025-04-01',52,0,'IT004','LIV012',22),(23,'validée','2025-04-02',44,1,'IT005','LIV013',24),(24,'validée','2025-04-02',31,1,'IT006','LIV014',25),(25,'en cours','2025-04-03',59,0,'IT007','LIV015',27),(26,'validée','2025-04-03',38,1,'IT008','LIV016',28),(27,'validée','2025-04-04',47,1,'IT009','LIV017',30),(28,'en cours','2025-04-04',40,0,'IT010','LIV018',32),(29,'validée','2025-04-05',55,1,'IT001','LIV019',33),(30,'en cours','2025-04-05',49,0,'IT002','LIV020',35),(31,'validée','2025-04-06',41,1,'IT003','LIV021',37),(32,'validée','2025-04-06',36,1,'IT004','LIV022',38),(33,'en cours','2025-04-07',50,0,'IT005','LIV023',39),(34,'validée','2025-04-07',45,1,'IT006','LIV024',40),(35,'en cours','2025-04-08',60,0,'IT007','LIV025',21),(36,'validée','2025-04-08',43,1,'IT008','LIV026',22),(37,'en cours','2025-04-09',57,0,'IT009','LIV027',24),(38,'validée','2025-04-09',35,1,'IT010','LIV028',25),(39,'en cours','2025-04-10',48,0,'IT001','LIV029',27),(40,'validée','2025-04-10',39,1,'IT002','LIV030',28);
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `id_utilisateur` int NOT NULL,
  `email` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`),
  CONSTRAINT `cuisinier_ibfk_1` FOREIGN KEY (`id_utilisateur`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES (2,'martin.carré@example.com'),(3,'nguyen.thao@example.com'),(5,'sophie.leclerc@example.com'),(6,'petit.marie@example.com'),(7,'jean.leroy@example.com'),(9,'elena.garcia@example.com'),(12,'antoine.dumas@example.com'),(14,'loic.roux@example.com'),(16,'sebastien.noir@example.com'),(18,'adrien.colin@example.com'),(22,'carlos.fernandez@example.com'),(23,'mei.zhang@example.com'),(25,'camila.silva@example.com'),(26,'omar.rahmani@example.com'),(27,'bao.nguyen@example.com'),(29,'aicha.kone@example.com'),(30,'diego.gonzalez@example.com'),(31,'samir.bouazizi@example.com'),(34,'jiho.kim@example.com'),(36,'mamadou.sy@example.com'),(38,'nour.haddad@example.com'),(40,'rafael.dasilva@example.com');
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itinéraire`
--

DROP TABLE IF EXISTS `itinéraire`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `itinéraire` (
  `id_itineraire` varchar(50) NOT NULL,
  `distance_km` decimal(25,2) DEFAULT NULL,
  `duree_min` decimal(25,2) DEFAULT NULL,
  `chemin` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_itineraire`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itinéraire`
--

LOCK TABLES `itinéraire` WRITE;
/*!40000 ALTER TABLE `itinéraire` DISABLE KEYS */;
INSERT INTO `itinéraire` VALUES ('IT001',5.20,20.00,'Ligne 1 > Ligne 4'),('IT002',3.50,15.00,'Ligne 6 > Ligne 2'),('IT003',4.80,18.00,'Ligne 2 > Ligne 3'),('IT004',6.00,22.00,'Ligne 1 > Ligne 7'),('IT005',3.90,16.00,'Ligne 4 > Ligne 5'),('IT006',7.10,25.00,'Ligne 3 > Ligne 8'),('IT007',2.80,12.00,'Ligne 5 > Ligne 6'),('IT008',5.50,19.00,'Ligne 7 > Ligne 2'),('IT009',4.30,17.00,'Ligne 8 > Ligne 1'),('IT010',6.30,23.00,'Ligne 9 > Ligne 10');
/*!40000 ALTER TABLE `itinéraire` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lignecommande`
--

DROP TABLE IF EXISTS `lignecommande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lignecommande` (
  `id_ligne_commande` int NOT NULL,
  `quantite` int DEFAULT NULL,
  `sous_total` decimal(25,2) DEFAULT NULL,
  `date_livraison` date DEFAULT NULL,
  `lieu_livraison` varchar(50) DEFAULT NULL,
  `id_commande` int NOT NULL,
  `id_plat` int NOT NULL,
  PRIMARY KEY (`id_ligne_commande`),
  KEY `id_commande` (`id_commande`),
  KEY `id_plat` (`id_plat`),
  CONSTRAINT `lignecommande_ibfk_1` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`),
  CONSTRAINT `lignecommande_ibfk_2` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lignecommande`
--

LOCK TABLES `lignecommande` WRITE;
/*!40000 ALTER TABLE `lignecommande` DISABLE KEYS */;
INSERT INTO `lignecommande` VALUES (1,1,10.00,'2025-03-20','12 rue des Lilas',1,1),(2,2,30.00,'2025-03-21','88 avenue Victor Hugo',2,2),(3,1,12.50,'2025-03-22','15 rue de la Paix',3,3),(4,2,14.00,'2025-03-23','20 boulevard Haussmann',4,4),(5,1,11.00,'2025-03-24','22 rue de Rivoli',5,5),(6,3,9.50,'2025-03-25','18 rue de Vaugirard',6,6),(7,2,13.00,'2025-03-26','33 avenue des Champs',7,7),(8,1,10.50,'2025-03-27','29 rue de Seine',8,8),(9,2,8.00,'2025-03-28','45 avenue de la République',9,9),(10,1,12.00,'2025-03-29','10 rue Victor Hugo',10,10),(11,2,11.00,'2025-03-30','12 rue des Lilas',11,11),(12,1,15.00,'2025-03-31','15 rue de la Paix',12,12),(13,1,16.00,'2025-04-01','18 boulevard Haussmann',13,13),(14,2,14.50,'2025-04-02','20 rue de Rivoli',14,14),(15,1,15.00,'2025-04-03','22 rue de Vaugirard',15,15),(16,2,10.00,'2025-04-04','24 avenue des Champs',16,16),(17,1,13.50,'2025-04-05','26 rue de Seine',17,17),(18,1,9.00,'2025-04-06','28 rue des Lilas',18,18),(19,2,12.50,'2025-04-07','30 rue de la Paix',19,19),(20,1,14.00,'2025-04-08','32 boulevard Haussmann',20,20),(21,1,10.00,'2025-04-09','34 rue des Lilas',1,2),(22,2,20.00,'2025-04-10','36 rue des Lilas',2,3),(23,1,17.00,'2025-04-11','38 rue des Lilas',3,4),(24,2,16.00,'2025-04-12','40 rue des Lilas',4,5),(25,1,18.00,'2025-04-13','42 rue des Lilas',5,6),(26,2,12.00,'2025-04-14','44 rue des Lilas',6,7),(27,1,14.50,'2025-04-15','46 rue des Lilas',7,8);
/*!40000 ALTER TABLE `lignecommande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison` (
  `id_livraison` varchar(50) NOT NULL,
  `date_livraison` date DEFAULT NULL,
  `adresse_livraison` varchar(50) DEFAULT NULL,
  `statut_livraison` varchar(50) DEFAULT NULL,
  `id_cuisinier` int NOT NULL,
  PRIMARY KEY (`id_livraison`),
  KEY `id_cuisinier` (`id_cuisinier`),
  CONSTRAINT `livraison_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `utilisateur` (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livraison`
--

LOCK TABLES `livraison` WRITE;
/*!40000 ALTER TABLE `livraison` DISABLE KEYS */;
INSERT INTO `livraison` VALUES ('LIV001','2025-03-20','12 rue des Lilas','en cours',2),('LIV002','2025-03-21','88 avenue Victor Hugo','préparée',3),('LIV003','2025-03-22','15 rue de la Paix','en cours',5),('LIV004','2025-03-23','20 boulevard Haussmann','préparée',7),('LIV005','2025-03-24','22 rue de Rivoli','en cours',9),('LIV006','2025-03-25','18 rue de Vaugirard','préparée',12),('LIV007','2025-03-26','33 avenue des Champs','en cours',14),('LIV008','2025-03-27','29 rue de Seine','préparée',16),('LIV009','2025-03-28','45 avenue de la République','en cours',18),('LIV010','2025-03-29','10 rue Victor Hugo','préparée',2),('LIV011','2025-04-01','34 rue Lamarck','préparée',22),('LIV012','2025-04-02','15 rue de Sèvres','en cours',23),('LIV013','2025-04-03','67 avenue Mozart','préparée',25),('LIV014','2025-04-04','10 boulevard Voltaire','livrée',26),('LIV015','2025-04-05','21 rue Oberkampf','en cours',27),('LIV016','2025-04-06','8 avenue de Clichy','préparée',29),('LIV017','2025-04-07','45 rue de Belleville','livrée',30),('LIV018','2025-04-08','19 rue Drouot','en cours',31),('LIV019','2025-04-09','73 avenue Foch','préparée',34),('LIV020','2025-04-10','61 rue Lafayette','préparée',36),('LIV021','2025-04-11','25 rue Monge','en cours',38),('LIV022','2025-04-12','40 rue Mouffetard','livrée',40),('LIV023','2025-04-13','12 avenue Gambetta','en cours',22),('LIV024','2025-04-14','33 rue Ordener','préparée',23),('LIV025','2025-04-15','77 rue Lecourbe','livrée',25),('LIV026','2025-04-16','5 avenue Jean Jaurès','en cours',26),('LIV027','2025-04-17','28 rue de la Glacière','préparée',27),('LIV028','2025-04-18','91 rue de Charenton','en cours',29),('LIV029','2025-04-19','39 rue Raspail','livrée',30),('LIV030','2025-04-20','64 rue Brancion','préparée',31);
/*!40000 ALTER TABLE `livraison` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `id_plat` int NOT NULL,
  `nom_plat` varchar(50) DEFAULT NULL,
  `type_plat` varchar(5) DEFAULT NULL,
  `portions_disponibles` int DEFAULT NULL,
  `date_fabrication` date DEFAULT NULL,
  `nombre_personnes` int DEFAULT NULL,
  `date_de_peremption` date DEFAULT NULL,
  `prix_par_personne` decimal(25,2) DEFAULT NULL,
  `nationalite_plat` varchar(50) DEFAULT NULL,
  `regime_alimentaire` varchar(50) DEFAULT NULL,
  `ingredients` varchar(50) DEFAULT NULL,
  `photo_url` varchar(50) DEFAULT NULL,
  `id_recette` int NOT NULL,
  PRIMARY KEY (`id_plat`),
  KEY `id_recette` (`id_recette`),
  CONSTRAINT `plat_ibfk_1` FOREIGN KEY (`id_recette`) REFERENCES `recette` (`id_recette`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES (1,'Tacos Viande','Plat',5,'2025-03-17',1,'2025-03-20',10.00,'Mexicaine','Halal','Tortilla, Viande','url1.jpg',1),(2,'Soupe Pho','Plat',3,'2025-03-17',1,'2025-03-21',15.00,'Vietnamienne','Sans gluten','Nouilles, Bouillon','url2.jpg',2),(3,'Pâtes Bolognese','Plat',4,'2025-03-18',1,'2025-03-22',12.50,'Italienne','Végétarien','Pâtes, Tomate, Fromage','url3.jpg',3),(4,'Curry de poulet','Plat',6,'2025-03-19',1,'2025-03-23',14.00,'Indienne','Halal','Poulet, Curry, Riz','url4.jpg',4),(5,'Salade niçoise','Plat',3,'2025-03-20',1,'2025-03-24',11.00,'Française','Sans gluten','Thon, Œufs, Olives','url5.jpg',5),(6,'Quiche lorraine','Plat',5,'2025-03-21',1,'2025-03-25',9.50,'Française','Halal','Pâte, Lardons, Crème','url6.jpg',6),(7,'Burger classique','Plat',4,'2025-03-22',1,'2025-03-26',13.00,'Américaine','Végétarien','Boeuf, Pain, Salade','url7.jpg',7),(8,'Ratatouille','Plat',3,'2025-03-23',1,'2025-03-27',10.50,'Française','Végétarien','Aubergine, Courgette, Tomate','url8.jpg',8),(9,'Crêpes sucrées','Plat',5,'2025-03-24',1,'2025-03-28',8.00,'Française','Végétarien','Farine, Œufs, Lait','url9.jpg',9),(10,'Gratin dauphinois','Plat',4,'2025-03-25',1,'2025-03-29',12.00,'Française','Sans gluten','Pommes de terre, Lait, Fromage','url10.jpg',10),(11,'Pizza Margherita','Plat',6,'2025-03-26',1,'2025-03-30',11.00,'Italienne','Halal','Tomate, Mozzarella, Basilic','url11.jpg',3),(12,'Sushi varié','Plat',5,'2025-03-27',1,'2025-03-31',18.00,'Japonaise','Sans gluten','Riz, Poisson, Algue','url12.jpg',2),(13,'Tartare de boeuf','Plat',4,'2025-03-28',1,'2025-04-01',16.00,'Française','Halal','Boeuf, Oignons, Câpres','url13.jpg',5),(14,'Poulet rôti','Plat',5,'2025-03-29',1,'2025-04-02',14.50,'Française','Halal','Poulet, Herbes, Légumes','url14.jpg',4),(15,'Steak frites','Plat',4,'2025-03-30',1,'2025-04-03',15.00,'Française','Halal','Boeuf, Frites, Sauce','url15.jpg',5),(16,'Salade César','Plat',3,'2025-03-31',1,'2025-04-04',10.00,'Américaine','Sans gluten','Laitue, Poulet, Parmesan','url16.jpg',6),(17,'Poke Bowl','Plat',5,'2025-04-01',1,'2025-04-05',13.50,'Hawaïenne','Halal','Riz, Poisson, Légumes','url17.jpg',7),(18,'Falafel','Plat',4,'2025-04-02',1,'2025-04-06',9.00,'Moyen-orientale','Végétarien','Pois chiches, Épices, Salade','url18.jpg',8),(19,'Burrito','Plat',5,'2025-04-03',1,'2025-04-07',12.50,'Mexicaine','Halal','Tortilla, Haricots, Viande','url19.jpg',9),(20,'Pad Thaï','Plat',4,'2025-04-04',1,'2025-04-08',14.00,'Thaïlandaise','Sans gluten','Nouilles, Crevettes, Cacahuètes','url20.jpg',10);
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propose`
--

DROP TABLE IF EXISTS `propose`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propose` (
  `id_cuisinier` int NOT NULL,
  `id_plat` int NOT NULL,
  PRIMARY KEY (`id_cuisinier`,`id_plat`),
  KEY `id_plat` (`id_plat`),
  CONSTRAINT `propose_ibfk_1` FOREIGN KEY (`id_cuisinier`) REFERENCES `cuisinier` (`id_utilisateur`),
  CONSTRAINT `propose_ibfk_2` FOREIGN KEY (`id_plat`) REFERENCES `plat` (`id_plat`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propose`
--

LOCK TABLES `propose` WRITE;
/*!40000 ALTER TABLE `propose` DISABLE KEYS */;
INSERT INTO `propose` VALUES (2,1),(3,2),(5,3),(7,4),(9,5),(12,6),(14,7),(16,8),(18,9),(2,10),(3,11),(5,12),(7,13),(9,14),(12,15),(14,16),(16,17),(18,18),(2,19),(3,20);
/*!40000 ALTER TABLE `propose` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recette`
--

DROP TABLE IF EXISTS `recette`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recette` (
  `id_recette` int NOT NULL,
  `nom_recette` varchar(50) DEFAULT NULL,
  `description` varchar(50) DEFAULT NULL,
  `liste_ingredients` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_recette`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recette`
--

LOCK TABLES `recette` WRITE;
/*!40000 ALTER TABLE `recette` DISABLE KEYS */;
INSERT INTO `recette` VALUES (1,'Tacos maison','Tacos avec légumes et viande','Tortilla, Viande, Légumes'),(2,'Soupe Pho','Bouillon vietnamien au bœuf','Nouilles, Bouillon, Boeuf, Herbes'),(3,'Pâtes Bolognese','Pâtes avec sauce tomate et viande','Pâtes, Tomate, Viande, Oignons'),(4,'Curry de poulet','Poulet au curry et lait de coco','Poulet, Curry, Lait de coco, Riz'),(5,'Salade niçoise','Salade composée traditionnelle','Thon, Œufs, Olives, Laitue'),(6,'Quiche lorraine','Quiche avec lardons et fromage','Pâte, Lardons, Crème, Fromage'),(7,'Burger classique','Burger avec frites maison','Boeuf, Pain, Tomate, Salade'),(8,'Ratatouille','Légumes mijotés','Aubergine, Courgette, Tomate, Poivron'),(9,'Crêpes sucrées','Crêpes avec garniture au sucre','Farine, Œufs, Lait, Sucre'),(10,'Gratin dauphinois','Pommes de terre au lait et fromage','Pommes de terre, Lait, Fromage, Ail');
/*!40000 ALTER TABLE `recette` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction` (
  `id_transaction` int NOT NULL,
  `mode_paiement` varchar(10) DEFAULT NULL,
  `statut_paiement` varchar(10) DEFAULT NULL,
  `date_paiement` date DEFAULT NULL,
  `id_commande` int NOT NULL,
  PRIMARY KEY (`id_transaction`),
  KEY `id_commande` (`id_commande`),
  CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id_commande`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
INSERT INTO `transaction` VALUES (1,'CB','payée','2025-03-18',1),(2,'espèces','en attente','2025-03-19',2),(3,'CB','payée','2025-03-20',3),(4,'CB','payée','2025-03-21',4),(5,'espèces','payée','2025-03-22',5),(6,'CB','payée','2025-03-23',6),(7,'CB','payée','2025-03-24',7),(8,'espèces','en attente','2025-03-25',8),(9,'CB','payée','2025-03-26',9),(10,'CB','payée','2025-03-27',10),(11,'espèces','en attente','2025-03-28',11),(12,'CB','payée','2025-03-29',12),(13,'CB','payée','2025-03-30',13),(14,'espèces','payée','2025-03-31',14),(15,'CB','payée','2025-04-01',15),(16,'espèces','en attente','2025-04-02',16),(17,'CB','payée','2025-04-03',17),(18,'CB','payée','2025-04-04',18),(19,'espèces','en attente','2025-04-05',19),(20,'CB','payée','2025-04-06',20),(21,'CB','payée','2025-04-01',21),(22,'espèces','en attente','2025-04-01',22),(23,'CB','payée','2025-04-02',23),(24,'CB','payée','2025-04-02',24),(25,'espèces','payée','2025-04-03',25),(26,'CB','payée','2025-04-03',26),(27,'CB','payée','2025-04-04',27),(28,'espèces','en attente','2025-04-04',28),(29,'CB','payée','2025-04-05',29),(30,'espèces','payée','2025-04-05',30),(31,'CB','payée','2025-04-06',31),(32,'CB','payée','2025-04-06',32),(33,'espèces','en attente','2025-04-07',33),(34,'CB','payée','2025-04-07',34),(35,'CB','payée','2025-04-08',35),(36,'espèces','payée','2025-04-08',36),(37,'CB','payée','2025-04-09',37),(38,'espèces','en attente','2025-04-09',38),(39,'CB','payée','2025-04-10',39),(40,'CB','payée','2025-04-10',40);
/*!40000 ALTER TABLE `transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `utilisateur` (
  `id_utilisateur` int NOT NULL,
  `nom` varchar(50) DEFAULT NULL,
  `prenom` varchar(50) DEFAULT NULL,
  `adresse` varchar(50) DEFAULT NULL,
  `station_metro` varchar(50) DEFAULT NULL,
  `telephone` varchar(20) DEFAULT NULL,
  `est_radie` tinyint(1) DEFAULT NULL,
  `mot_de_passe` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_utilisateur`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utilisateur`
--

LOCK TABLES `utilisateur` WRITE;
/*!40000 ALTER TABLE `utilisateur` DISABLE KEYS */;
INSERT INTO `utilisateur` VALUES (1,'Dupont','Alice','12 rue des Lilas','Châtelet','0123456789',0,'alice123'),(2,'Martin','Carré','5 rue Montmartre','Saint-Lazare','1122334455',0,'martin2025'),(3,'Nguyen','Thao','10 avenue de Choisy','Bercy','2233445566',0,'thao456'),(4,'Durand','Pierre','15 rue de la Paix','République','3344556677',0,'pierre789'),(5,'Leclerc','Sophie','20 boulevard Haussmann','Gare du Nord','4455667788',0,'sophie2024'),(6,'Petit','Marie','22 rue de Rivoli','Opéra','5566778899',0,'mariepass'),(7,'Leroy','Jean','33 avenue des Champs','Invalides','6677889900',0,'jeansecure'),(8,'Moreau','Luc','29 rue de Seine','Montparnasse','7788990011',0,'lucmotdepasse'),(9,'Garcia','Elena','45 avenue de la République','Nation','8899001122',0,'elena88'),(10,'Bernard','Maxime','10 rue Victor Hugo','Opéra','9900112233',0,'max2025'),(11,'Fontaine','Chloe','3 rue des Fleurs','République','1011121314',0,'chloe321'),(12,'Dumas','Antoine','77 rue de Paris','Chatelet','1112131415',0,'antoinepass'),(13,'Fournier','Camille','88 rue du Moulin','Gare du Nord','1213141516',0,'camillemdp'),(14,'Roux','Loic','56 boulevard Saint-Germain','Saint-Lazare','1314151617',0,'loicroux'),(15,'Mercier','Lucie','10 rue du Bac','Opéra','1415161718',0,'lucieParis'),(16,'Noir','Sébastien','24 avenue Foch','Invalides','1516171819',0,'seb1234'),(17,'Giraud','Manon','36 rue de Lille','Montparnasse','1617181920',0,'manon987'),(18,'Colin','Adrien','42 rue d\'Austerlitz','Bastille','1718192021',0,'adrienpass'),(19,'Perrin','Emma','50 boulevard Malesherbes','Chatelet','1819202122',0,'emmaLVP'),(20,'Boulangerie','StMichel','88 avenue Victor Hugo','Nation','1920212223',0,'boulangerie2025'),(21,'Benali','Yasmine','18 rue des Amandiers','Chatelet','0611122334',0,'yasben123'),(22,'Fernandez','Carlos','12 avenue de Clichy','Nation','0622233445',0,'carlospass'),(23,'Zhang','Mei','67 rue d\'Aboukir','République','0633344556',0,'meizhang'),(24,'Dia','Abdoulaye','9 rue Marcadet','Gare du Nord','0644455667',0,'abdia123'),(25,'Silva','Camila','77 rue de Javel','Montparnasse','0655566778',0,'camisilva'),(26,'Rahmani','Omar','33 avenue Jean Moulin','Bercy','0666677889',0,'omarrah'),(27,'Nguyen','Bao','25 rue Lecourbe','Saint-Lazare','0677788990',0,'baong'),(28,'Lopez','Isabella','80 boulevard de l\'Hôpital','Opéra','0688899001',0,'isalopez'),(29,'Kone','Aïcha','39 rue de Flandre','Chatelet','0699900112',0,'aichakone'),(30,'Gonzalez','Diego','13 rue Montorgueil','Nation','0610011223',0,'diegogz'),(31,'Bouazizi','Samir','42 rue de Belleville','République','0621122334',0,'samirbz'),(32,'Tavares','Inès','14 avenue Philippe Auguste','Bercy','0632233445',0,'inestav'),(33,'Ali','Layla','31 rue du Faubourg','Opéra','0643344556',0,'laylali'),(34,'Kim','Jiho','19 rue du Chemin Vert','Montparnasse','0654455667',0,'jihokim'),(35,'Rodriguez','Lucia','88 rue Oberkampf','Invalides','0665566778',0,'lucia123'),(36,'Sy','Mamadou','50 avenue Parmentier','Gare du Nord','0676677889',0,'mamasy'),(37,'Chavez','Maria','60 rue Popincourt','Chatelet','0687788990',0,'mariachz'),(38,'Haddad','Nour','27 rue des Pyrénées','Saint-Lazare','0698899001',0,'nourhd'),(39,'Park','Soojin','11 rue des Petites Écuries','République','0619900112',0,'soojinpk'),(40,'Da Silva','Rafael','5 avenue Gambetta','Nation','0621011223',0,'rafael75');
/*!40000 ALTER TABLE `utilisateur` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04  8:59:52
