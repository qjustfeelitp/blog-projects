using System.ComponentModel.DataAnnotations;
using BenchmarkDotNet.Attributes;
using RangeAttributeIsNotThreadSafe;

namespace BenchmarkComparison;

[MemoryDiagnoser]
public class Benchy
{
    private const decimal ToValidate = 101m;

    private static readonly string Min = 00.01m.ToString("N");
    private static readonly string Max = 100.01m.ToString("N");

    [Benchmark]
    public bool Range()
    {
        var a = new RangeAttribute(typeof(decimal), Min, Max);

        return a.IsValid(ToValidate);
    }

    [Benchmark]
    public bool LazyRange()
    {
        var a = new LazyRangeAttribute<decimal>(Min, Max);

        return a.IsValid(ToValidate);
    }

    [Benchmark]
    public bool ParsableRange()
    {
        var a = new ParsableRangeAttribute<decimal>(Min, Max);

        return a.IsValid(ToValidate);
    }
}
