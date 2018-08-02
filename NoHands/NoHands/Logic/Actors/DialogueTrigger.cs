using System;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Resources;
using Resources.Sprites;
using Microsoft.Xna.Framework;
using Monofoxe.Utils;

namespace NoHands.Logic
{
	public class DialogueTrigger : Actor
	{
		
		int _index;
		
		NPC _other;

		SpeechBubble _bubble;

		string[] _villainDialogue = {
			"me",
			"hey.",
			"other",
			"sup.",
			"me",
			"are you the guy who stole my hands?",
			"other",
			"yep.",
			"me",
			"oh, cool.",
			"me",
			"can you please" + Environment.NewLine + "give them back?",
			"other",
			"yeah, sure.",
			"me",
			"thanks.",
			"me",
			"so, what are doing today?",
			"other",
			"nothing, really.",
			"other",
			"walking around, stealing hands.",
			"other",
			"you know.",
			"me",
			"wanna go play some videogames?",
			"other",
			"sure.",
			"other",
			"i've heard," + Environment.NewLine + "OH MY GOD, LOOK AT THIS KNIGHT" + Environment.NewLine + "has been released.",
			"me",
			"oh, really? i thought devs will never finish it.",
			"other",
			"yeah, me too.",
		};

		string[] _npcDialogue = {
			"other",
			"oh no, someone" + Environment.NewLine + "stole my hands!",
			"other",
			"is there a hero who" + Environment.NewLine + "will save us?",
			"me",
			"maybe, i dunno.",
			"other",
			"i also saw many traps" + Environment.NewLine + "villain left.",
			"me",
			"huh, this sounds like something" + Environment.NewLine + "straight out of a videogame.",
		};

		string[] _coinDialogue = {
			"me",
			"oh no.",
			"me",
			"there is a coin on the ground.",
			"me",
			"i bet someone forgot it here.",
			"me",
			"damn.",
			"me",
			"i'd better leave it right here.",
			"me",
			"i'm not a thief or something.",
		};


		string[] Lines;
		int LinePtr = -1;

		bool _active = false;

		public DialogueTrigger(Vector2 pos, byte index)
		{
			Position = pos;
			_index = index;
			if (index == 0)
			{
				Lines = _npcDialogue;
			}
			if (index == 1)
			{
				Lines = _coinDialogue;
			}
			if (index == 2)
			{
				Lines = _villainDialogue;
			}

		}
		
		public override void Update()
		{
			var player = Objects.ObjFind<Player>(0);

			if (player != null)
			{
				if (GameMath.Distance(Position, player.Position) < 100 && !_active)
				{
					_active = true;
					player.ControlsEnabled = false;
					player.Speaking = false;
					player.LinePtr -= 1;
					if (player.LinePtr < -1)
					{
						player.LinePtr = -1;
					}
					Objects.Destroy(player.Bubble);

					var npcs = Objects.GetList<NPC>();

					double shortestDist = 1000000000;

					foreach(NPC npc in npcs)
					{
						var d = GameMath.Distance(Position, npc.Position);
						if (d < shortestDist)
						{
							_other = npc;
							shortestDist = d;
						}
					}

				}
			}

			if (_active && (_bubble == null || _bubble.Destroyed))
			{
				LinePtr += 1;
				if (LinePtr >= Lines.Length / 2)
				{
					Objects.Destroy(this);
					player.ControlsEnabled = true;
					player.Speaking = true;
				}
				else
				{
					if (Lines[LinePtr * 2] == "me")
					{
						_bubble = new SpeechBubble(player, Lines[LinePtr * 2 + 1]);
					}
					else
					{
						_bubble = new SpeechBubble(_other, Lines[LinePtr * 2 + 1]);		
					}
				}
			}

				
		}
	}
}