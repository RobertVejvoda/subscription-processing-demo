using System;

namespace Core.Types
{
    public readonly struct Money : IEquatable<Money>
    {
        public Money(decimal amount, Currency? currency = null)
        {
            Amount = amount;
            Currency = currency ?? Currency.EUR;
        }

        public Money(double amount, Currency? currency = null) : this((decimal)amount, currency)
        {
        }

        public Money(int amount, Currency? currency = null) : this((decimal)amount, currency)
        {
        }

        public Money GetPositiveOrZero() => IsNegative() ? Zero(Currency) : this;

        public decimal Amount { get; }
        public Currency Currency { get; }

        public static Money Eur(decimal amount) => new Money(amount, Currency.EUR);
        public static Money Zero() => new Money(0, Currency.EUR);

        public static Money Zero(Currency currency) => new Money(0, currency);

        public bool IsNegative()
            => Amount < 0;

        public override string ToString() => $"{Amount:n2} {Currency.Name}";

        public static Money operator +(Money x, Money y)
        {
            if (!x.Currency.Equals(y.Currency)) throw new InvalidOperationException("Měny musí být stejné.");
            return new Money(x.Amount + y.Amount, x.Currency);
        }

        public static Money operator -(Money x, Money y)
        {
            if (!x.Currency.Equals(y.Currency)) throw new InvalidOperationException("Měny musí být stejné.");
            return new Money(x.Amount - y.Amount, x.Currency);
        }

        public static Money operator +(Money x, decimal y)
            => new Money(x.Amount + y, x.Currency);

        public static Money operator -(Money x, decimal y)
            => new Money(x.Amount - y, x.Currency);

        public static Money operator *(Money x, decimal y)
            => new Money(x.Amount * y, x.Currency);

        public static Money operator *(Money x, double y)
            => new Money(x.Amount * (decimal)y, x.Currency);

        public static implicit operator decimal(Money m)
            => m.Amount;

        /// <summary>
        /// Zero amount in any currency is equal to another zero amount.
        /// </summary>
        public bool Equals(Money other)
            => Amount == other.Amount && (Currency.Equals(other.Currency) || Amount == 0);

        public override bool Equals(object? obj)
            => obj is Money other && Equals(other);

        public override int GetHashCode()
            => (Amount, Currency).GetHashCode();
    }
}