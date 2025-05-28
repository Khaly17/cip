using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace Gefco.CipQuai.Web.Controllers
{
    public class MyAuthorizeAttribute : AuthorizeAttribute
    {
        private string _rolesNational;
        private string[] _rolesSplitNational = new string[0];
        public string NationalRoles
        {
            get
            {
                return this._rolesNational ?? string.Empty;
            }
            set
            {
                this._rolesNational = value;
                this._rolesSplitNational = SplitString(value);
            }
        }

        private string _rolesRegion;
        private string[] _rolesSplitRegion = new string[0];
        public string RegionRoles
        {
            get
            {
                return this._rolesRegion ?? string.Empty;
            }
            set
            {
                this._rolesRegion = value;
                this._rolesSplitRegion = SplitString(value);
            }
        }

        private string _rolesAgence;
        private string[] _rolesSplitAgence = new string[0];
        public string AgenceRoles
        {
            get
            {
                return this._rolesAgence ?? string.Empty;
            }
            set
            {
                this._rolesAgence = value;
                this._rolesSplitAgence = SplitString(value);
            }
        }

        public InputType InputType { get; set; }
        public string UniqueModelId { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Authorize(httpContext);
        }

        public bool Authorize(HttpContextBase httpContext)
        {
            var usr = httpContext.User;
            if (!usr.Identity.IsAuthenticated)
                return false;

            using (var dal = new Dal())
            {
                var userId = usr.Identity.GetUserId();
                var user = dal.FindUser(userId);
                if (user == null)
                    return false;

                if (user.NationalRoles.Any(p => _rolesSplitNational.Contains(p.NationalRole.Value)))
                    return true;

                Guid agenceId = Guid.Empty;
                Guid regionId = Guid.Empty;
                switch (InputType)
                {
                    case InputType.Region:
                        Guid.TryParse(httpContext.Request.Path.Substring(httpContext.Request.Path.LastIndexOf('/') + 1), out regionId);
                        break;
                    case InputType.Agence:
                        Guid.TryParse(httpContext.Request.Path.Substring(httpContext.Request.Path.LastIndexOf('/') + 1), out agenceId);
                        break;
                    case InputType.DP:
                    {
                        var id = httpContext.Request.Path.Substring(httpContext.Request.Path.LastIndexOf('/') + 1);
                        var declaration = dal.FindDeclarationDoublePlancher(id);
                        Guid.TryParse(declaration.Traction.AgenceDepart.Region_Id, out regionId);
                        Guid.TryParse(declaration.Traction.AgenceDepart.Id, out agenceId);
                    }
                        break;
                    case InputType.NC:
                    {
                        var id = httpContext.Request.Path.Substring(httpContext.Request.Path.LastIndexOf('/') + 1);
                        var declaration = dal.FindDeclarationNonConformite(id);
                        Guid.TryParse(declaration.Agence.Region_Id, out regionId);
                        Guid.TryParse(declaration.Agence.Id, out agenceId);
                    }
                        break;
                    case InputType.DPParam:
                    {
                        var id = UniqueModelId;
                        var declaration = dal.FindDeclarationDoublePlancher(id);
                        Guid.TryParse(declaration.Traction.AgenceDepart.Region_Id, out regionId);
                        Guid.TryParse(declaration.Traction.AgenceDepart.Id, out agenceId);
                    }
                        break;
                    case InputType.NCParam:
                    {
                        var id = UniqueModelId;
                        var declaration = dal.FindDeclarationNonConformite(id);
                        Guid.TryParse(declaration.Agence.Region_Id, out regionId);
                        Guid.TryParse(declaration.Agence.Id, out agenceId);
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (user.RegionRoles.Any(p => _rolesSplitRegion.Contains(p.RegionRole.Value) && regionId.ToString().ToLower() == p.Region_Id.ToLower()))
                    return true;

                if (user.AgenceRoles.Any(p => _rolesSplitAgence.Contains(p.AgenceRole.Value) && agenceId.ToString().ToLower() == p.Agence_Id.ToLower()))
                    return true;

                var isInRole = base.AuthorizeCore(httpContext);
                if (isInRole)
                {
                    if (usr.IsInRole("National Users") || usr.IsInRole("Admin") || usr.IsInRole("Super Admin"))
                        return true;
                    if (usr.IsInRole("Regional Users"))
                    {
                        if (regionId.ToString().ToLower() == user.WebUserAgence.Region_Id.ToLower())
                            return true;
                        return false;
                    }
                    if (usr.IsInRole("Agence Users"))
                    {
                        if (agenceId.ToString().ToLower() == user.WebUserAgence_Id.ToLower())
                            return true;
                        return false;
                    }
                }

                return false;
            }
        }

        private static readonly char[] _splitParameter = new char[1]
        {
            ','
        }; internal static string[] SplitString(string original)
        {
            if (string.IsNullOrEmpty(original))
                return new string[0];
            return original.Split(_splitParameter).Select(piece => new
            {
                piece,
                trimmed = piece.Trim()
            }).Where(_param1 => !string.IsNullOrEmpty(_param1.trimmed)).Select(_param1 => _param1.trimmed).ToArray();
        }
    }

    public class AuthorizeBusinessRole
    {
        public InputType InputType { get; set; }
        public string BusinessRoleName { get; set; }
    }
    public enum InputType
    {
        Region,
        Agence,
        DP,
        NC,
        DPParam,
        NCParam,
    }
}