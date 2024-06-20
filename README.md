# Projet .NET/C#
## Sujet
1. Objectifs :  
Le projet doit refléter votre maîtrise de la plateforme .NET Core. Votre thème fera l’objet d’une validation et sera individuel.

2. Technologies à utiliser
* Backoffice
  * SGBD
     * SQL Server
  * Accès aux données
    * Entity framework
  * Présentation
    * Razor page
* Frontoffice
  * SGBD
    * SQL Server
  * Accès aux données
    * ADO.NET
  * Présentation
    * MVC (core)
    * REST API (qlq partie)

3. Autres requirements
* Pagination
* Export PDF
* Import CSV
* CSS
* Indexation   
...

4. Difficultés
* BDD
* Métier (pas seulement des CRUD)
* Affichage (design convivial)

## Thème
### Food Ordering Web Application

1. Front-office : 
- inscription et login utilisateur
- liste paginée avec recherche des plats
- passer une commande
- ajouter les achats à faire dans un panier
- suivi en temps réel de l’état des commande (préparation, livraison, annulation)
- export PDF d’un bon de commande
- Notification quand la commande est arrivée (à voir)
  
2. Back-office : 
- inscription et login restaurant
- gestion des plats(CRUD)
- tableau de bord
- traitement des commandes (refus/acceptation, préparation, livraison)
- Historique des commandes en cours (état, date et heure de commande, combien d’heure s’est passé depuis)
- importation de fichier CSV pour l’insertion de nouveaux plats

3. Difficultés : 
- le suivi des commandes se fait en temps réel
- l'état de d’une commande varie (soumis, accepte, refuse)
- difficulté du côté affichage sur la fonctionnalité de notification (à voir)
- ajout multiple de plats à partir d'un fichier CSV


Pour lancer:
git clone
Changer  “Server” et “Database” dans appsettings.json
Outils > Gestionnaire de package NuGet > Console de gestionnaire de package
Add-Migration InitialCreate
Update-Database
