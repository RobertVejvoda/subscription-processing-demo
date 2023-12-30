﻿using System.Reflection;

namespace Core.Types
{
    public abstract class Enumeration(int id, string name) : IComparable
    {
        public string Name { get; private set; } = name;

        public int Id { get; } = id;

        public override string ToString() => Name;

        // ReSharper disable once MemberCanBePrivate.Global
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Enumeration otherValue)
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
            return matchingItem;
        }

        private static T Parse<T, TK>(TK value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

            return matchingItem;
        }


        public int CompareTo(object? other)
        {
            if (other == null) return 1;

            if (other is Enumeration otherEnumeration)
                return Id.CompareTo(otherEnumeration.Id);

            throw new ArgumentException("Object is not Enumeration");
        }

        public static bool operator ==(Enumeration? x, Enumeration? y)
        {
            return x != null && x.Equals(y);
        }

        public static bool operator !=(Enumeration? x, Enumeration? y)
        {
            return !(x == y);
        }
        
        public static bool operator <(Enumeration x, Enumeration y)
        {
            return x.Id < y.Id;
        }

        public static bool operator <=(Enumeration x, Enumeration y)
        {
            return x.Id <= y.Id;
        }

        public static bool operator >(Enumeration x, Enumeration y)
        {
            return x.Id > y.Id;
        }

        public static bool operator >=(Enumeration x, Enumeration y)
        {
            return x.Id >= y.Id;
        }

        public static implicit operator int(Enumeration x) => x.Id;
    }
}