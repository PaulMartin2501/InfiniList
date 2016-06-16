using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace InfiniList.Models
{
    public class ContactForm
    {
        [Required, Display(Name = "Name")]
        public string Name { get; set; }

        [Required, Display(Name = "Email"), EmailAddress]
        public string Email { get; set; }

        [Required, Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}