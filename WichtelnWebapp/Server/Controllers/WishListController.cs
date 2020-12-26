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


        // Get all wishes
        // GET: api/WishList/GetAll
        [HttpGet("GetAll")]
        public Task<List<AccountModel>> GetAll()
        {
            string sql = "select * from Wish";

            return _db.LoadData<AccountModel, dynamic>(sql, new { });
        }

        // Get Wish by user id
        // GET: api/WishList/GetByID/{id}
        [HttpGet("GetByID/{id}")]
        public Task<List<WishModel>> GetByID(int id)
        {
            string sql = @"SELECT WISH_ID, ITEM_TITLE, ITEM_DESCRIPTION, GRANTED FROM Wish 
                            JOIN Account on Account.ACCOUNT_ID = Wish.FK_ACCOUNT_ID
                            WHERE Account.ACCOUNT_ID = @ACCOUNT_ID;";
            return _db.LoadData<WishModel, dynamic>(sql, new { ACCOUNT_ID = id });

        }

        // Delete Wish by id
        // DELETE: api/WishList/Delete/{id}
        [HttpDelete("Delete/{id}")]
        public async Task Delete(int id)
        {
            string sqlCommand = "DELETE FROM Wish WHERE WISH_ID=@WISH_ID";
            await _db.SaveData(sqlCommand, new { WISH_ID = id });
        }

        // Add Wish by User id
        // POST: api/WishList/AddWish/{id}
        [HttpPost("AddWish/{id}")]
        public async Task AddWish(int id, WishModel wish)
        {
            string sqlCommand = @"INSERT INTO Wish(FK_ACCOUNT_ID, ITEM_TITLE, ITEM_DESCRIPTION)
                                          VALUES(@FK_ACCOUNT_ID, @ITEM_TITLE, @ITEM_DESCRIPTION);";
            await _db.SaveData(sqlCommand, new { FK_ACCOUNT_ID = id,ITEM_TITLE = wish.ITEM_TITLE, ITEM_DESCRIPTION = wish.ITEM_DESCRIPTION});
        }
    }
}
