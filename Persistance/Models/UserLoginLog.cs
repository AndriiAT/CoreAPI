using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models
{
    internal class UserLoginLog
    {
        public string LogId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
