using Oxide.Core;
using Oxide.Core.Extensions;
using Oxide.Ext.WebSocket.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.Ext.WebSocket
{
    public class WebSocketExtension : Extension
    {
        private IWebSocket WebSocket;
        public override string Name => "WebSocket";
        public override string Author => "mattwilshire";
        public override VersionNumber Version => new VersionNumber(1, 0, 0);

        private bool SECURE = false;

        public WebSocketExtension(ExtensionManager manager) : base(manager)
        {

        }

        public override void Load()
        {
            if(SECURE)
            {
                Manager.RegisterLibrary("WebSocket", (Core.Libraries.Library)(WebSocket = new Libraries.WebSocketSecure()));
            } 
            else
            {
                Manager.RegisterLibrary("WebSocket", (Core.Libraries.Library)(WebSocket = new Libraries.WebSocket()));
            }
            
        }

        public override void LoadPluginWatchers(string plugindir)
        {
            WebSocket?.Initialize();
        }

        public override void OnShutdown()
        {
            WebSocket?.Shutdown();
        }
    }
}
