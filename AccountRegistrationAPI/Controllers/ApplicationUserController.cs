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
        //This is use for the http of the log in 
        [HttpPost]
        [Route("Login")]
        // POST: api/AccountDetails
        public async Task<IActionResult> Login(LoginDetail model) //we use the ;ogin, which have the password and username as the parameter and name it as model
        {
            //create the variable to store the fullname 
            var checkName =  await _context.AccountDetails.FirstOrDefaultAsync(c => c.FullName == model.LoginUserName);//select * from AccountDetails where FullName == to login user name 
            var checkPassword = await _context.AccountDetails.FirstOrDefaultAsync(c => c.FullName == model.LoginUserName && c.UserPassword == model.LoginPassword); // select * from AccountDetails where FullName == loin user name and userpassword == to login passwoed 
            //this is the case trying if it's work for the quering in entity framework core with the Include 
           /* var checkLogin = _context.AccountDetails.Where(e => e.FullName == model.LoginUserName).Include(model.LoginPassword).FirstOrDefault();*/
            //requirements
            //checking the usename matches in database
            if (checkName != null && checkPassword != null)
            {
                //trying to create a new token and return a token from token description
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("AccountID", User.Identities.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567890123456")), SecurityAlgorithms.HmacSha256Signature)
                    //aaa.bbb.ccc this is signiture used to create the signature part you have to take the encoded header, paylead and the verifity signiture that used to verify the message wasn't changed along the way
                    //aa is header and bbb is payload and ccc is for signiture 
                };
                //this is use to handle the token that we create
                var tokenHandler = new JwtSecurityTokenHandler();//handling the new web token 
                var securityToken = tokenHandler.CreateToken(tokenDescriptor); //create the token from the previous creating with header,payload and signiture
                var token = tokenHandler.WriteToken(securityToken);//write the web token 
                //if the username is found inside the table we can go head to check the password 
                //checking if the password matches in the database

                return Ok( new { token }); //return okay to indicate htat both username and password are both correct with the new token value, which we can use to work with the Authentication Guard

            }
            else //otherwise, either password or username is incorrect then we go head return badrequest to indicate that username or password is incorrect 
            {
                return BadRequest(new { message = "UserName or Password is incorrect" });
            }
        }
    }
}
