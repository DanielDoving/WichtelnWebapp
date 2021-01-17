using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WichtelnWebapp.Shared
{
    public class CommentModel
    {
        [Required(ErrorMessage = "Comment is Required")]
        public string COMMENT { get; set; }

        [Required]
        public string COMMENTER { get; set; }

        public string TIMESTAMP { get; set; }

    }
}