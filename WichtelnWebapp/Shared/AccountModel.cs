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


        //[Required]
        //[StringLength(50, ErrorMessage = "E-Mail too long")]
        public string EMAIL { get; set; }

        //[Required]
        //[StringLength(20, ErrorMessage = "Username too long")]
        public string USERNAME { get; set; }

        //[Required]
        //[StringLength(20, ErrorMessage = "Name too long")]
        public string NAME { get; set; }

        //[Required]
        //[StringLength(20, ErrorMessage = "Surname too long")]
        public string SURNAME { get; set; }

        //[Required]
        //[StringLength(50, ErrorMessage = "Password too long")]
        public string PASSWORD { get; set; }

    }
}
