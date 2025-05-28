using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Gefco.CipQuai.Web;
using Gefco.CipQuai.Web.Controllers.Api;
using Gefco.CipQuai.Web.Exceptions;
using Gefco.CipQuai.Web.Models;
using Gefco.CipQuai.Web.Results;
using Microsoft.Exchange.WebServices.Data;

namespace Gefco.CipQuai.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            //var controller = new AccountController();
            //using (var ctx = new Entities())
            //{
            //    var users = ctx.TempUsers.ToList();
            //    foreach (var user in users)
            //    {
            //        var pin = Tools.GenerateRandomNumber(4);
            //        controller.Register(user.FGU, pin, user.FirstName, user.LastName, null);
            //    }
            //}

                        //4da88b52-7c6b-48f6-b53e-ddf1e37fab3f
            //var dal = new Dal();
            //{
            //    var result = new PictureServiceResult();
            //    //if (declarationId != null)
            //    {
            //        var declaration = dal.FindDeclarationSimplePlancher("4da88b52-7c6b-48f6-b53e-ddf1e37fab3f");
            //        if (declaration == null)
            //        {
            //            var exception1 = new ArgumentOutOfRangeException("nameof(declarationId)");
            //            SimpleLogger.GetOne().Error(exception1);
            //            result.SetError(exception1);
            //        }
            //    }
            //}
            Instances.SendEmail("Coucou", "salut c'est moi", new EmailAddress("waxalica@hotmail.fr"));
        }
    }
}
