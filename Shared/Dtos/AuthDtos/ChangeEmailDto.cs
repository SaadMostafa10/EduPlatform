using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.AuthDtos
{
    public class ChangeEmailDto
    {
        [Required(ErrorMessage = "New email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string NewEmail { get; set; } = null!;

        [Required(ErrorMessage = "Current password is required to confirm identity.")]
        public string CurrentPassword { get; set; } = null!;
    }
}
