using CommunityToolkit.Mvvm.ComponentModel;

namespace ChahriyaPro.ViewModels;

/*
 * Fichier : BaseViewModel.cs
 * Emplacement : ViewModels/
 * Description : Classe de base pour tous les ViewModels de l'application.
 * Elle hérite de ObservableObject pour fournir une implémentation de INotifyPropertyChanged,
 * permettant ainsi la mise à jour automatique de l'interface utilisateur lors du changement des propriétés.
 */

public partial class BaseViewModel : ObservableObject
{
    // Propriété indiquant si le ViewModel est en cours de chargement (ex: lors d'une opération asynchrone)
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    // Propriété inverse de IsBusy pour faciliter le binding dans le XAML
    public bool IsNotBusy => !IsBusy;

    // Titre de la page associée au ViewModel
    [ObservableProperty]
    private string title = string.Empty;
}
