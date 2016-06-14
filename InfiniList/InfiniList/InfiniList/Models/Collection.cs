using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfiniList.Models
{
    public class Collection
    {
        public int ID { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required]
        [StringLength(65)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Format")]
        public string Format { get; set; }

        [Display(Name = "Size")]
        public int Size { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<List> Lists { get; set; }



    }
}
