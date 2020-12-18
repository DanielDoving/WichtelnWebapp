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
            account.PASSWORD = HashPassword(account.PASSWORD);
            string sqlCommand = @"INSERT INTO Account(EMAIL, USERNAME, NAME, SURNAME, PASSWORD)
                                          VALUES(@EMAIL, @USERNAME, @NAME, @SURNAME, @PASSWORD);";
            await _db.SaveData(sqlCommand, account);
        }

        // Get all users
        // GET: api/Accounts/GetAll
        [HttpGet("GetAll")]
        public Task<List<AccountModel>> GetAll()
        {
            string sql = "select * from Account";

            return _db.LoadData<AccountModel, dynamic>(sql, new { });
        }

        // Get User by ID
        // GET: api/Accounts/GetByID/{id}
        [HttpGet("GetByID/{id}")]
        public Task<List<AccountModel>> GetByID(int id)
        {
            string sql = "select * from Account where ACCOUNT_ID=@ACCOUNT_ID";
            return _db.LoadData<AccountModel, dynamic>(sql, new {ACCOUNT_ID = id});
        }


        // Authenticate User
        // GET: api/Accounts/Authenticate
        [HttpGet("Authenticate/{EMAIL}/{PASSWORD}")]
        public async Task<AccountModel> Authenticate(string EMAIL, string PASSWORD)
        {
            if (string.IsNullOrEmpty(EMAIL) || string.IsNullOrEmpty(PASSWORD))
                return null;


            string sql = "select * from Account where EMAIL=@EMAIL";

            var query = await _db.LoadData<AccountModel, dynamic>(sql, new { EMAIL});
           
            if(query != null)
            {
                AccountModel account = query.FirstOrDefault();
                if(VerifyHashedPassword(account.PASSWORD, PASSWORD))
                {
                    return account;
                }
            }
            return null;

        }

                



        private string HashPassword(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            // Produce a version 0 (see comment above) password hash.
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

            // Verify a version 0 (see comment above) password hash.

            if (hashedPasswordBytes.Length != (1 + SaltSize + PBKDF2SubkeyLength) || hashedPasswordBytes[0] != 0x00)
            {
                // Wrong length or version header.
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
