using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.AuthDtos
{
    public class ResetPasswordRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "OTP must be 6 digits")]
        public string Otp { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; }

    }
}
