using System.Collections.Generic;
using System.Text;

namespace Gefco.CipQuai.Services
{
    /// <summary>
    /// Interface of the network service. Tells whether the device is connected.
    /// </summary>
    public interface IServiceNetwork
    {
        /// <summary>
        /// Determines whether the device is connected.
        /// </summary>
        /// <returns><c>true</c> if the device is connected; otherwise, <c>false</c>.</returns>
        bool IsConnected();

        // StatusConnexions GetCurrentConnexion();
    }
}
