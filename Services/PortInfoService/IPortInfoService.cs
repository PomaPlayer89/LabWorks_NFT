using nat.Storage.Entity;
using System.Collections.Generic;

namespace nat.Services
{
    public interface IPortInfoService
    {
        public List<EndPoint> GetActiveTcpListeners();
        public List<PortInfo> GetActiveTcpConnections();
        public List<EndPoint> GetActiveUdpListeners();
    }
}
