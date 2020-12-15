using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WichtelnWebapp.Shared;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;

namespace WichtelnWebapp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SqlController _db;
        public AccountController(SqlController db)
        {
            _db = db;
        }

        [HttpPut]
        public Task Put(AccountModel account)
        {
            account.PASSWORD = HashPassword(account.PASSWORD);
            string sqlCommand = @"INSERT INTO Account(EMAIL, USERNAME, NAME, SURNAME, PASSWORD)
                                          VALUES(@EMAIL, @USERNAME, @NAME, @SURNAME, @PASSWORD);";

            return _db.SaveData(sqlCommand, account);
        }


        [Http]

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

    }
}
