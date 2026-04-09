using System.Globalization;
using Microsoft.Maui.Graphics;

namespace ChahriyaPro.Helpers;

/*
 * Fichier : InverseBoolToColorConverter.cs
 * Emplacement : Helpers/
 * Description : Convertisseur de valeur XAML qui transforme l'inverse d'un booléen en une couleur spécifique.
 * Utilisé pour styliser les boutons de type (Dépense vs Revenu) de manière contrastée.
 */

public class InverseBoolToColorConverter : IValueConverter
{
    // Méthode de conversion : !Bool -> Color
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is Color colorParameter)
        {
            // Si la valeur est fausse (ex: IsIncome = false), retourne la couleur passée en paramètre (ex: Rouge Dépense)
            return !boolValue ? colorParameter : Color.FromArgb("#ADB5BD");
        }
        return Color.FromArgb("#ADB5BD");
    }

    // Méthode de conversion inverse (non utilisée ici)
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
