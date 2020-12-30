using AccountRegistrationAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountRegistrationAPI.Controllers
{
    //this controller is using to be access using the  the JWT token
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
        //we ned to inject the user manager 
    {
        private UserManager<AccountDetail> _userManager;
        public UserProfileController(UserManager<AccountDetail> userManager)
        {
            _userManager = userManager;

        }
    //this web api method using to retrieve the email, full name and the email 
    [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //add the attribute authorize
        //Get:/api/Userprofile
        public async Task<Object> GetUserProfile() {
            string userId = User.Claims.First(c => c.Type == "AccountId").Value;
            var user = await _userManager.FindByIdAsync(userId);
            return new
            {
                user.FullName,
                user.GmailAccount,
                user.UserPassword
            };
        }
    }
}
