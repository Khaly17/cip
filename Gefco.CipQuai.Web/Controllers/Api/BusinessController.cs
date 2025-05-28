using System;
using System.Linq;
using System.Web.Http;
using Gefco.CipQuai.Web.Exceptions;
using Gefco.CipQuai.Web.Extensions;
using Gefco.CipQuai.Web.Results;

namespace Gefco.CipQuai.Web.Controllers.Api
{
    public class BusinessController : ApiController
    {
        [Route(nameof(GetResources))]
        public ResourceListServiceResult GetResources(string appVersion, string userId, DateTime lastUpdate)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                if (user == null)
                {
                    var exception = new InvalidUserException(userId);
                    SimpleLogger.GetOne().Error(exception);
                    return new ResourceListServiceResult(exception); ;
                }
                user.AppVersion = appVersion;
                dal.ObjectContext.SaveChanges();
                return new ResourceListServiceResult(dal.GetResources(user, lastUpdate).CloneList());
                
            }
        }

        [Route(nameof(GetRemorqueStatuses))]
        public RemorqueStatusListServiceResult GetRemorqueStatuses(string appVersion, string userId)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                if (user == null)
                {
                    var exception = new InvalidUserException(userId);
                    SimpleLogger.GetOne().Error(exception);
                    return new RemorqueStatusListServiceResult(exception);
                }
                user.AppVersion = appVersion;
                dal.ObjectContext.SaveChanges();
                return new RemorqueStatusListServiceResult(dal.GetRemorqueStatuses().CloneList());
                
            }
        }

        [Route(nameof(GetConfigurations))]
        public ConfigurationListServiceResult GetConfigurations(string appVersion, string userId)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                if (user == null)
                {
                    var exception = new InvalidUserException(userId);
                    SimpleLogger.GetOne().Error(exception);
                    return new ConfigurationListServiceResult(exception);
                }
                user.AppVersion = appVersion;
                dal.ObjectContext.SaveChanges();
                return new ConfigurationListServiceResult(dal.GetConfigurations().CloneList());
            }
        }

        [Route(nameof(GetAgenceTypes))]
        public AgenceTypeListServiceResult GetAgenceTypes(string appVersion, string userId)
        {
            using (var dal = new Dal())
            {
                var user = dal.FindUser(userId);
                if (user == null)
                {
                    var exception = new InvalidUserException(userId);
                    SimpleLogger.GetOne().Error(exception);
                    return new AgenceTypeListServiceResult(exception);
                }
                user.AppVersion = appVersion;
                dal.ObjectContext.SaveChanges();
                return new AgenceTypeListServiceResult(dal.GetAgenceTypes().ToList());
            }
        }
    }
}
