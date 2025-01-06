using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Persistance.DTOs.Enums;

namespace Persistance.DTOs
{
    public class ApplicationUserDTO
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Address { get; set; }
    }
}
