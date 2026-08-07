using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.AuthDtos
{
    public class RegisterRequestDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Full name must be at least 3 characters")]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression(@"^01[0125][0-9]{8}$", ErrorMessage = "Invalid parent phone number format")]
        public string ParentPhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Grade must be selected")]
        public int GradeId { get; set; }
    }
}
