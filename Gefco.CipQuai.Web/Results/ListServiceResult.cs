using System;
using System.Collections.Generic;
using Gefco.CipQuai.Web.Models;

namespace Gefco.CipQuai.Web.Results
{
    /// <summary>
    ///     A base class to hold the return code from a Web service call, and associated information as List<T>.
    /// </summary>
    public abstract class ListServiceResult<T> : BaseServiceResult
    {
        private List<T> _values;

        /// <summary>
        ///     Gets or sets the data object returned by the Web service.
        /// </summary>
        /// <value>
        ///     The data object returned by the Web service.
        /// </value>
        public List<T> Values
        {
            get => _values;
            set
            {
                _values = value;
                IsSuccess = true;
            }
        }

        protected ListServiceResult(Exception ex) : base(ex)
        {
            SetError(ex);
        }
        protected ListServiceResult(IEnumerable<T> values)
        {
            Values = new List<T>(values);
        }
    }
    
    public class TractionListServiceResult : ListServiceResult<Traction>
    {
        public TractionListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public TractionListServiceResult(IEnumerable<Traction> items) : base(items)
        {
            
        }
    }
    public class ConfigurationListServiceResult : ListServiceResult<Models.Configuration>
    {
        public ConfigurationListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public ConfigurationListServiceResult(IEnumerable<Models.Configuration> items) : base(items)
        {
            
        }
    }
    public class AgenceTypeListServiceResult : ListServiceResult<AgenceType>
    {
        public AgenceTypeListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public AgenceTypeListServiceResult(IEnumerable<AgenceType> items) : base(items)
        {
            
        }
    }
    public class MotifDPListServiceResult : ListServiceResult<MotifDP>
    {
        public MotifDPListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public MotifDPListServiceResult(IEnumerable<MotifDP> items) : base(items)
        {
            
        }
    }
    public class MotifNCListServiceResult : ListServiceResult<MotifNC>
    {
        public MotifNCListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public MotifNCListServiceResult(IEnumerable<MotifNC> items) : base(items)
        {
            
        }
    }
    public class ResourceListServiceResult : ListServiceResult<Resource>
    {
        public ResourceListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public ResourceListServiceResult(IEnumerable<Resource> items) : base(items)
        {
            
        }
    }
    public class AgenceListServiceResult : ListServiceResult<Agence>
    {
        public AgenceListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public AgenceListServiceResult(IEnumerable<Agence> items) : base(items)
        {
            
        }
    }
    public class RemorqueStatusListServiceResult : ListServiceResult<RemorqueStatus>
    {
        public RemorqueStatusListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public RemorqueStatusListServiceResult(IEnumerable<RemorqueStatus> items) : base(items)
        {
            
        }
    }
    public class DeclarationDoublePlancherListServiceResult : ListServiceResult<DeclarationDoublePlancher>
    {
        public DeclarationDoublePlancherListServiceResult(IEnumerable<DeclarationDoublePlancher> items) : base(items)
        {
            
        }
        public DeclarationDoublePlancherListServiceResult(Exception ex) : base(ex)
        {
            
        }
    }
    public class DeclarationBonnePratiqueListServiceResult : ListServiceResult<DeclarationBonnePratique>
    {
        public DeclarationBonnePratiqueListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public DeclarationBonnePratiqueListServiceResult(IEnumerable<DeclarationBonnePratique> items) : base(items)
        {
            
        }
    }
    public class ApplicationUserListServiceResult : ListServiceResult<ApplicationUser>
    {
        public ApplicationUserListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public ApplicationUserListServiceResult(IEnumerable<ApplicationUser> items) : base(items)
        {
            
        }
    }
    public class DeclarationNonConformiteListServiceResult : ListServiceResult<DeclarationNonConformite>
    {
        public DeclarationNonConformiteListServiceResult(Exception ex) : base(ex)
        {
            
        }
        public DeclarationNonConformiteListServiceResult(IEnumerable<DeclarationNonConformite> items) : base(items)
        {
            
        }
    }
}