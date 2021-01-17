using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WichtelnWebapp.Shared;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net;

namespace WichtelnWebapp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ISqlController _db;
        public AccountController(ISqlController db)
        {
            _db = db;
        }

        // Add Account
        // POST: api/Account/AddAccount
        [HttpPost("AddAccount")]
        public async Task Create(AccountModel account)
        {
            string sql = "select * from Account where EMAIL=@EMAIL OR USERNAME=@USERNAME;";

            var query = await _db.LoadData<AccountModel, dynamic>(sql, new { EMAIL = account.EMAIL, USERNAME = account.USERNAME });
            
            if(query.Count != 0)
            {
                throw new Exception();
                return;
            }


            account.PASSWORD = HashPassword(account.PASSWORD);
            string sqlCommand = @"INSERT INTO Account(EMAIL, USERNAME, NAME, SURNAME, PASSWORD)
                                          VALUES(@EMAIL, @USERNAME, @NAME, @SURNAME, @PASSWORD);";
            await _db.SaveData(sqlCommand, account);
        }

        // Get all users
        // GET: api/Account/GetAll
        [HttpGet("GetAll")]
        public Task<List<AccountModel>> GetAll()
        {
            string sql = "select * from Account";

            return _db.LoadData<AccountModel, dynamic>(sql, new { });
        }

        // Get User by ID
        // GET: api/Account/GetByID/{id}
        [HttpGet("GetByID/{id}")]
        public async Task<AccountModel> GetByID(int id)
        {
            string sql = "select * from Account where ACCOUNT_ID=@ACCOUNT_ID";
            var user_query = await _db.LoadData<AccountModel, dynamic>(sql, new {ACCOUNT_ID = id});
            AccountModel account = user_query.FirstOrDefault();
            return account;
        }


        // Authenticate User
        // GET: api/Account/Authenticate
        [HttpGet("Authenticate/{EMAIL}/{PASSWORD}")]
        public async Task<AccountModel> Authenticate(string EMAIL, string PASSWORD)
        {
            if (string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(PASSWORD))
                return new AccountModel();


            string sql = "select * from Account where EMAIL=@EMAIL OR USERNAME=@EMAIL;";

            var query = await _db.LoadData<AccountModel, dynamic>(sql, new { EMAIL});
           
            if(query != null)
            {
                AccountModel account = query.FirstOrDefault();
                if(account == null)
                {
                    return new AccountModel();
                }

                if(VerifyHashedPassword(account.PASSWORD, PASSWORD))
                {
                    return account;
                }
            }
            return new AccountModel();

        }

        // Edit account by id
        // POST: api/Account/EditAccount/{id}
        [HttpPost("EditAccount/{id}")]
        public async Task EditAccount(int id, AccountModel account)
        {
            
            string sqlCommand = @"UPDATE Account
                                  SET EMAIL = @EMAIL,
                                  USERNAME = @USERNAME,
                                  NAME = @NAME,
                                  SURNAME = @SURNAME
                                  WHERE ACCOUNT_ID=@ACCOUNT_ID;";
            await _db.SaveData(sqlCommand, new { ACCOUNT_ID = id, EMAIL = account.EMAIL, USERNAME = account.USERNAME, NAME = account.NAME, SURNAME = account.SURNAME});
        }


        // Edit update password by id
        // POST: api/Account/EditAccount/{id}
        [HttpPost("EditPassword/{id}")]
        public async Task EditPassword(int id, AccountModel account)
        {
            account.PASSWORD = HashPassword(account.PASSWORD);
            string sqlCommand = @"UPDATE Account
                                  SET PASSWORD = @PASSWORD
                                  WHERE ACCOUNT_ID=@ACCOUNT_ID;";
            await _db.SaveData(sqlCommand, new { ACCOUNT_ID = id, PASSWORD = account.PASSWORD});
        }

        private string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] salt;
            byte[] subkey;
            int SaltSize = 16;
            int PBKDF2SubkeyLength = 32;
            int PBKDF2IterCount = 5;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, SaltSize, PBKDF2IterCount))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + SaltSize + PBKDF2SubkeyLength];
            Array.Copy(salt, 0, outputBytes, 1, SaltSize);
            Array.Copy(subkey, 0, outputBytes, 1 + SaltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException("hashedPassword");
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            int SaltSize = 16;
            int PBKDF2SubkeyLength = 32;
            int PBKDF2IterCount = 5;

            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);


            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
            {
                return false;
            }

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashedPasswordBytes, 1, salt, 0, SaltSize);
            byte[] storedSubkey = new byte[PBKDF2SubkeyLength];
            Array.Copy(hashedPasswordBytes, 1 + SaltSize, storedSubkey, 0, PBKDF2SubkeyLength);

            byte[] generatedSubkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, PBKDF2IterCount))
            {
                generatedSubkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            return storedSubkey.SequenceEqual(generatedSubkey);
        }

    }
}
