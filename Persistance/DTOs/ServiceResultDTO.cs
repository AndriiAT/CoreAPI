using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.DTOs
{
    public class ServiceResultDTO<T>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public object Data { get; set; }
    }
}
