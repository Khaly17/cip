using System;
using System.Collections.Generic;
using System.Text;

namespace Gefco.CipQuai.Services
{
    /// <summary>
    ///     Interface of a cultur service. Allows to set the language, resource file.
    /// </summary>
    public interface IServiceDeviceInfo
    {
        void GetDeviceToken(Action<string> callback);
    }
}
