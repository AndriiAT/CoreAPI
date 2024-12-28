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
    public static class Enums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum OrderStatus
        {
            [EnumMember(Value = "Placed")]
            Placed = 0,
            [EnumMember(Value = "Pending")]
            Pending = 1,
            [EnumMember(Value = "Updated")]
            Updated = 2,
            [EnumMember(Value = "Completed")]
            Completed = 3,
            [EnumMember(Value = "Canceled")]
            Cancelled = 4
        }
    }

    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<OrderedProductDTO> OrderedProducts { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime AddDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
