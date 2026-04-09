// ============================================
// FICHIER: AppShell.xaml.cs
// EMPLACEMENT: Racine du projet
// ============================================

using ChahriyaPro.Views;

namespace ChahriyaPro;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // ✅ CORRECTION: AJOUTER HistoryPage! (MANQUANTE DANS TON CODE)
        Routing.RegisterRoute(nameof(AddTransactionPage), typeof(AddTransactionPage));
        Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
    }
}