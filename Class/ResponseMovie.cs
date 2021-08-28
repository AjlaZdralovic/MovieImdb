using MovieImdb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieImdb.Class
{
    public class ResponseMovie
    {
        public string title { get; set; }
        public string description { get; set; }
        public DateTime releaseDate { get; set; }
        public string photoMap { get; set; }

        public List<Actor> actors { get; set; }

        public int rating { get; set; }


    }

    public class Result
    {
        public string profile_path { get; set; }
        public bool adult { get; set; }
        public int id { get; set; }
     
        public string name { get; set; }
        public double popularity { get; set; }
    }
}