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
    internal class qryUser {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal qryUser() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Warenet.WebApi.QuerySource.qryUser", typeof(qryUser).Assembly);
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
        ///   Looks up a localized string similar to spd_saus1.
        /// </summary>
        internal static string deleteUser {
            get {
                return ResourceManager.GetString("deleteUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spi_saus1.
        /// </summary>
        internal static string insertUser {
            get {
                return ResourceManager.GetString("insertUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT * FROM saus1 WHERE UserId = @UserId.
        /// </summary>
        internal static string selectUser {
            get {
                return ResourceManager.GetString("selectUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM saus1 WHERE UserId = @UserId.
        /// </summary>
        internal static string selectUserCount {
            get {
                return ResourceManager.GetString("selectUserCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT TOP 1 UserRoleName FROM saur1 WHERE UserRoleId = @UserRoleId.
        /// </summary>
        internal static string selectUserRoleName {
            get {
                return ResourceManager.GetString("selectUserRoleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to spu_saus1.
        /// </summary>
        internal static string updateUser {
            get {
                return ResourceManager.GetString("updateUser", resourceCulture);
            }
        }
    }
}