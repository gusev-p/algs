using System;
using System.Collections.Generic;
using System.Linq;
using Algs.Core;
using Algs.TestUtilities;

namespace Algs.Tasks.Heaps
{
    public static class MinimumAverageWaitingTime
    {
        public static void TaskMain()
        {
            var n = Input.ReadInt();
            var customers = new Customer[n];
            for (var i = 0; i < n; i++)
            {
                var line = Input.ReadStrings();
                customers[i] = new Customer(long.Parse(line[0]), long.Parse(line[1]));
            }
            Array.Sort(customers, new ArrivalTimeComparer());
            Serve(customers);
            var minAverage = (long) customers.Average(c => c.servedTime);
            Console.WriteLine(minAverage);
        }

        private static void Serve(Customer[] customers)
        {
            var servingCustomer = customers[0];
            var timeNextCustomerCanBeServed = customers[0].arrivalTime + customers[0].cookingTime;
            var queue = new PriorityQueue<Customer>(customers.Length,
                delegate(Customer c1, Customer c2)
                {
                    if (c1.cookingTime < c2.cookingTime)
                        return MostPriority.First;
                    if (c2.cookingTime < c1.cookingTime)
                        return MostPriority.Second;
                    return MostPriority.Both;
                });
            var nextArrivingCustomerIndex = 1;
            //цикл по готовности
            foreach (var _ in customers)
            {
                while (nextArrivingCustomerIndex < customers.Length &&
                       customers[nextArrivingCustomerIndex].arrivalTime <= timeNextCustomerCanBeServed)
                {
                    queue.Insert(customers[nextArrivingCustomerIndex]);
                    nextArrivingCustomerIndex++;
                }
                servingCustomer.servedTime = timeNextCustomerCanBeServed - servingCustomer.arrivalTime;
                if (queue.Count > 0)
                    servingCustomer = queue.ExtractTop();
                else if (nextArrivingCustomerIndex < customers.Length)
                {
                    servingCustomer = customers[nextArrivingCustomerIndex];
                    nextArrivingCustomerIndex++;
                }
                timeNextCustomerCanBeServed += servingCustomer.cookingTime;
            }
        }

        private class ArrivalTimeComparer : IComparer<Customer>
        {
            public int Compare(Customer x, Customer y)
            {
                return x.arrivalTime.CompareTo(y.arrivalTime);
            }
        }

        private class Customer
        {
            public readonly long arrivalTime;
            public readonly long cookingTime;
            public long servedTime;

            public Customer(long arrivalTime, long cookingTime)
            {
                this.arrivalTime = arrivalTime;
                this.cookingTime = cookingTime;
            }
        }
    }
}