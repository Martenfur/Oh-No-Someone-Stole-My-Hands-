using Monofoxe.Engine.Audio;


namespace Resources
{
	public static class Sounds
	{
		public static Sound MusicMain;
		public static Sound Step;
		

		public static void Load()
		{
			MusicMain = AudioMgr.LoadStreamedSound("Music/ingame");
			Step = AudioMgr.LoadSound("Sounds/step", FMOD.MODE._3D);
		}

		public static void Unload()
		{
			MusicMain.Unload();
			Step.Unload();
		}

	}
}
