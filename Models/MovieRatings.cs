//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MovieImdb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MovieRatings
    {
        public int Id { get; set; }
        public string User { get; set; }
        public int MovieId { get; set; }
        public int Rating { get; set; }
    }
}
