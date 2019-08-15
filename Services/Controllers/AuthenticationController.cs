using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Users.DBModel;
using Services.Users.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Users.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UsersContext UsersContext;
        public AuthenticationController(UsersContext UsersContext)
        {
            this.UsersContext = UsersContext;
        }

        private static readonly Random Random = new Random();

        [Route("auth/login"), HttpPost]
        public async Task<ActionResult> Login(AuthenticationLoginModel auth)
        {
            var user = await UsersContext.Users.FirstOrDefaultAsync(w => w.Password == auth.Password && w.Username == auth.Username);
            if (user is null) return Unauthorized();

            var token = (ulong)(((long)Random.Next() << 32) + Random.Next());
            UsersContext.Tokens.Add(new Token { User = user, TokenValue = token });
            await UsersContext.SaveChangesAsync();

            return Ok(new { token });
        }

        [Route("auth/check"), HttpGet]
        public async Task<ActionResult> Check(ulong token)
        {
            var user = (await UsersContext.Tokens.Include(w => w.User).FirstOrDefaultAsync(w => w.TokenValue == token))?.User;
            return user is null ? (ActionResult)Unauthorized() : Ok(new { username = user.Username });
        }
    }
}
