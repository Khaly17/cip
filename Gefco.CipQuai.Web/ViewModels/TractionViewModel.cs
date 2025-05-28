using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gefco.CipQuai.Web.Models
{
    public class TractionViewModel
    {
        public TractionDefinition Model { get; }

        public TractionViewModel()
        {

        }
        public TractionViewModel(TractionDefinition model)
        {
            Model = model;
            Name = model.Name;
            Id = model.Id;
            AgenceDepart_Id = model.AgenceDepart?.Id;
            AgenceArrivee_Id = model.AgenceArrivee?.Id;
            AgenceDepart = model.AgenceDepart?.Name;
            AgenceArrivee = model.AgenceArrivee?.Name;
            DaysOfWeek = model.DaysOfWeekValue;
        }

        public TractionViewModel(TractionDefinition model, DateTime startDate, DateTime endDate) : this(model)
        {
            string status = null;
            var start = startDate.Date;
            var end = endDate.Date.AddDays(1);
            while (start < end)
            {
                if (model.DaysOfWeek.Contains((Days) start.DayOfWeek))
                {
                    var traction = model.Tractions.SingleOrDefault(p => p.DueDate == start);
                    if (traction != null)
                    {
                        if (traction.IsCancelled)
                        {
                            if (status == null || status == "Pas de traction")
                                status = "Oui";
                            else if (status != "Oui")
                                status = "Pas tout annulé";
                        }
                        else
                        {
                            if (status == null || status == "Pas de traction")
                                status = "";
                            else if (status == "Oui")
                                status = "Pas tout annulé";
                        }
                    }
                    else
                    {
                        if (status == null || status == "Pas de traction")
                            status = "";
                        else if (status == "Oui")
                            status = "Pas tout annulé";
                    }
                }
                else
                {
                    if (status == null)
                        status = "Pas de traction";
                }
                start = start.AddDays(1);
            }
            CheckCancelled = status;

        }

        public string Name { get; set; }
        public string AgenceDepart_Id { get; set; }
        public string AgenceArrivee_Id { get; set; }
        public string AgenceDepart { get; set; }
        public string AgenceArrivee { get; set; }
        public string DaysOfWeek { get; set; }
        public string CheckCancelled { get; set; }

        public string Id { get; set; }

        public void UpdateModel(TractionDefinition dbItem)
        {
            dbItem.Name = Name;
            dbItem.DaysOfWeekValue = DaysOfWeek;
            //dbItem.Region_Id
        }

    }

    public class JoursFeriesViewModel
    {
        public JoursFeriesViewModel(List<TractionDefinition> list, DateTime startDate, DateTime endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            Tractions = list.Select(p => new TractionViewModel(p, startDate, endDate)).ToList();
        }


        public List<TractionViewModel> Tractions { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}