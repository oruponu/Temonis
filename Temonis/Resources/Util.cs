using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Temonis.Resources
{
    internal class Util
    {
        public static readonly Color Black = Color.FromArgb(26, 26, 36);

        public static readonly Color Gray = Color.FromArgb(116, 116, 116);

        public static readonly Color White = Color.FromArgb(226, 226, 226);

        public static readonly Color Red = Color.FromArgb(255, 40, 0);

        public static readonly Color Blue = Color.FromArgb(0, 65, 255);

        public static readonly Color Yellow = Color.FromArgb(250, 245, 0);

        public static readonly Color Purple = Color.FromArgb(200, 0, 255);

        /// <summary>
        /// GZIP圧縮されたリソースファイルを展開する
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        private static byte[] DecompressResource(byte[] buffer, int bufferSize)
        {
            var bytes = new byte[bufferSize];
            using (var originalMemoryStream = new MemoryStream(buffer))
            using (var zipStream = new GZipStream(originalMemoryStream, CompressionMode.Decompress))
            using (var decompressedMemoryStream = new MemoryStream())
            {
                while (true)
                {
                    var size = zipStream.Read(bytes, 0, bytes.Length);
                    if (size == 0) break;
                    decompressedMemoryStream.Write(bytes, 0, size);
                }
            }

            return bytes;
        }

        public class Kyoshin
        {
            /// <summary>
            /// 地図の種類
            /// </summary>
            public static ReadOnlyCollection<string> MapType { get; } = new ReadOnlyCollection<string>(new[]
            {
                "jma",
                "acmap",
                "vcmap",
                "dcmap",
                "rsp0125",
                "rsp0250",
                "rsp0500",
                "rsp1000",
                "rsp2000",
                "rsp4000"
            });

            /// <summary>
            /// リアルタイム震度の色
            /// </summary>
            public static ReadOnlyDictionary<Color, float> ColorMap { get; } = new ReadOnlyDictionary<Color, float>(new Dictionary<Color, float>
            {
                [Color.FromArgb(0, 0, 205)] = -3.0f,
                [Color.FromArgb(0, 7, 209)] = -2.9f,
                [Color.FromArgb(0, 14, 214)] = -2.8f,
                [Color.FromArgb(0, 21, 218)] = -2.7f,
                [Color.FromArgb(0, 28, 223)] = -2.6f,
                [Color.FromArgb(0, 36, 227)] = -2.5f,
                [Color.FromArgb(0, 43, 231)] = -2.4f,
                [Color.FromArgb(0, 50, 236)] = -2.3f,
                [Color.FromArgb(0, 57, 240)] = -2.2f,
                [Color.FromArgb(0, 64, 245)] = -2.1f,
                [Color.FromArgb(0, 72, 250)] = -2.0f,
                [Color.FromArgb(0, 85, 238)] = -1.9f,
                [Color.FromArgb(0, 99, 227)] = -1.8f,
                [Color.FromArgb(0, 112, 216)] = -1.7f,
                [Color.FromArgb(0, 126, 205)] = -1.6f,
                [Color.FromArgb(0, 140, 194)] = -1.5f,
                [Color.FromArgb(0, 153, 183)] = -1.4f,
                [Color.FromArgb(0, 167, 172)] = -1.3f,
                [Color.FromArgb(0, 180, 161)] = -1.2f,
                [Color.FromArgb(0, 194, 150)] = -1.1f,
                [Color.FromArgb(0, 208, 139)] = -1.0f,
                [Color.FromArgb(6, 212, 130)] = -0.9f,
                [Color.FromArgb(12, 216, 121)] = -0.8f,
                [Color.FromArgb(18, 220, 113)] = -0.7f,
                [Color.FromArgb(25, 224, 104)] = -0.6f,
                [Color.FromArgb(31, 228, 96)] = -0.5f,
                [Color.FromArgb(37, 233, 88)] = -0.4f,
                [Color.FromArgb(44, 237, 79)] = -0.3f,
                [Color.FromArgb(50, 241, 71)] = -0.2f,
                [Color.FromArgb(56, 245, 62)] = -0.1f,
                [Color.FromArgb(63, 250, 54)] = 0.0f,
                [Color.FromArgb(75, 250, 49)] = 0.1f,
                [Color.FromArgb(88, 250, 45)] = 0.2f,
                [Color.FromArgb(100, 251, 41)] = 0.3f,
                [Color.FromArgb(113, 251, 37)] = 0.4f,
                [Color.FromArgb(125, 252, 33)] = 0.5f,
                [Color.FromArgb(138, 252, 28)] = 0.6f,
                [Color.FromArgb(151, 253, 24)] = 0.7f,
                [Color.FromArgb(163, 253, 20)] = 0.8f,
                [Color.FromArgb(176, 254, 16)] = 0.9f,
                [Color.FromArgb(189, 255, 12)] = 1.0f,
                [Color.FromArgb(195, 254, 10)] = 1.1f,
                [Color.FromArgb(202, 254, 9)] = 1.2f,
                [Color.FromArgb(208, 254, 8)] = 1.3f,
                [Color.FromArgb(215, 254, 7)] = 1.4f,
                [Color.FromArgb(222, 255, 5)] = 1.5f,
                [Color.FromArgb(228, 254, 4)] = 1.6f,
                [Color.FromArgb(235, 255, 3)] = 1.7f,
                [Color.FromArgb(241, 254, 2)] = 1.8f,
                [Color.FromArgb(248, 255, 1)] = 1.9f,
                [Color.FromArgb(255, 255, 0)] = 2.0f,
                [Color.FromArgb(254, 251, 0)] = 2.1f,
                [Color.FromArgb(254, 248, 0)] = 2.2f,
                [Color.FromArgb(254, 244, 0)] = 2.3f,
                [Color.FromArgb(254, 241, 0)] = 2.4f,
                [Color.FromArgb(255, 238, 0)] = 2.5f,
                [Color.FromArgb(254, 234, 0)] = 2.6f,
                [Color.FromArgb(255, 231, 0)] = 2.7f,
                [Color.FromArgb(254, 227, 0)] = 2.8f,
                [Color.FromArgb(255, 224, 0)] = 2.9f,
                [Color.FromArgb(255, 221, 0)] = 3.0f,
                [Color.FromArgb(254, 213, 0)] = 3.1f,
                [Color.FromArgb(254, 205, 0)] = 3.2f,
                [Color.FromArgb(254, 197, 0)] = 3.3f,
                [Color.FromArgb(254, 190, 0)] = 3.4f,
                [Color.FromArgb(255, 182, 0)] = 3.5f,
                [Color.FromArgb(254, 174, 0)] = 3.6f,
                [Color.FromArgb(255, 167, 0)] = 3.7f,
                [Color.FromArgb(254, 159, 0)] = 3.8f,
                [Color.FromArgb(255, 151, 0)] = 3.9f,
                [Color.FromArgb(255, 144, 0)] = 4.0f,
                [Color.FromArgb(254, 136, 0)] = 4.1f,
                [Color.FromArgb(254, 128, 0)] = 4.2f,
                [Color.FromArgb(254, 121, 0)] = 4.3f,
                [Color.FromArgb(254, 113, 0)] = 4.4f,
                [Color.FromArgb(255, 106, 0)] = 4.5f,
                [Color.FromArgb(254, 98, 0)] = 4.6f,
                [Color.FromArgb(255, 90, 0)] = 4.7f,
                [Color.FromArgb(254, 83, 0)] = 4.8f,
                [Color.FromArgb(255, 75, 0)] = 4.9f,
                [Color.FromArgb(255, 68, 0)] = 5.0f,
                [Color.FromArgb(254, 61, 0)] = 5.1f,
                [Color.FromArgb(253, 54, 0)] = 5.2f,
                [Color.FromArgb(252, 47, 0)] = 5.3f,
                [Color.FromArgb(251, 40, 0)] = 5.4f,
                [Color.FromArgb(250, 33, 0)] = 5.5f,
                [Color.FromArgb(249, 27, 0)] = 5.6f,
                [Color.FromArgb(248, 20, 0)] = 5.7f,
                [Color.FromArgb(247, 13, 0)] = 5.8f,
                [Color.FromArgb(246, 6, 0)] = 5.9f,
                [Color.FromArgb(245, 0, 0)] = 6.0f,
                [Color.FromArgb(238, 0, 0)] = 6.1f,
                [Color.FromArgb(230, 0, 0)] = 6.2f,
                [Color.FromArgb(223, 0, 0)] = 6.3f,
                [Color.FromArgb(215, 0, 0)] = 6.4f,
                [Color.FromArgb(208, 0, 0)] = 6.5f,
                [Color.FromArgb(200, 0, 0)] = 6.6f,
                [Color.FromArgb(192, 0, 0)] = 6.7f,
                [Color.FromArgb(185, 0, 0)] = 6.8f,
                [Color.FromArgb(177, 0, 0)] = 6.9f
            });

            /// <summary>
            /// 観測点リスト
            /// </summary>
            public static readonly string[][] Stations = Encoding.UTF8.GetString(DecompressResource(Properties.Resources.Stations, 48141)).TrimEnd('\0').Split('\n').Select(x => x.Split(',')).ToArray();

            /// <summary>
            /// 強震モニタ
            /// </summary>
            public static readonly ColorMap[] MapRealTime =
            {
                new ColorMap
                {
                    OldColor = Color.FromArgb(0, 0, 0),
                    NewColor = Black
                }
            };

            /// <summary>
            /// 緊急地震速報 P波・S波到達予想円
            /// </summary>
            public static readonly ColorMap[] MapPSWave = 
            {
                new ColorMap
                {
                    OldColor = Color.FromArgb(0, 0, 255),
                    NewColor = Blue
                },
                new ColorMap
                {
                    OldColor = Color.FromArgb(255, 0, 0),
                    NewColor = Red
                }
            };
        }

        public class EqInfo
        {
            public static ReadOnlyDictionary<string, Color> ColorMap { get; } = new ReadOnlyDictionary<string, Color>(new Dictionary<string, Color>
            {
                ["震度1"] = Color.FromArgb(0, 140, 194),
                ["震度2"] = Color.FromArgb(31, 228, 96),
                ["震度3"] = Yellow,
                ["震度4"] = Color.FromArgb(252, 208, 0),
                ["震度5弱"] = Color.FromArgb(255, 170, 0),
                ["震度5強"] = Color.FromArgb(255, 105, 0),
                ["震度6弱"] = Red,
                ["震度6強"] = Color.FromArgb(165, 0, 33),
                ["震度7"] = Purple
            });

            public static ReadOnlyDictionary<string, string> CityAbbreviation { get; } = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>
            {
                ["渡島北斗市"] = "北斗市",
                ["渡島森町"] = "森町",
                ["渡島松前町"] = "松前町",
                ["檜山江差町"] = "江差町",
                ["上川中川町"] = "中川町",
                ["上川地方上川町"] = "上川町",
                ["宗谷枝幸町"] = "枝幸町",
                ["胆振伊達市"] = "伊達市",
                ["日高地方日高町"] = "日高町",
                ["十勝清水町"] = "清水町",
                ["十勝池田町"] = "池田町",
                ["十勝大樹町"] = "大樹町",
                ["青森南部町"] = "南部町",
                ["岩手洋野町"] = "洋野町",
                ["宮城加美町"] = "加美町",
                ["宮城美里町"] = "美里町",
                ["宮城川崎町"] = "川崎町",
                ["秋田美郷町"] = "美郷町",
                ["山形金山町"] = "金山町",
                ["山形朝日町"] = "朝日町",
                ["山形川西町"] = "川西町",
                ["山形小国町"] = "小国町",
                ["福島伊達市"] = "伊達市",
                ["福島広野町"] = "広野町",
                ["福島金山町"] = "金山町",
                ["福島昭和村"] = "昭和村",
                ["茨城古河市"] = "古河市",
                ["茨城鹿嶋市"] = "鹿嶋市",
                ["栃木さくら市"] = "さくら市",
                ["栃木那珂川町"] = "那珂川町",
                ["群馬高山村"] = "高山村",
                ["群馬昭和村"] = "昭和村",
                ["群馬上野村"] = "上野村",
                ["群馬南牧村"] = "南牧村",
                ["群馬明和町"] = "明和町",
                ["埼玉美里町"] = "美里町",
                ["埼玉神川町"] = "神川町",
                ["埼玉三芳町"] = "三芳町",
                ["千葉佐倉市"] = "佐倉市",
                ["東京府中市"] = "府中市",
                ["東京利島村"] = "利島村",
                ["神奈川大井町"] = "大井町",
                ["富山朝日町"] = "朝日町",
                ["福井坂井市"] = "坂井市",
                ["福井池田町"] = "池田町",
                ["山梨北杜市"] = "北杜市",
                ["山梨南部町"] = "南部町",
                ["長野池田町"] = "池田町",
                ["長野高山村"] = "高山村",
                ["長野川上村"] = "川上村",
                ["長野南牧村"] = "南牧村",
                ["長野高森町"] = "高森町",
                ["岐阜山県市"] = "山県市",
                ["岐阜池田町"] = "池田町",
                ["静岡清水町"] = "清水町",
                ["静岡森町"] = "森町",
                ["愛知津島市"] = "津島市",
                ["愛知江南市"] = "江南市",
                ["愛知みよし市"] = "みよし市",
                ["愛知美浜町"] = "美浜町",
                ["三重朝日町"] = "朝日町",
                ["三重明和町"] = "明和町",
                ["三重大紀町"] = "大紀町",
                ["三重紀北町"] = "紀北町",
                ["三重御浜町"] = "御浜町",
                ["滋賀日野町"] = "日野町",
                ["大阪堺市"] = "堺市",
                ["大阪和泉市"] = "和泉市",
                ["大阪岬町"] = "岬町",
                ["大阪太子町"] = "太子町",
                ["兵庫香美町"] = "香美町",
                ["兵庫稲美町"] = "稲美町",
                ["兵庫神河町"] = "神河町",
                ["兵庫太子町"] = "太子町",
                ["奈良川西町"] = "川西町",
                ["奈良川上村"] = "川上村",
                ["和歌山広川町"] = "広川町",
                ["和歌山美浜町"] = "美浜町",
                ["和歌山日高町"] = "日高町",
                ["和歌山印南町"] = "印南町",
                ["鳥取若桜町"] = "若桜町",
                ["鳥取南部町"] = "南部町",
                ["鳥取日野町"] = "日野町",
                ["島根美郷町"] = "美郷町",
                ["岡山美咲町"] = "美咲町",
                ["広島三次市"] = "三次市",
                ["広島府中市"] = "府中市",
                ["徳島三好市"] = "三好市",
                ["愛媛松前町"] = "松前町",
                ["愛媛鬼北町"] = "鬼北町",
                ["高知香南市"] = "香南市",
                ["高知津野町"] = "津野町",
                ["福岡古賀市"] = "古賀市",
                ["福岡那珂川町"] = "那珂川町",
                ["福岡川崎町"] = "川崎町",
                ["福岡広川町"] = "広川町",
                ["佐賀鹿島市"] = "鹿島市",
                ["長崎対馬市"] = "対馬市",
                ["熊本小国町"] = "小国町",
                ["熊本高森町"] = "高森町",
                ["熊本美里町"] = "美里町",
                ["宮崎都農町"] = "都農町",
                ["宮崎美郷町"] = "美郷町",
                ["鹿児島出水市"] = "出水市",
                ["鹿児島十島村"] = "十島村"
            });
        }
    }
}
