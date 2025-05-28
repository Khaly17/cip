using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gefco.CipQuai.Web.Models
{
    public class PageViewModel
    {
        public PageViewModel()
        {

        }
        public PageViewModel(Page page)
        {
            Icon = page.Icon;
            Link = page.Link;
            SectionId = page.Section != null ? Guid.Parse(page.Section.Id) : Guid.Empty;
            SortOrder = page.SortOrder;
            Name = page.Name;
            MenuTag = page.MenuTag;
            Id = page.Id;
            Roles = page.Roles;
        }

        public List<PageRole> Roles { get; set; } = new List<PageRole>();

        public string Icon { get; set; }

        public string Link { get; set; }
        public string MenuTag { get; set; }

        public Guid SectionId { get; set; }
        public int SortOrder { get; set; }

        public string Name { get; set; }

        public string Id { get; set; }

        public void UpdateModel(Page dbItem)
        {
            dbItem.Name = Name;
            dbItem.SortOrder = SortOrder;
            dbItem.Icon = Icon;
            dbItem.Link = Link;
            dbItem.MenuTag = MenuTag;
        }
    }
}