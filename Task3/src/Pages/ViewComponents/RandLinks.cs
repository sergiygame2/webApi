using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pages.Models;
using Pages.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Pages.ViewComponents
{
    public class RandLinks : ViewComponent
    {
        private ApplicationDbContext db;

        public RandLinks(ApplicationDbContext context)
        {
            db = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await GetItemsAsync();
            return View(items);
        }

        private Task<List<Page>> GetItemsAsync()
        {
            return db.Page.OrderBy(link => Guid.NewGuid()).Take(3).ToListAsync();
        }
    }
}
