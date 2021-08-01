using System;

namespace SugarMonkey.Models.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public Nullable<int> Cellphone { get; set; }
        public string Email { get; set; }
        public int CredentialId { get; set; }
        public string ProfilePhotoPath { get; set; }
        public Nullable<bool> IsCustomer { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<bool> ISActive { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public string Password { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}