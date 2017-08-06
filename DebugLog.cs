using UnityEngine;

namespace JapaneseTrafficLights
{
	public static class Log
	{
		public enum mode
		{
			normal,error,warning
		}

		public static void Display(mode mode, string message, bool alwaysDisplay = false)
		{
			if (alwaysDisplay)
			{
				DisplayLog(mode, message);
			}
			else
			{
				#if DEBUG
				DisplayLog(mode, message);
				#endif
			}
		}

		private static void DisplayLog(mode mode, string message)
		{
			switch (mode)
			{
				case mode.normal:
					Debug.Log(message);
					break;

				case mode.error:
					Debug.LogError(message);
					break;

				case mode.warning:
					Debug.LogWarning(message);
					break;
			}
		}
	}
}
