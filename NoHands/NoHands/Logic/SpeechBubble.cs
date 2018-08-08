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

		float _textRubberBand = 8f;

		public Vector2 Offset = new Vector2(0, -120);

		AutoAlarm _typeAlarm = new AutoAlarm(0.05);
		Alarm _delayAlarm = new Alarm();


		IFont _font = Resources.Fonts.CartonSix;

		Vector2 _textSize, _targetTextSize;
		
		int _cornerVecticesCount = 4;
		int _sideVecticesCount = 8;

		Vector2[] _wiggleys;
		double[] _wiggleysPhase;
		float _wiggleysSpd = 0.25f * 20;
		float _wiggleysR = 1;
		int _wiggleyId = 0;

		public SpeechBubble(Actor owner, string text)
		{
			Owner = owner;
			Text = text;

			var r = new RandomExt();

			_wiggleys = new Vector2[(_cornerVecticesCount + _sideVecticesCount) * 4];
			_wiggleysPhase = new double[_wiggleys.Length];

			for(var i = 0; i < _wiggleysPhase.Length; i += 1)
			{
				_wiggleysPhase[i] = r.NextDouble(Math.PI * 2);
			}

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

			var str = Text.Substring(0, TextPtr + 1);
			_targetTextSize = _font.MeasureString(str);

			_textSize.X += (_targetTextSize.X - _textSize.X) / _textRubberBand;
			_textSize.Y += (_targetTextSize.Y - _textSize.Y) / _textRubberBand;


			if (_delayAlarm.Update())
			{
				Objects.Destroy(this);
			}

			var phaseAdd = GameCntrl.Time(_wiggleysSpd);

			for(var i = 0; i < _wiggleys.Length; i += 1)
			{
				_wiggleysPhase[i] += phaseAdd;
				if (_wiggleysPhase[i] > Math.PI * 2)
				{
					_wiggleysPhase[i] -= Math.PI * 2;
				}

				_wiggleys[i] = new Vector2(
					(float)Math.Cos(_wiggleysPhase[i]), 
					(float)Math.Sin(_wiggleysPhase[i])
				) * _wiggleysR;
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

			DrawSpeechBubble(orig, _textSize);

			DrawCntrl.CurrentColor = new Color(Color.Black, 0.7f);
			DrawCntrl.DrawText(str, orig);
		}



		void DrawSpeechBubble(Vector2 pos, Vector2 size)
		{
			DrawCntrl.CurrentColor = new Color(Color.White, 1.5f);

			_wiggleyId = 0;

			var flippedSize = new Vector2(size.X, -size.Y);

			DrawCntrl.PrimitiveBegin();
		
			DrawArc(pos - size / 2, 90);
			DrawLine(pos - size / 2, Vector2.UnitX, size.X);
			
			DrawArc(pos + flippedSize / 2, 0);
			DrawLine(pos + flippedSize / 2, Vector2.UnitY, size.Y);

			DrawArc(pos + size / 2, 270);
			DrawLine(pos + size / 2, -Vector2.UnitX, size.X);

			DrawArc(pos - flippedSize / 2, 180);
			DrawLine(pos - flippedSize / 2, -Vector2.UnitY, size.Y);
			
			//DrawCntrl.PrimitiveSetLineStripIndices(true);
			DrawCntrl.PrimitiveSetTriangleFanIndices();
			DrawCntrl.PrimitiveEnd();
		}

		void DrawArc(Vector2 pos, float ang)
		{
			var stepAng = 90f / _cornerVecticesCount;
			for(var i = _cornerVecticesCount; i >= 0; i -= 1)
			{
				var dir = MathHelper.ToRadians(ang + i * stepAng);
				DrawCntrl.PrimitiveAddVertex(pos + _wiggleys[i] + new Vector2((float)Math.Cos(dir), -(float)Math.Sin(dir)) * Border);
				_wiggleyId += 1;
			}
		}

		void DrawLine(Vector2 pos, Vector2 dir, float length)
		{
			var step = length / _sideVecticesCount;
			var borderOffset = new Vector2(dir.Y, -dir.X) * Border;
			for(var i = 0; i <= _sideVecticesCount; i += 1)
			{
				DrawCntrl.PrimitiveAddVertex(pos + _wiggleys[i] + dir * step * i + borderOffset);
				_wiggleyId += 1;
			}
		}


	}
}
