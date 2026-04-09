namespace ChahriyaPro.Models;

/*
 * Fichier : Category.cs
 * Emplacement : Models/
 * Description : Représente une catégorie de transaction (ex: Alimentation, Salaire, Transport).
 * Chaque catégorie possède un nom, une icône et une couleur associée pour une identification visuelle rapide.
 */

public class Category
{
    // Identifiant unique de la catégorie
    public int Id { get; set; }

    // Nom de la catégorie (ex: "Alimentation", "Transport")
    public string Name { get; set; } = string.Empty;

    // Chemin vers l'icône de la catégorie (ex: "food_icon.png")
    public string Icon { get; set; } = string.Empty;

    // Couleur associée à la catégorie (format hexadécimal, ex: "#E74C3C")
    public string ColorHex { get; set; } = "#000000";

    // Indique si la catégorie est destinée aux revenus (true) ou aux dépenses (false)
    public bool IsIncome { get; set; }
}
