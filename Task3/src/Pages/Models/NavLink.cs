using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Models
{
    public class NavLink
    {
        [Key]
        public int NLId { get; set; }
        public string NavLinkTitle { get; set; }

        public int ParentLinkId { get; set; }

        public int PageId { get; set; }
        public Page Page { get; set; }
        public string Position { get; set; }

    }
}
