using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedByInheritanceAndOverride.Test
{
    /// <summary>
    /// OrderServiceTest 的摘要描述
    /// </summary>
    [TestFixture]
    public class OrderServiceTest
    {
        public OrderServiceTest()
        {
            //
            // TODO:  在此加入建構函式的程式碼
            //
        }

        private TestContext testContextInstance;
        private OrderServiceForTest _orderServiceForTest = new OrderServiceForTest();
        private IBookDao _mockBookDao = Substitute.For<IBookDao>();

        /// <summary>
        ///取得或設定提供目前測試回合
        ///的相關資訊與功能的測試內容。
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        #region 其他測試屬性

        //
        // 您可以使用下列其他屬性撰寫您的測試:
        //
        // 執行該類別中第一項測試前，使用 ClassInitialize 執行程式碼
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在類別中的所有測試執行後，使用 ClassCleanup 執行程式碼
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在執行每一項測試之前，先使用 TestInitialize 執行程式碼
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在執行每一項測試之後，使用 TestCleanup 執行程式碼
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion 其他測試屬性

        [SetUp]
        public void Setup()
        {
            _orderServiceForTest.SetBookDao(_mockBookDao);
        }

        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            GivenOrders(
                new Order { Type = "Book" },
                new Order { Type = "CD" },
                new Order { Type = "Book" }
            );

            _orderServiceForTest.SyncBookOrders();

            BookDaoShouldInsertTimes(2);
        }

        public void BookDaoShouldInsertTimes(int times)
        {
            _mockBookDao.Received(times)
                .Insert(Arg.Is<Order>(o => o.Type == "Book"));
        }

        private void GivenOrders(params Order[] orders)
        {
            _orderServiceForTest.SetOrders(orders.ToList());
        }
    }

    public class OrderServiceForTest : OrderService
    {
        private List<Order> _orders;
        private IBookDao _dao;

        public void SetBookDao(IBookDao dao)
        {
            this._dao = dao;
        }

        protected override IBookDao GetBookDao()
        {
            return this._dao;
        }

        public void SetOrders(List<Order> orders)
        {
            this._orders = orders;
        }

        protected override List<Order> GetOrders()
        {
            return this._orders;
        }
    }
}