//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebAppEka.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Logins
    {
        public int LoginId { get; set; }
        [Required(ErrorMessage = "Anna käyttäjätunnus!")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Anna salasana!")]
        public string PassWord { get; set; }
        public string LoginErrorMessage { get; set; }
    }
}
