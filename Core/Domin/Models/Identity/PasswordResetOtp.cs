using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Identity
{
    public class PasswordResetOtp
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public bool IsUsed { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsValid => !IsUsed && !IsExpired;

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
