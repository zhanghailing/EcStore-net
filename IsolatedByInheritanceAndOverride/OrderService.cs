﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Formatting;
using System.Text;

namespace IsolatedByInheritanceAndOverride
{
    public class OrderService
    {
        private string _filePath = @"C:\temp\testOrders.csv";
        private List<Order> _Result;
        private IBookDao _BookDao;

        public void SyncBookOrders()
        {
           
            var orders = GetOrders();

            // only get orders of book
            var ordersOfBook = orders.Where(x => x.Type == "Book");

            _BookDao = GetBookDao();
            foreach (var order in ordersOfBook)
            {
                _BookDao.Insert(order);
            }
        }

        protected virtual IBookDao GetBookDao()
        {
            return new BookDao();
        }

        protected virtual List<Order> GetOrders()
        {
            // parse csv file to get orders
            _Result = new List<Order>();

            // directly depend on File I/O
            using (StreamReader sr = new StreamReader(this._filePath, Encoding.UTF8))
            {
                int rowCount = 0;

                while (sr.Peek() > -1)
                {
                    rowCount++;

                    var content = sr.ReadLine();

                    // Skip CSV header line
                    if (rowCount > 1)
                    {
                        string[] line = content.Trim().Split(',');

                        _Result.Add(this.Mapping(line));
                    }
                }
            }

            return _Result;
        }

        private Order Mapping(string[] line)
        {
            var result = new Order
            {
                ProductName = line[0],
                Type = line[1],
                Price = Convert.ToInt32(line[2]),
                CustomerName = line[3]
            };

            return result;
        }
    }

    public class Order
    {
        public string Type { get; set; }

        public int Price { get; set; }

        public string ProductName { get; set; }

        public string CustomerName { get; set; }
    }

    public interface IBookDao
    {
        void Insert(Order order);
    }

    public class BookDao : IBookDao
    {
        public void Insert(Order order)
        {
            // directly depend on some web service
            //var client = new HttpClient();
            //var response = client.PostAsync<Order>("http://api.joey.io/Order", order, new JsonMediaTypeFormatter()).Result;

            //response.EnsureSuccessStatusCode();
            throw new NotImplementedException();
        }
    }
}
