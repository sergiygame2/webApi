using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Pages.Models
{
    public class PagesViewModel
    {
        public List<Page> pages;
        public string pageTitle { get; set; }

        public IEnumerable<Page> checkdeItems { get; set; }

        public int numberOfObjectsPerPage { get; set; }
        public int pagesAmount { get; set; }
        public int currentPage { get; set; }
        public int pagesLinks { get; set; }
    }

    public class Page
    {
        public int PageID { get; set; }

        //Custom validator [Remote(action: "validateUrl", controller: "Pages")]

        [Required]
        [Remote(action: "validateUrl", controller: "Pages")]
        public string UrlName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }

        public bool IsSelected { get; set; }

        public virtual List<NavLink> Links { get; set; }
    } 
}
