//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SugarMonkey.Models
{
    using System;
    
    public partial class STP_UpdateUser_Result
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public Nullable<int> Cellphone { get; set; }
        public string Email { get; set; }
        public int CredentialID { get; set; }
        public string ProfilePhotoPath { get; set; }
        public Nullable<bool> isCustomer { get; set; }
        public Nullable<bool> isAdmin { get; set; }
        public Nullable<bool> iSActive { get; set; }
        public Nullable<System.DateTime> lastLogin { get; set; }
        public string Password { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}