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

		string[] _testDialogue = {
			"me",
			"Hey.",
			"other",
			"Sup.",
			"other",
			"So, um, how are you today?",
			"me",
			"Ok, I guess.",
		};

		string[] Lines;
		int LinePtr = -1;

		bool _active = false;

		public DialogueTrigger(Vector2 pos, byte index)
		{
			Position = pos;
			_index = index;
			Lines = _testDialogue;
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