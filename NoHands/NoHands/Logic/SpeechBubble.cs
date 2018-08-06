using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monofoxe.Engine;
using Monofoxe.Engine.Drawing;
using Monofoxe.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoHands.Logic
{
	public class SpeechBubble : GameObj
	{
		public readonly string Text = "test";
		public int TextPtr = 0;
		public readonly Actor Owner;
		public const int Border = 16;

		
		public Vector2 Offset = new Vector2(0, -120);

		AutoAlarm _typeAlarm = new AutoAlarm(0.05);
		Alarm _delayAlarm = new Alarm();


		IFont _font = Resources.Fonts.CartonSix;

		public SpeechBubble(Actor owner, string text)
		{
			Owner = owner;
			Text = text;
		}

		public override void Update()
		{
			if (_typeAlarm.Update())
			{
				TextPtr += 1;
				
				try
				{
					if (Text[TextPtr] == ' ')
					{
						TextPtr += 1;
					}

					if (Text[TextPtr] == Environment.NewLine[0])
					{
						TextPtr += Environment.NewLine.Length;
					}

					if (TextPtr >= Text.Length)
					{
						throw new Exception();
					}
				}
				catch(Exception)
				{
					_typeAlarm.Active = false;
					TextPtr = Text.Length - 1;
					_delayAlarm.Set(1 + Text.Length * 0.1);
				}
			}

			if (_delayAlarm.Update())
			{
				Objects.Destroy(this);
			}
			
		}

		public override void DrawEnd()
		{
			var str = Text.Substring(0, TextPtr + 1);

			var orig = Test.RoundVector2(Owner.Position + Offset);

			DrawCntrl.CurrentColor = new Color(Color.White, 0.5f);
			DrawCntrl.CurrentFont = _font;
			DrawCntrl.HorAlign = TextAlign.Center;
			DrawCntrl.VerAlign = TextAlign.Center;

			var textSize = _font.MeasureString(str) + Vector2.One * Border;

			DrawCntrl.DrawRectangle(orig - textSize / 2, orig + textSize / 2, false);
			
			DrawCntrl.CurrentColor = new Color(Color.Black, 0.7f);
			DrawCntrl.DrawText(str, orig);
		}
	}
}
