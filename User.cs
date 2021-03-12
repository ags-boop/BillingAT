using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BillingEnd.Models
{
    public partial class User
    {
        
        [Required(ErrorMessage = "UserName is required")]  
        public string UserName { get; set; }  
        [Required(ErrorMessage = "Password is required")]  
        [DataType(DataType.Password)]  
        public string Password { get; set; }  
    }
}
