using System;

namespace Task2
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ===== 4.1. Коректне створення =====
            Run("С1  Коректне створення", () =>
            {
                var a = new BankAccount("1234567890", "Іван Петренко", 100m);
                Console.WriteLine($"     {a.AccountNumber}, {a.OwnerName}, {a.Balance}");
            });

            Run("С2  Ім'я з пробілами, нульовий баланс", () =>
            {
                var a = new BankAccount("0000000001", "  Ол  ", 0m);
                Console.WriteLine($"     Ім'я після нормалізації: '{a.OwnerName}', баланс: {a.Balance}");
            });

            // ===== 4.2. Коректні операції =====
            var acc = new BankAccount("1234567890", "Іван Петренко", 100m);

            Run("С3  Поповнення на 50", () =>
            {
                acc.Deposit(50m);
                Console.WriteLine($"     Баланс: {acc.Balance}");       // 150
            });

            Run("С4  Зняття 30", () =>
            {
                acc.Withdraw(30m);
                Console.WriteLine($"     Баланс: {acc.Balance}");       // 120
            });

            Run("С5  Зняття всієї суми", () =>
            {
                acc.Withdraw(acc.Balance);
                Console.WriteLine($"     Баланс: {acc.Balance}");       // 0
            });

            Run("С6  Зміна імені власника", () =>
            {
                acc.OwnerName = "Марія Коваль";
                Console.WriteLine($"     Власник: {acc.OwnerName}");
            });

            // ===== 4.3. Некоректні аргументи (очікуємо винятки) =====
            Run("С7a Номер null", () => new BankAccount(null, "Іван", 0m));
            Run("С7b Номер порожній", () => new BankAccount("", "Іван", 0m));
            Run("С8a Номер закороткий", () => new BankAccount("12345", "Іван", 0m));
            Run("С8b Номер з літерами", () => new BankAccount("12345abcde", "Іван", 0m));
            Run("С9a Власник null", () => new BankAccount("1234567890", null, 0m));
            Run("С9b Власник порожній", () => new BankAccount("1234567890", "", 0m));
            Run("С9c Власник з пробілів", () => new BankAccount("1234567890", "   ", 0m));
            Run("С10a Власник 1 символ", () => new BankAccount("1234567890", "І", 0m));
            Run("С10b Власник > 50 символів", () => new BankAccount("1234567890", new string('а', 51), 0m));
            Run("С11 Від'ємний початковий баланс", () => new BankAccount("1234567890", "Іван", -10m));

            var acc2 = new BankAccount("1111111111", "Петро Сидоренко", 100m);

            Run("С12a Поповнення на 0", () => acc2.Deposit(0m));
            Run("С12b Поповнення на від'ємну суму", () => acc2.Deposit(-5m));
            Run("С13a Зняття 0", () => acc2.Withdraw(0m));
            Run("С13b Зняття від'ємної суми", () => acc2.Withdraw(-5m));
            Run("С14 Зняття більше, ніж баланс", () => acc2.Withdraw(150m));
            Console.WriteLine($"     Баланс після невдалих операцій: {acc2.Balance}"); // лишається 100

            Run("С15a Власник -> порожній", () => acc2.OwnerName = "");
            Run("С15b Власник -> пробіли", () => acc2.OwnerName = "    ");
            Console.WriteLine($"     Власник після невдалих змін: {acc2.OwnerName}");   // лишається "Петро Сидоренко"

            // ===== 4.4. Заборонені шляхи (помилки КОМПІЛЯЦІЇ) =====
            // Щоб продемонструвати у звіті, розкоментуйте рядок і зробіть скріншот помилки:
            // acc2.Balance = 1000m;          // С16: CS0200 — немає публічного сеттера
            // acc2.AccountNumber = "999";    // С17: CS0200 — властивість лише для читання
            // acc2.OpenedAt = DateTime.Now;  // С17: CS0200
            // var x = acc2._balance;         // С18: CS0122 — поле недоступне (private)

            Console.WriteLine("\nГотово. Натисніть Enter...");
            Console.ReadLine();
        }

        /// <summary>Виконує сценарій і виводить: успіх або тип винятку з повідомленням.</summary>
        static void Run(string name, Action scenario)
        {
            try
            {
                scenario();
                Console.WriteLine($"[УСПІХ]  {name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ВИНЯТОК] {name} -> {ex.GetType().Name}: {ex.Message.Split('\n')[0]}");
            }
        }
    }
}