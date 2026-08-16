using Domain.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Common
{
    public class Grade
    {
        public int Id { get; set; }
        public string Name { get; set; }       
        public int Order { get; set; }  
        public ICollection<ApplicationUser> Students { get; set; } = new List<ApplicationUser>();

    }
}
