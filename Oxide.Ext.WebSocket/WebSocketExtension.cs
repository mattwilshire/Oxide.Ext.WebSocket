using Oxide.Core;
using Oxide.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oxide.Ext.WebSocket
{
    public class WebSocketExtension : Extension
    {
        private Libraries.WebSocket WebSocket;
        public override string Name => "WebSocket";
        public override string Author => "mattwilshire";
        public override VersionNumber Version => new VersionNumber(1, 0, 0);

        public WebSocketExtension(ExtensionManager manager) : base(manager)
        {

        }

        public override void Load()
        {
            Manager.RegisterLibrary("WebSocket", WebSocket = new Libraries.WebSocket());
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
