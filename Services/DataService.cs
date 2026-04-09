// ===========================================================================
// FICHIER : DataService.cs
// EMPLACEMENT : /Services/
// RÔLE : Gère le stockage et la récupération des données de l'application.
// DESCRIPTION : Ce service simule une base de données en utilisant des listes 
//               statiques en mémoire. Il permet de gérer les transactions 
//               financières et les catégories associées.
// ===========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChahriyaPro.Models;

namespace ChahriyaPro.Services;

/// <summary>
/// Service central pour la gestion des données de l'application.
/// </summary>
public class DataService
{
    // Liste statique pour stocker les transactions durant la session
    private static readonly List<Transaction> _transactions = new();
    
    // Liste statique pour stocker les catégories disponibles
    private static readonly List<Category> _categories = new();

    /// <summary>
    /// Constructeur : Initialise les catégories par défaut au premier lancement.
    /// </summary>
    public DataService()
    {
        // Si aucune catégorie n'est présente, on initialise les valeurs par défaut
        if (!_categories.Any())
        {
            InitializeDefaultCategories();
        }
    }

    /// <summary>
    /// Récupère la liste de toutes les transactions enregistrées.
    /// </summary>
    /// <returns>Une liste de transactions triées par date décroissante.</returns>
    public Task<List<Transaction>> GetTransactionsAsync()
    {
        // Retourne les transactions triées de la plus récente à la plus ancienne
        return Task.FromResult(_transactions.OrderByDescending(t => t.Date).ToList());
    }

    /// <summary>
    /// Enregistre une nouvelle transaction.
    /// </summary>
    /// <param name="transaction">L'objet transaction à sauvegarder.</param>
    public Task SaveTransactionAsync(Transaction transaction)
    {
        // Ajoute la transaction à la liste en mémoire
        _transactions.Add(transaction);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Récupère la liste de toutes les catégories.
    /// </summary>
    /// <returns>La liste complète des catégories.</returns>
    public Task<List<Category>> GetCategoriesAsync()
    {
        // Retourne une copie de la liste des catégories
        return Task.FromResult(_categories.ToList());
    }

    /// <summary>
    /// Initialise les catégories par défaut adaptées au contexte tunisien.
    /// </summary>
    private void InitializeDefaultCategories()
    {
        // --- Catégories de Dépenses (IsIncome = false) ---
        _categories.Add(new Category { Id = 1, Name = "Alimentation", Icon = "food.png", ColorHex = "#E67E22", IsIncome = false });
        _categories.Add(new Category { Id = 2, Name = "Transport (Taxi/Louage)", Icon = "transport.png", ColorHex = "#3498DB", IsIncome = false });
        _categories.Add(new Category { Id = 3, Name = "Factures (STEG/SONEDE)", Icon = "bill.png", ColorHex = "#E74C3C", IsIncome = false });
        _categories.Add(new Category { Id = 4, Name = "Loyer", Icon = "home.png", ColorHex = "#9B59B6", IsIncome = false });
        _categories.Add(new Category { Id = 5, Name = "Loisirs", Icon = "fun.png", ColorHex = "#F1C40F", IsIncome = false });
        _categories.Add(new Category { Id = 6, Name = "Santé", Icon = "health.png", ColorHex = "#2ECC71", IsIncome = false });

        // --- Catégories de Revenus (IsIncome = true) ---
        _categories.Add(new Category { Id = 10, Name = "Salaire (Chahriya)", Icon = "salary.png", ColorHex = "#27AE60", IsIncome = true });
        _categories.Add(new Category { Id = 11, Name = "Revenus Supplémentaires", Icon = "extra.png", ColorHex = "#16A085", IsIncome = true });
        _categories.Add(new Category { Id = 12, Name = "Cadeaux", Icon = "gift.png", ColorHex = "#D35400", IsIncome = true });
    }
}
