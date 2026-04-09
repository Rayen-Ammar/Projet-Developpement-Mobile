using System.Globalization;
using Microsoft.Maui.Graphics;

namespace ChahriyaPro.Helpers;

/*
 * Fichier : BoolToColorConverter.cs
 * Emplacement : Helpers/
 * Description : Convertisseur de valeur XAML qui transforme un booléen en une couleur spécifique.
 * Utilisé pour changer dynamiquement la couleur des éléments de l'interface utilisateur en fonction de l'état (ex: Revenu vs Dépense).
 */

public class BoolToColorConverter : IValueConverter
{
    // Méthode de conversion : Bool -> Color
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool boolValue && parameter is Color colorParameter)
        {
            // Si la valeur est vraie, retourne la couleur passée en paramètre, sinon retourne une couleur par défaut (gris)
            return boolValue ? colorParameter : Color.FromArgb("#ADB5BD");
        }
        return Color.FromArgb("#ADB5BD");
    }

    // Méthode de conversion inverse (non utilisée ici)
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
