using System.Text;

namespace BuilderPatternWithFluentApi;

internal sealed class NaiveSandwich
{
    private readonly ISandwichBuilder builder;

    public NaiveSandwich(ISandwichBuilder builder)
    {
        this.builder = builder;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Bread {this.builder.Bread:G}");
        sb.AppendLine($"Dressing {this.builder.Dressing:G}");
        sb.AppendLine($"Vegetables {this.builder.Vegetables:F}");

        //..

        return sb.ToString();
    }
}
