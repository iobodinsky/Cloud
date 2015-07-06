//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cloud.Repositories.DataContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUsers_Storages = new HashSet<AspNetUsers_Storages>();
            this.DropboxUserTokens = new HashSet<DropboxUserToken>();
            this.GoogleDriveUserTokens = new HashSet<GoogleDriveUserToken>();
        }
    
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    
        public virtual ICollection<AspNetUsers_Storages> AspNetUsers_Storages { get; set; }
        public virtual ICollection<DropboxUserToken> DropboxUserTokens { get; set; }
        public virtual ICollection<GoogleDriveUserToken> GoogleDriveUserTokens { get; set; }
    }
}
