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
    public class RP: ViewComponent
    {
        private ApplicationDbContext db;

        public RP(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int pageId)
        {
            var items = await GetItemsAsync(pageId);
            return View(items);
        }   

        private Task<List<Page>> GetItemsAsync(int pageId)
        {
            var list = new List<Page>();

            var res1 = db.RelatedPages.Where(rp => rp.FirstPageId == pageId).Select(rp => rp.SecondPageId);
            var res2 = db.RelatedPages.Where(rp => rp.SecondPageId == pageId).Select(rp => rp.FirstPageId);
            foreach (var item in res1.Union(res2))
            {
                list.Add(db.Page.Single(p => p.PageID == item));
            }

            return Task<List<Page>>.Run(()=>list);
        }
    }
}
