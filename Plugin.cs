using System;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using Rocket.Core;
using Rocket.Core.Logging;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Events;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Plugins;
using Rocket.Unturned.Plugins;
using SDG;
using SDG.Unturned;
using UnityEngine;

namespace CallVote
{
	public class Plugin : RocketPlugin<Configuration>
	{
		//globals
		public int onlinePlayers = 0;
		public int totalFor = 0;
		public bool voteInProgress = false;
		public bool dayVoteInCooldown = false;
		public Dictionary<Steamworks.CSteamID, String> voteTracker = new Dictionary<Steamworks.CSteamID, String>();

		public static Plugin Instance;

		protected override void Load()
		{
			Instance = this;
			Logger.Log("CallVote has been loaded!");
			U.Events.OnPlayerConnected += Events_OnPlayerConnected;
			U.Events.OnPlayerDisconnected += Events_OnPlayerDisconnected;
		}
		protected override void Unload()
		{
			U.Events.OnPlayerConnected -= Events_OnPlayerConnected;
			U.Events.OnPlayerDisconnected -= Events_OnPlayerDisconnected;
			base.Unload ();
		}
		private void Events_OnPlayerConnected(UnturnedPlayer player)
		{
			if (!player.HasPermission("callvote.exclude"))
			{
				onlinePlayers = onlinePlayers + 1;
			}
		}
		private void Events_OnPlayerDisconnected(UnturnedPlayer player)
		{
			if (!player.HasPermission("callvote.exclude"))
			{
				onlinePlayers = onlinePlayers - 1;
			}
		}
	}
	public class Configuration : IRocketPluginConfiguration
	{
		public static Configuration Instance;

		public int dayVoteTimer;
		public int dayVoteCooldown;
		public int successfulDayVotePercent;

		public void LoadDefaults()
		{
			dayVoteTimer = 20;
			dayVoteCooldown = 120;
			successfulDayVotePercent = 60;
		}
	}
}
