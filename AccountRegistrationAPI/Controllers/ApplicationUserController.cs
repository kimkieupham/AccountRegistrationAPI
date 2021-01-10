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


        //this method use to update and make change to the password as reset the password 
        [HttpPut] //this is the method for update or modify object 
        [Route("resetPassword")]
        public async Task <IActionResult> resetPassword(PasswordDetail modeldetail)
        {
            //create a var to check if the old password is match 
            var checkPass = await _context.AccountDetails.FirstOrDefaultAsync(e => e.UserPassword == modeldetail.OldPassword);
            var checkNamepass = await _context.AccountDetails.FirstOrDefaultAsync(c =>  c.UserPassword == modeldetail.OldPassword);
            
            //do the compare if the password of that username is existing inside the database then go head 
            if (checkNamepass != null) 
            {
                //if there is a user and password match inside the database msqll then go head make change to the checkword 
                checkNamepass.UserPassword = modeldetail.NewPassword;
                checkNamepass.ConfirmPassword = modeldetail.ConfirmNewPassword;
                await _context.SaveChangesAsync();


                return Ok(modeldetail);
            }
            else //otherwise just return bad request which identity it's wrong
            {
                return BadRequest();
            }
        } 

        //this method will be used to update or make change to the gmail or email 
        [HttpPut] //which is use to change the email as user required
        [Route("resetEGmail")]
        public async Task <IActionResult> ResetGmail(PasswordDetail modeldetail)//need to pass in the gmail and the new once
        {
            //create a thing to check the current password and gmail
            var checkUser = await _context.AccountDetails.FirstOrDefaultAsync(e => e.GmailAccount == modeldetail.CurrentGmail && e.UserPassword == modeldetail.OldPassword);
            if(checkUser != null)
            {
                //then update the new gmail 
                checkUser.GmailAccount = modeldetail.NewGmail;
                await _context.SaveChangesAsync();//after doing the update we need to save the change 
                return Ok(modeldetail);
            }
            else //in case,couldn't find the gmail and password
            {
                return BadRequest();
            }
        }
    }
}
