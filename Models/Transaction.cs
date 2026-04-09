using System;

namespace ChahriyaPro.Models;

/*
 * Fichier : Transaction.cs
 * Emplacement : Models/
 * Description : Représente une transaction financière (revenu ou dépense).
 * Contient les informations essentielles telles que le montant, la date, la catégorie et une description optionnelle.
 */

public class Transaction
{
    // Identifiant unique de la transaction
    public Guid Id { get; set; } = Guid.NewGuid();

    // Montant de la transaction (en Dinars Tunisiens - DT)
    public decimal Amount { get; set; }

    // Date à laquelle la transaction a été effectuée
    public DateTime Date { get; set; } = DateTime.Now;

    // Description ou note optionnelle pour la transaction (ex: "Courses hebdomadaires")
    public string Description { get; set; } = string.Empty;

    // Identifiant de la catégorie associée à la transaction
    public int CategoryId { get; set; }

    // Indique si la transaction est un revenu (true) ou une dépense (false)
    public bool IsIncome { get; set; }

    // Propriété calculée pour obtenir le montant formaté avec la devise (ex: "150.000 DT")
    public string FormattedAmount => $"{(IsIncome ? "+" : "-")} {Amount:N3} DT";

    // Propriété calculée pour obtenir la couleur du montant (vert pour revenu, rouge pour dépense)
    public string AmountColor => IsIncome ? "#27AE60" : "#E74C3C";
}
