using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace IsolatedByInheritanceAndOverride.Test
{
    /// <summary>
    /// OrderServiceTest 的摘要描述
    /// </summary>
    [TestFixture]
    public class OrderServiceTest
    {
        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            var orderService = new TestBookOrderServiec();
            orderService.SyncBookOrders();
            var book = NSubstitute.Substitute.For<BookDao>();
            book.Received(2);
            // hard to isolate dependency to unit test
            //var books = new TestBookOrderServiec();
            //var target = new OrderService();
            //target.SyncBookOrders();
        }
    }

    public class TestBookOrderServiec : OrderService
    {
        protected override List<Order> GetOrders()
        {
           var orderList = new List<Order>();
           orderList.Add(new Order(){Type = "Book",CustomerName = "Test",Price = 123,ProductName = "Product"});
           orderList.Add(new Order(){Type = "Book",CustomerName = "Test",Price = 123,ProductName = "Product"});
           orderList.Add(new Order(){Type = "Other",CustomerName = "Test",Price = 123,ProductName = "Product"});
           return orderList;
        }
    }
}