using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SourceGenerator_MediatR_CQRS
{
    /// <summary>
    /// This class will handle the queries that are sent to the system
    /// </summary>
    public class QueryHandlers : IRequestHandler<ListAllOrders, List<Order>>,
                                 IRequestHandler<ListAllOrdersForCustomer, List<Order>>,
                                 IRequestHandler<ListTodaysOrders, List<Order>>,
                                 IRequestHandler<GetOrder, Order>
    {
        public Task<List<Order>> Handle(ListAllOrders request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Order>() {
                new Order() { CustomerId = 1234, Id = 2201, OrderTotal = 9.95m, OrderLines = new List<OrderLine>() },
                new Order() { CustomerId = 1235, Id = 2201, OrderTotal = 29.95m, OrderLines = new List<OrderLine>() },
                new Order() { CustomerId = 1236, Id = 2201, OrderTotal = 1219.95m, OrderLines = new List<OrderLine>() }
            });
        }

        public Task<List<Order>> Handle(ListAllOrdersForCustomer request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Order>() {
                new Order() { CustomerId = 1235, Id = 2201, OrderTotal = 9.95m, OrderLines = new List<OrderLine>() },
                new Order() { CustomerId = 1236, Id = 2201, OrderTotal = 29.95m, OrderLines = new List<OrderLine>() },
                new Order() { CustomerId = 1237, Id = 2201, OrderTotal = 100.0m, OrderLines = new List<OrderLine>() }
            });
        }

        public Task<List<Order>> Handle(ListTodaysOrders request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<Order>() {
                new Order() { CustomerId = 1231, Id = 2201, OrderTotal = 92.95m, OrderLines = new List<OrderLine>() },
                new Order() { CustomerId = 1232, Id = 2201, OrderTotal = 591.95m, OrderLines = new List<OrderLine>() },
                new Order() { CustomerId = 1233, Id = 2201, OrderTotal = 192.95m, OrderLines = new List<OrderLine>() }
            });
        }

        public Task<Order> Handle(GetOrder request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Order()
            {
                CustomerId = 1234,
                Id = 2201,
                OrderTotal = 9.95m,
                OrderLines = new List<OrderLine>()
            });
        }
    }
}
