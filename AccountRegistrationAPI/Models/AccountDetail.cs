using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AccountRegistrationAPI.Models
{
    public class AccountDetail
    {
        [Key]
        public int AccountID { get; set; }

        [Column(TypeName="nvarchar(100)")]
        public string FullName { get; set; }


        [Column(TypeName ="nvarchar(100)")]
        public string GmailAccount { get; set; }


        [Column(TypeName ="nvarchar(100)")]
        public string UserPassword { get; set; }


        [Column(TypeName ="nvarchar(100)")]
        public string ConfirmPassword { get; set; }
    }
}
