
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SourceGenerator_MediatR_CQRS
{
    /// <summary>
    /// Interface that represents a query to the system
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuery<T> : IRequest<T>
    {
    }


    /// <summary>
    /// List all orders command
    /// </summary>
    /// <remarks>
    /// Send this query command to get a list of all the orders in the system
    /// </remarks>
    public record ListAllOrders : IQuery<List<Order>>
    {
    }


    /// <summary>
    /// List all orders for customer
    /// </summary>
    /// <remarks>
    /// Send this query command to get a list of all the orders in the system
    /// </remarks>
    public record ListAllOrdersForCustomer : IQuery<List<Order>>
    {

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        public int CustomerID { get; set; }
    }


    /// <summary>
    /// List all orders that have arrived today
    /// </summary>
    /// <remarks>
    /// Only returns todays finalized orders. Cancelled orders are not included.
    /// </remarks>
    public record ListTodaysOrders : IQuery<List<Order>>
    {

        /// <summary>
        /// CustomerID
        /// </summary>
        /// <example>1234</example>
        public int CustomerID { get; set; }
    }


    /// <summary>
    /// Returns a specific order
    /// </summary>
    /// <remarks>
    /// Send this query command to get a specific order
    /// </remarks>
    public record GetOrder : IQuery<Order>
    {
        /// <summary>
        /// OrderID
        /// </summary>
        /// <example>1234</example>
        public int OrderId { get; set; }
    }
}
