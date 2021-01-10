using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountRegistrationAPI.Models
{
    public class PasswordDetail
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        //to save space we will contain thing to do the change to the gmail
        public string CurrentGmail { get; set; }
        public string NewGmail {get; set; }
        
    }
}
