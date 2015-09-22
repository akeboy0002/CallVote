using System;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using SDG.Unturned;
using UnityEngine;

namespace CallVote
{
	public class Utility
	{
		public static Utility Instance;

		public static void initiateDayVote()
		{
			new Thread(() => 
				{
					Thread.CurrentThread.IsBackground = true; 
					Thread.Sleep(CallVote.Plugin.Instance.Configuration.Instance.dayVoteTimer*1000);

					double percentFor = (double)((CallVote.Plugin.Instance.totalFor/CallVote.Plugin.Instance.onlinePlayers)*100);
					percentFor = System.Math.Round(percentFor,2);
					if(percentFor >= CallVote.Plugin.Instance.Configuration.Instance.successfulDayVotePercent)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say ("The vote to make it daytime was successful.", Color.green);
						Steam.ConsoleInput.onInputText("day");
					}
					else if (percentFor < CallVote.Plugin.Instance.Configuration.Instance.successfulDayVotePercent)
					{
						Rocket.Unturned.Chat.UnturnedChat.Say ("The vote to make it daytime was unsuccessful.", Color.red);
					}
					CallVote.Plugin.Instance.voteTracker.Clear();
					CallVote.Plugin.Instance.dayVoteInCooldown = true;
					CallVote.Plugin.Instance.voteInProgress = false;
					CallVote.Plugin.Instance.totalFor = 0;

					initiateDayVoteCooldown();
				}).Start();
		}
		public static void initiateDayVoteCooldown()
		{
			new Thread(() => 
				{
					Thread.CurrentThread.IsBackground = true; 
					Thread.Sleep(CallVote.Plugin.Instance.Configuration.Instance.dayVoteCooldown*1000);

					CallVote.Plugin.Instance.dayVoteInCooldown = false;
				}).Start();
		}
	}
}

