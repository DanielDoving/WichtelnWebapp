using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WichtelnWebapp.Shared
{
    public class WishModel
    {
        public int WISH_ID { get; set; }

        public int FK_USER_ID { get; set; }

        [Required(ErrorMessage = "Title is Required")]
        public String ITEM_TITLE { get; set; }
        public String ITEM_DESCRIPTION { get; set; }
        public Boolean GRANTED { get; set; }
        public String GRANTED_BY { get; set; }

    }
}
