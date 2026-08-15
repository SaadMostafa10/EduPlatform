using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Identity
{
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }
        public string? ParentPhoneNumber { get; set; }
        public int? GradeId { get; set; }
        public Grade Grade { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
