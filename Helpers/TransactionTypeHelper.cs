// ============================================
// FICHIER: TransactionTypeHelper.cs
// EMPLACEMENT: Helpers/
// RÔLE: Gère la sélection du type de transaction (Revenu/Dépense).
// CORRECTION: Assure que les paramètres des boutons sont correctement convertis en booléens.
// ============================================

using CommunityToolkit.Mvvm.Input;
using System;

namespace ChahriyaPro.Helpers;

/// <summary>
/// Helper pour gérer le type de transaction (Revenu ou Dépense).
/// Ce fichier centralise la logique de sélection du type pour l'UI.
/// </summary>
public class TransactionTypeHelper
{
    // Action qui sera appelée dans le ViewModel quand le type change
    private readonly Action<bool> _onTypeChanged;

    // Stocke le type actuel (true = Revenu, false = Dépense)
    private bool _isIncome;

    /// <summary>
    /// Initialise le helper avec une callback pour notifier les changements.
    /// </summary>
    /// <param name="onTypeChanged">Action à exécuter lors du changement de type.</param>
    public TransactionTypeHelper(Action<bool> onTypeChanged)
    {
        _onTypeChanged = onTypeChanged;
        _isIncome = false; // Par défaut, on commence sur "Dépense"
    }

    /// <summary>
    /// Obtient ou définit si la transaction est un revenu.
    /// </summary>
    public bool IsIncome
    {
        get => _isIncome;
        set
        {
            if (_isIncome != value)
            {
                _isIncome = value;
                // Notifie le ViewModel que le type a changé
                _onTypeChanged?.Invoke(_isIncome);
            }
        }
    }

    /// <summary>
    /// Command pour définir le type (utilisée par les boutons XAML).
    /// </summary>
    public IRelayCommand<object?> SetTypeCommand => new RelayCommand<object?>(SetType);

    /// <summary>
    /// Méthode interne appelée par la Command pour traiter le paramètre.
    /// </summary>
    /// <param name="parameter">La valeur envoyée par le bouton (True ou False).</param>
    private void SetType(object? parameter)
    {
        // CORRECTION: Gestion robuste de la conversion du paramètre
        if (parameter == null) return;

        if (parameter is bool boolVal)
        {
            IsIncome = boolVal;
        }
        else if (bool.TryParse(parameter.ToString(), out bool parsedVal))
        {
            IsIncome = parsedVal;
        }
    }

    /// <summary>
    /// Retourne le nom textuel du type actuel.
    /// </summary>
    public string GetTypeName()
    {
        return IsIncome ? "Revenu" : "Dépense";
    }

    /// <summary>
    /// Retourne la couleur hexadécimale associée au type actuel.
    /// </summary>
    public string GetColorHex()
    {
        // Vert pour Revenu, Rouge pour Dépense
        return IsIncome ? "#00C853" : "#FF3D00";
    }
}
