using System;

namespace Gefco.CipQuai.Web.Models
{
    public class Device : BaseModel
    {
        public virtual string UserId { get; set; }
        public virtual DeviceType DeviceType { get; set; }
        public virtual string DeviceToken { get; set; }

        public override BaseModel Clone()
        {
            var obj = new Device
            {
                DeviceToken = DeviceToken,
                DeviceType = DeviceType,
                UserId = UserId,
                Id = Id,
                CreationDate = CreationDate
            };
            return obj;
        }
    }
    public enum DeviceType
    {
        Apple = 0,
        Android = 1,
        WindowsPhone = 2,
        Blackberry = 3
    }

}