using System;
using System.Collections.Generic;

namespace SourceGenerator_MediatR_CQRS
{
    public class Order
    {
        /// <summary>
        /// The Order ID
        /// </summary>
        /// <example>1234</example>
        public int Id { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        public int CustomerId { get; set; }


        /// <summary>
        /// OrderLines
        /// </summary>
        public List<OrderLine> OrderLines { get; set; }


        /// <summary>
        /// The total value of this order
        /// </summary> 
        /// <remarks>This is the total value exclusive VAT</remarks>
        /// <example>995.95</example>
        public Decimal OrderTotal { get; set; }
    }
}
