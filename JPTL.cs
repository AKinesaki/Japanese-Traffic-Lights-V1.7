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
		private static readonly PropInfo JPTLmain, JPTLsub, JPTLmainB, JPTLsubB;
		private static readonly JapaneseTrafficLightsConfiguration config;

		//スタイルを追加した時は、CheckRoadStyleと信号の置き換え部分も更新する
		private enum style
		{
			def,
			none,
			white,
			brown
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		static JPTL()
		{
			//左側通行(LHT) 809633246.
			//右側通行(RHT) 810355214.
			string workshopId = "";
			//																L or R 確認
			JPTLmain = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLmainNormalL_Data"); // 白　メイン
			JPTLsub = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLsubNormalL_Data"); // 白　反対側
			JPTLmainB = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLmainBrownL_Data"); // 茶　メイン
			JPTLsubB = PrefabCollection<PropInfo>.FindLoaded(workshopId + "JPTLsubBrownL_Data"); // 茶　反対側、歩行者

			if (JPTLmain == null || JPTLsub == null || JPTLmainB == null || JPTLsubB == null)
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

						// 道路の種別を判別してstyleを指定する
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
										laneProp.m_finalProp = JPTLmainB;
										laneProp.m_prop = JPTLmainB;
										break;
								}
								break;

							//（右側通行）これの中身を入れ替え
							case "Traffic Light 01 Mirror":
							case "Traffic Light European 01 Mirror":
							case "Traffic Light 02":
							case "Traffic Light European 02":
								if (
										//road.name.Contains("FourDevidedLaneAvenue")
										road.name.Contains("WideAvenue")
								){
									style = style.none;
								}

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
										laneProp.m_finalProp = JPTLsubB;
										laneProp.m_prop = JPTLsubB;
										break;
								}
								break;

							case "Traffic Light Pedestrian":
							case "Traffic Light Pedestrian European":
								//（右側通行）この行を削除
								laneProp.m_angle = laneProp.m_angle + 180;

								// NE2の中央道路の場合は、歩行者信号を非表示にする
								if (
									road.name.Contains("FourDevidedLaneAvenue")
									//road.name.Contains("WideAvenue")
								){
									style = style.none;
								}

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
										laneProp.m_finalProp = JPTLsubB;
										laneProp.m_prop = JPTLsubB;
										break;
								}
								break;
						}
					}
				}
			}
		}

		/// <summary>
		/// 全てのNetInfoを読み込み、配列で返す。
		/// </summary>
		private static NetInfo[] GetRegisteredNetInfos()
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
		/// 全てのNetInfoを読み込み、テキストに出力する。
		/// </summary>
		public static void OutputRegisteredNetInfos()
		{
			System.IO.StreamWriter sw = new System.IO.StreamWriter("AllNetInfo.txt", false, System.Text.Encoding.GetEncoding("utf-8"));

			for (uint i = 0; i < PrefabCollection<NetInfo>.PrefabCount(); i++)
			{
				NetInfo info = PrefabCollection<NetInfo>.GetPrefab(i);
				if (info == null) continue;

				sw.WriteLine(info.name.ToString());
			}

			sw.Close();
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
			}
		}

		/// <summary>
		/// 道路名に対応したsyleを返す。
		/// </summary>
		private static style ReturnStyleFromRoadname(string name)
		{
			// 手違いでdefが残った場合にはデフォルトの信号が現れる
			style style = style.def;

			// Tiny Roads
			if (
				name.Contains("Gravel Road") ||
				name.Contains("PlainStreet2L") ||
				name.Contains("Two-Lane Alley") ||
				name.Contains("Two-Lane Oneway") ||
				name.Contains("One-Lane Oneway With") ||
				name == "One-Lane Oneway"
			) { style = ReturnStyleFromConfig(config.TinyRoads); }

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
				// Small Road Monorail
				name.Contains("Small Road")
			) { style = ReturnStyleFromConfig(config.SmallRoads); }

			// Small Heavy Roads
			if (
				name.Contains("BasicRoadTL") ||
				name.Contains("AsymRoadL1R2") ||
				name.Contains("Oneway3L") ||
				name.Contains("Small Avenue") ||
				name.Contains("AsymRoadL1R3") ||
				name.Contains("Oneway4L") ||
				name.Contains("OneWay3L")
			) { style = ReturnStyleFromConfig(config.SmallHeavyRoads); }

			// Medium Roads
			if (
				name.Contains("Medium Road") ||
				name.Contains("Avenue Large With") ||
				name.Contains("Medium Avenue") ||
				//FourDevidedLaneAvenue4Parking
				//FourDevidedLaneAvenue2Bus
				name.Contains("FourDevidedLaneAvenue") ||
				name.Contains("AsymAvenueL2R4") ||
				name.Contains("AsymAvenueL2R3")
			) { style = ReturnStyleFromConfig(config.MediumRoads); }

			// Large Roads
			if (
				name.Contains("Large Road") ||
				name.Contains("Large Oneway") ||
				name.Contains("Eight-Lane Avenue")
			) { style = ReturnStyleFromConfig(config.LargeRoads); }

			// Wide Roads
			if (
				//WideAvenue6LBusCenterBike
				name.Contains("WideAvenue")
			) { style = ReturnStyleFromConfig(config.WideRoads); }

			// Highway
			if (
				name.Contains("Highway")
			) { style = ReturnStyleFromConfig(config.Highways); }

			// Pedestrian Roads
			if (name.Contains("Zonable Pedestrian"))
			{
				if (config.HidePedRoadsSignal) { style = style.none; }
				else { style = ReturnStyleFromConfig(config.PedestrianRoads); }
			}

			// Promenade
			if (name.Contains("Zonable Promenade"))
			{
				if (config.HidePromenadeSignal) { style = style.none; }
				else { style = ReturnStyleFromConfig(config.PedestrianRoads); }
			}

			// Monorail
			if (name.Contains("Monorail")) { if (config.Monorail) { style = ReturnStyleFromConfig(config.Global); } }

			// Global
			if (style == style.def) { style = ReturnStyleFromConfig(config.Global); }

			return style;
		}
	}
}
