using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Resources.Sprites;
using Monofoxe.Engine.Drawing;

namespace Resources
{
	public static class Fonts
	{
		private static ContentManager _content;
		
		static string Ascii = " !" + '"' + @"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
		
		public static IFont Arial;
		
		public static void Load(ContentManager content)
		{
			_content = new ContentManager(content.ServiceProvider);
			_content.RootDirectory = content.RootDirectory;
			
			Arial = new Font(_content.Load<SpriteFont>("fonts/arial"));
		}

		public static void Unload()
		{
			_content.Unload();
		}

	}
}
