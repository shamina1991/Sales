using System;
using System.Collections.Generic;

namespace SalesTaxApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var input1 = new List<Item>
            {
                new Item { Name = "book", Price = 12.49m, IsExempt = true, Quantity = 1 },
                new Item { Name = "music CD", Price = 14.99m, IsExempt = false, Quantity = 1 },
                new Item { Name = "chocolate bar", Price = 0.85m, IsExempt = true, Quantity = 1 }
            };

            var input2 = new List<Item>
            {
                new Item { Name = "imported box of chocolates", Price = 10.00m, IsExempt = true, IsImported = true, Quantity = 1 },
                new Item { Name = "imported bottle of perfume", Price = 47.50m, IsExempt = false, IsImported = true, Quantity = 1 }
            };

            var input3 = new List<Item>
            {
                new Item { Name = "imported bottle of perfume", Price = 27.99m, IsExempt = false, IsImported = true, Quantity = 1 },
                new Item { Name = "bottle of perfume", Price = 18.99m, IsExempt = false, Quantity = 1 },
                new Item { Name = "packet of headache pills", Price = 9.75m, IsExempt = true, Quantity = 1 },
                new Item { Name = "box of imported chocolates", Price = 11.25m, IsExempt = true, IsImported = true, Quantity = 1 }
            };

            var calculator = new SalesTaxCalculator();

            var receipt1 = calculator.CalculateReceipt(input1);
            var receipt2 = calculator.CalculateReceipt(input2);
            var receipt3 = calculator.CalculateReceipt(input3);

            PrintReceipt("Output 1", receipt1);
            PrintReceipt("Output 2", receipt2);
            PrintReceipt("Output 3", receipt3);

            Console.ReadLine();
        }

        static void PrintReceipt(string title, Receipt receipt)
        {
            Console.WriteLine($"{title}:");
            foreach (var item in receipt.Items)
            {
                Console.WriteLine($"{item.Quantity} {item.Name}: {item.Price + item.SalesTax}");
            }
            Console.WriteLine($"Sales Taxes: {receipt.SalesTax}");
            Console.WriteLine($"Total: {receipt.Total}");
            Console.WriteLine();
        }
    }

    public class Item
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsImported { get; set; }
        public bool IsExempt { get; set; }
        public int Quantity { get; set; }
        public decimal SalesTax { get; set; }
    }

    public class Receipt
    {
        public List<Item> Items { get; set; }
        public decimal SalesTax { get; set; }
        public decimal Total { get; set; }
    }

    public class SalesTaxCalculator
    {
        private const decimal BasicSalesTaxRate = 0.1m;
        private const decimal ImportDutyRate = 0.05m;

        public Receipt CalculateReceipt(List<Item> items)
        {
            var receipt = new Receipt();
            receipt.Items = items;

            decimal totalSalesTax = 0m;
            decimal totalPrice = 0m;

            foreach (var item in items)
            {
                decimal taxRate = item.IsExempt ? 0m : BasicSalesTaxRate;
                if (item.IsImported)
                {
                    taxRate += ImportDutyRate;
                }

                item.SalesTax = CalculateSalesTax(item.Price, taxRate);
                totalSalesTax += item.SalesTax;

                totalPrice += item.Price;
            }

            receipt.SalesTax = RoundToNearestFiveCents(totalSalesTax);
            receipt.Total = RoundToNearestFiveCents(totalPrice + totalSalesTax);

            return receipt;
        }

        private decimal CalculateSalesTax(decimal price, decimal taxRate)
        {
            return RoundToNearestFiveCents(price * taxRate);
        }

        private decimal RoundToNearestFiveCents(decimal value)
        {
            return Math.Ceiling(value * 20) / 20;
        }
    }
}