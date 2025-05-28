using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Views;
using Microsoft.Ajax.Utilities;

namespace Gefco.CipQuai.Web.Controllers
{
    [Authorize]
    public class RapportsController : Controller
    {
        private readonly ApplicationDbContext _ctx;
        public RapportsController()
        {
            _ctx = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _ctx.Dispose();
            base.Dispose(disposing);
        }

        [Authorize(Roles = "Super Admin,Admin,National Users")]
        public ActionResult NCNational(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date;
            var model = GetNationalNonConformitesData(startDate, endDate);
            model.Id = viewModel.Id;
            model.StartDate = viewModel.StartDate;
            model.EndDate = viewModel.EndDate;
            model.FilterType = viewModel.FilterType;
            return View(model);
        }

        private NCNationalStatViewModel GetNationalNonConformitesData(DateTime startDate, DateTime endDate)
        {
            endDate = endDate.AddDays(1);
            var model = new NCNationalStatViewModel();
            var france = _ctx.DeclarationNonConformites.Where(p => p.AgenceId != null && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.AgenceConcernée.AgenceType.Key == 1).ToList();
            var others = _ctx.DeclarationNonConformites.Where(p => p.AgenceId != null && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.AgenceConcernée.AgenceType.Key != 1).ToList();
            var regioned = france.Where(p => p.AgenceConcernée.Region != null).GroupBy(p => p.AgenceConcernée.Region);
            foreach (var items in regioned)
            {
                var ncs = items.SelectMany(p => p.MotifNCs).ToList();
                var values = ncs.OrderBy(p => p.DisplayOrder).GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();
                model.StackedRegionCategoriesData.Add(new CategoryDataValue(items.Key.Name, values));
            }
            var ncsOthers = others.SelectMany(p => p.MotifNCs).ToList();
            var valuesOthers = ncsOthers.GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();
            model.StackedRegionCategoriesData.Add(new CategoryDataValue("Réseau Etranger", valuesOthers));
            model.StackedRegionCategoriesData.Sort((data1, data2) => data1.Values.Sum(q => q.Value) > data2.Values.Sum(q => q.Value) ? -1 : 1);
            model.Categories = model.StackedRegionCategoriesData.SelectMany(p => p.Values).Select(p => p.Name).Distinct().ToList();
            var all = france.Union(others).ToList();
            List<Tuple<int, string>> cats = new List<Tuple<int, string>>();

            //Stack Agences
            var agenced = france.GroupBy(p => p.AgenceConcernée);
            foreach (var items in agenced)
            {
                var ncs = items.SelectMany(p => p.MotifNCs).ToList();
                var values = ncs.OrderBy(p => p.DisplayOrder).GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();
                model.StackedAgenceCategoriesData.Add(new CategoryDataValue(items.Key.Name, values));
            }
            model.StackedAgenceCategoriesData.Sort((data1, data2) => data1.Values.Sum(q => q.Value) > data2.Values.Sum(q => q.Value) ? -1 : 1);

            //Week
            var days = (endDate.Date - startDate.Date).TotalDays + 1;
            var months = (endDate.Date.AddDays(-1) - startDate.Date).TotalMonths();
            List<DataValue> weekData = new List<DataValue>();
            if (months >= 3)
            {
                for (int i = 0; i < days - 1; i++)
                {
                    var day = startDate.AddDays(i);
                    var month = day.Date.Month;
                    var cnt = all.Where(p => p.CreationDate.Date == day.Date).SelectMany(p => p.MotifNCs).Count();
                    var monthName = new DateTime(DateTime.Now.Year, month, 1).ToString("MMMM");
                    cats.Add(new Tuple<int, string>(cnt, monthName));
                    weekData.Add(new DataValue(monthName, cnt));
                }
            }
            else
                for (int i = 0; i < days; i++)
                {
                    var day = startDate.AddDays(i);
                    var week = Tools.GetIso8601WeekOfYear(day.Date);
                    var cnt = all.Where(p => p.CreationDate.Date == day.Date).SelectMany(p => p.MotifNCs).Count();
                    cats.Add(new Tuple<int, string>(cnt, "S" + week));
                    weekData.Add(new DataValue("S" + week, cnt));
                }
            model.WeekData = weekData.GroupBy(p => p.Name).Select(p => new DataValue(p.Key, p.Sum(q => q.Value))).ToList();
            
            //Contributeurs
            model.ContributorsData = all.GroupBy(p => p.Agence).Select(p => new DataValue(p.Key.Name, p.SelectMany(q => q.MotifNCs).Count())).ToList();
            model.ContributorsData.Sort((data1, data2) => data1.Value > data2.Value ? -1 : 1);

            //Catégories
            var ncsCat = all.SelectMany(p => p.MotifNCs).ToList();
            model.PieData = ncsCat.OrderBy(p => p.DisplayOrder).GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();

            return model;
        }

        [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users")]
        public ActionResult NCRegional(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var region = _ctx.Regions.Single(p => p.Id == viewModel.Id);
            ViewBag.Region = region;
            ViewBag.Title = region.Name + " - Non conformités";
            
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var model = GetRegionalNonConformitesData(viewModel.Id, startDate, endDate);
            model.Id = viewModel.Id;
            model.StartDate = viewModel.StartDate;
            model.EndDate = viewModel.EndDate;
            model.FilterType = viewModel.FilterType;
            return View(model);
        }

        private NCRegionalStatViewModel GetRegionalNonConformitesData(string id, DateTime startDate, DateTime endDate)
        {
            var model = new NCRegionalStatViewModel();
            var france = _ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée.Region_Id == id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.AgenceConcernée.AgenceType.Key == 1).ToList();

            var topAgence = france.GroupBy(p => p.AgenceConcernée).OrderByDescending(p => p.Sum(x => x.MotifNCs.Count)).FirstOrDefault();

            //Week
            Dictionary<int, List<DeclarationNonConformite>> weeked = new Dictionary<int, List<DeclarationNonConformite>>();
            Dictionary<int, List<DeclarationNonConformite>> weekedTopAgence = new Dictionary<int, List<DeclarationNonConformite>>();
            var days = (endDate.Date - startDate.Date).TotalDays + 1;
            for (int i = 0; i < days; i++)
            {
                var day = startDate.AddDays(i);
                var week = Tools.GetIso8601WeekOfYear(day.Date);
                var ncs = france.Where(p => p.CreationDate.Date == day.Date).ToList();
                if (weeked.ContainsKey(week))
                    weeked[week].AddRange(ncs);
                else
                    weeked.Add(week, ncs);
                if (topAgence != null)
                {
                    ViewBag.TopAgence = topAgence.Key.Name;
                    var topAgenceNcs = france.Where(p => p.AgenceConcernée_Id == topAgence.Key.Id && p.CreationDate.Date == day.Date).ToList();
                    if (weekedTopAgence.ContainsKey(week))
                        weekedTopAgence[week].AddRange(topAgenceNcs);
                    else
                        weekedTopAgence.Add(week, topAgenceNcs);
                }
                else
                {
                    if (!weekedTopAgence.ContainsKey(week))
                        weekedTopAgence.Add(week, new List<DeclarationNonConformite>());
                }
            }

            foreach (var items in weeked)
            {
                var ncs = items.Value.SelectMany(p => p.MotifNCs).ToList();
                var values = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();
                model.StackedWeekCategoriesData.Add(new CategoryDataValue("S" + items.Key.ToString(), values));
            }
            //model.StackedWeekCategoriesData.Sort((d1, d2) => d1.Values.Count < d2.Values.Count ? 1 : -1);
            model.Categories = model.StackedWeekCategoriesData.SelectMany(p => p.Values).Select(p => p.Name).Distinct().ToList();

            foreach (var items in weekedTopAgence)
            {
                var ncs = items.Value.SelectMany(p => p.MotifNCs).ToList();
                var values = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();
                model.StackedTopAgenceCategoriesData.Add(new CategoryDataValue("S" + items.Key.ToString(), values));
            }

            //Stack Agences
            var agenced = france.GroupBy(p => p.AgenceConcernée);
            foreach (var items in agenced)
            {
                var ncs = items.SelectMany(p => p.MotifNCs).ToList();
                var values = ncs.GroupBy(p => p).Select(p => new DataValue(p.Key.Name, p.Count(), p.Key.Color)).ToList();
                model.StackedAgenceCategoriesData.Add(new CategoryDataValue(items.Key.Name, values));
            }
            model.StackedAgenceCategoriesData.Sort((data1, data2) => data1.Values.Sum(x => x.Value) > data2.Values.Sum(x => x.Value) ? -1 : 1);

            return model;
        }

        [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users")]
        public ActionResult NCRegionalEmises(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var region = _ctx.Regions.Single(p => p.Id == viewModel.Id);
            ViewBag.Region = region;
            ViewBag.Title = region.Name + " - Non conformités émises";
            ViewBag.Agences = _ctx.Agences.Where(p => p.Region_Id == viewModel.Id).OrderBy(p => p.Name).ToList().Select(p => new ListItem(p.Id, p.Name));
            ViewBag.Months = ListItem.Months;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var model = new NCStatListViewModel(viewModel);
            model.Data = _ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée_Id != null && p.Agence.Region_Id == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted).ToList();
            return View(model);
        }
        [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users")]
        public ActionResult NCRegionalTypoEmises(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var region = _ctx.Regions.Single(p => p.Id == viewModel.Id);
            ViewBag.Region = region;
            ViewBag.Title = region.Name + " - Non conformités émises";
            ViewBag.Agences = _ctx.Agences.Where(p => p.Region_Id == viewModel.Id).OrderBy(p => p.Name).ToList().Select(p => new ListItem(p.Id, p.Name));
            ViewBag.Months = ListItem.Months;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var model = new NCStatListViewModel(viewModel);
            model.Data = _ctx.DeclarationNonConformites.Where(p => p.Agence.Region_Id == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted).ToList();
            return View(model);
        }
        [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users")]
        public ActionResult NCRegionalRecues(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var region = _ctx.Regions.Single(p => p.Id == viewModel.Id);
            ViewBag.Region = region;
            ViewBag.Title = region.Name + " - Non conformités reçues";
            ViewBag.Agences = _ctx.Agences.Where(p => p.Region_Id == viewModel.Id).OrderBy(p => p.Name).ToList().Select(p => new ListItem(p.Id, p.Name));
            ViewBag.Months = ListItem.Months;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var model = new NCStatListViewModel(viewModel);
            model.Data = _ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée.Region_Id == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted).ToList();
            return View(model);
        }
        [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users")]
        public ActionResult NCRegionalTypoRecues(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var region = _ctx.Regions.Single(p => p.Id == viewModel.Id);
            ViewBag.Region = region;
            ViewBag.Title = region.Name + " - Non conformités reçues";
            ViewBag.Agences = _ctx.Agences.Where(p => p.Region_Id == viewModel.Id).OrderBy(p => p.Name).ToList().Select(p => new ListItem(p.Id, p.Name));
            ViewBag.Months = ListItem.Months;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var model = new NCStatListViewModel(viewModel);
            model.Data = _ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée.Region_Id == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted).ToList();
            return View(model);
        }
        [Authorize(Roles = "Super Admin,Admin,National Users,Regional Users")]
        public ActionResult NCRegionalEvolution(FilterViewModel viewModel)
        {
            ViewBag.StartDate = viewModel.StartDate;
            ViewBag.EndDate = viewModel.EndDate;
            ViewBag.FilterType = viewModel.FilterType;

            var region = _ctx.Regions.Single(p => p.Id == viewModel.Id);
            ViewBag.Region = region;
            ViewBag.Title = region.Name + " - Non conformités";
            ViewBag.Agences = _ctx.Agences.Where(p => p.Region_Id == viewModel.Id).OrderBy(p => p.Name).ToList().Select(p => new ListItem(p.Id, p.Name));
            ViewBag.Months = ListItem.Months;
            var startDate = viewModel.StartDate.Date;
            var endDate = viewModel.EndDate.Date.AddDays(1);
            var model = new NCStatListViewModel(viewModel);
            model.Data = _ctx.DeclarationNonConformites.Where(p => p.AgenceConcernée.Region_Id == viewModel.Id && p.CreationDate < endDate && p.CreationDate >= startDate && !p.IsDeleted && p.AgenceConcernée.AgenceType.Key == 1).ToList();
            return View(model);
        }
    }
}