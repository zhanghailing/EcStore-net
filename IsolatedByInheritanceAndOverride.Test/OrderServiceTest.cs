using Moq;
using Moq.Protected;
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
        private Mock<OrderService> _orderService;
        private Mock<IBookDao> _bookDao;

        [SetUp]
        public void Setup()
        {
            _orderService = new Mock<OrderService>();
            _bookDao = new Mock<IBookDao>();
            _orderService.Protected()
                .Setup<IBookDao>("GetBookDao")
                .Returns(_bookDao.Object);
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
            _orderService.Object.SyncBookOrders();
        }

        private void ShouldNotInsertOrder(string type)
        {
            //_bookDao.DidNotReceive().Insert(Arg.Is<Order>(order => order.Type == type));
            _bookDao.Verify(x => x.Insert(It.Is<Order>(o => o.Type == type)), Times.Never);
        }

        private void ShouldInsertOrders(int times, string type)
        {
            //_bookDao.Received(times).Insert(Arg.Is<Order>(order => order.Type == type));
            _bookDao.Verify(x => x.Insert(It.Is<Order>(o => o.Type == type)), Times.Exactly(times));
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
            //_orderService.SetOrders(orders);
            _orderService.Protected()
                .Setup<List<Order>>("GetOrders")
                .Returns(orders);
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