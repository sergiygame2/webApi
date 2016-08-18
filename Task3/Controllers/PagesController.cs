using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pages.Data;
using Pages.Models;

namespace Pages.Controllers
{
    [RequireHttps]
    public class PagesController : Controller
    {
        public readonly ApplicationDbContext _context;

        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pages
        public async Task<IActionResult> Index(string pageUrl, string pageTitle, string sortOrder, string currPage)
        {
            var pages = from p in _context.Page select p;

            int total = pages.Count();

            var pagesVM = new PagesViewModel();
            pagesVM.pagesAmount = total;
            pagesVM.currentPage = String.IsNullOrEmpty(currPage) ? 1 : Int32.Parse(currPage);
            ViewBag.PageParam = pagesVM.currentPage;

            //pagination
            pagesVM.numberOfObjectsPerPage = 10;
            double am = (pagesVM.pagesAmount / pagesVM.numberOfObjectsPerPage);
            if ((pagesVM.pagesAmount % pagesVM.numberOfObjectsPerPage) > 4 && pagesVM.pagesAmount > pagesVM.numberOfObjectsPerPage)
            {
                var amount = (int)am;
                pagesVM.pagesLinks = (int)(amount + 1);
            }
            else if (pagesVM.pagesAmount < pagesVM.numberOfObjectsPerPage)
                pagesVM.pagesLinks = 1;
            else
                pagesVM.pagesLinks = (int)am;


            var pagesDifference = (pagesVM.pagesAmount - pagesVM.numberOfObjectsPerPage) > 0 ? (pagesVM.pagesAmount - pagesVM.numberOfObjectsPerPage) : 0;

            if (!String.IsNullOrEmpty(currPage))
                pages = pages.Skip(pagesVM.numberOfObjectsPerPage * (pagesVM.currentPage - 1)).Take(pagesVM.numberOfObjectsPerPage);
            else
                pages = pages.Take(pagesVM.numberOfObjectsPerPage);


            if (!String.IsNullOrEmpty(pageUrl))
            {
                pages = pages.Where(p => p.UrlName.Contains(pageUrl));
            }

            if (!String.IsNullOrEmpty(pageTitle))
            {
                pages = pages.Where(p => p.Title.Contains(pageTitle));
            }




            ///SORTING START
            ViewBag.UrlSortParm = !String.IsNullOrEmpty(sortOrder) ? "url_desc" : ""; //default order is asc
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "descript_desc" : "Description";
            ViewBag.ContentSortParm = sortOrder == "Content" ? "content_desc" : "Content";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";


            switch (sortOrder)
            {
                case "url_desc":
                    pagesVM.pages = await pages.OrderByDescending(p => p.UrlName).ToListAsync();
                    break;
                case "Title":
                    pagesVM.pages = await pages.OrderBy(p => p.Title).ToListAsync();
                    break;
                case "title_desc":
                    pagesVM.pages = await pages.OrderByDescending(p => p.Title).ToListAsync();
                    break;
                case "Description":
                    pagesVM.pages = await pages.OrderBy(p => p.Description).ToListAsync();
                    break;
                case "descript_desc":
                    pagesVM.pages = await pages.OrderByDescending(p => p.Description).ToListAsync();
                    break;
                case "Content":
                    pagesVM.pages = await pages.OrderBy(p => p.Content).ToListAsync();
                    break;
                case "content_desc":
                    pagesVM.pages = await pages.OrderByDescending(p => p.Content).ToListAsync();
                    break;
                case "date":
                    pagesVM.pages = await pages.OrderBy(p => p.AddedDate).ToListAsync();
                    break;
                case "date_desc":
                    pagesVM.pages = await pages.OrderByDescending(p => p.AddedDate).ToListAsync();
                    break;
                default:
                    pagesVM.pages = await pages.OrderBy(p => p.UrlName).ToListAsync();
                    break;
            }
            ///END SORTING
            return View(pagesVM);
        }

        // GET: Pages/Custom/5
        public async Task<IActionResult> Custom(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page.SingleOrDefaultAsync(m => m.PageID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
        // GET: Pages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page.SingleOrDefaultAsync(m => m.PageID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        [HttpPost]
        public string CheckBoxRP(IEnumerable<Page> rp)
        {
            var list = new List<Page>();
            if (rp.Count(relp => relp.IsSelected) > 0)
            {
                foreach (var item in rp)
                {
                    if (item.IsSelected)
                        list.Add(item);
                }
                _context.RelatedPages.Add(new RelatedPages { FirstPageId = list.First().PageID, SecondPageId = list.Last().PageID });
                _context.SaveChanges();
                return "SUCCESS!";
            }
            return "FAIL";
        }

        // GET: Pages/Create
        public IActionResult Create()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> validateUrl([Bind("PageID,AddedDate,Content,Description,Title,UrlName")] Page pageRec)
        {
            var page = await _context.Page.SingleOrDefaultAsync(m => m.UrlName == pageRec.UrlName);

            if (page != null)
                return Json(data: $"Url {pageRec.UrlName} already exists.");
            return Json(data: true);
        }
        // POST: Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PageID,AddedDate,Content,Description,Title,UrlName")] Page page)
        {
            if (ModelState.IsValid)
            {
                _context.Add(page);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(page);
        }
        // GET: Pages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page.SingleOrDefaultAsync(m => m.PageID == id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

        // POST: Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PageID,AddedDate,Content,Description,Title,UrlName")] Page page)
        {
            if (id != page.PageID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(page);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PageExists(page.PageID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(page);
        }

        // GET: Pages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var page = await _context.Page.SingleOrDefaultAsync(m => m.PageID == id);
            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }

        // POST: Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var page = await _context.Page.SingleOrDefaultAsync(m => m.PageID == id);
            _context.Page.Remove(page);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PageExists(int id)
        {
            return _context.Page.Any(e => e.PageID == id);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
