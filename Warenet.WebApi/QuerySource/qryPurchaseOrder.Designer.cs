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
    internal class qryPurchaseOrder {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryPurchaseOrder() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryPurchaseOrder", typeof(qryPurchaseOrder).Assembly);
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
        ///   Looks up a localized string similar to spd_whpo1.
        /// </summary>
        internal static string deletePo {
            get {
                return ResourceManager.GetString("deletePo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM whpo2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string deletePoDetail {
            get {
                return ResourceManager.GetString("deletePoDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whpo1.
        /// </summary>
        internal static string insertPo {
            get {
                return ResourceManager.GetString("insertPo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whpo2.
        /// </summary>
        internal static string insertPoDetail {
            get {
                return ResourceManager.GetString("insertPoDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DECLARE @PoMth varchar(20) = CONVERT(CHAR(4), @PoDate, 12)
        ///DECLARE @LastSeqNo int;
        ///DECLARE @NewPoNo nvarchar(20);
        ///
        ///SELECT @LastSeqNo = ISNULL(MAX(CONVERT(INT,RIGHT(PurchaseOrderNo,5))),0)
        ///FROM whpo1
        ///WHERE SUBSTRING(PurchaseOrderNo,4,4) = CONVERT(CHAR(4), @PoDate, 12)
        ///
        ///SET @LastSeqNo = @LastSeqNo + 1;
        ///SET @NewPoNo = &apos;PON&apos; + @PoMth + RIGHT(&apos;00000&apos; + CONVERT(VARCHAR,@LastSeqNo),5);
        ///SELECT @NewPoNo.
        /// </summary>
        internal static string selectNewPoNo {
            get {
                return ResourceManager.GetString("selectNewPoNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whpo1 WHERE TrxNo = @TrxNo.
        /// </summary>
        internal static string selectPo {
            get {
                return ResourceManager.GetString("selectPo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whpo2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string selectPoDetail {
            get {
                return ResourceManager.GetString("selectPoDetail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(LineItemNo) FROM whpo2 WHERE TrxNo = @TrxNo AND LineItemNo = @LineItemNo.
        /// </summary>
        internal static string selectPoDetailCount {
            get {
                return ResourceManager.GetString("selectPoDetailCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whpo2 WHERE TrxNo = @TrxNo.
        /// </summary>
        internal static string selectPoDetails {
            get {
                return ResourceManager.GetString("selectPoDetails", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whpo1.
        /// </summary>
        internal static string updatePo {
            get {
                return ResourceManager.GetString("updatePo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whpo2.
        /// </summary>
        internal static string updatePoDetail {
            get {
                return ResourceManager.GetString("updatePoDetail", resourceCulture);
            }
        }
    }
}
