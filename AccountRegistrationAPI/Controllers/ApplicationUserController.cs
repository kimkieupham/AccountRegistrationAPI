using AccountRegistrationAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private UserManager<AccountDetail> _userManager;
        private SignInManager<AccountDetail> _signInManager;
        private readonly ApplicationSetting _appSetting;

        //inside the contructor ,we will  need to pass the two classes as the parameter . This will help us to access the injected instance 
        public ApplicationUserController(UserManager<AccountDetail> userManager, SignInManager<AccountDetail> signInManager, IOptions<ApplicationSetting> appSetting)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSetting = appSetting.Value;
        }

        //this is trying to use another web api for the register account of the user from the empty controller not the existing one 
        [HttpPost]
        [Route("Register")]
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
                var result = await _userManager.CreateAsync(applicationUser);
                return Ok(result);
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        //This is use for the http of the log in 
        [HttpPost]
        [Route("Login")]
        // POST: api/AccountDetails
        public async Task<IActionResult> Login(LoginDetail model) //we use the ;ogin, which have the password and username as the parameter and name it as model
        {
            var user = await _userManager.FindByNameAsync(model.LoginUserName);//this is use to pass by and find the user name in the AccountDetails table DB Context 
                                                                               // ^^^ need to find a way to pass the value in and do the checking with the findByNameAsync vs Find vs FindByName

            //then we will do the compare to check if the value is matched with the one in the table, if it does then return 
            if (user != null && await _userManager.CheckPasswordAsync(user, model.LoginPassword))//need to check for checkpasswordAsync vs check
            {
                var tokenDescriptior = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim("AccountID",user.AccountID.ToString())
                    }),
                    //adding the expires of the time
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSetting.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                //then we need to have the token handler
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptior);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });

            }
            else
            {
                return BadRequest(new { message = "UserName or Password is incorrect." });//dispaly the error message if the user name and password is incorrect 
            }
        }
    }
}
