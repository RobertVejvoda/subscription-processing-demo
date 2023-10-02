using System;

namespace Core.Types
{
    public readonly struct Price : IEquatable<Price>
    {
        public Price(decimal value, Currency currency)
        {
            Value = value;
            Currency = currency;
        }

        public decimal Value { get; }
        public Currency Currency { get; }

        public Price Negate()
        {
            return new Price(-Value, Currency);
        }

        public Price Reset()
        {
            return new Price(default, Currency);
        }

        public bool IsNegative()
        {
            return Value < 0;
        }

        public void EnsureNotNegative()
        {
            if (IsNegative())
                throw new InvalidOperationException("Price can't be a negative value.");
        }

        public bool IsPositive()
        {
            return Value > 0;
        }

        public bool IsZero()
        {
            return Value == 0;
        }

        public Money ToMoney() => new Money(Value, Currency);

        public override string ToString()
        {
            return $"{Value} {Abbreviation}";
        }

        public string Abbreviation => Currency.Code;

        public static Price CZK(decimal amount)
        {
            return new Price(amount, Currency.CZK);
        }

        public static Price EUR(decimal amount)
        {
            return new Price(amount, Currency.EUR);
        }

        public static Price USD(decimal amount)
        {
            return new Price(amount, Currency.USD);
        }

        public static Price AUD(decimal amount)
        {
            return new Price(amount, Currency.AUD);
        }

        public static Price GBP(decimal amount)
        {
            return new Price(amount, Currency.GBP);
        }

        public static Price CHF(decimal amount)
        {
            return new Price(amount, Currency.CHF);
        }

        public bool Equals(Price other)
            => Value == other.Value && (Currency.Equals(other.Currency) || Value == 0);

        public override bool Equals(object? obj)
            => obj is Price other && Equals(other);

        public override int GetHashCode()
            => (Value, Currency).GetHashCode();

        public static Price operator -(Price x, Money y)
        {
            if (!x.Currency.Equals(y.Currency))
                throw new ArgumentException("Currency must be same.");

            return new Price(x.Value - y.Amount, x.Currency);
        }

        public static Price operator +(Price x, Money y)
        {
            if (!x.Currency.Equals(y.Currency))
                throw new ArgumentException("Currency must be same.");

            return new Price(x.Value + y.Amount, x.Currency);
        }

        public static Price operator *(Price x, decimal y)
        {
            return new Price(x.Value * y, x.Currency);
        }

        public static Price operator *(decimal x, Price y)
        {
            return y * x;
        }

        public static Price operator /(Price x, decimal y)
        {
            return new Price(x.Value / y, x.Currency);
        }

        public static Price operator /(decimal x, Price y)
        {
            return y / x;
        }

        public static bool operator ==(Price x, Price y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Price x, Price y)
        {
            return !(x == y);
        }

        public static bool operator <(Price x, Price y)
        {
            if (!x.Currency.Equals(y.Currency))
                throw new ArgumentException("Currency must be same.");

            return x.Value < y.Value;
        }

        public static bool operator <=(Price x, Price y)
        {
            if (!x.Currency.Equals(y.Currency))
                throw new ArgumentException("Currency must be same.");

            return x.Value <= y.Value;
        }

        public static bool operator >(Price x, Price y)
        {
            if (!x.Currency.Equals(y.Currency))
                throw new ArgumentException("Currency must be same.");

            return x.Value > y.Value;
        }

        public static bool operator >=(Price x, Price y)
        {
            if (!x.Currency.Equals(y.Currency))
                throw new ArgumentException("Currency must be same.");

            return x.Value >= y.Value;
        }

        public static implicit operator decimal(Price p)
            => p.Value;
    }
}