using System;

namespace Gefco.CipQuai.Web.Results
{
    public class BooleanServiceResult : BaseServiceResult
    {
        public bool Value { get; set; }

        public BooleanServiceResult()
        {
        }

        public override void SetError(Exception e)
        {
            base.SetError(e);
            Value = false;
        }
    }
}