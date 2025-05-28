using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Views;
using Microsoft.AspNet.Identity;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users")]
    public class AgenceDashboardController : ZoneController
    {
        public AgenceDashboardController() : base(Zone.Agence)
        {
        }

        [HttpGet]
        public ActionResult Index(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            using (var dal = new Dal())
                dal.GetTractions(null);

            var user = Ctx.Users.Find(User.Identity.GetUserId());
            ViewBag.UserName = user.FirstName + " " + user.LastName;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Agence = agence;
            return View(viewModel);
        }


        [ChildActionOnly]
        public ActionResult UtilisationDoublePlancher(FilterViewModel viewModel, string size, string title = null, string subTitle = "Utilisation double plancher", string centerLabel = "UTILISATION DP")
        {
            var viewModelStartDate = viewModel.StartDate;
            var viewModelEndDate = viewModel.EndDate;

            var startDate = viewModelStartDate.Date;
            var endDate = viewModelEndDate.Date;
            var model = GetDPData(viewModel.Id, startDate, endDate);

            var bpData = GetBPData(viewModel.Id, startDate, endDate);
            ViewBag.BPData = bpData;
            ViewBag.BPCenterValue = bpData.Value;
            ViewBag.BPDataUrl = Url.Action("GetBPModel", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.BPDetailsUrl = Url.Action("ListeBP", "AgenceDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });

            var SPData = GetSPData(viewModel.Id, startDate, endDate);
            ViewBag.SPData = SPData;
            ViewBag.SPCenterValue = SPData.Value;
            ViewBag.SPDataUrl = Url.Action("GetSPModel", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.SPDetailsUrl = Url.Action("ListeSP", "AgenceDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });

            var CRData = GetCRData(viewModel.Id, startDate, endDate);
            ViewBag.CRData = CRData;
            ViewBag.CRCenterValue = CRData.Value;
            ViewBag.CRDataUrl = Url.Action("GetCRModel", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.CRDetailsUrl = Url.Action("ListeCR", "AgenceDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });

            ViewBag.CenterValue = model.T2 + "%";
            ViewBag.CenterLabel = centerLabel;
            var agence = Ctx.Agences.Find((string)viewModel.Id);
            ViewBag.Size = !size.IsNullOrWhiteSpace() ? size : "Medium";
            ViewBag.PieTitle = !title.IsNullOrWhiteSpace() ? title : agence?.Name;
            ViewBag.SubTitle = subTitle;
            ViewBag.DeclarationType = "DP";
            ViewBag.EventType = "Agence";
            ViewBag.EventId = viewModel.Id;

            ViewBag.T1 = model.T1;
            ViewBag.T2 = model.T2;

            ViewBag.DetailsUrl = Url.Action("Index", "AgenceDashboard", new { viewModel.Id, startDate = viewModelStartDate.ToString("dd/MM/yyyy"), endDate = viewModelEndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetDPModel", new { viewModel.Id, startDate = viewModelStartDate.ToString("dd/MM/yyyy"), endDate = viewModelEndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModelStartDate;
            ViewBag.EndDate = viewModelEndDate;
            return PartialView("GaugeLayout", model.Data);
        }

        #region DP

        public ActionResult GetDPModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetDPData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatDPViewModel GetDPData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatDPViewModel();
            var data = HeatmapDataSource(new FilterViewModel()
            {
                StartDate = startDate,
                EndDate = endDate,
                Id = id
            }, endDate, startDate);

            //Calcul T1 et T2
            int usedDPs = 0, justifiedDPs = 0, toDeclareDPs = 0, notDeclaredDPs = 0, toJustifyDPs = 0, notValidDPs = 0;
            usedDPs = (int)TempData["usedDPsOut"];
            justifiedDPs = (int)TempData["justifiedDPsOut"];
            toDeclareDPs = (int)TempData["toDeclareDPs"];
            notDeclaredDPs = (int)TempData["notDeclaredDPs"];
            toJustifyDPs = (int)TempData["toJustifyDPs"];
            notValidDPs = (int)TempData["notValidDPs"];

            model.DpAttendues = toDeclareDPs;
            model.DpUtilisées = usedDPs;
            model.DpNonUtilisées = Ctx.DeclarationDoublePlanchers.Count(p => p.Traction.AgenceDepart.Id == id && p.Traction != null && !p.Traction.IsDeleted && !p.Traction.IsCancelled && !p.IsDeleted && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name) && !p.IsDPUsed);

            var i1 = usedDPs + justifiedDPs + notDeclaredDPs + notValidDPs + toJustifyDPs;
            if (i1 > 0)
            {
                model.T1 = (int)Math.Ceiling((usedDPs + justifiedDPs) * 100.0 / i1);
            }
            else
            {
                model.T1 = 0;
            }
            var i2 = toDeclareDPs;
            if (i2 > 0)
            {
                model.T2 = (int)Math.Ceiling(usedDPs * 100.0 / i2);
            }
            else
            {
                model.T2 = 0;
            }

            model.Data = new List<DataValue>()
            {
                new DataValue("A déclarer", model.ADéclarer),
                new DataValue("Utilisées", model.DpUtilisées),
                new DataValue("Non utilisées", model.DpNonUtilisées),
            };
            return model;
        }

        [MyAuthorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users", AgenceRoles = "AQHSE,Alternant QHSE,RQHSE,RQCO", InputType = InputType.Agence)]
        public ActionResult GrilleDP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate;
            endDate = endDate.AddDays(1);

            var dataSource = HeatmapDataSource(viewModel, endDate, startDate);
            ViewBag.dataSource = dataSource;
            return View(viewModel);
        }

        [MyAuthorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users", AgenceRoles = "AQHSE,Alternant QHSE,RQHSE,RQCO", InputType = InputType.Agence)]
        public ActionResult GrilleDPA(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate;
            endDate = endDate.AddDays(1);

            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.PdfName = "Agence - " + agence.Name;
            ViewBag.Title = agence.Name + " - Doubles planchers";
            var definitions = Ctx.TractionDefinitions.Where(p => p.AgenceDepart.Id == viewModel.Id).OrderBy(p => p.AgenceDepart.Name).ThenBy(p => p.AgenceDepart.Name).ToList();
            var tractionsHorsDefinition = Ctx.Tractions.Where(p => p.DueDate < endDate && p.DueDate >= startDate && p.TractionDefinition == null && p.AgenceDepart.Id == viewModel.Id).ToList();
            definitions.AddRange(tractionsHorsDefinition.Select(p =>
            {
                p.TractionDefinition = new TractionDefinition()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "*" + p.AgenceDepart.Name + "-" + p.AgenceArrivee.Name
                };
                return p.TractionDefinition;
            }));
            var tractions = Ctx.Tractions.Where(p => p.AgenceDepart.Id == viewModel.Id && p.DueDate < endDate && p.DueDate >= startDate).ToList();
            var dpDeclarées = Ctx.DeclarationDoublePlanchers.Where(p => p.Traction != null && p.Traction.AgenceDepart.Id == viewModel.Id && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && !RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name) || p.IsDeleted).ToList();

            int?[,] dataSource = new int?[(int)(endDate - startDate).TotalDays, definitions.Count];
            List<List<string>> ids = new List<List<string>>();
            List<List<string>> names = new List<List<string>>();
            List<List<string>> dests = new List<List<string>>();
            for (var i = 0; startDate.AddDays(i) < endDate; i++)
            {
                var idss = new List<string>();
                var namess = new List<string>();
                var destss = new List<string>();
                for (int j = 0; j < definitions.Count; j++)
                {
                    var definition = definitions.ElementAt(j);
                    var date = startDate.AddDays(i).Date;
                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    {
                        dataSource[i, j] = null;
                        idss.Add(null);
                        namess.Add(null);
                        destss.Add(null);
                        continue;
                    }
                    var traction = tractions.SingleOrDefault(p => p.TractionDefinition?.Id == definition.Id && p.DueDate.Date == date);
                    DeclarationDoublePlancher dp = null;
                    if (traction != null)
                        dp = dpDeclarées.SingleOrDefault(p => p.Traction_Id == traction.Id && p.Traction.DueDate.Date == traction.DueDate.Date);
                    if (dp == null)
                    {
                        dataSource[i, j] = -1;
                        idss.Add("");
                        namess.Add("");
                        destss.Add("");
                    }
                    else
                    {
                        idss.Add(dp.Id);
                        namess.Add(dp.CreatedBy.FullName + $" ({dp.CurrentWorkflowStep})");
                        destss.Add(dp.Traction.AgenceDepart.Name.ToUpper() + $" - {(dp.Traction.AgenceArrivee?.Name ?? dp.AutreAgenceArrivee).ToUpper()}");
                        if ((dp.CurrentStatus?.Name ?? "InProgress") == "InProgress")
                        {
                            dataSource[i, j] = -2;
                        }
                        else if (dp.CurrentStatus.Name == "PausedAndLocked")
                        {
                            dataSource[i, j] = -5;
                        }
                        else if (dp.IsDeleted)
                        {
                            dataSource[i, j] = -3;
                        }
                        else
                        {
                            dataSource[i, j] = -4;
                        }
                    }
                }

                ids.Add(idss);
                names.Add(namess);
                dests.Add(destss);
            }

            List<string> yLabels = new List<string>();
            yLabels.AddRange(definitions.Select(p => p.Name));
            ViewBag.yLabels = yLabels.ToArray();
            ViewBag.border = new { color = "white" };
            ViewBag.dataSource = dataSource;
            ViewBag.height = ((definitions.Count) * 30) + 100 + "px";
            ViewBag.ids = ids;
            ViewBag.names = names;
            ViewBag.dests = dests;
            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            ViewBag.ListeUrl = Url.Action("ListeDPA", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.GrilleDPAUrl = Url.Action("GrilleDP", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            return View(viewModel);
        }

        [MyAuthorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users", RegionRoles = "AQHSE,Alternant QHSE,RQHSE,RQCO", InputType = InputType.Agence)]
        public ActionResult ListeDP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Doubles planchers";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = Ctx.DeclarationDoublePlanchers.Where(p => p.Traction != null && p.Traction.AgenceDepart.Id == viewModel.Id && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name)).ToList();

            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            ViewBag.GrilleUrl = Url.Action("GrilleDP", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });

            var model = new ListeDPViewModel(viewModel, dpDeclarées.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "DoublesPlanchers", new { id = item.Id, rUrl = Server.UrlDecode(rUrl) });
            }

            return View(model);
        }

        [MyAuthorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users", RegionRoles = "AQHSE,Alternant QHSE,RQHSE,RQCO", InputType = InputType.Agence)]
        public ActionResult ListeDPA(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Doubles planchers";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = Ctx.DeclarationDoublePlanchers.Where(p => p.Traction != null && p.Traction.AgenceDepart.Id == viewModel.Id && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && !RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name)).ToList();

            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            ViewBag.GrilleUrl = Url.Action("GrilleDPA", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });

            var model = new ListeDPViewModel(viewModel, dpDeclarées.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "DoublesPlanchers", new { id = item.Id, rUrl = Server.UrlDecode(rUrl) });
            }

            return View("ListeDP", model);
        }
        #endregion

        #region BP

        public ActionResult GetBPModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetBPData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private DataValue GetBPData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var bp = Ctx.DeclarationBonnePratiques.Count(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId == id);
            return new DataValue("Bonnes Pratiques", bp);
        }

        public ActionResult ListeBP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Bonnes pratiques";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = Ctx.DeclarationBonnePratiques.Where(p => p.CreationDate != null && p.AutreAgentConcerné != null && p.AgenceId == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate).ToList();

            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);

            var model = new ListeBPViewModel(viewModel, dpDeclarées.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "BonnesPratiques", new { id = item.Id, rUrl });
            }

            return View(model);
        }

        #endregion

        #region SP

        public ActionResult GetSPModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetSPData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private DataValue GetSPData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var bp = Ctx.DeclarationSimplePlanchers.Count(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId == id && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted);
            return new DataValue("Simples Planchers", bp);
        }
        [MyAuthorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users", RegionRoles = "AQHSE,Alternant QHSE,RQHSE,RQCO", InputType = InputType.Agence)]
        public ActionResult ListeSP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Simples planchers";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = Ctx.DeclarationSimplePlanchers.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId == viewModel.Id && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted).ToList();

            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);

            var model = new ListeSPViewModel(viewModel, dpDeclarées.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "SimplesPlanchers", new { id = item.Id, rUrl = Server.UrlDecode(rUrl) });
            }

            return View(model);
        }

        #endregion
        #region CR

        public ActionResult GetCRModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetCRData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private DataValue GetCRData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var bp = Ctx.DeclarationControleReceptions.Count(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId == id && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted);
            return new DataValue("Contrôles arrivage", bp);
        }
        [MyAuthorize(Roles = "Super Admin,Admin,National Users,Regional Users,Agence Users,Web Users", RegionRoles = "AQHSE,Alternant QHSE,RQHSE,RQCO", InputType = InputType.Agence)]
        public ActionResult ListeCR(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Contrôles arrivage";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = Ctx.DeclarationControleReceptions.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId == viewModel.Id && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted).ToList();

            ViewBag.rUrl = Server.UrlEncode(Request.Url.PathAndQuery);

            var model = new ListeCRViewModel(viewModel, dpDeclarées.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "ControlesReception", new { id = item.Id, rUrl = Server.UrlDecode(rUrl) });
            }

            return View(model);
        }

        #endregion

        #region NC Reçues

        [ChildActionOnly]
        public ActionResult NCRecues(FilterViewModel viewModel)
        {
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCRecuesData(viewModel.Id, startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "Agence";
            ViewBag.DataUrl = Url.Action("GetNCRecuesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }
        public ActionResult ListeNCRecues(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Non conformités reçues";
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var ncs = Ctx.DeclarationNonConformites.Where(p => !p.IsDeleted && p.AgenceConcernée_Id == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId != null);
            var model = new ListeNCViewModel(viewModel, ncs.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "NonConformites", new { id = item.Id, rUrl });
            }

            return View("ListeNC", model);
        }

        public ActionResult GetNCRecuesModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetNCRecuesData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCRecuesData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var ncs = Ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée_Id == id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.AgenceId != null && p.MotifNCs.Any()).GroupBy(p => p.Agence);
            model.Data = ncs.ToList().Select(p => new DataValue(p.Key.Name, p.Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }

        [ChildActionOnly]
        public ActionResult NCTypoRecues(FilterViewModel viewModel)
        {
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCTypoRecuesData(viewModel.Id, startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "Agence";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCTypoRecuesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }

        public ActionResult ListeNCTypoRecues(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Typologies de non conformités reçues";
            var model = GetNCTypoRecuesData(viewModel.Id, viewModel.StartDate, viewModel.EndDate);
            model.Id = viewModel.Id;
            model.StartDate = viewModel.StartDate;
            model.EndDate = viewModel.EndDate;
            model.FilterType = viewModel.FilterType;
            return View("ListeNCTypo", model);
        }

        public ActionResult GetNCTypoRecuesModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetNCTypoRecuesData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCTypoRecuesData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var items = Ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée_Id == id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.MotifNCs.Any()).ToList();
            var ncs = items.SelectMany(p => p.MotifNCs).Select(p => p.Name).ToList();
            model.Data = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key, p.Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }


        #endregion

        #region NC Emises

        [ChildActionOnly]
        public ActionResult NCEmises(FilterViewModel viewModel)
        {
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCEmisesData(viewModel.Id, startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "Agence";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCEmisesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }
        public ActionResult ListeNCEmises(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Non conformités émises";
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var ncs = Ctx.DeclarationNonConformites.Where(p => !p.IsDeleted && p.CreationDate < endDate && p.CreationDate >= startDate && p.AgenceId == viewModel.Id);
            var model = new ListeNCViewModel(viewModel, ncs.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "NonConformites", new { id = item.Id, rUrl });
            }

            return View("ListeNC", model);
        }

        public ActionResult GetNCEmisesModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetNCEmisesData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCEmisesData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var ncs = Ctx.DeclarationNonConformites.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.AgenceId == id && p.MotifNCs.Any() && p.AgenceConcernée_Id != null).GroupBy(p => p.AgenceConcernée);
            model.Data = ncs.ToList().Select(p => new DataValue(p.Key.Name, p.Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }

        [ChildActionOnly]
        public ActionResult NCTypoEmises(FilterViewModel viewModel)
        {
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCTypoEmisesData(viewModel.Id, startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "Agence";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCTypoEmisesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }

        public ActionResult ListeNCTypoEmises(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var agence = Ctx.Agences.Single(p => p.Id == viewModel.Id);
            ViewBag.Title = agence.Name + " - Typologies de non conformités émises";
            var model = GetNCTypoEmisesData(viewModel.Id, viewModel.StartDate, viewModel.EndDate);
            model.Id = viewModel.Id;
            model.StartDate = viewModel.StartDate;
            model.EndDate = viewModel.EndDate;
            model.FilterType = viewModel.FilterType;
            return View("ListeNCTypo", model);
        }

        public ActionResult GetNCTypoEmisesModel(string id, DateTime startDate, DateTime endDate)
        {
            var model = GetNCTypoEmisesData(id, startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCTypoEmisesData(string id, DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var items = Ctx.DeclarationNonConformites.Where(p => p.AgenceId == id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.MotifNCs.Any()).ToList();
            var ncs = items.SelectMany(p => p.MotifNCs).Select(p => p.Name).ToList();
            model.Data = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key, p.Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }


        #endregion

    }
}