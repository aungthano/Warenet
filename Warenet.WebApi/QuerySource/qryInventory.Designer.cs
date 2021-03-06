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
    internal class qryInventory {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryInventory() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryInventory", typeof(qryInventory).Assembly);
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
        ///   Looks up a localized string similar to DELETE FROM whiv1 WHERE BatchNo = @BatchNo.
        /// </summary>
        internal static string deleteByBatch {
            get {
                return ResourceManager.GetString("deleteByBatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spd_whiv1.
        /// </summary>
        internal static string deleteInv {
            get {
                return ResourceManager.GetString("deleteInv", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM whiv1 WHERE BatchNo = @BatchNo AND BatchLineItemNo = @BatchLineItemNo AND WarehouseCode = @WarehouseCode.
        /// </summary>
        internal static string deleteInvByBatch {
            get {
                return ResourceManager.GetString("deleteInvByBatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whiv1.
        /// </summary>
        internal static string insertInv {
            get {
                return ResourceManager.GetString("insertInv", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whiv1 WHERE TrxNo = @TrxNo.
        /// </summary>
        internal static string selectInv {
            get {
                return ResourceManager.GetString("selectInv", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whiv1 WHERE BatchNo = @BatchNo AND BatchLineItemNo = @BatchLineItemNo.
        /// </summary>
        internal static string selectInvByBatchLiNo {
            get {
                return ResourceManager.GetString("selectInvByBatchLiNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whiv1 WHERE BatchNo = @BatchNo.
        /// </summary>
        internal static string selectInvByBatchNo {
            get {
                return ResourceManager.GetString("selectInvByBatchNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM whiv1 WHERE BatchRefNo = @BatchRefNo.
        /// </summary>
        internal static string selectInvRefnoCnt {
            get {
                return ResourceManager.GetString("selectInvRefnoCnt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT ISNULL(SUM(SpaceArea),0) FROM whiv1 WHERE WarehouseCode = @WarehouseCode AND BinNo = @BinNo.
        /// </summary>
        internal static string selectInvStoreSpaceByBin {
            get {
                return ResourceManager.GetString("selectInvStoreSpaceByBin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whiv1 WHERE WarehouseCode = @WarehouseCode AND BinNo = @BinNo.
        /// </summary>
        internal static string selectItemsByBinNo {
            get {
                return ResourceManager.GetString("selectItemsByBinNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT iv1.* FROM whiv1 iv1
        ///	INNER JOIN whgr1 gr1 ON gr1.GoodsReceiptNoteNo = iv1.BatchNo
        ///WHERE iv1.StatusCode = &apos;USE&apos; AND iv1.WarehouseCode = @WarehouseCode AND gr1.SupplierCode = @SupplierCode.
        /// </summary>
        internal static string selectItemsByWhSupplier {
            get {
                return ResourceManager.GetString("selectItemsByWhSupplier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT TOP 1 TrxNo FROM whiv1 WHERE BatchNo = @BatchNo AND BatchLineItemNo = @BatchLineItemNo.
        /// </summary>
        internal static string selectTrxNoByBatchLiNo {
            get {
                return ResourceManager.GetString("selectTrxNoByBatchLiNo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whiv1.
        /// </summary>
        internal static string updateInv {
            get {
                return ResourceManager.GetString("updateInv", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE whiv1 
        ///SET WarehouseCode = @WarehouseCode,
        ///	StatusCode = @StatusCode,
        ///	UpdateBy = @UpdateBy,
        ///	UpdateDateTime = GETDATE()
        ///WHERE BatchNo = @BatchNo.
        /// </summary>
        internal static string updateWhCodeByBatch {
            get {
                return ResourceManager.GetString("updateWhCodeByBatch", resourceCulture);
            }
        }
    }
}
