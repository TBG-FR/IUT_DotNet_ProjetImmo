
# IUT_DotNet_ProjetImmo
**Conception d'une Application WPF (type "ERP") pour la Gestion de Biens Immobiliers**  
*Arnaud Berlendis et Tom-Brian Garcia*

## Fonctionnalités implémentées
- `DisplayStatsPage` : Affichage de statistiques diverses sur la page d'accueil : biens vendus, biens en vente, graphiques des ventes, le tout avec des périodes (mois, année, etc). Les données sont gérées avec des propriétés, dont la plupart sont des dictionnaires, permettant de classer les statistiques selon la période de temps, le type (vente/location) et le status (disponible ou non)

- `BrowseEstatesPage` : Interface destinée au client, qui liste les biens immobiliers disponibles, et permet de les trier (par la barre de recherche, qui compare l'input avec les mot-clés et le nom/prénom des propriétaires). Lors de la sélection d'un bien, ses détails sont affichés.

- `ManageEstatesPage` : Interface destiné aux vendeurs, qui permet de voir tous les biens immobiliers (même ceux indisponibles). Lors de la sélection d'un bien, ses détails sont affichés, ainsi que des boutons permettant de gérer les transactions de ce bien. Plusieurs boutons permettent également d'ajouter, modifier ou supprimer des biens immobiliers.

- `ManageClientsPage` : Interface destiné aux vendeurs, qui permet de lister les clients existants, mais égalent d'ajouter, modifier ou supprimer des clients. On peut également effectuer une recherche. Les détails de ces derniers sont affichés lors de la sélection.

- `ManageTransactionsPage` : Interface destiné aux vendeurs, qui permet de lister toutes les transactions existantes, et de les supprimer, même si ce n'est pas recommandé (la dernière transaction donne l'état du bien immobilier, il vaut mieux interagir par l'interface de gestion des biens). Les détails des transactions sont affichées lors de la transaction. Nous avons préféré implémenter ces fonctionnalités plutôt que celles liées aux rendez-vous (RDV).

- Plusieurs fenêtres permettent de finaliser l'ajout et la modification de personnes, transactions ou biens immobiliers (`UpsertTransactionWindow`, `UpsertClientWindow`, `UpsertPersonWindow`)

## Fonctionnalités non-implémentées

- `DisplayStatsPage` : Il est actuellement impossible de switcher entre les 5 différents types de graphiques, mais ceux-ci sont fonctionnels. Nous n'avons pas réussi à implémenter un binding sur les boutons permettant de changer les différents bindings du graphique (Series, Labels, etc).

- `BrowseEstatesPage` et `ManageEstatesPage` : Les filtres ne sont pas implémentés, le bouton "Filtres" est présent et ouvre une fenêtre, mais rien n'est implémenté. Le tri par Date/Prix/Superficie n'est pas implémenté non plus, malgré la présence des boutons adaptés. Cependant, le tri par la barre de recherche fonctionne.

- `ManageClientsPage` : Les filtres ne sont pas implémentés, le bouton "Filtres" est présent et ouvre une fenêtre, mais rien n'est implémenté. Le tri par Date/Prix/Superficie n'est pas implémenté non plus, malgré la présence des boutons adaptés. Le tri par la barre de recherche fonctionne en revanche.

- `ManageTransactionsPage` : Les filtres ne sont pas implémentés, le bouton "Filtres" est présent et ouvre une fenêtre, mais rien n'est implémenté. Le tri par Date/Prix/Superficie n'est pas implémenté non plus, malgré la présence des boutons adaptés. Le tri par la barre de recherche n'a pas été testé, il ne fonctionne sûrement pas.

- Nous avons commencé à réfléchir et implémenter un système de connexion, où la première fenêtre affichée serait celle de connexion, qui ensuite redirigerait sur l'application "complète", en affichant seulement les pages liées au type d'utilisateur (client, administrateur, employé)

- Nous n'avons pas implémenté la gestion des images, par manque de temps et préférant nous concentrer sur les fonctionnalités liées aux biens et transactions.

## Bugs connus

- `DisplayStatsPage` : Les statistiques se mettaient toutes à 0 lors d'un refresh, le refresh est donc désactivé pour cette page (dans `DisplayStatsViewModel`)

- `ManageTransactionsPage` : Lors du premier chargement, le champ "client" s'affiche vide (dans les détails), mais il est fonctionnel par la suite

- `UpsertTransactionWindow` : Lorsqu'on valide une transaction, si l'on ferme la fenêtre sans valider, cela se valide quand même

## Notes

- La recherche fonctionne avec la fonction "lower()" et ne matche donc que les mots-clés en minuscule et les prénoms/nom des propriétaires également
