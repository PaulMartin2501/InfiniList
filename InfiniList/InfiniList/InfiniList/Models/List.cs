using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfiniList.Models
{
    public class List
    {
        public int ID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }

        [StringLength(100, MinimumLength = 0)]
        public string Notes { get; set; }

        public int CollectionID { get; set; }

        public virtual Collection Collection { get; set; }

        public string ListIdTitle
        {
            get
            {
                return ID + "; " + Title;
            }
        }
    }
}