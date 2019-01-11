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
        private FakeOrderService _orderService;
        private IBookDao _bookDao;

        [SetUp]
        public void Setup()
        {
            _orderService = new FakeOrderService();
            _bookDao = Substitute.For<IBookDao>();
            _orderService.SetBookDao(_bookDao);
        }

        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            GivenOrders(new List<Order>
            {
                CreateOrder("Book"),
                CreateOrder("CD"),
                CreateOrder("Book"),
            });

            WhenSyncBookOrders();

            ShouldInsertOrders(2, "Book");
            ShouldNotInsertOrder("CD");
        }

        private void WhenSyncBookOrders()
        {
            _orderService.SyncBookOrders();
        }

        private void ShouldNotInsertOrder(string type)
        {
            _bookDao.DidNotReceive().Insert(Arg.Is<Order>(order => order.Type == type));
        }

        private void ShouldInsertOrders(int times, string type)
        {
            _bookDao.Received(times).Insert(Arg.Is<Order>(order => order.Type == type));
        }

        private static Order CreateOrder(string type)
        {
            return new Order()
            {
                Type = type
            };
        }

        private void GivenOrders(List<Order> orders)
        {
            _orderService.SetOrders(orders);
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