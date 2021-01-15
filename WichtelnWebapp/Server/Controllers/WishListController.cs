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
            string sql = "select * from Wish WHERE Wish.TIMESTAMP >= cast(dateadd(day, -3, getdate()) as date);";

            return _db.LoadData<AccountModel, dynamic>(sql, new { });
        }

        // Get all users excluding id
        // GET: api/WishList/GetAllBut/{id}
        [HttpGet("GetAllBut/{id}")]
        public Task<List<AccountModel>> GetAll(int id)
        {
            string sql = @"select distinct Account.ACCOUNT_ID, Account.USERNAME
                            from Account JOIN Wish on FK_ACCOUNT_ID=Account.ACCOUNT_ID 
                            WHERE FK_ACCOUNT_ID!=@FK_ACCOUNT_ID AND Wish.TIMESTAMP >= cast(dateadd(day, -3, getdate()) as date);";
            return _db.LoadData<AccountModel, dynamic>(sql, new { FK_ACCOUNT_ID = id });
        }

        // Get All Wishes by user id
        // GET: api/WishList/GetAllByID/{id}
        [HttpGet("GetAllByID/{id}")]
        public Task<List<WishModel>> GetAllByID(int id)
        {
            string sql = @"SELECT WISH_ID, ITEM_TITLE, ITEM_DESCRIPTION, GRANTED, GRANTED_BY FROM Wish 
                            JOIN Account on Account.ACCOUNT_ID = Wish.FK_ACCOUNT_ID
                            WHERE Account.ACCOUNT_ID = @ACCOUNT_ID AND Wish.TIMESTAMP >= cast(dateadd(day, -3, getdate()) as date);";
            return _db.LoadData<WishModel, dynamic>(sql, new { ACCOUNT_ID = id });

        }

        // Get Wishes by wish id
        // GET: api/WishList/GetByID/{id}
        [HttpGet("GetByID/{id}")]
        public async Task<WishModel> GetByID(int id)
        {
            string sql = @"SELECT WISH_ID, ITEM_TITLE, ITEM_DESCRIPTION, GRANTED, GRANTED_BY FROM Wish
                            WHERE WISH_ID=@WISH_ID;";
            var wish_query = await _db.LoadData<WishModel, dynamic>(sql, new { WISH_ID = id });

            WishModel wish = wish_query.FirstOrDefault();

            return wish;

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
            await _db.SaveData(sqlCommand, new { FK_ACCOUNT_ID = id, ITEM_TITLE = wish.ITEM_TITLE, ITEM_DESCRIPTION = wish.ITEM_DESCRIPTION });
        }

        // Edit wish by Wish id
        // POST: api/WishList/EditWish/{id}
        [HttpPost("EditWish/{id}")]
        public async Task EditWish(int id, WishModel wish)
        {
            string sqlCommand = @"UPDATE Wish
                                  SET ITEM_TITLE = @ITEM_TITLE,
                                  ITEM_DESCRIPTION = @ITEM_DESCRIPTION
                                  WHERE WISH_ID=@WISH_ID;";
            await _db.SaveData(sqlCommand, new { WISH_ID = id, ITEM_TITLE = wish.ITEM_TITLE, ITEM_DESCRIPTION = wish.ITEM_DESCRIPTION });
        }

        // Edit wish by Wish id
        // POST: api/WishList/GrantWish/{id}
        [HttpPost("GrantWish/{id}")]
        public async Task GrantWish(int id, WishModel wish)
        {
            string sql_username = @"SELECT * FROM Account where ACCOUNT_ID=@ACCOUNT_ID";
            var acc_query = await _db.LoadData<AccountModel, dynamic>(sql_username, new { ACCOUNT_ID = id });
            AccountModel acc = acc_query.FirstOrDefault();
            string username = acc.USERNAME;

            string sqlCommand = @"UPDATE Wish
                                  SET GRANTED=1,
                                  GRANTED_BY=@GRANTED_BY
                                  WHERE WISH_ID=@WISH_ID;";
            await _db.SaveData(sqlCommand, new { WISH_ID = wish.WISH_ID, GRANTED_BY = username });
        }

        // Add Comment
        // POST: api/WishList/AddComment/{id}
        [HttpPost("AddComment/{id}")]
        public async Task AddComment(int id, CommentModel comment)
        {
            string sqlCommand = @"INSERT INTO Comment(COMMENTER, FK_ACCOUNT_ID, COMMENT)
                                   VALUES(@COMMENTER, @FK_ACCOUNT_ID, @COMMENT)";
            await _db.SaveData(sqlCommand, new { FK_ACCOUNT_ID = id, COMMENTER = comment.COMMENTER, COMMENT = comment.COMMENT });
        }


        // Get Comment By ID
        // GET: api/WishList/GetCommentByID/{id}
        [HttpGet("GetCommentByID/{id}")]
        public Task<List<CommentModel>> GetCommentByID(int id)
        {
            string sql = "select * from Comment WHERE FK_ACCOUNT_ID=@ACCOUNT_ID";
            return _db.LoadData<CommentModel, dynamic>(sql, new { ACCOUNT_ID = id });
        }
       
    }
}
