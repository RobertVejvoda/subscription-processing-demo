namespace Core.Helpers
{
    public static class Guard
    {
        public static void ArgumentNotNull(object value, string? nameOfValue = default)
        {
            if (value is null)
                throw new ArgumentException($"Value '{nameOfValue ?? nameof(value)}' can't be NULL.");
        }

        public static void ArgumentNotNullOrEmpty(string value, string? nameOfValue = default)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(
                    $"Value '{nameOfValue ?? nameof(value)}' can't be NULL or empty string.");
        }

        public static void ArgumentPositive(int value, string? nameOfValue = default)
        {
            if (value <= 0)
                throw new ArgumentException(
                    $"Value '{nameOfValue ?? nameof(value)}' must be positive number greater than 0.");
        }

        public static void ArgumentPositive(double value, string? nameOfValue = default)
        {
            if (value <= 0)
                throw new ArgumentException(
                    $"Value '{nameOfValue ?? nameof(value)}' must be positive number greater than 0.");
        }
        
        public static void ArgumentPositive(decimal value, string? nameOfValue = default)
        {
            if (value <= 0)
                throw new ArgumentException(
                    $"Value '{nameOfValue ?? nameof(value)}' must be positive number greater than 0.");
        }

        public static void ArgumentNotNegative(int value, string? nameOfValue = default)
        {
            if (value <= 0)
                throw new ArgumentException($"Value '{nameOfValue ?? nameof(value)}' must be positive number.");
        }

        public static void ArgumentNotNegative(double value, string? nameOfValue = default)
        {
            if (value <= 0)
                throw new ArgumentException($"Value '{nameOfValue ?? nameof(value)}' must be positive number.");
        }

        public static void ArgumentNotNegative(decimal value, string? nameOfValue = default)
        {
            if (value <= 0)
                throw new ArgumentException($"Value '{nameOfValue ?? nameof(value)}' must be positive number.");
        }
    }
}