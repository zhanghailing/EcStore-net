using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

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
            var orderService = new FakeOrderService();
            orderService.SetOrders(new List<Order>
            {
                new Order()
                {
                    Type = "Book"
                },
                new Order()
                {
                    Type = "CD"
                },
                new Order()
                {
                    Type = "Book"
                },
            });
            //var target = new OrderService();

            var bookDao = Substitute.For<IBookDao>();
            orderService.SetBookDao(bookDao);

            orderService.SyncBookOrders();

            bookDao.Received(2).Insert(Arg.Is<Order>(order => order.Type == "Book"));
            bookDao.DidNotReceive().Insert(Arg.Is<Order>(order => order.Type == "CD"));
        }
    }

    internal class FakeOrderService : OrderService
    {
        private List<Order> _orders;
        private IBookDao _bookDao;

        public void SetBookDao(IBookDao bookDao)
        {
            _bookDao = bookDao;
        }

        protected override IBookDao GetBookDao()
        {
            return _bookDao;
        }

        public void SetOrders(List<Order> orders)
        {
            _orders = orders;
        }

        protected override List<Order> GetOrders()
        {
            return _orders;
        }
    }
}