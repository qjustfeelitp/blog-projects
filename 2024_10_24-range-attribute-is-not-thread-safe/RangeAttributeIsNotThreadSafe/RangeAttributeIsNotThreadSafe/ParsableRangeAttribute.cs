using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RangeAttributeIsNotThreadSafe;

/// <summary>
/// The <see cref="RangeAttribute"/> is not thread safe, it sets up conversion to the property during calling IsValid!
/// If <see cref="RangeAttribute"/> is used on property in some class, then the same attribute gets cached by runtime and therefore there could be race condition in setting up the conversion.
/// More info here https://github.com/dotnet/runtime/issues/1143
/// This attribute does it thread safely.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ParsableRangeAttribute<TType> : ValidationAttribute
    where TType : IComparable, ISpanParsable<TType>
{
    /// <summary>
    /// Gets the minimum value for the range
    /// </summary>
    public TType Minimum { get; }

    /// <summary>
    /// Gets the maximum value for the range
    /// </summary>
    public TType Maximum { get; }

    /// <summary>
    /// Specifies whether validation should fail for values that are equal to <see cref="Minimum"/>.
    /// </summary>
    public bool MinimumIsExclusive { get; set; }

    /// <summary>
    /// Specifies whether validation should fail for values that are equal to <see cref="Maximum"/>.
    /// </summary>
    public bool MaximumIsExclusive { get; set; }

    public ParsableRangeAttribute(TType minimum, TType maximum)
    {
        this.Minimum = minimum;
        this.Maximum = maximum;
    }

    public ParsableRangeAttribute(string minimum, string maximum, bool parseLimitsInInvariantCulture = false)
    {
        var formatProvider = parseLimitsInInvariantCulture
            ? CultureInfo.InvariantCulture
            : null;

        this.Minimum = TType.Parse(minimum, formatProvider);
        this.Maximum = TType.Parse(maximum, formatProvider);
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
        if (value is null or string { Length: 0 } || value is not TType converted)
        {
            return true;
        }

        return ValidateRange(this.Minimum, this.Maximum, converted);
    }

    /// <summary>
    /// Validates range.
    /// </summary>
    /// <param name="min">Minimum</param>
    /// <param name="max">Maximum</param>
    /// <param name="value">Value</param>
    private bool ValidateRange(IComparable min, IComparable max, IComparable value)
    {
        return (this.MinimumIsExclusive
                   ? min.CompareTo(value) < 0
                   : min.CompareTo(value) <= 0)
            && (this.MaximumIsExclusive
                   ? max.CompareTo(value) > 0
                   : max.CompareTo(value) >= 0);
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Minimum, this.Maximum);
    }
}
