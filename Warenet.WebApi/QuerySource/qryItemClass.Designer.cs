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
    internal class qryItemClass {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryItemClass() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryItemClass", typeof(qryItemClass).Assembly);
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
        ///   Looks up a localized string similar to spd_whic1.
        /// </summary>
        internal static string deleteItemClass {
            get {
                return ResourceManager.GetString("deleteItemClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_whic1.
        /// </summary>
        internal static string insertItemClass {
            get {
                return ResourceManager.GetString("insertItemClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM whic1 WHERE ItemClassCode = @ItemClassCode.
        /// </summary>
        internal static string selectItemClass {
            get {
                return ResourceManager.GetString("selectItemClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM whic1 WHERE ItemClassCode = @ItemClassCode.
        /// </summary>
        internal static string selectItemClassCount {
            get {
                return ResourceManager.GetString("selectItemClassCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_whic1.
        /// </summary>
        internal static string updateItemClass {
            get {
                return ResourceManager.GetString("updateItemClass", resourceCulture);
            }
        }
    }
}