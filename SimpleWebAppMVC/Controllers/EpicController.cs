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

        /**
        * GET: /EpicController/Create
        */
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Models.Epic());
        }

        /**
         * POST: /EpicController/Create
         * http://go.microsoft.com/fwlink/?LinkId=317598
         * @param epicModel Epic model
         */
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, Title, Date, Stories")] Models.Epic epicModel)
        {
            if (ModelState.IsValid)
            {
                this.dbContext.Add(epicModel);
                await this.dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(epicModel);
        }

        /**
         * GET: /EpicController/Delete/<id>
         * @param id Epic Id
         */
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var epicModel = await this.dbContext.Epics.SingleOrDefaultAsync(epic => epic.Id == id);

            if (epicModel == null)
                return NotFound();

            return View(epicModel);
        }

        /**
        * POST: /EpicController/Delete/<id>
        * @param id Task Id
        */
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var epicModel = await this.dbContext.Epics.SingleOrDefaultAsync(epic => epic.Id == id);

            this.dbContext.Epics.Remove(epicModel);
            await this.dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /**
        * GET: /EpicController/Edit/<id>
        * @param id Epic Id
        */
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var epicModel = await this.dbContext.Epics.SingleOrDefaultAsync(epic => epic.Id == id);

            if (epicModel == null)
                return NotFound();

            return View(epicModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id, Title, Date, Stories")] Models.Epic epicModel)
        {
            if (id != epicModel.Id)
                return NotFound();

            if (!this.dbContext.Epics.Any(t => t.Id == id))
                return NotFound();

            if (ModelState.IsValid)
            {
                this.dbContext.Update(epicModel);
                await this.dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

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
