using System;

namespace Task2
{
    /// <summary>
    /// Банківський рахунок. Інваріанти типу:
    ///  1) номер рахунку: рівно 10 цифр, не змінюється після створення;
    ///  2) ім'я власника: не null, не порожнє, 2–50 символів після Trim;
    ///  3) баланс: завжди >= 0, змінюється лише через Deposit / Withdraw;
    ///  4) дата відкриття: задається при створенні, лише для читання.
    /// </summary>
    public class BankAccount
    {
        private const int AccountNumberLength = 10;
        private const int OwnerNameMinLength = 2;
        private const int OwnerNameMaxLength = 50;

        // Внутрішній стан — закриті поля, прямий доступ ззовні неможливий
        private readonly string _accountNumber;
        private readonly DateTime _openedAt;
        private string _ownerName;
        private decimal _balance;

        /// <summary>Створює рахунок; при некоректних даних кидає виняток, об'єкт не створюється.</summary>
        public BankAccount(string accountNumber, string ownerName, decimal initialBalance = 0m)
        {
            // Спочатку валідуємо ВСІ аргументи, і лише потім присвоюємо поля
            _accountNumber = ValidateAccountNumber(accountNumber);
            _ownerName = ValidateOwnerName(ownerName);

            if (initialBalance < 0m)
                throw new ArgumentOutOfRangeException(nameof(initialBalance),
                    "Початковий баланс не може бути від'ємним.");

            _balance = initialBalance;
            _openedAt = DateTime.Now;
        }

        /// <summary>Номер рахунку (лише читання).</summary>
        public string AccountNumber => _accountNumber;

        /// <summary>Дата відкриття (лише читання).</summary>
        public DateTime OpenedAt => _openedAt;

        /// <summary>Поточний баланс (лише читання; змінюється тільки операціями).</summary>
        public decimal Balance => _balance;

        /// <summary>Ім'я власника; сеттер виконує валідацію та нормалізацію.</summary>
        public string OwnerName
        {
            get => _ownerName;
            set => _ownerName = ValidateOwnerName(value);
        }

        /// <summary>Поповнення рахунку. Сума має бути строго більшою за 0.</summary>
        public void Deposit(decimal amount)
        {
            if (amount <= 0m)
                throw new ArgumentOutOfRangeException(nameof(amount),
                    "Сума поповнення має бути більшою за 0.");

            _balance += amount;
        }

        /// <summary>Зняття коштів. Сума > 0 і не більша за поточний баланс.</summary>
        public void Withdraw(decimal amount)
        {
            if (amount <= 0m)
                throw new ArgumentOutOfRangeException(nameof(amount),
                    "Сума зняття має бути більшою за 0.");

            if (amount > _balance)
                throw new InvalidOperationException(
                    "Недостатньо коштів на рахунку для виконання операції.");

            _balance -= amount;
        }

        // ---------- Приватні методи валідації ----------

        private static string ValidateAccountNumber(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Номер рахунку не може бути null.");

            if (value.Length != AccountNumberLength)
                throw new ArgumentException(
                    $"Номер рахунку має містити рівно {AccountNumberLength} символів.", nameof(value));

            foreach (char c in value)
            {
                if (c < '0' || c > '9')
                    throw new ArgumentException("Номер рахунку має складатися лише з цифр.", nameof(value));
            }

            return value;
        }

        private static string ValidateOwnerName(string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Ім'я власника не може бути null.");

            string trimmed = value.Trim(); // нормалізація: прибираємо крайні пробіли

            if (trimmed.Length == 0)
                throw new ArgumentException("Ім'я власника не може бути порожнім.", nameof(value));

            if (trimmed.Length < OwnerNameMinLength || trimmed.Length > OwnerNameMaxLength)
                throw new ArgumentException(
                    $"Довжина імені власника має бути від {OwnerNameMinLength} до {OwnerNameMaxLength} символів.",
                    nameof(value));

            return trimmed;
        }
    }
}