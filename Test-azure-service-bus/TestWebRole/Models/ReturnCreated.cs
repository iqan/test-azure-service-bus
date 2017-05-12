using System;
using System.ComponentModel.DataAnnotations;

namespace TestWebRole.Models
{
    public enum ReturnMethod
    {
        DropOff = 0,
        Collection = 1
    }
    public class ReturnCreated
    {
        public string Id { get; set; }
        [Required]
        public string ReturnReference { get; set; }
        [Required]
        public string OrderReference { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        public int ReturnMethod { get; set; }
        public DateTime Timestamp { get; set; }
    }
}