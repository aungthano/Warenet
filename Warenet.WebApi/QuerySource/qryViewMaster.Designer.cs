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
    internal class qryViewMaster {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryViewMaster() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryViewMaster", typeof(qryViewMaster).Assembly);
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
        ///   Looks up a localized string similar to ;WITH vc AS (
        ///    SELECT	ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS TrxNo,
        ///			v1.ViewName,
        ///			vd.SplitData AS ColumnName 
        ///	FROM cmvd1 v1
        ///		CROSS APPLY fnSplitString(v1.ViewContent,&apos;,&apos;) vd
        ///	WHERE v1.ViewName = @ViewName
        ///)
        ///SELECT c1.[name] AS ColumnName, 150 AS ColumnWidth
        ///FROM sys.columns c1
        ///WHERE	c1.object_id = OBJECT_ID(@ViewName) AND 
        ///		&apos;[&apos; + c1.[name] + &apos;]&apos; NOT IN (SELECT ColumnName FROM vc).
        /// </summary>
        internal static string selectAvaliableColDef {
            get {
                return ResourceManager.GetString("selectAvaliableColDef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM cmvd1 WHERE ViewName = @ViewName AND UserId = @UserId.
        /// </summary>
        internal static string selectColDefCnt {
            get {
                return ResourceManager.GetString("selectColDefCnt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT &apos;[&apos; + [name] + &apos;]&apos; AS ColumnName, 150 AS ColumnWidth FROM sys.columns WHERE object_id = OBJECT_ID(@ViewName).
        /// </summary>
        internal static string selectDefaultColDef {
            get {
                return ResourceManager.GetString("selectDefaultColDef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT column_id AS TrxNo,&apos;[&apos; + [name] + &apos;]&apos; AS ColumnName, 150 AS ColumnWidth FROM sys.columns WHERE object_id = OBJECT_ID(@ViewName)
        ///UNION
        ///SELECT 0 AS TrxNo,&apos;[@ViewKey]&apos; AS ColumnName, 0 AS ColumnWidth
        ///ORDER BY TrxNo.
        /// </summary>
        internal static string selectDefaultColumns {
            get {
                return ResourceManager.GetString("selectDefaultColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;WITH vc AS (
        ///    SELECT	ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS TrxNo,
        ///			v1.ViewName,
        ///			vd.SplitData AS ColumnName 
        ///	FROM cmvd1 v1
        ///		CROSS APPLY fnSplitString(v1.ViewContent,&apos;,&apos;) vd
        ///	WHERE v1.ViewName = @ViewName AND v1.UserId = @UserId
        ///),
        ///vd AS (
        ///    SELECT	ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS TrxNo,
        ///			vd.SplitData AS ColumnWidth 
        ///	FROM cmvd1 v1
        ///		CROSS APPLY fnSplitString(v1.ViewWidth,&apos;,&apos;) vd
        ///	WHERE v1.ViewName = @ViewName AND v1.UserId = @UserId
        ///)
        ///SELECT vc.ColumnName, vd.Colu [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string selectViewColDef {
            get {
                return ResourceManager.GetString("selectViewColDef", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;WITH vc AS (
        ///    SELECT	ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS TrxNo,
        ///			v1.ViewName,
        ///			vd.SplitData AS ColumnName 
        ///	FROM cmvd1 v1
        ///		CROSS APPLY fnSplitString(v1.ViewContent,&apos;,&apos;) vd
        ///	WHERE v1.ViewName = @ViewName AND v1.UserId = @UserId
        ///),
        ///vd AS (
        ///    SELECT	ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS TrxNo,
        ///			vd.SplitData AS ColumnWidth from cmvd1 v1
        ///		CROSS APPLY fnSplitString(v1.ViewWidth,&apos;,&apos;) vd
        ///	WHERE v1.ViewName = @ViewName AND v1.UserId = @UserId
        ///)
        ///SELECT vc.TrxNo,vc.ColumnName, v [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string selectViewColumns {
            get {
                return ResourceManager.GetString("selectViewColumns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT TOP 1 * FROM {0} WHERE {1} = @KeyValue.
        /// </summary>
        internal static string selectViewRow {
            get {
                return ResourceManager.GetString("selectViewRow", resourceCulture);
            }
        }
    }
}
