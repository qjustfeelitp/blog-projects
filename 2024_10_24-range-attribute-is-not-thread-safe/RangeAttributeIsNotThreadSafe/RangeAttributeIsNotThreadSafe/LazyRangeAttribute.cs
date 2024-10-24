using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace RangeAttributeIsNotThreadSafe;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LazyRangeAttribute<TType> : ValidationAttribute
    where TType : IComparable
{
    /// <summary>
    /// Gets the minimum value for the range
    /// </summary>
    public string Minimum { get; }

    /// <summary>
    /// Gets the maximum value for the range
    /// </summary>
    public string Maximum { get; }

    /// <summary>
    /// Specifies whether validation should fail for values that are equal to <see cref="Minimum"/>.
    /// </summary>
    public bool MinimumIsExclusive { get; set; }

    /// <summary>
    /// Specifies whether validation should fail for values that are equal to <see cref="Maximum"/>.
    /// </summary>
    public bool MaximumIsExclusive { get; set; }

    /// <summary>
    /// Determines whether string values for <see cref="Minimum"/> and <see cref="Maximum"/> are parsed in the invariant
    /// culture rather than the current culture in effect at the time of the validation.
    /// </summary>
    public bool ParseLimitsInInvariantCulture { get; set; }

    /// <summary>
    /// Convertor.
    /// </summary>
    private Lazy<Func<string, TType>> Convertor { get; }

    /// <summary>
    /// Allows for specifying range for arbitrary types. The minimum and maximum strings
    /// will be converted to the target type.
    /// </summary>
    /// <param name="minimum">The minimum allowable value.</param>
    /// <param name="maximum">The maximum allowable value.</param>
    public LazyRangeAttribute(string minimum, string maximum)
    {
        this.Minimum = minimum;
        this.Maximum = maximum;

        this.Convertor = new Lazy<Func<string, TType>>(s =>
        {
            var converter = GetOperandTypeConverter();

            var comparable = (TType)(this.ParseLimitsInInvariantCulture
                ? converter.ConvertFromInvariantString(s)!
                : converter.ConvertFromString(s)!);

            return comparable;
        });
    }

    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
        if (value is null or string { Length: 0 } || value is not TType converted)
        {
            return true;
        }

        var min = this.Convertor.Value(this.Minimum);
        var max = this.Convertor.Value(this.Maximum);

        return ValidateRange(min, max, converted);
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

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode",
                                     Justification = "The ctor that allows this code to be called is marked with RequiresUnreferencedCode.")]
    private static TypeConverter GetOperandTypeConverter()
    {
        return TypeDescriptor.GetConverter(typeof(TType));
    }

    /// <inheritdoc />
    public override string FormatErrorMessage(string name)
    {
        return string.Format(CultureInfo.CurrentCulture, this.ErrorMessageString, name, this.Minimum, this.Maximum);
    }
}

