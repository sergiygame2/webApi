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
    public class RelatedPagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RelatedPagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RelatedPages
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RelatedPages.Include(r => r.FirstPage).Include(r => r.SecondPage);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RelatedPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.RowId == id);
            if (relatedPages == null)
            {
                return NotFound();
            }

            return View(relatedPages);
        }

        // GET: RelatedPages/Create
        public IActionResult Create()
        {
            ViewData["FirstPageId"] = new SelectList(_context.Page, "PageID", "Content");
            ViewData["SecondPageId"] = new SelectList(_context.Page, "PageID", "Content");
            return View();
        }

        // POST: RelatedPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RowId,FirstPageId,SecondPageId")] RelatedPages relatedPages)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relatedPages);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["FirstPageId"] = new SelectList(_context.Page, "PageID", "Content", relatedPages.FirstPageId);
            ViewData["SecondPageId"] = new SelectList(_context.Page, "PageID", "Content", relatedPages.SecondPageId);
            return View(relatedPages);
        }


        // GET: RelatedPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.RowId == id);
            if (relatedPages == null)
            {
                return NotFound();
            }
            ViewData["FirstPageId"] = new SelectList(_context.Page, "PageID", "Content", relatedPages.FirstPageId);
            ViewData["SecondPageId"] = new SelectList(_context.Page, "PageID", "Content", relatedPages.SecondPageId);
            return View(relatedPages);
        }

        // POST: RelatedPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RowId,FirstPageId,SecondPageId")] RelatedPages relatedPages)
        {
            if (id != relatedPages.RowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relatedPages);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelatedPagesExists(relatedPages.RowId))
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
            ViewData["FirstPageId"] = new SelectList(_context.Page, "PageID", "Content", relatedPages.FirstPageId);
            ViewData["SecondPageId"] = new SelectList(_context.Page, "PageID", "Content", relatedPages.SecondPageId);
            return View(relatedPages);
        }

        // GET: RelatedPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.RowId == id);
            if (relatedPages == null)
            {
                return NotFound();
            }

            return View(relatedPages);
        }

        // POST: RelatedPages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relatedPages = await _context.RelatedPages.SingleOrDefaultAsync(m => m.RowId == id);
            _context.RelatedPages.Remove(relatedPages);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool RelatedPagesExists(int id)
        {
            return _context.RelatedPages.Any(e => e.RowId == id);
        }
    }
}
