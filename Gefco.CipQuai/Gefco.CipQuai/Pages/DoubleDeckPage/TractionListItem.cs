using System;
using Gefco.CipQuai.ApiClient.Models;

namespace Gefco.CipQuai.DoubleDeckPage
{
    public partial class DPViewModel
    {
        public class TractionListItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsDeclaration { get; set; }
            public string DeclarationId { get; set; }

            public Agence AgenceDepart { get; set; }
            public Agence AgenceArrivee { get; set; }
            public DateTime DueDate { get; set; }

            public TractionListItem(Traction traction)
            {
                Id = traction.Id;
                if (TimeZoneInfo.ConvertTimeFromUtc(traction.DueDate, timeInfo).Date == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeInfo).Date)
                    Name = traction.Name;
                else
                    Name = $"{TimeZoneInfo.ConvertTimeFromUtc(traction.DueDate, timeInfo):dd/MM} -> {traction.Name}";
                AgenceDepart = traction.AgenceDepart;
                AgenceArrivee = traction.AgenceArrivee;
                DueDate = traction.DueDate;
            }
            public TractionListItem(DeclarationDoublePlancher declaration)
            {
                Id = declaration.Traction.Id;
                Name = $"{TimeZoneInfo.ConvertTimeFromUtc(declaration.Traction.DueDate, timeInfo):dd/MM} -> {declaration.Traction.Name}";
                AgenceDepart = declaration.Traction.AgenceDepart;
                AgenceArrivee = declaration.Traction.AgenceArrivee;
                DueDate = declaration.Traction.DueDate;
                IsDeclaration = true;
                DeclarationId = declaration.Id;
            }
        }
    }
}