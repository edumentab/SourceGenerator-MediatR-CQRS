using System;

namespace SourceGenerator_MediatR_CQRS
{
    public class OrderLine
    {
        /// <summary>
        /// The OrderLine Id
        /// </summary>
        /// <example>1001</example>
        public int Id { get; set; }

        /// <summary>
        /// The Order ID
        /// </summary>
        /// <example>1234</example>
        public int OrderId { get; set; }

        /// <summary>
        /// The Product ID
        /// </summary>
        /// <example>2048</example>
        public int ProductId { get; set; }

        /// <summary>
        /// The Quantity of products
        /// </summary> 
        /// <example>10</example>
        public int Quantity { get; set; }

        /// <summary>
        /// The Quantiy of products
        /// </summary> 
        /// <example>9.95</example>
        public Decimal Price { get; set; }
    }
}
