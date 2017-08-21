// source code from
// https://github.com/gansaku/CSLMapView
// by Gansaku

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JapaneseTrafficLights {
    /// <summary>
    /// 多言語化対応
    /// </summary>
    internal class Globalization {
        internal const string DEFAULT_LANGUAGE = "en";
        private readonly string[] supportedLanguages = new[] { "en", "ja" };
        private Dictionary<string, Dictionary<StringKeys, string>> strings;
        /// <summary>
        /// 文字列のキー
        /// </summary>
        internal enum StringKeys {
			/// <summary>
			/// ヘッダー
			/// </summary>
			HeaderText,
			/// <summary>
			/// スタイル
			/// </summary>
			StyleWhite,
			StyleBrown,
			StyleBrown2,
			/// <summary>
			/// グローバル
			/// </summary>
			GlobalText,
			/// <summary>
			/// 道路種別
			/// </summary>
			TinyRoadsText,
			SmallRoadsText,
			SmallHeavyRoadsText,
			MediumRoadsText,
			LargeRoadsText,
			WideRoadsText,
			HighwaysText,
			PedestrianRoadsText,
			BusText,
			MonorailText,
			GrassText,
			TreesText,
			/// <summary>
			/// オプションテキスト
			/// </summary>
			OptionStyleText,
			OptionEnableText,
			OptionPedRoadsText,
			OptionPromenadeText
		}
        /// <summary>
        /// 現在の言語
        /// </summary>
        internal string Language { get; set; } = DEFAULT_LANGUAGE;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        internal Globalization() {
            strings = new Dictionary<string, Dictionary<StringKeys, string>>();

            strings.Add( "en", new Dictionary<StringKeys, string>() );
            initEn( strings["en"] );
            strings.Add( "ja", new Dictionary<StringKeys, string>() );
            initJa( strings["ja"] );
        }
        private void initJa( Dictionary<StringKeys, string> dic ) {
			dic.Add(StringKeys.HeaderText, "日本風信号機MODオプション - 変更はセーブデータの再読込後に適応されます。");
			dic.Add(StringKeys.OptionStyleText, "スタイル");
			dic.Add(StringKeys.StyleWhite, "デフォルト");
			dic.Add(StringKeys.StyleBrown, "茶色");
			dic.Add(StringKeys.StyleBrown2, "茶色(街灯無し)");
			dic.Add(StringKeys.GlobalText, "グローバル(新しい道路に適応されます)");
			dic.Add(StringKeys.TinyRoadsText, "Tiny Roads");
			dic.Add(StringKeys.SmallRoadsText, "小さい道路");
			dic.Add(StringKeys.SmallHeavyRoadsText, "Small Heavy Roads");
			dic.Add(StringKeys.MediumRoadsText, "中くらいの道路");
			dic.Add(StringKeys.LargeRoadsText, "広い道路");
			dic.Add(StringKeys.WideRoadsText, "Wide Roads");
			dic.Add(StringKeys.HighwaysText, "高速道路");
			dic.Add(StringKeys.PedestrianRoadsText, "Pedestrian Roads");
			dic.Add(StringKeys.OptionEnableText, "有効にする");
			dic.Add(StringKeys.OptionPedRoadsText, "Pedestrian Roadsの信号機を非表示にする");
			dic.Add(StringKeys.OptionPromenadeText, "Promenadeの信号機を非表示にする");
			dic.Add(StringKeys.BusText, "[Beta]バスレーンのある道路(いくつかの道路を除く)");
			dic.Add(StringKeys.MonorailText, "[Beta]モノレール軌道のある道路");
			dic.Add(StringKeys.GrassText, "[Beta]芝生のある道路(分離帯付きの中道路を除く)");
			dic.Add(StringKeys.TreesText, "[Beta]街路樹のある道路(分離帯付きの中道路を除く)");
		}
        private void initEn( Dictionary<StringKeys, string> dic ) {
            dic.Add( StringKeys.HeaderText, "Japanese Traffic Lights Options - Changes will apply after reload savedata.");
			dic.Add( StringKeys.OptionStyleText, "Style");
			dic.Add(StringKeys.StyleWhite, "Default");
			dic.Add(StringKeys.StyleBrown, "Brown");
			dic.Add(StringKeys.StyleBrown2, "Brown(without Street Light)");
			dic.Add( StringKeys.GlobalText, "Global(It applies to new roads)");
			dic.Add( StringKeys.TinyRoadsText, "Tiny Roads");
			dic.Add( StringKeys.SmallRoadsText, "Small Roads");
			dic.Add( StringKeys.SmallHeavyRoadsText, "Small Heavy Roads");
			dic.Add( StringKeys.MediumRoadsText, "Medium Roads");
			dic.Add( StringKeys.LargeRoadsText, "Large Roads");
			dic.Add( StringKeys.WideRoadsText, "Wide Roads");
			dic.Add( StringKeys.HighwaysText, "Highways");
			dic.Add( StringKeys.PedestrianRoadsText, "Pedestrian Roads");
			dic.Add( StringKeys.OptionEnableText, "Enable");
			dic.Add( StringKeys.OptionPedRoadsText, "Hide signals on Pedestrian Roads");
			dic.Add( StringKeys.OptionPromenadeText, "Hide signals on Promenade");
			dic.Add( StringKeys.BusText, "[Beta]Road with Bus Lanes(except some roads)");
			dic.Add( StringKeys.MonorailText, "[Beta]Road with Monorail Track");
			dic.Add( StringKeys.GrassText, "[Beta]Road with Grass(except for Medium Roads with median)");
			dic.Add( StringKeys.TreesText, "[Beta]Road with Trees(except for Medium Roads with median)");
		}
        /// <summary>
        /// デフォルト言語での文字列を取得します。
        /// </summary>
        /// <param name="key">文字列キー</param>
        /// <returns></returns>
        internal string GetString( StringKeys key ) {
            return GetString( Language, key );
        }

        /// <summary>
        /// 指定した言語での文字列を取得します。
        /// </summary>
        /// <param name="lang">言語(en,ja)</param>
        /// <param name="key">文字列キー</param>
        /// <returns></returns>
        internal string GetString( string lang, StringKeys key ) {
            if( !supportedLanguages.Contains( lang ) ) {
                lang = DEFAULT_LANGUAGE;
            }
            Dictionary<StringKeys, string> dic = strings[lang];
            if( dic.ContainsKey( key ) ) {
                return dic[key];
            } else {
                if( lang == DEFAULT_LANGUAGE ) {
                    return null;
                }
                return GetString( DEFAULT_LANGUAGE, key );
            }
        }
    }
}
