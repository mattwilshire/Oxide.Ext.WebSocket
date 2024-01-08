using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.Ext.WebSocket.Libraries
{
    internal interface IWebSocket
    {
        void Initialize();
        void Shutdown();
    }
}
