
using Microsoft.AspNet.Identity;
using MovieImdb.Class;
using MovieImdb.Models;
using Newtonsoft.Json;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MovieImdb.Controllers
{
    public class HomeController : Controller
    {

        private MoviesDBEntities _entity = new MoviesDBEntities();

       // [HttpGet]

        public ActionResult Index(string option, string search)
        {

            /*if(option == "title") {
                List<Movie> list = _entity.Movie.Where(s => s.title.StartsWith(search) || s.title.Contains(search)).ToList();
                return View(list.OrderByDescending(t => t.rating).ToList());
            }
            else if (option == "description")
            {
                List<Movie> list = _entity.Movie.Where(s => s.description.StartsWith(search)).ToList();
                return View(list.OrderByDescending(t => t.rating).ToList());
            }
            else 
            */
            List<Movie> list = _entity.Movie.Where(s => s.title.StartsWith(search) || s.title.Contains(search)
                                   || s.description.Contains(search)
                                   ).ToList();
            return View(_entity.Movie.OrderByDescending(t => t.rating).ToList());
        }



        public ActionResult AutoComplete(string term)
        {
            List<Movie> list = _entity.Movie.Where(s => s.title.StartsWith(term)).ToList();
            return View(list.OrderByDescending(t => t.rating).ToList());

        }



       /* public JsonResult AutoComplete(string term)
        {
            var result = (from r in _entity.Movie
                          where r.title.ToLower().Contains(term.ToLower())
                          select new { r.title }).Distinct();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        */

        [HttpGet]
        public ActionResult Edit(int ID)
        {
            Movie movie = new Movie();
            MoviesDBEntities entity = new MoviesDBEntities();
            Movie _movie = entity.Movie.Where(c => c.Id == ID).SingleOrDefault();
            return View(_movie);
        }


        [HttpPost]
        public ActionResult Edit(Movie movie)
        {
            
            MoviesDBEntities entity = new MoviesDBEntities();
            List<Movie> listOfMovis = entity.Movie.ToList<Movie>();

        
            string userName= (string.IsNullOrEmpty(User.Identity.GetUserId()) || string.IsNullOrWhiteSpace(User.Identity.GetUserId())) ? "Anonymous" : User.Identity.GetUserId();
            

            var qinsert = "INSERT into MovieRatings ([User],[MovieId],[Rating]) values ('" + userName+ "'," + movie.Id + "," +
                          movie.rating + ")";
 

            var query = "UPDATE [Movie] SET rating = (SELECT AVG(CAST(Rating as decimal(7, 2))) FROM [MovieRatings] " + 
                        " WHERE [MovieRatings].[MovieId] = " + movie.Id + ") where Id = " +movie.Id;

            try { 
            using (var context = new MoviesDBEntities())
            {
                context.Database.ExecuteSqlCommand(qinsert);
                context.Database.ExecuteSqlCommand(query);

            }

            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.GetBaseException().Message;
            }
            var movieCh = listOfMovis.Where(s => s.Id == movie.Id).FirstOrDefault();

            listOfMovis.Remove(movieCh);
            listOfMovis.Add(movie);

            return RedirectToAction("Index");
            
          
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Movie movie)
        {
            MoviesDBEntities entity = new MoviesDBEntities();
            List<Movie> listOfMovis = entity.Movie.ToList<Movie>();
            listOfMovis.Add(movie);

            var qinsert = "INSERT into Movie ([title],[description],[releaseDate],[rating],[releaseMap]) values ('" + movie.title + "','" +
                           movie.description + "',"+
                           " TRY_CONVERT(datetime,'" + movie.releaseDate + "'),"+ movie.rating + ",'" + movie.releaseMap + "')";

            try { 
            using (var context = new MoviesDBEntities())
            {
                context.Database.ExecuteSqlCommand(qinsert);
               
            }
            }catch(Exception ex)
            {

                ViewBag.Error = ex.GetBaseException().Message;
            }


            return RedirectToAction("Index");
        }

        // <summary>
        /// Sort and paginate records from the database
        /// </summary>
        /// <param name="sortOn">field to sort on field</param>
        /// <param name="orderBy">order by ascending or descending</param>
        /// <param name="pSortOn">previous sorted on field</param>
        /// <param name="page">page number to show</param>
        /// <returns></returns>
        public ActionResult PagingAndSorting(string sortOn, string orderBy, string pSortOn, string keyword, int? page)
        {
            int recordsPerPage = 5;
            if (!page.HasValue)
            {
                page = 1; // set initial page value
                if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                {
                    orderBy = "desc";
                }
                else
                {
                    orderBy = "asc";
                }
            }
            
            if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,StringComparison.CurrentCultureIgnoreCase))
            {
                orderBy = "asc";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SortOn = sortOn;
            ViewBag.Keyword = keyword;

            var list = _entity.Movie.AsQueryable();

            switch (sortOn)
            {
                case "releaseDate":
                    if (orderBy.Equals("desc"))
                    {
                        list = list.OrderByDescending(p => p.releaseDate);
                    }
                    else
                    {
                        list = list.OrderBy(p => p.releaseDate);
                    }
                    break;
                case "title":
                    if (orderBy.Equals("desc"))
                    {
                        list = list.OrderByDescending(p => p.title);
                    }
                    else
                    {
                        list = list.OrderBy(p => p.title);
                    }
                    break;
                case "description":
                    if (orderBy.Equals("desc"))
                    {
                        list = list.OrderByDescending(p => p.description);
                    }
                    else
                    {
                        list = list.OrderBy(p => p.description);
                    }
                    break;
                default:
                    list = list.OrderBy(p => p.rating);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                list = list.Where(f => f.title.StartsWith(keyword));
            }

            var finalList = list.ToPagedList(page.Value, recordsPerPage);
            return View(finalList);
        }



    }
}