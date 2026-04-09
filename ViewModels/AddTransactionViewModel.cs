// ===========================================================================
// FICHIER : AddTransactionViewModel.cs
// EMPLACEMENT : /ViewModels/
// RÔLE : Gère la logique métier de la page d'ajout de transaction.
// DESCRIPTION : Ce ViewModel assure la saisie des données, la validation du 
//               montant et de la catégorie, et l'enregistrement de la transaction 
//               via le service de données.
// ===========================================================================

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using ChahriyaPro.Models;
using ChahriyaPro.Services;
using ChahriyaPro.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChahriyaPro.ViewModels;

/// <summary>
/// ViewModel pour la page d'ajout de transaction (Revenu ou Dépense).
/// </summary>
public partial class AddTransactionViewModel : BaseViewModel
{
    // Services et Helpers injectés
    private readonly DataService _dataService;
    private readonly TransactionTypeHelper _typeHelper;

    // --- Propriétés Observables (Bindées à l'UI) ---
    
    // Le montant saisi par l'utilisateur
    [ObservableProperty]
    private decimal amount;

    // La date de la transaction (par défaut : aujourd'hui)
    [ObservableProperty]
    private DateTime date = DateTime.Now;

    // Description facultative de la transaction
    [ObservableProperty]
    private string description = string.Empty;

    // Indique si la transaction est un revenu (true) ou une dépense (false)
    [ObservableProperty]
    private bool isIncome;

    // La catégorie sélectionnée par l'utilisateur
    [ObservableProperty]
    private Category? selectedCategory;

    // Nom textuel du type pour l'affichage (Revenu/Dépense)
    [ObservableProperty]
    private string transactionTypeName = "Dépense";

    // Liste des catégories filtrées à afficher dans le sélecteur
    public ObservableCollection<Category> Categories { get; } = new();

    /// <summary>
    /// Constructeur : Initialise les services et charge les catégories.
    /// </summary>
    /// <param name="dataService">Le service de données injecté.</param>
    public AddTransactionViewModel(DataService dataService)
    {
        _dataService = dataService;
        Title = "Ajouter Transaction";

        // Initialisation du helper de type avec une callback de mise à jour
        _typeHelper = new TransactionTypeHelper(OnTransactionTypeChanged);

        // Liaison de la commande de changement de type
        SetTransactionTypeCommand = _typeHelper.SetTypeCommand;

        // Chargement initial des catégories (par défaut : Dépenses)
        LoadCategories();
    }

    /// <summary>
    /// Callback exécutée lorsque l'utilisateur change le type de transaction.
    /// </summary>
    /// <param name="isIncomeValue">Nouvelle valeur du type.</param>
    private void OnTransactionTypeChanged(bool isIncomeValue)
    {
        // Mise à jour des propriétés locales
        IsIncome = isIncomeValue;
        TransactionTypeName = _typeHelper.GetTypeName();

        // Rafraîchissement de la liste des catégories selon le nouveau type
        FilterCategoriesByType();
    }

    /// <summary>
    /// Filtre et remplit la collection de catégories selon le type (Revenu/Dépense).
    /// </summary>
    private void FilterCategoriesByType()
    {
        // Récupération asynchrone des catégories
        var allCategories = _dataService.GetCategoriesAsync().Result;
        
        // Nettoyage et filtrage
        Categories.Clear();
        foreach (var cat in allCategories.Where(c => c.IsIncome == IsIncome))
        {
            Categories.Add(cat);
        }
    }

    /// <summary>
    /// Commande pour charger les catégories initialement.
    /// </summary>
    [RelayCommand]
    private void LoadCategories()
    {
        FilterCategoriesByType();
    }

    // Commande exposée à l'UI pour changer le type de transaction
    public IRelayCommand<object?> SetTransactionTypeCommand { get; }

    /// <summary>
    /// Commande pour annuler et revenir au tableau de bord.
    /// </summary>
    [RelayCommand]
    public async Task CancelAsync()
    {
        // Retourne à la page principale via le Shell
        await Shell.Current.GoToAsync("//MainPage");
    }

    /// <summary>
    /// Commande pour valider et enregistrer la transaction.
    /// </summary>
    [RelayCommand]
    public async Task SaveTransactionAsync()
    {
        // Validation : Le montant doit être positif
        if (Amount <= 0)
        {
            await Shell.Current.DisplayAlert("Erreur", "Le montant doit être supérieur à 0", "OK");
            return;
        }

        // Validation : Une catégorie doit être choisie
        if (SelectedCategory == null)
        {
            await Shell.Current.DisplayAlert("Erreur", "Veuillez sélectionner une catégorie", "OK");
            return;
        }

        try
        {
            // Création de l'objet transaction à partir des saisies
            var transaction = new Transaction
            {
                Amount = Amount,
                Date = Date,
                Description = string.IsNullOrWhiteSpace(Description) ? SelectedCategory.Name : Description,
                CategoryId = SelectedCategory.Id,
                IsIncome = IsIncome
            };

            // Sauvegarde via le service
            await _dataService.SaveTransactionAsync(transaction);
            
            // Notification de succès et retour au menu
            await Shell.Current.DisplayAlert("Succès", "Transaction enregistrée !", "OK");
            await Shell.Current.GoToAsync("//MainPage");
        }
        catch (Exception ex)
        {
            // Gestion des erreurs imprévues
            await Shell.Current.DisplayAlert("Erreur", "Une erreur est survenue : " + ex.Message, "OK");
        }
    }
}
