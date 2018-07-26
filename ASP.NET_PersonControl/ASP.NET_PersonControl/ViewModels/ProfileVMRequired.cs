using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.ViewModels
{
    public class ProfileVMRequired
    {
       
        [Required(ErrorMessage = "Please enter your Name.")]
        [StringLength(50)]
        public String DisplayName { get; set; }
       
        [Required(ErrorMessage = "Please enter valid email.")]
        [EmailAddress]
        public String Email { get; set; }
        
        [Required(ErrorMessage = "Please enter valid phone number.")]
        [Phone]
        public String PhoneNumber { get; set; }
       
        [Required(ErrorMessage = "Please enter your Country.")]
        public String Country { get; set; }
        
        [Required(ErrorMessage = "Please enter your City.")]
        public String City { get; set; }
        
        [Required(ErrorMessage = "Please enter your Adress.")]
        public String Address { get; set; }
    }

}