using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui; // ✅ L'IMPORT INDISPENSABLE
using ChahriyaPro.ViewModels;
using ChahriyaPro.Views;
using ChahriyaPro.Services;
using ChahriyaPro.Helpers;

namespace ChahriyaPro;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>() // ✅ On initialise l'App
            .UseMauiCommunityToolkit() // ✅ ON CHAINE LE TOOLKIT ICI (Fix Error MCT001)
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // ========================================================================
        // INJECTION DE DÉPENDANCES (DI)
        // ========================================================================

        // --- 1. Services (Logique métier et Data) ---
        // Singleton : Une seule instance pour toute la durée de vie de l'app
        builder.Services.AddSingleton<DataService>();

        // --- 2. Helpers ---
        // Transient : Une nouvelle instance à chaque fois qu'on en a besoin
        builder.Services.AddTransient<TransactionTypeHelper>();

        // --- 3. ViewModels (Le cerveau de tes pages) ---
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<AddTransactionViewModel>();
        // ✅ J'ajoute le ViewModel de l'historique si tu l'as créé
        // builder.Services.AddTransient<HistoryViewModel>(); 

        // --- 4. Pages (L'interface UI) ---
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<AddTransactionPage>();
        builder.Services.AddTransient<HistoryPage>(); // ✅ Ajouté comme demandé

        return builder.Build();
    }
}