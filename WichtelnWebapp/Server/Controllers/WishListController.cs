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
    [Route("api/[controller]")]
    public class WishListController : ControllerBase
    {
        private readonly ISqlController _db;
        public WishListController(ISqlController db)
        {
            _db = db;
        }


        [HttpGet]
        public List<WishModel> GetWishList()
        {
            List<WishModel> wishlist = new List<WishModel>();

            wishlist.Add(new WishModel { WISH_ID = 1, ITEM = "Lego Starwars", DESCRIPTION = "Millenium Falcon" });

            return wishlist;
        }

        [HttpGet("GetByID/{id}")]
        public Task<List<WishModel>> GetByID(int id)
        {
            string sql = @"SELECT WISH_ID, ITEM_TITLE, ITEM_DESCRIPTION, GRANTED FROM Wish 
                            JOIN List on Wish.FK_LIST_ID = List.LIST_ID
                            JOIN Account on Account.ACCOUNT_ID = List.FK_ACCOUNT_ID
                            WHERE Account.ACCOUNT_ID = @ACCOUNT_ID;";
            return _db.LoadData<WishModel, dynamic>(sql, new { ACCOUNT_ID = id });

        }
    }
}
