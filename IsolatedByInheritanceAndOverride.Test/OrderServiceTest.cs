using System.Collections.Generic;
using System.Linq;
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
            var bookDao = Substitute.For<IBookDao>();

            var orderService = new TestBookOrderServiec();
            orderService.SetOrders(new Order{Type="Book"},new Order(){Type="Book"},new Order(){Type="Test"});
            orderService.SetBookDao(bookDao);
            orderService.SyncBookOrders();


            bookDao.Received(2).Insert(Arg.Is<Order>(order => order.Type == "Book"));
            // hard to isolate dependency to unit test
            //var books = new TestBookOrderServiec();
            //var target = new OrderService();
            //target.SyncBookOrders();
        }
    }

    public class TestBookOrderServiec : OrderService
    {
        public IBookDao _BookDao;
        private List<Order> _Orders;

        public void SetBookDao(IBookDao bookDao)
        {
            _BookDao = bookDao;
        }
        protected override IBookDao GetBookDao()
        {
            return _BookDao;
        }

        public void SetOrders(params Order[] orders)
        {
            _Orders = orders.ToList();
        }
        protected override List<Order> GetOrders()
        {
            return _Orders;
        }
    }
}