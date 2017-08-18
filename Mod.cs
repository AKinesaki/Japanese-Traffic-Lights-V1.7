using UnityEngine;
using ColossalFramework;
using ICities;
using System;

namespace JapaneseTrafficLights
{
	public class Mod : IUserMod
    {
        public string Name => "Japanese Traffic Lights";
        public string Description => "Replaces traffic lights with more Japanese-looking versions";

		public void OnSettingsUI(UIHelperBase helper)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			UIHelperBase GroupHeader = helper.AddGroup("Japanese Traffic Lights Options - Changes will apply after reload savedata.");
			//group.AddCheckbox("More realistic layout (may affect performance)", config.realisticLayout, IsRealStyle);
			GroupHeader.AddCheckbox("Use Global style on Monorail Roads", config.Monorail, IsMonorail);

			UIHelperBase GroupTinyRoads = helper.AddGroup("Tiny Roads");					
			GroupTinyRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.TinyRoads, TinyRoads);

			UIHelperBase GroupPedestrianRoads = helper.AddGroup("Pedestrian Roads");
			GroupPedestrianRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.PedestrianRoads, PedestrianRoads);
			GroupPedestrianRoads.AddCheckbox("Hide signals on Pedestrian Roads", config.HidePedRoadsSignal, IsPedRoadsSignal);
			GroupPedestrianRoads.AddCheckbox("Hide signals on Promenade", config.HidePromenadeSignal, IsPromenadeSignal);

			UIHelperBase GroupSmallRoads = helper.AddGroup("Small Roads");
			GroupSmallRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.SmallRoads, SmallRoads);

			UIHelperBase GroupSmallHeavyRoads = helper.AddGroup("Small Heavy Roads");
			GroupSmallHeavyRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.SmallHeavyRoads, SmallHeavyRoads);

			UIHelperBase GroupMediumRoads = helper.AddGroup("Medium Roads");
			GroupMediumRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.MediumRoads, MediumRoads);

			UIHelperBase GroupLargeRoads = helper.AddGroup("Large Roads");
			GroupLargeRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.LargeRoads, LargeRoads);

			UIHelperBase GroupWideRoads = helper.AddGroup("Wide Roads");
			GroupWideRoads.AddDropdown("Style", new string[] { "White", "Brown" }, config.WideRoads, WideRoads);

			UIHelperBase GroupHighways = helper.AddGroup("Highways");
			GroupHighways.AddDropdown("Style", new string[] { "White", "Brown" }, config.Highways, Highways);

			UIHelperBase GroupGlobal = helper.AddGroup("Global(It applies to new roads)");
			GroupGlobal.AddDropdown("Style", new string[] { "White", "Brown" }, config.Global, Global);
		}

		/*private void IsRealStyle(bool c)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.realisticLayout = c;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}*/

		private void IsPedRoadsSignal(bool b)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.HidePedRoadsSignal = b;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void IsPromenadeSignal(bool b)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.HidePromenadeSignal = b;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void IsMonorail(bool b)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.Monorail = b;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void Global(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.Global = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void TinyRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.TinyRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void PedestrianRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.PedestrianRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void SmallRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.SmallRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void SmallHeavyRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.SmallHeavyRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void MediumRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.MediumRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void LargeRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.LargeRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void WideRoads(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.WideRoads = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}

		private void Highways(int i)
		{
			var config = Configuration<JapaneseTrafficLightsConfiguration>.Load();

			config.Highways = i;
			Configuration<JapaneseTrafficLightsConfiguration>.Save();
		}
	}

	[ConfigurationPath("JapaneseTrafficLights.xml")]
	public class JapaneseTrafficLightsConfiguration
	{
		//public bool realisticLayout { get; set; }
		public bool HidePedRoadsSignal { get; set; } = true;
		public bool HidePromenadeSignal { get; set; } = false;
		public bool Monorail { get; set; } = true;
		public int Global { get; set; } = 0;
		public int TinyRoads { get; set; } = 0;
		public int PedestrianRoads { get; set; } = 0;
		public int SmallRoads { get; set; } = 0;
		public int SmallHeavyRoads { get; set; } = 0;
		public int MediumRoads { get; set; } = 0;
		public int LargeRoads { get; set; } = 0;
		public int WideRoads { get; set; } = 0;
		public int Highways { get; set; } = 0;
	}
}
