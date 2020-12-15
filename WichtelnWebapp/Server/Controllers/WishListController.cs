using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WichtelnWebapp.Shared;

namespace WichtelnWebapp.Server.Controllers
{
    [ApiController]
    public class WishListController : ControllerBase
    {
        [HttpGet]
        [Route("GetWishList")]
        public List<WishModel> GetWishList()
        {
            List<WishModel> wishlist = new List<WishModel>();

            wishlist.Add(new WishModel { WISH_ID = 1, ITEM = "Lego Starwars", DESCRIPTION = "Millenium Falcon" });

            return wishlist;
        }
    }
}
