using System.Collections.Generic;
using System;
using System.Reflection;
using System.Data;
using System.Linq;
using UnityEngine;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Logging;
using Oxide.Core.Plugins;
 
namespace Oxide.Plugins
{
    [Info("Example", "Matt", 1.0)]
    class Example : RustPlugin
    {      
        private void OnServerInitialized()
        {
			PrintWarning("Example");
        }
			   
		[ChatCommand("example")]
		private void StatsCommand(BasePlayer player, string command, string[] args)
		{
			SendReply(player, "Test!!!");
		}

		private void OnWebSocketMessage(string message)
		{
			PrintWarning(message);
			BasePlayer player = Player.FindById("76561198154363268");
			SendReply(player, message);
			Player.GiveItem(player, 1545779598);
		}
	}
}
