﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Warenet.WebApi.QuerySource {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class qryGoodsIssueNote {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryGoodsIssueNote() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryGoodsIssueNote", typeof(qryGoodsIssueNote).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to spd_whgi1.
        /// </summary>
        internal static string deleteGin {
            get {
                return ResourceManager.GetString("deleteGin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM whgi2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string deleteGinDetail {
            get {
                return ResourceManager.GetString("deleteGinDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whgi1.
        /// </summary>
        internal static string insertGin {
            get {
                return ResourceManager.GetString("insertGin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whgi2.
        /// </summary>
        internal static string insertGinDetail {
            get {
                return ResourceManager.GetString("insertGinDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whgi1 WHERE TrxNo = @TrxNo.
        /// </summary>
        internal static string selectGin {
            get {
                return ResourceManager.GetString("selectGin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whgi2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string selectGinDetail {
            get {
                return ResourceManager.GetString("selectGinDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(LineItemNo) FROM whgi2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string selectGinDetailCount {
            get {
                return ResourceManager.GetString("selectGinDetailCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whgi2 WHERE TrxNo = @TrxNo.
        /// </summary>
        internal static string selectGinDetails {
            get {
                return ResourceManager.GetString("selectGinDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @GinMth varchar(20) = CONVERT(CHAR(4), @IssueDate, 12);
        ///DECLARE @LastSeqNo int;
        ///DECLARE @NewGinNo nvarchar(20);
        ///
        ///SELECT @LastSeqNo = ISNULL(MAX(CONVERT(INT,RIGHT(GoodsIssueNoteNo,5))),0)
        ///FROM whgi1
        ///WHERE SUBSTRING(GoodsIssueNoteNo,4,4) = CONVERT(CHAR(4), @IssueDate, 12)
        ///
        ///SET @LastSeqNo = @LastSeqNo + 1;
        ///SET @NewGinNo = &apos;GIN&apos; + @GinMth + RIGHT(&apos;00000&apos; + CONVERT(VARCHAR,@LastSeqNo),5);
        ///SELECT @NewGinNo.
        /// </summary>
        internal static string selectNewGinNo {
            get {
                return ResourceManager.GetString("selectNewGinNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whgi1.
        /// </summary>
        internal static string updateGin {
            get {
                return ResourceManager.GetString("updateGin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whgi2.
        /// </summary>
        internal static string updateGinDetail {
            get {
                return ResourceManager.GetString("updateGinDetail", resourceCulture);
            }
        }
    }
}
