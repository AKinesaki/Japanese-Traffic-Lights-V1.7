using UnityEngine;
using ColossalFramework;
using ICities;
using System;
using System.Linq;
using System.Collections.Generic;

namespace JapaneseTrafficLights
{
	public class JPTL
	{
		// 更新用メモ
		// 1. メンバ変数を追加
		// 2. コンストラクタにアセットを追加
		// 3. ReturnStyleFromConfigに処理を追加
		// 4. ReplaceTLに処理を追加

		/// <summary>
		/// メンバ変数
		/// </summary>
		private static readonly JapaneseTrafficLightsConfiguration config;
		private static readonly PropInfo JPTLmain, JPTLsub, JPTLmainBrown, JPTLsubBrown, JPTLmainBrown2, JPTLsubBrown2;
		private enum style { none, white, brown, brown2 }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		static JPTL()
		{
			//左側通行(LHT) 809633246.
			//右側通行(RHT) 810355214.
			string workshopId = "";
			//																L or R 確認
			JPTLmain = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLmainNormalL" + "_Data");
			JPTLsub = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLsubNormalL" + "_Data");
			JPTLmainBrown = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLmainBrownL" + "_Data");
			JPTLsubBrown = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLsubBrownL" + "_Data");
			JPTLmainBrown2 = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLmainBrown2L" + "_Data");
			JPTLsubBrown2 = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLsubBrown2L" + "_Data");

			if (JPTLmain == null || JPTLsub == null || JPTLmainBrown == null || JPTLsubBrown == null || JPTLmainBrown2 == null || JPTLsubBrown2 == null)
			{
				Log.Display(Log.mode.error, "Prop Not Found", true);
				return;
			}

			config = Configuration<JapaneseTrafficLightsConfiguration>.Load();
		}

		/// <summary>
		/// 信号を置き換える。
		/// </summary>
		public static void ReplaceAllTL()
		{
			Log.Display(Log.mode.warning, $"ReplaceAllTL Init");

			NetInfo[] roads = GetRegisteredNetInfos();

			if (roads == null || roads.Length == 0)
			{
				Log.Display(Log.mode.error, "ReplaceAllTL - NetInfo[] is null.");
				return;
			}

			ReplaceTL(roads);
		}

		/// <summary>
		/// 全てのNetInfoを読み込み、配列で返す。
		/// </summary>
		public static NetInfo[] GetRegisteredNetInfos()
		{
			var allNetinfos = new List<NetInfo>();
			for (uint i = 0; i < PrefabCollection<NetInfo>.PrefabCount(); i++)
			{
				NetInfo info = PrefabCollection<NetInfo>.GetPrefab(i);
				if (info == null) continue;

				allNetinfos.Add(info);
			}
			return allNetinfos.ToArray();
		}

		/// <summary>
		/// NetInfoコレクションにある信号を置き換える。
		/// </summary>
		private static void ReplaceTL(IEnumerable<NetInfo> roads)
		{
			Log.Display(Log.mode.warning, "ReplaceTL Init");

			foreach (var road in roads)
			{
				foreach (var lane in road.m_lanes)
				{
					if (lane?.m_laneProps?.m_props == null)
					{
						continue;
					}

					foreach (var laneProp in lane.m_laneProps.m_props)
					{
						var prop = laneProp.m_finalProp;

						if (prop == null)
						{
							continue;
						}

						// 道路を判別してstyleを指定する
						style style = ReturnStyleFromRoadname(road.name);

						// ここから置き換え
						switch (prop.name)
						{
							//（右側通行）これと
							case "Traffic Light 01":
							case "Traffic Light European 01":
							case "Traffic Light 02 Mirror":
							case "Traffic Light European 02 Mirror":							
								switch(style)
								{
									case style.none:
										laneProp.m_finalProp = null;
										laneProp.m_prop = null;
										break;

									case style.white:
										laneProp.m_finalProp = JPTLmain;
										laneProp.m_prop = JPTLmain;
										break;

									case style.brown:
										laneProp.m_finalProp = JPTLmainBrown;
										laneProp.m_prop = JPTLmainBrown;
										break;

									case style.brown2:
										laneProp.m_finalProp = JPTLmainBrown2;
										laneProp.m_prop = JPTLmainBrown2;
										break;
								}
								break;

							//（右側通行）これの中身を入れ替え
							case "Traffic Light 01 Mirror":
							case "Traffic Light European 01 Mirror":
							case "Traffic Light 02":
							case "Traffic Light European 02":
								//WideAvenueの時はTL1Mを消す。歩行者信号と重なってしまうため。
								if (road.name.Contains("WideAvenue")) { style = style.none; }

								switch (style)
								{
									case style.none:
										laneProp.m_finalProp = null;
										laneProp.m_prop = null;
										break;

									case style.white:
										laneProp.m_finalProp = JPTLsub;
										laneProp.m_prop = JPTLsub;
										break;

									case style.brown:
										laneProp.m_finalProp = JPTLsubBrown;
										laneProp.m_prop = JPTLsubBrown;
										break;

									case style.brown2:
										laneProp.m_finalProp = JPTLsubBrown2;
										laneProp.m_prop = JPTLsubBrown2;
										break;
								}
								break;

							case "Traffic Light Pedestrian":
							case "Traffic Light Pedestrian European":
								//（右側通行）この行を削除
								laneProp.m_angle = laneProp.m_angle + 180;

								// FourDevidedLaneAvenueの時は消す。車道用信号か重なってしまうため。
								if (road.name.Contains("FourDevidedLaneAvenue")){ style = style.none; }

								switch (style)
								{
									case style.none:
										laneProp.m_finalProp = null;
										laneProp.m_prop = null;
										break;

									case style.white:
										laneProp.m_finalProp = JPTLsub;
										laneProp.m_prop = JPTLsub;
										break;

									case style.brown:
										laneProp.m_finalProp = JPTLsubBrown;
										laneProp.m_prop = JPTLsubBrown;
										break;

									case style.brown2:
										laneProp.m_finalProp = JPTLsubBrown2;
										laneProp.m_prop = JPTLsubBrown2;
										break;
								}
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// コンフィグの値に対応したstyleを返す。
		/// </summary>
		private static style ReturnStyleFromConfig(int i)
		{
			switch (i)
			{
				case 0:
				default:
					return style.white;
				case 1:
					return style.brown;
				case 2:
					return style.brown2;
			}
		}

		/// <summary>
		/// 道路名に対応したsyleを返す。
		/// </summary>
		private static style ReturnStyleFromRoadname(string name)
		{
			style style = ReturnStyleFromConfig(config.Global);

			// Bus
			if (name.ToLower().Contains("bus") && config.EnableBus) { return style = ReturnStyleFromConfig(config.Bus); }

			// Monorail
			if (name.Contains("Monorail") && config.EnableMonorail) { return style = ReturnStyleFromConfig(config.Monorail); }

			// Grass
			if (name.Contains("Grass") && config.EnableGrass)
			{
				if(!name.Contains("Medium Road"))
				{
					return style = ReturnStyleFromConfig(config.Grass);
				}
			}

			// Trees
			if (name.Contains("Trees") && config.EnableTrees)
			{
				if (!name.Contains("Medium Road"))
				{
					return style = ReturnStyleFromConfig(config.Trees);
				}
			}

			// Tiny Roads
			if (
				name.Contains("Gravel Road") ||
				name.Contains("PlainStreet2L") ||
				name.Contains("Two-Lane Alley") ||
				name.Contains("Two-Lane Oneway") ||
				name.Contains("One-Lane Oneway With") ||
				name == "One-Lane Oneway"
			) { return style = ReturnStyleFromConfig(config.TinyRoads); }

			// Small Roads
			if (
				name.Contains("Basic Road") ||
				name.Contains("BasicRoadPntMdn") ||
				name.Contains("BasicRoadMdn") ||
				name.Contains("Oneway Road") ||
				name.Contains("Asymmetrical Three Lane Road") ||
				name.Contains("One-Lane Oneway with") ||
				name.Contains("Small Busway") ||
				name.Contains("Harbor Road") ||
				name.Contains("Tram Depot Road") ||
				name.Contains("Small Road")
			) { return style = ReturnStyleFromConfig(config.SmallRoads); }

			// Small Heavy Roads
			if (
				name.Contains("BasicRoadTL") ||
				name.Contains("AsymRoadL1R2") ||
				name.Contains("Oneway3L") ||
				name.Contains("Small Avenue") ||
				name.Contains("AsymRoadL1R3") ||
				name.Contains("Oneway4L") ||
				name.Contains("OneWay3L")
			) { return style = ReturnStyleFromConfig(config.SmallHeavyRoads); }

			// Medium Roads
			if (
				name.Contains("Medium Road") ||
				name.Contains("Avenue Large With") ||
				name.Contains("Medium Avenue") ||
				name.Contains("FourDevidedLaneAvenue") ||
				name.Contains("AsymAvenueL2R4") ||
				name.Contains("AsymAvenueL2R3")
			) { return style = ReturnStyleFromConfig(config.MediumRoads); }

			// Large Roads
			if (
				name.Contains("Large Road") ||
				name.Contains("Large Oneway") ||
				name.Contains("Eight-Lane Avenue")
			) { return style = ReturnStyleFromConfig(config.LargeRoads); }

			// Wide Roads
			if (
				name.Contains("WideAvenue")
			) { return style = ReturnStyleFromConfig(config.WideRoads); }

			// Highway
			if (
				name.Contains("Highway")
			) { return style = ReturnStyleFromConfig(config.Highways); }

			// Pedestrian Roads
			if (name.Contains("Zonable Pedestrian"))
			{
				if (config.HidePedRoadsSignal) { return style = style.none; }
				else { return style = ReturnStyleFromConfig(config.PedestrianRoads); }
			}

			// Promenade
			if (name.Contains("Zonable Promenade"))
			{
				if (config.HidePromenadeSignal) { return style = style.none; }
				else { return style = ReturnStyleFromConfig(config.PedestrianRoads); }
			}

			return style;
		}
	}
}
