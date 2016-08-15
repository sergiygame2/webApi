using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pages.Models
{
    public class RelatedPages
    {
        [Key]
        public int RowId { get; set; }

        public int FirstPageId { get; set; }
        public Page FirstPage { get; set; }

        public int SecondPageId { get; set; }
        public Page SecondPage { get; set; }

    }
}
