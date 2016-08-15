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
    public class NavLinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavLinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NavLinks
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NavLink.Include(n => n.Page);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NavLinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navLink = await _context.NavLink.SingleOrDefaultAsync(m => m.NLId == id);
            if (navLink == null)
            {
                return NotFound();
            }

            return View(navLink);
        }

        // GET: NavLinks/Create
        public IActionResult Create()
        {
            ViewData["PageId"] = new SelectList(_context.Page, "PageID", "Content");
            ViewData["ParentLinkId"] = new SelectList(_context.NavLink, "NLId", "NLId");

            return View();
        }

        // POST: NavLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NLId,NavLinkTitle,PageId,ParentLinkId,Position")] NavLink navLink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(navLink);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["PageId"] = new SelectList(_context.Page, "PageID", "Content", navLink.PageId);
            ViewData["ParentLinkId"] = new SelectList(_context.NavLink, "NLId", "NLId", navLink.ParentLinkId);
            return View(navLink);
        }

        // GET: NavLinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navLink = await _context.NavLink.SingleOrDefaultAsync(m => m.NLId == id);
            if (navLink == null)
            {
                return NotFound();
            }
            ViewData["PageId"] = new SelectList(_context.Page, "PageID", "Content", navLink.PageId);
            ViewData["ParentLinkId"] = new SelectList(_context.NavLink, "NLId", "NLId", navLink.ParentLinkId);
            return View(navLink);
        }

        // POST: NavLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NLId,NavLinkTitle,PageId,ParentLinkId,Position")] NavLink navLink)
        {
            if (id != navLink.NLId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navLink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavLinkExists(navLink.NLId))
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
            ViewData["PageId"] = new SelectList(_context.Page, "PageID", "Content", navLink.PageId);
            ViewData["ParentLinkId"] = new SelectList(_context.NavLink, "NLId", "NLId", navLink.ParentLinkId);
            return View(navLink);
        }

        // GET: NavLinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navLink = await _context.NavLink.SingleOrDefaultAsync(m => m.NLId == id);
            if (navLink == null)
            {
                return NotFound();
            }

            return View(navLink);
        }

        // POST: NavLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var navLink = await _context.NavLink.SingleOrDefaultAsync(m => m.NLId == id);
            _context.NavLink.Remove(navLink);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool NavLinkExists(int id)
        {
            return _context.NavLink.Any(e => e.NLId == id);
        }
    }
}
