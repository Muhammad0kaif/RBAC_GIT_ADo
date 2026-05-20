using System;
using System.Collections.Generic;
using System.Text;

namespace PocoClasses
{
    public class Order
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
