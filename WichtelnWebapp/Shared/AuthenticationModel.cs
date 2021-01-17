using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WichtelnWebapp.Shared
{
    public class AuthenticationModel
    {
        [Required(ErrorMessage = "Email/Username is Required")]
        public string EMAIL { get; set; }
        [Required(ErrorMessage = "Password is Required")]
        public string PASSWORD { get; set; }
    }
}
