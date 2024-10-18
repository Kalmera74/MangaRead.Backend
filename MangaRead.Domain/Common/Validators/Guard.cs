namespace MangaRead.Domain.Common.Validators;
public static class Guard
{
    public static void AgainstNullOrEmpty(string value, string parameterName)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException($"'{parameterName}' cannot be null or empty.");
        }
    }

    public static void AgainstNull(object value, string parameterName)
    {
        if (value == null)
        {
            throw new ArgumentNullException(parameterName);
        }
    }

    public static void AgainstSelfReference(object referrer, object reference, string parameterName)
    {
        if (ReferenceEquals(referrer, reference))
        {
            throw new ArgumentException($"{nameof(parameterName)} cannot be the same object as the {referrer.GetType()} ");
        }
    }

    internal static void AgainstOutOfRange(int order, string parameterName, int min, int maxValue)
    {
        if (order < min || order > maxValue)
        {
            throw new ArgumentOutOfRangeException($"{parameterName} must be between {min} and {maxValue}");
        }
    }
}
