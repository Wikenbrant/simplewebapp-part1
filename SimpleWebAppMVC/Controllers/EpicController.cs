using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleWebAppMVC.Data;

namespace SimpleWebAppMVC.Controllers
{
    public class EpicController : Controller
    {
        private readonly AppDbContext dbContext;

        public EpicController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: EpicController
        public async Task<ActionResult> Index(string sort)
        {
            ViewBag.TitleSortParm = (sort == "Title" ? "Title_desc" : "Title");
            ViewBag.DescriptionSortParm = (sort == "Description" ? "Description_desc" : "Description");
            ViewBag.DateSortParm = (sort == "Date" ? "Date_desc" : "Date");

            ViewData["sortJSON"] = sort;

            return View(await this.GetSorted(sort).ToListAsync());
        }

        // GET: EpicController/Details/5
        public async Task<ActionResult> Details(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return NotFound();

            var epicModel = await this.dbContext.Epics.SingleOrDefaultAsync(task => task.Title == title);

            if (epicModel == null)
                return NotFound();

            return View(epicModel);
        }


        private IQueryable<Models.Epic> GetSorted(string sort)
        {
            IQueryable<Models.Epic> tasks = this.dbContext.Epics;

            tasks = sort switch
            {
                "Title" => tasks.OrderBy(s => s.Title),
                "Title_desc" => tasks.OrderByDescending(s => s.Title),
                "Date" => tasks.OrderBy(s => s.Date),
                "Date_desc" => tasks.OrderByDescending(s => s.Date),
                _ => tasks.OrderBy(s => s.Title),
            };

            return tasks;
        }
    }
}
