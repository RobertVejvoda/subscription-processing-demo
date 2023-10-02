using System.Collections.Generic;
using System.Linq;

namespace Core.Types
{
    public readonly struct Percent
    {
        public static Percent Zero { get; } = new Percent(0);

        public static Percent Hundred { get; } = new Percent(100);

        public decimal Current { get; }

        public decimal Maximum { get; }

        public decimal Value => Current / (Maximum == 0 ? 100 : Maximum) * 100;

        public Percent(decimal value, decimal max = 100)
        {
            Current = value;
            Maximum = max;
        }

        public static Percent operator +(Percent left, Percent right)
        {
            var max = left.Maximum > right.Maximum ? left.Maximum : right.Maximum;
            var current = left.Current * (max / left.Maximum) + right.Current * (max / right.Maximum);
            return new Percent(current, max);
        }

        public static Percent operator -(Percent left, Percent right)
        {
            var max = left.Maximum > right.Maximum ? left.Maximum : right.Maximum;
            var current = left.Current * (max / left.Maximum) - right.Current * (max / right.Maximum);
            return new Percent(current, max);
        }

        public static Percent operator *(Percent left, Percent right)
        {
            var max = left.Maximum > right.Maximum ? left.Maximum : right.Maximum;
            var current = left.Current * (max / left.Maximum) * right.Current * (max / right.Maximum);
            return new Percent(current, max);
        }

        public static Percent operator /(Percent left, Percent right)
        {
            var max = left.Maximum > right.Maximum ? left.Maximum : right.Maximum;
            var current = left.Current * (max / left.Maximum) / right.Current * (max / right.Maximum);
            return new Percent(current, max);
        }

        public static implicit operator decimal(Percent percent) => percent.Value;

        public static implicit operator Percent(decimal percent) => new Percent(percent);

        public static Percent Average(IEnumerable<Percent> collection)
        {
            var current = collection.Select(s => s.Value).Average();

            return new Percent(current);
        }

        public override string ToString()
        {
            return $"{Value}%";
        }
    }

    public static class PercentExtensions
    {
        public static Percent Average(this IEnumerable<Percent> collection)
        {
            return Percent.Average(collection);
        }
    }
}