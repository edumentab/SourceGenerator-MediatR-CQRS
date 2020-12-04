using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceGenerator_MediatR_CQRS
{
    /// <summary>
    /// Interface that represents a command to the system
    /// </summary>
    public interface ICommand<T> : IRequest<T>
    {
    }


    /// <summary>
    /// Create a new order
    /// </summary>
    /// <remarks>
    /// Send this command to create a new order in the system for a given customer
    /// </remarks>
    public record CreateOrder : ICommand<string>
    {
        /// <summary>
        /// OrderId
        /// </summary>
        /// <remarks>This is the customers internal ID of the order.</remarks>      
        /// <example>123</example> 
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public int CustomerId { get; set; }
    }


    /// <summary>
    /// Add a new product to existing order
    /// </summary>
    /// <remarks>
    /// Send this command to add a new product to existing order. The order must still be open for this command to be accepted.
    /// </remarks>
    public record AddProduct : ICommand<string>
    {
        /// <summary>
        /// The Order ID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// The Product ID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// The Quantity of products to add
        /// </summary> 
        /// <example>10</example>
        [Required]
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Remove product from order
    /// </summary>
    /// <remarks>
    /// Removes the product from the provided order. The order must still be open for this command to be accepted.
    /// </remarks>
    public record RemoveProduct : ICommand<string>
    {
        /// <summary>
        /// The Order ID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// The Product ID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public int ProductId { get; set; }
    }

    /// <summary>
    /// Cancels an order
    /// </summary>
    /// <remarks>
    /// This command will cancel the order. The order must still be open for this command to be accepted.
    /// </remarks>
    public record CancelOrder : ICommand<string>
    {
        /// <summary>
        /// The Order ID
        /// </summary>
        /// <example>1234</example>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// The reason for why this order is cancelled
        /// </summary>
        /// <example>1234</example>
        [Required]
        public string Reason { get; set; }

        /// <summary>
        /// Who cancelled this order?
        /// </summary>
        /// <example>Tore Nestenius</example>
        [Required]
        public string CancelledBy { get; set; }
    }
}