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
    internal class qryPickList {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryPickList() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryPickList", typeof(qryPickList).Assembly);
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
        ///   Looks up a localized string similar to spd_whpl1.
        /// </summary>
        internal static string deletePickList {
            get {
                return ResourceManager.GetString("deletePickList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM whpl2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string deletePickListDetail {
            get {
                return ResourceManager.GetString("deletePickListDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whpl1.
        /// </summary>
        internal static string insertPickList {
            get {
                return ResourceManager.GetString("insertPickList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whpl2.
        /// </summary>
        internal static string insertPickListDetail {
            get {
                return ResourceManager.GetString("insertPickListDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @PickMth varchar(20) = CONVERT(CHAR(4), @PickDate, 12)
        ///DECLARE @LastSeqNo int;
        ///DECLARE @NewPickNo nvarchar(20);
        ///
        ///SELECT @LastSeqNo = ISNULL(MAX(CONVERT(INT,RIGHT(PickNo,5))),0)
        ///FROM whpl1 
        ///WHERE SUBSTRING(PickNo,4,4) = CONVERT(CHAR(4), @PickDate, 12)
        ///
        ///SET @LastSeqNo = @LastSeqNo + 1;
        ///SET @NewPickNo = &apos;PIN&apos; + @PickMth + RIGHT(&apos;00000&apos; + CONVERT(VARCHAR,@LastSeqNo),5);
        ///SELECT @NewPickNo.
        /// </summary>
        internal static string selectNewPickNo {
            get {
                return ResourceManager.GetString("selectNewPickNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whpl1 WHERE TrxNo = @TrxNo.
        /// </summary>
        internal static string selectPickList {
            get {
                return ResourceManager.GetString("selectPickList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whpl2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string selectPickListDetail {
            get {
                return ResourceManager.GetString("selectPickListDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM whpl2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string selectPickListDetailCnt {
            get {
                return ResourceManager.GetString("selectPickListDetailCnt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whpl2 WHERE TrxNo = @TrxNo ORDER BY LineItemNo.
        /// </summary>
        internal static string selectPickListDetails {
            get {
                return ResourceManager.GetString("selectPickListDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whpl1.
        /// </summary>
        internal static string updatePickList {
            get {
                return ResourceManager.GetString("updatePickList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whpl2.
        /// </summary>
        internal static string updatePickListDetail {
            get {
                return ResourceManager.GetString("updatePickListDetail", resourceCulture);
            }
        }
    }
}
