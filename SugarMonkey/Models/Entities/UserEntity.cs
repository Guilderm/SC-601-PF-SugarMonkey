using System;

namespace SugarMonkey.Models.Entities
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public int? Cellphone { get; set; }
        public string Email { get; set; }
        public int CredentialId { get; set; }
        public string ProfilePhotoPath { get; set; }
        public bool? IsCustomer { get; set; }
        public bool? IsAdmin { get; set; }
        public bool? ISActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Password { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}