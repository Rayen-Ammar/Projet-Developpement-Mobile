// ============================================
// FICHIER: MainViewModel.cs
// EMPLACEMENT: ViewModels/
// RÔLE: Gère la logique du tableau de bord et de l'historique mensuel.
// AMÉLIORATION: Ajout de l'historique groupé par mois avec totaux par mois.
// ============================================

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using ChahriyaPro.Models;
using ChahriyaPro.Services;
using ChahriyaPro.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization;

namespace ChahriyaPro.ViewModels;

/// <summary>
/// Modèle de données pour grouper les transactions par mois dans l'historique.
/// </summary>
public class MonthlyHistoryGroup : ObservableCollection<Transaction>
{
    public string MonthName { get; set; } = string.Empty;
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public string FormattedMonthlyIncome => $"+{MonthlyIncome:N3} DT";
    public string FormattedMonthlyExpenses => $"-{MonthlyExpenses:N3} DT";

    public MonthlyHistoryGroup(string monthName, List<Transaction> transactions) : base(transactions)
    {
        MonthName = monthName;
        MonthlyIncome = transactions.Where(t => t.IsIncome).Sum(t => t.Amount);
        MonthlyExpenses = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);
    }
}

/// <summary>
/// ViewModel principal gérant l'état de l'application (Tableau de bord et Historique).
/// </summary>
public partial class MainViewModel : BaseViewModel
{
    private readonly DataService _dataService;

    // Collections pour l'UI
    public ObservableCollection<Transaction> RecentTransactions { get; } = new();
    public ObservableCollection<MonthlyHistoryGroup> MonthlyHistory { get; } = new();

    // Propriétés observables pour les totaux globaux
    [ObservableProperty]
    private decimal totalIncome;

    [ObservableProperty]
    private decimal totalExpenses;

    [ObservableProperty]
    private decimal currentBalance;

    // Propriétés formatées pour l'affichage (Lecture seule)
    public string FormattedBalance => $"{CurrentBalance:N3} DT";
    public string FormattedIncome => $"+{TotalIncome:N3} DT";
    public string FormattedExpenses => $"-{TotalExpenses:N3} DT";

    /// <summary>
    /// Constructeur avec injection du service de données.
    /// </summary>
    /// <param name="dataService">Le service gérant les transactions.</param>
    public MainViewModel(DataService dataService)
    {
        _dataService = dataService;
        Title = "Chahriya Pro";
    }

    /// <summary>
    /// Charge les données du tableau de bord et l'historique mensuel.
    /// </summary>
    [RelayCommand]
    public async Task LoadDashboardAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Récupère toutes les transactions depuis le service
            var transactions = await _dataService.GetTransactionsAsync();

            // 1. Mise à jour des transactions récentes (Top 5)
            RecentTransactions.Clear();
            foreach (var transaction in transactions.OrderByDescending(t => t.Date).Take(5))
            {
                RecentTransactions.Add(transaction);
            }

            // 2. Calcul des totaux globaux
            TotalIncome = transactions.Where(t => t.IsIncome).Sum(t => t.Amount);
            TotalExpenses = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);
            CurrentBalance = TotalIncome - TotalExpenses;

            // 3. Construction de l'historique mensuel
            BuildMonthlyHistory(transactions);

            // Notification de changement pour les propriétés calculées
            OnPropertyChanged(nameof(FormattedBalance));
            OnPropertyChanged(nameof(FormattedIncome));
            OnPropertyChanged(nameof(FormattedExpenses));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erreur", "Impossible de charger les données : " + ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Groupe les transactions par mois pour l'affichage de l'historique.
    /// </summary>
    /// <param name="transactions">Liste complète des transactions.</param>
    private void BuildMonthlyHistory(List<Transaction> transactions)
    {
        MonthlyHistory.Clear();

        // Groupement par année et mois
        var grouped = transactions
            .OrderByDescending(t => t.Date)
            .GroupBy(t => new { t.Date.Year, t.Date.Month });

        foreach (var group in grouped)
        {
            // Nom du mois (ex: "Mars 2026")
            string monthName = new DateTime(group.Key.Year, group.Key.Month, 1)
                .ToString("MMMM yyyy", CultureInfo.CurrentCulture);
            
            // Capitaliser la première lettre
            monthName = char.ToUpper(monthName[0]) + monthName.Substring(1);

            MonthlyHistory.Add(new MonthlyHistoryGroup(monthName, group.ToList()));
        }
    }

    /// <summary>
    /// Navigation vers la page d'ajout de transaction.
    /// </summary>
    [RelayCommand]
    public async Task GoToAddTransactionAsync()
    {
        await Shell.Current.GoToAsync(nameof(AddTransactionPage));
    }

    /// <summary>
    /// Navigation vers la page d'historique complet.
    /// </summary>
    [RelayCommand]
    public async Task GoToHistoryAsync()
    {
        await Shell.Current.GoToAsync(nameof(HistoryPage));
    }
}
