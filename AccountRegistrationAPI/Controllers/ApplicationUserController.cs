using AccountRegistrationAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountRegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private UserManager<AccountDetail> _userManager;
        private SignInManager<AccountDetail> _signInManager;

        public ApplicationUserController()
        {
                        
        }
    }


    //This is use for the httpost of the log in 
    [HttpPost]
    [Route("Login")]
    // POST: api/AccountDetails
    public async Task<IActionResult> Login(LoginDetail model) //we use the ;ogin, which have the password and username as the parameter and name it as model
    {
        var user = await _context.AccountDetails.FindByNameAsync(model.LoginUserName);//this is use to pass by and find the user name in the AccountDetails table DB Context 
                                                                                      // ^^^ need to find a way to pass the value in and do the checking with the findByNameAsync vs Find vs FindByName

        //then we will do the compare to check if the value is matched with the one in the table, if it does then return 
        if (user != null && await _context.CheckPasswordAsync(user, model.LoginPassword))//need to check for checkpasswordAsync vs check
        {
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                        new Claim("UserID",user.Id.ToString())
                }),
                //adding the expires of the time
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901223456")), SecurityAlgorithms.HmacSha256Signature)
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
