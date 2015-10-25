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
using draw = System.Drawing;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Unturned.Commands;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using UnityEngine;

namespace CallVoteCommands
{
	public class CVoteCommand : IRocketCommand
	{
		public void Execute(IRocketPlayer caller, string[] command)
		{
			UnturnedPlayer cPlayer = (UnturnedPlayer)caller;

			if (command.Length < 1)
			{
				return;
			}
			else if (command[0].ToString().ToLower() == "day" && cPlayer.HasPermission("callvote.day") && CallVote.Plugin.Instance.voteInProgress == false && CallVote.Plugin.Instance.dayVoteInCooldown == false)
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer.CharacterName + " has called a vote to make it daytime. You have "
					+ CallVote.Plugin.Instance.Configuration.Instance.dayVoteTimer + " วินาทีที่จะ พิมพ์  </cvote yes> การออกเสียงลงคะแนน", Color.yellow);
				CallVote.Plugin.Instance.voteInProgress = true;

				CallVote.Utility.initiateDayVote();
			}
			else if (command[0].ToString().ToLower() == "yes" && cPlayer.HasPermission("callvote.vote") && CallVote.Plugin.Instance.voteInProgress == true)
			{
				if (!CallVote.Plugin.Instance.voteTracker.ContainsKey(cPlayer.CSteamID))
				{
					CallVote.Plugin.Instance.totalFor = CallVote.Plugin.Instance.totalFor + 1;
					float percentFor = (CallVote.Plugin.Instance.totalFor/CallVote.Plugin.Instance.onlinePlayers)*100;
					Rocket.Unturned.Chat.UnturnedChat.Say (percentFor + "% Yes, Required: " + CallVote.Plugin.Instance.Configuration.Instance.successfulDayVotePercent
						+ "%.", Color.yellow);
					CallVote.Plugin.Instance.voteTracker.Add(cPlayer.CSteamID, cPlayer.CharacterName);
				}
				else if (CallVote.Plugin.Instance.voteTracker.ContainsKey(cPlayer.CSteamID))
				{
					Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "You have already voted!", Color.yellow);
				}
			}
			else if (command[0].ToString().ToLower() == "day" && !cPlayer.HasPermission("callvote.day"))
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "You do not have permission to start a day vote!", Color.red);
			}
			else if (command[0].ToString().ToLower() == "day" && CallVote.Plugin.Instance.voteInProgress == true)
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Only one vote may be called at a time.", Color.yellow);
			}
			else if (command[0].ToString().ToLower() == "day" && CallVote.Plugin.Instance.dayVoteInCooldown == true)
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "A day vote may only be called every " + CallVote.Plugin.Instance.Configuration.Instance.dayVoteCooldown
					+ " seconds.", Color.yellow);
			}
			else if (command[0].ToString().ToLower() == "yes" && !cPlayer.HasPermission("callvote.vote"))
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "You do not have permission to vote!", Color.red);
			}
			else if (command[0].ToString().ToLower() == "yes" && CallVote.Plugin.Instance.voteInProgress == false)
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "There are no votes currently active.", Color.red);
			}
			else
			{
				Rocket.Unturned.Chat.UnturnedChat.Say (cPlayer, "Invalid command parameter.", Color.yellow);
			}
		}
		public bool AllowFromConsole
		{
			get
			{
				return false;
			}
		}
		public bool RunFromConsole
		{
			get { return false; }
		}
		public string Name
		{
			get { return "callvote.cvote"; }
		}
		public List<string> Aliases
		{
			get { return new List<string> { "cvote" }; }
		}
		public string Syntax
		{
			get
			{
				return "<vote type> <yes or no>";
			}
		}
		public string Help
		{
			get { return "Enter vote node to start a vote. Enter yes or no to vote."; }
		}
		public List<string> Permissions
		{
			get
			{
				return new List<string>() { 
					"callvote.cvote"
				};
			}
		}
	}

}
