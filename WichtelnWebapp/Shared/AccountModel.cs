using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WichtelnWebapp.Shared
{
    public class AccountModel
    {
        public int ACCOUNT_ID { get; set; }


        [Required(ErrorMessage = "E-Mail is Required")]
        [MinLength(5, ErrorMessage = "E-Mail too short (min. 5 Characters)")]
        [StringLength(50, ErrorMessage = "E-Mail too long")]
        [EmailAddress]
        public string EMAIL { get; set; }

        [Required(ErrorMessage = "Username is Required")]
        [MinLength(3, ErrorMessage = "Username too short (min. 3 Characters)")]
        [StringLength(20, ErrorMessage = "Username too long")]
        public string USERNAME { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        [MinLength(3, ErrorMessage = "Name too short (min. 3 Characters)")]
        [StringLength(20, ErrorMessage = "Name too long")]
        public string NAME { get; set; }

        [Required(ErrorMessage = "Surname is Required")]
        [MinLength(3, ErrorMessage = "Surname too short (min. 3 Characters)")]
        [StringLength(20, ErrorMessage = "Surname too long")]
        public string SURNAME { get; set; }

        [MinLength(5, ErrorMessage = "Password too short (min. 5 Characters)")]
        public string PASSWORD { get; set; }

    }
}
