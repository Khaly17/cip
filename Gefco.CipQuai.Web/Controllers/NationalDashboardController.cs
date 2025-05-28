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
    [Authorize(Roles = "Super Admin,Admin,National Users")]
    public class NationalDashboardController : ZoneController
    {
        private readonly ApplicationDbContext _ctx;

        public NationalDashboardController() : base(Zone.National)
        {
            _ctx = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Index(FilterViewModel viewModel)
        {
            using (var dal = new Dal())
                dal.GetTractions(null);

            ViewBag.Regions = _ctx.Regions.CloneList();
            ViewBag.RegionUrl = Url.Action("Index", "RegionDashboard", new
            {
                startDate = viewModel.StartDate.ToString("dd/MM/yyyy"),
                endDate = viewModel.EndDate.ToString("dd/MM/yyyy"),
                filterType = viewModel.FilterType
            });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            return View(viewModel);
        }

        [ChildActionOnly]
        public ActionResult UtilisationDoublePlancher(FilterViewModel viewModel, string size, string title = "Utilisation double plancher", string subTitle = null, string centerLabel = "UTILISATION DP")
        {
            ViewBag.DashboardKey = viewModel.Id;

            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetDPData(startDate, endDate);

            var bpData = GetBPData(startDate, endDate);
            ViewBag.BPData = bpData;
            ViewBag.BPCenterValue = bpData.Value;
            ViewBag.BPDataUrl = Url.Action("GetBPModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            if (size == "Medium")
                ViewBag.BPDetailsUrl = Url.Action("ListeBP", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            else
                ViewBag.BPDetailsUrl = Url.Action("ListeBP", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });

            var SPData = GetSPData(startDate, endDate);
            ViewBag.SPData = SPData;
            ViewBag.SPCenterValue = SPData.Value;
            ViewBag.SPDataUrl = Url.Action("GetSPModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            if (size == "Medium")
                ViewBag.SPDetailsUrl = Url.Action("ListeSP", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            else
                ViewBag.SPDetailsUrl = Url.Action("ListeSP", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });

            var CRData = GetCRData(startDate, endDate);
            ViewBag.CRData = CRData;
            ViewBag.CRCenterValue = CRData.Value;
            ViewBag.CRDataUrl = Url.Action("GetCRModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            if (size == "Medium")
                ViewBag.CRDetailsUrl = Url.Action("ListeCR", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            else
                ViewBag.CRDetailsUrl = Url.Action("ListeCR", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });

            ViewBag.CenterValue = model.T2 + "%";
            ViewBag.CenterLabel = centerLabel;
            ViewBag.Size = size;
            ViewBag.PieTitle = title;
            ViewBag.SubTitle = subTitle;
            ViewBag.DeclarationType = "DP";
            ViewBag.EventType = "National";

            ViewBag.T1 = model.T1;
            ViewBag.T2 = model.T2;

            if (size == "Medium")
                ViewBag.DetailsUrl = Url.Action("GrilleDP", new { viewModel.FilterType, viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            else
                ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetDPModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("GaugeLayout", model.Data);
        }

        #region DP

        public ActionResult GetDPModel(DateTime startDate, DateTime endDate)
        {
            var model = GetDPData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatDPViewModel GetDPData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatDPViewModel();
            var data = HeatmapDataSource(new FilterViewModel()
            {
                StartDate = startDate,
                EndDate = endDate
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
            model.DpNonUtilisées = _ctx.DeclarationDoublePlanchers.Count(p => p.Traction != null && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && !p.Traction.IsDeleted && !p.Traction.IsCancelled && !p.IsDeleted && RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name) && !p.IsDPUsed);

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

        [Authorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult GrilleDP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Doubles planchers";
            ViewBag.PdfName = "National";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate;
            endDate = endDate.AddDays(1);
            var dataSource = HeatmapDataSource(viewModel, endDate, startDate);
            ViewBag.dataSource = dataSource;
            return View(viewModel);
        }

        [Authorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult GrilleDPA(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Doubles planchers";
            ViewBag.PdfName = "National";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate;
            endDate = endDate.AddDays(1);
            var definitions = _ctx.TractionDefinitions.OrderBy(p => p.AgenceDepart.Region.Name).ThenBy(p => p.AgenceDepart.Name).ToList();
            var tractionsHorsDefinition = _ctx.Tractions.Where(p => p.DueDate < endDate && p.DueDate >= startDate && p.TractionDefinition == null).ToList();
            definitions.AddRange(tractionsHorsDefinition.Select(p =>
            {
                p.TractionDefinition = new TractionDefinition()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "*" + p.AgenceDepart.Name + "-" + p.AgenceArrivee.Name
                };
                return p.TractionDefinition;
            }));
            var tractions = _ctx.Tractions.Where(p => p.DueDate < endDate && p.DueDate >= startDate).ToList();
            var dpDeclarées = _ctx.DeclarationDoublePlanchers.Where(p => p.Traction != null && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && !RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name) || p.IsDeleted).ToList();

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
        [MyAuthorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult ListeDP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Doubles planchers";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = _ctx.DeclarationDoublePlanchers.Where(p => p.Traction != null && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name));

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

        [MyAuthorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult ListeDPA(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Doubles planchers";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = _ctx.DeclarationDoublePlanchers.Where(p => p.Traction != null && p.Traction.DueDate < endDate && p.Traction.DueDate >= startDate && !RemorqueStatus.DashboardStatuses.Contains(p.CurrentStatus.Name) || p.IsDeleted);

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

        public ActionResult GetBPModel(DateTime startDate, DateTime endDate)
        {
            var model = GetBPData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private DataValue GetBPData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var bp = _ctx.DeclarationBonnePratiques.Count(p => p.CreationDate < endDate && p.CreationDate >= startDate);
            return new DataValue("Bonnes Pratiques", bp);
        }

        public ActionResult ListeBP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Bonnes pratiques";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = _ctx.DeclarationBonnePratiques.Where(p => p.CreationDate != null && p.AutreAgentConcerné != null && p.CreationDate < endDate && p.CreationDate >= startDate);

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

        public ActionResult GetSPModel(DateTime startDate, DateTime endDate)
        {
            var model = GetSPData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private DataValue GetSPData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var sp = _ctx.DeclarationSimplePlanchers.Count(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted);
            return new DataValue("Simples Planchers", sp);
        }

        [MyAuthorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult ListeSP(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Simples planchers";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = _ctx.DeclarationSimplePlanchers.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted);

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

        public ActionResult GetCRModel(DateTime startDate, DateTime endDate)
        {
            var model = GetCRData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private DataValue GetCRData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var sp = _ctx.DeclarationControleReceptions.Count(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted);
            return new DataValue("Contrôles arrivage", sp);
        }

        [MyAuthorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult ListeCR(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Contrôles arrivage";
            var startDate = viewModel.StartDate;
            var endDate = viewModel.EndDate.AddDays(1);
            var dpDeclarées = _ctx.DeclarationControleReceptions.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && p.Traction_Id == null && p.AutreAgenceArrivee != null && !p.IsDeleted);

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
            ViewBag.DashboardKey = viewModel.Id;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCRecuesData(startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "National";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCRecuesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }
        public ActionResult ListeNC(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Non conformités";
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var ncs = _ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée_Id != null && p.CreationDate < endDate && p.CreationDate >= startDate);
            if (!User.IsInRole("Super Admin") && !User.IsInRole("Admin"))
                ncs = ncs.Where(p => !p.IsDeleted);
            var model = new ListeNCViewModel(viewModel, ncs.ToList());
            var rUrl = Server.UrlEncode(Request.Url.PathAndQuery);
            foreach (var item in model.Items)
            {
                item.DetailsLink = Url.Action("Detail", "NonConformites", new { id = item.Id, rUrl });
            }

            return View(model);
        }

        public ActionResult GetNCRecuesModel(DateTime startDate, DateTime endDate)
        {
            var model = GetNCRecuesData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCRecuesData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var ncs = _ctx.DeclarationNonConformites.Where(p => !p.IsDeleted && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.MotifNCs.Any() && p.AgenceConcernée_Id != null).GroupBy(p => p.AgenceConcernée);
            model.Data = ncs.ToList().Select(p => new DataValue(p.Key.Name, p.SelectMany(q => q.MotifNCs).Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }

        [ChildActionOnly]
        public ActionResult NCTypoRecues(FilterViewModel viewModel)
        {
            ViewBag.DashboardKey = viewModel.Id;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCTypoRecuesData(startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "National";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCTypoRecuesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }
        public ActionResult ListeNCTypo(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;
            ViewBag.Title = "National - Typologies de non conformités";
            var model = GetNCTypoRecuesData(viewModel.StartDate, viewModel.EndDate);
            model.Id = viewModel.Id;
            model.StartDate = viewModel.StartDate;
            model.EndDate = viewModel.EndDate;
            model.FilterType = viewModel.FilterType;
            return View(model);
        }

        public ActionResult GetNCTypoRecuesModel(DateTime startDate, DateTime endDate)
        {
            var model = GetNCTypoRecuesData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCTypoRecuesData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var items = _ctx.DeclarationNonConformites.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.MotifNCs.Any()).ToList();
            var ncs = items.SelectMany(p => p.MotifNCs).Select(p => p.Name).ToList();
            model.Data = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key, p.Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }


        #endregion
        
        #region NC Emises

        [ChildActionOnly]
        public ActionResult NCEmises(FilterViewModel viewModel)
        {
            ViewBag.DashboardKey = viewModel.Id;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCEmisesData(startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "National";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCEmisesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }

        public ActionResult GetNCEmisesModel(DateTime startDate, DateTime endDate)
        {
            var model = GetNCEmisesData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCEmisesData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var ncs = _ctx.DeclarationNonConformites.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.MotifNCs.Any() && p.AgenceId != null).GroupBy(p => p.Agence);
            model.Data = ncs.ToList().Select(p => new DataValue(p.Key.Name, p.SelectMany(q => q.MotifNCs).Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }

        [ChildActionOnly]
        public ActionResult NCTypoEmises(FilterViewModel viewModel)
        {
            ViewBag.DashboardKey = viewModel.Id;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNCTypoEmisesData(startDate, endDate);
            model.Data = model.Data.ToList();
            ViewBag.DeclarationType = "NC";
            ViewBag.EventType = "National";
            ViewBag.DetailsUrl = Url.Action("Index", "RegionDashboard", new { viewModel.Id, startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy"), filterType = viewModel.FilterType });
            ViewBag.DataUrl = Url.Action("GetNCTypoEmisesModel", new { startDate = viewModel.StartDate.ToString("dd/MM/yyyy"), endDate = viewModel.EndDate.ToString("dd/MM/yyyy") });
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            return PartialView("ParetoLayout", model.Data);
        }

        public ActionResult GetNCTypoEmisesModel(DateTime startDate, DateTime endDate)
        {
            var model = GetNCTypoEmisesData(startDate, endDate);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        private StatNCViewModel GetNCTypoEmisesData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new StatNCViewModel();
            var items = _ctx.DeclarationNonConformites.Where(p => p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.MotifNCs.Any()).ToList();
            var ncs = items.SelectMany(p => p.MotifNCs).Select(p => p.Name).ToList();
            model.Data = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key, p.Count())).OrderByDescending(p => p.Value).Take(3).ToList();
            return model;
        }


        #endregion

    }

    public class HeatmapDataSource
    {
        public bool isJsonData { get; set; } = true;
        public string adaptorType { get; set; } = "Cell";
        public string xDataMapping { get; set; } = "columnid";
        public string yDataMapping { get; set; } = "rowid";
        public string valueMapping { get; set; } = "value";
        public List<HeatmapData> data { get; set; } = new List<HeatmapData>();
    }

    public class HeatmapData
    {
        public string rowid { get; set; }
        public string columnid { get; set; }
        public string id { get; set; }
        public int? value { get; set; }
    }
}