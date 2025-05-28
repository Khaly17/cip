using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Gefco.CipQuai.Web.Models;
using Microsoft.Ajax.Utilities;

namespace Gefco.CipQuai.Web.Controllers
{
    public enum Zone
    {
        National,
        Regional,
        Agence
    }

    public abstract class ZoneController : Controller
    {
        protected readonly ApplicationDbContext Ctx;
        public Zone Zone { get; }

        protected ZoneController(Zone zone)
        {
            Ctx = new ApplicationDbContext();
            Zone = zone;
        }
        protected override void Dispose(bool disposing)
        {
            Ctx.Dispose();
            base.Dispose(disposing);
        }

        public HeatmapDataSource HeatmapDataSource(FilterViewModel viewModel, DateTime endDate, DateTime startDate)
        {
            List<TractionDefinition> definitions = null;
            List<Traction> tractionsHorsDefinition = null;
            int x = 0;
            Dictionary<TractionDefinition, int> offDays = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> dpUsed = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> justified = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> toDeclare = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> notDeclared = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> deleted = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> toJustify = new Dictionary<TractionDefinition, int>();
            Dictionary<TractionDefinition, int> notValid = new Dictionary<TractionDefinition, int>();
            List<Traction> tractions = null;
            List<DeclarationDoublePlancher> dpDeclarées = null;

            switch (Zone)
            {
                case Zone.National:
                    definitions = Ctx.TractionDefinitions.Where(p => !p.IsDeleted && p.Tractions.Any(t => t.DueDate < endDate && t.DueDate >= startDate && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name)))).OrderBy(p => p.AgenceDepart.Region.Name).ThenBy(p => p.AgenceDepart.Name).ToList();
                    tractionsHorsDefinition = Ctx.Tractions.Where(t => t.DueDate < endDate && t.DueDate >= startDate && t.TractionDefinition == null && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name))).ToList();
                    tractions = Ctx.Tractions.Where(t => t.DueDate < endDate && t.DueDate >= startDate && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name))).ToList();
                    dpDeclarées = Ctx.DeclarationDoublePlanchers.Where(d => d.Traction != null && d.Traction.DueDate < endDate && d.Traction.DueDate >= startDate && RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name)).ToList();
                    break;
                case Zone.Regional:
                    definitions = Ctx.TractionDefinitions.Where(p => !p.IsDeleted && p.AgenceDepart.Region_Id == viewModel.Id && p.Tractions.Any(t => !t.IsDeleted && t.DueDate < endDate && t.DueDate >= startDate && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name)))).OrderBy(p => p.AgenceDepart.Region.Name).ThenBy(p => p.AgenceDepart.Name).ToList();
                    tractionsHorsDefinition = Ctx.Tractions.Where(t => t.DueDate < endDate && t.DueDate >= startDate && t.TractionDefinition == null && t.AgenceDepart.Region_Id == viewModel.Id && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name))).ToList();
                    tractions = Ctx.Tractions.Where(t => t.AgenceDepart.Region_Id == viewModel.Id && t.DueDate < endDate && t.DueDate >= startDate && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name))).ToList();
                    dpDeclarées = Ctx.DeclarationDoublePlanchers.Where(d => d.Traction != null && d.Traction.AgenceDepart.Region_Id == viewModel.Id && d.Traction.DueDate < endDate && d.Traction.DueDate >= startDate && RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name)).ToList();
                    break;
                case Zone.Agence:
                    definitions = Ctx.TractionDefinitions.Where(p => !p.IsDeleted && p.AgenceDepart_Id == viewModel.Id && p.Tractions.Any(t => !t.IsDeleted && t.DueDate < endDate && t.DueDate >= startDate && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name)))).OrderBy(p => p.AgenceDepart.Region.Name).ThenBy(p => p.AgenceDepart.Name).ToList();
                    tractionsHorsDefinition = Ctx.Tractions.Where(t => t.DueDate < endDate && t.DueDate >= startDate && t.TractionDefinition == null && t.AgenceDepart.Id == viewModel.Id && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name))).ToList();
                    tractions = Ctx.Tractions.Where(t => t.AgenceDepart.Id == viewModel.Id && t.DueDate < endDate && t.DueDate >= startDate && t.Declarations.Any(d => RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name))).ToList();
                    dpDeclarées = Ctx.DeclarationDoublePlanchers.Where(d => d.Traction != null && d.Traction.AgenceDepart.Id == viewModel.Id && d.Traction.DueDate < endDate && d.Traction.DueDate >= startDate && RemorqueStatus.DashboardStatuses.Contains(d.CurrentStatus.Name)).ToList();
                    break;
            }

            definitions.AddRange(tractionsHorsDefinition.Select(p =>
            {
                x++;
                var name = "#" + x + " " + p.Name;
                p.TractionDefinition = new TractionDefinition()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = name,
                    Tractions = new List<Traction>()
                    {
                        p
                    }
                };
                toDeclare.Add(p.TractionDefinition, 1);
                return p.TractionDefinition;
            }));

            HeatmapDataSource dataSource = new HeatmapDataSource();
            List<List<string>> ids = new List<List<string>>();
            List<List<DateTime>> names = new List<List<DateTime>>();

            int usedDPsOut = 0, justifiedDPsOut = 0, toDeclareDPsOut = 0, notDeclaredDPsOut = 0, toJustifyDPsOut = 0, notValidDPsOut = 0;

            for (var i = 0; startDate.AddDays(i) < endDate; i++)
            {
                var date = startDate.AddDays(i).Date;
                var dat = date.ToString("dd/MM/yyyy");
                var idss = new List<string>();
                var namess = new List<DateTime>();
                int usedDPs = 0, justifiedDPs = 0, toDeclareDPs = 0, notDeclaredDPs = 0, toJustifyDPs = 0, notValidDPs = 0, deletedDPs = 0;

                for (int j = 0; j < definitions.Count; j++)
                {
                    TractionDefinition definition = definitions.ElementAt(j);
                    if (!toDeclare.ContainsKey(definition))
                        toDeclare.Add(definition, 0);
                    if (!dpUsed.ContainsKey(definition))
                        dpUsed.Add(definition, 0);
                    if (!justified.ContainsKey(definition))
                        justified.Add(definition, 0);
                    if (!notDeclared.ContainsKey(definition))
                        notDeclared.Add(definition, 0);
                    if (!offDays.ContainsKey(definition))
                        offDays.Add(definition, 0);
                    if (!deleted.ContainsKey(definition))
                        deleted.Add(definition, 0);
                    if (!toJustify.ContainsKey(definition))
                        toJustify.Add(definition, 0);
                    if (!notValid.ContainsKey(definition))
                        notValid.Add(definition, 0);

                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dataSource.data.Add(new HeatmapData()
                        {
                            rowid = definition.Name,
                            columnid = dat,
                            id = "0"
                        });
                        idss.Add(null);
                        namess.Add(date);
                        offDays[definition]++;
                        continue;
                    }
                    if (definition.DaysOfWeek.Contains((Days)date.DayOfWeek))
                    {
                        toDeclare[definition]++;
                        toDeclareDPs++;
                    }

                    var traction = tractions.SingleOrDefault(p => p.TractionDefinition?.Id == definition.Id && p.DueDate.Date == date);
                    DeclarationDoublePlancher dp = null;
                    if (traction != null)
                    {
                        dp = dpDeclarées.SingleOrDefault(p => p.Traction_Id == traction.Id && p.Traction.DueDate.Date == traction.DueDate.Date);
                    }
                    else if (definition.CreationDate == DateTime.MinValue)
                    {
                        offDays[definition]++;
                        continue;
                    }

                    if (traction?.IsCancelled ?? false)
                    {
                        dataSource.data.Add(new HeatmapData()
                        {
                            rowid = definition.Name,
                            columnid = dat,
                            value = -4,
                            id = dp?.Id ?? "0"
                        });
                        //todo justified[definition]++;
                    }
                    else if (dp == null)
                    {
                        if (definition.DaysOfWeek.Contains((Days)date.DayOfWeek))
                        {
                            notDeclared[definition]++;
                            notDeclaredDPs++;
                        }
                        dataSource.data.Add(new HeatmapData()
                        {
                            rowid = definition.Name,
                            columnid = dat,
                            value = -1,
                            id = "0"
                        });
                        idss.Add("");
                        namess.Add(date);
                    }
                    else
                    {
                        idss.Add(dp.Id);
                        namess.Add(date);
                        if (dp.IsDeleted)
                        {
                            dataSource.data.Add(new HeatmapData()
                            {
                                rowid = definition.Name,
                                columnid = dat,
                                value = -7,
                                id = dp.Id
                            });
                            deleted[definition]++;
                            deletedDPs++;
                        }
                        else if (dp.CurrentStatus.Name == "ToBeValidated")
                        {
                            dataSource.data.Add(new HeatmapData()
                            {
                                rowid = definition.Name,
                                columnid = dat,
                                value = -2,
                                id = dp.Id
                            });
                            //todo toValidate[definition]++;
                        }
                        else if (dp.CurrentStatus.Name == "ToJustify")
                        {
                            dataSource.data.Add(new HeatmapData()
                            {
                                rowid = definition.Name,
                                columnid = dat,
                                value = -5,
                                id = dp.Id
                            });
                            toJustifyDPs++;
                            toJustify[definition]++;
                        }
                        else if (dp.CurrentStatus.Name == "NotValid")
                        {
                            dataSource.data.Add(new HeatmapData()
                            {
                                rowid = definition.Name,
                                columnid = dat,
                                value = -6,
                                id = dp.Id
                            });
                            notValidDPs++;
                            notValid[definition]++;
                        }
                        else if (dp.IsDPUsed)
                        {
                            dataSource.data.Add(new HeatmapData()
                            {
                                rowid = definition.Name,
                                columnid = dat,
                                value = -3,
                                id = dp.Id
                            });
                            usedDPs++;
                            dpUsed[definition]++;
                        }
                        else
                        {
                            dataSource.data.Add(new HeatmapData()
                            {
                                rowid = definition.Name,
                                columnid = dat,
                                value = -4,
                                id = dp.Id
                            });
                            justifiedDPs++;
                            justified[definition]++;
                        }
                    }
                }

                //Calcul T1 et T2 du jour
                var i1 = usedDPs + justifiedDPs + notDeclaredDPs + notValidDPs + toJustifyDPs;
                if (i1 > 0)
                {
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = "T1 Taux de déclarations conformes",
                        columnid = dat,
                        value = (int?) Math.Ceiling((usedDPs + justifiedDPs) * 100.0 / i1),
                        id = "0"
                    });
                }
                else
                {
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = "T1 Taux de déclarations conformes",
                        columnid = dat,
                        value = 0,
                        id = "0"
                    });
                }
                var i2 = toDeclareDPs - deletedDPs;
                if (i2 > 0)
                {
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = "T2 Taux d’utilisation des remorques DP",
                        columnid = dat,
                        value = (int?) Math.Ceiling(usedDPs * 100.0 / i2),
                        id = "0"
                    });
                }
                else
                {
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = "T2 Taux d’utilisation des remorques DP",
                        columnid = dat,
                        value = 0,
                        id = "0"
                    });
                }
                ids.Add(idss);
                names.Add(namess);

                usedDPsOut += usedDPs;
                justifiedDPsOut += justifiedDPs;
                toDeclareDPsOut += toDeclareDPs - deletedDPs;
                notDeclaredDPsOut += notDeclaredDPs;
                toJustifyDPsOut += toJustifyDPs;
                notValidDPsOut += notValidDPs;
            }
            //Calcul T1 et T2 traction
            foreach (var definition in definitions)
            {
                var t1 = dpUsed[definition] + justified[definition] + notDeclared[definition] + notValid[definition] + toJustify[definition];
                var days = (endDate - startDate).Days - offDays[definition];
                if (t1 > 0)
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = definition.Name,
                        columnid = "T1",
                        value = (int?) Math.Ceiling((dpUsed[definition] + justified[definition]) * 100.0 / t1),
                        id = "0"
                    });
                else
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = definition.Name,
                        columnid = "T1",
                        value = 0,
                        id = "0"
                    });
                if (toDeclare[definition] - deleted[definition] > 0)
                {
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = definition.Name,
                        columnid = "T2",
                        value = (int?) Math.Ceiling(dpUsed[definition] * 100.0 / (toDeclare[definition] - deleted[definition])),
                        id = "0"
                    });

                }
                else
                {
                    dataSource.data.Add(new HeatmapData()
                    {
                        rowid = definition.Name,
                        columnid = "T2",
                        value = 0,
                        id = "0"
                    });

                }
                //var name1 = dpUsed[name];
            }

            List<string> yLabels = new List<string>();
            yLabels.AddRange(definitions.Select(p => p.Name));
            yLabels.AddRange(new List<string>()
            {
                "T1 Taux de déclarations conformes" , "T2 Taux d’utilisation des remorques DP", ""
            });
            ViewBag.yLabels = yLabels.ToArray();
            ViewBag.border = new
            {
                color = "white"
            };
            ViewBag.height = ((definitions.Count + 1) * 30) + 100 + "px";
            ViewBag.ids = ids;
            ViewBag.dates = names;
            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            ViewBag.ListeUrl = Url.Action("ListeDP", new
            {
                viewModel.FilterType,
                viewModel.Id,
                startDate = viewModel.StartDate.ToString("dd/MM/yyyy"),
                endDate = viewModel.EndDate.ToString("dd/MM/yyyy")
            });
            ViewBag.GrilleDPAUrl = Url.Action("GrilleDPA", new
            {
                viewModel.FilterType,
                viewModel.Id,
                startDate = viewModel.StartDate.ToString("dd/MM/yyyy"),
                endDate = viewModel.EndDate.ToString("dd/MM/yyyy")
            });

            TempData["usedDPsOut"] = usedDPsOut;
            TempData["justifiedDPsOut"] = justifiedDPsOut;
            TempData["toDeclareDPs"] = toDeclareDPsOut;
            TempData["notDeclaredDPs"] = notDeclaredDPsOut;
            TempData["toJustifyDPs"] = toJustifyDPsOut;
            TempData["notValidDPs"] = notValidDPsOut;

            return dataSource;
        }
    }
}