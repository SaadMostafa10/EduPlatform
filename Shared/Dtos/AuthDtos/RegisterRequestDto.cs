using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.AuthDtos
{
    public class RegisterRequestDto
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ParentPhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int GradeId { get; set; }
    }
}
