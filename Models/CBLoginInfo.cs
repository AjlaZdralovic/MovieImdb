//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieImdb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CBLoginInfo
    {
        public int CustomerId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<int> LoginFailedCount { get; set; }
        public string LoginIPAddress { get; set; }
        public string CustomerTimeZone { get; set; }
        public Nullable<System.DateTime> LastAccessedDate { get; set; }
        public Nullable<bool> AccountLocked { get; set; }
    }
}
