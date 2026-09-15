using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace VgAutoDrill.Fundation.Utils;

public static class ThrowHelper
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullException(object? obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullException(object? obj, string? message)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj), message);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullException(string? param)
    {
        if (param == null) throw new ArgumentNullException(param);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullOrEmptyException(string? param)
    {
        if (string.IsNullOrEmpty(param)) throw new ArgumentNullException(param);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentNullOrWhiteSpaceException(string? param)
    {
        if (string.IsNullOrWhiteSpace(param)) throw new ArgumentNullException(param);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentException(string message, string paramName, Exception innerException)
    {
        throw new ArgumentException(message, paramName, innerException);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentException(string message, string paramName)
    {
        throw new ArgumentException(message, paramName);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentException(string message)
    {
        throw new ArgumentException(message);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(string? paramName = null)
    {
        throw new ArgumentOutOfRangeException(paramName);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowArgumentOutOfRangeException(string paramName, string message)
    {
        throw new ArgumentOutOfRangeException(paramName, message);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowNotSupportedException(string? message = null)
    {
        throw new NotSupportedException(message);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidOperationException(string? message = null)
    {
        throw new InvalidOperationException(message);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowInvalidCastException(string message)
    {
        throw new InvalidCastException(message);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowFormatException(string message, Exception? innerException = null)
    {
        throw new FormatException(message, innerException);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowNotImplementedException()
    {
        throw new NotImplementedException();
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowKeyNotFoundException()
    {
        throw new KeyNotFoundException();
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowObjectDisposedException(string objectName)
    {
        throw new ObjectDisposedException(objectName);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowTypeLoadException(string message)
    {
        throw new TypeLoadException(message);
    }

    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void ThrowSerializationException(string message)
    {
        throw new SerializationException(message);
    }
}
