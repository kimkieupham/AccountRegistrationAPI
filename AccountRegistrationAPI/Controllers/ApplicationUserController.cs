using AccountRegistrationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AccountRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
       // private UserManager<AccountDetail> _userManager;
       // private SignInManager<AccountDetail> _signInManager;
        private readonly ApplicationSetting _appSetting;
        private readonly AccountDetailContext _context;


        //inside the contructor ,we will  need to pass the two classes as the parameter . This will help us to access the injected instance 
        public ApplicationUserController(IOptions<ApplicationSetting> appSetting, AccountDetailContext context)
        {
           // _userManager = userManager;
          // _signInManager = signInManager;
            _appSetting = appSetting.Value;
            _context = context;

        }

        /*this is trying to use another web api for the register account of the user from the empty controller not the existing one 
        [HttpPost]
       // [Route("Register")]
        //Post: /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(AccountDetail model)
        {
            var applicationUser = new AccountDetail()
            {
                FullName = model.FullName,
                GmailAccount = model.GmailAccount,
                UserPassword = model.UserPassword,
                ConfirmPassword = model.ConfirmPassword,
            };
            try
            {
                var result = await CreateAsync(applicationUser);
                return Ok(result);
            }
            catch (Exception ex) {
                throw ex;
            }
        }*/

        //This is use for the http of the log in 
        [HttpPost]
        [Route("Login")]
        // POST: api/AccountDetails
        public async Task<IActionResult> Login(LoginDetail model) //we use the ;ogin, which have the password and username as the parameter and name it as model
        {
            //create the variable to store the fullname 
            var checkName =  await _context.AccountDetails.FirstOrDefaultAsync(c => c.FullName == model.LoginUserName);//select * from AccountDetails where FullName == to login user name 
            var checkPassword = await _context.AccountDetails.FirstOrDefaultAsync(c => c.FullName == model.LoginUserName && c.UserPassword == model.LoginPassword); // select * from AccountDetails where FullName == loin user name and userpassword == to login passwoed 
            //requirements
            //checking the usename matches in database
            if (checkName != null && checkPassword != null)
            {
                //if the username is found inside the table we can go head to check the password 
                //checking if the password matches in the database

                return Ok(model); //return okay to indicate htat both username and password are both correct 

            }
            else //otherwise, either password or username is incorrect then we go head return badrequest to indicate that username or password is incorrect 
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }
        }
    }
}
