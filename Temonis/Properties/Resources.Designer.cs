﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Temonis.Properties {
    using System;
    
    
    /// <summary>
    ///   ローカライズされた文字列などを検索するための、厳密に型指定されたリソース クラスです。
    /// </summary>
    // このクラスは StronglyTypedResourceBuilder クラスが ResGen
    // または Visual Studio のようなツールを使用して自動生成されました。
    // メンバーを追加または削除するには、.ResX ファイルを編集して、/str オプションと共に
    // ResGen を実行し直すか、または VS プロジェクトをビルドし直します。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   このクラスで使用されているキャッシュされた ResourceManager インスタンスを返します。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Temonis.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   すべてについて、現在のスレッドの CurrentUICulture プロパティをオーバーライドします
        ///   現在のスレッドの CurrentUICulture プロパティをオーバーライドします。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   http://www.kmoni.bosai.go.jp/webservice/hypo/eew/ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string EewUri {
            get {
                return ResourceManager.GetString("EewUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   https://www.data.jma.go.jp/developer/xml/feed/ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string JmxUri {
            get {
                return ResourceManager.GetString("JmxUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   http://www.kmoni.bosai.go.jp/data/map_img/ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string KyoshinUri {
            get {
                return ResourceManager.GetString("KyoshinUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   http://www.kmoni.bosai.go.jp/webservice/server/pros/latest.json に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string LatestTimeUri {
            get {
                return ResourceManager.GetString("LatestTimeUri", resourceCulture);
            }
        }
        
        /// <summary>
        ///   https://temonis.azurewebsites.net/ に類似しているローカライズされた文字列を検索します。
        /// </summary>
        internal static string TemonisUri {
            get {
                return ResourceManager.GetString("TemonisUri", resourceCulture);
            }
        }
    }
}
