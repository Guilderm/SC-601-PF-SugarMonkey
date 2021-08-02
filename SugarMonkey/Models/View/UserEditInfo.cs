using System.ComponentModel.DataAnnotations;

namespace SugarMonkey.Models.View
{
    public class UserEditInfo
    {
        [Required] [Display(Name = "Nombre")] public string FirstName { get; set; }

        [Required]
        [Display(Name = "Primer Apellido")]
        public string FirstLastName { get; set; }

        [Display(Name = "Segundo Apellido")] public string SecondLastName { get; set; }

        [Display(Name = "Numero Celular")] public int Cellphone { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$",
            ErrorMessage = "E-mail id is not valid")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}