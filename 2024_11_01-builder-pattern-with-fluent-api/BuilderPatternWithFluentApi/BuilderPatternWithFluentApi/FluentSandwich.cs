using System.Text;
using M31.FluentApi.Attributes;

namespace BuilderPatternWithFluentApi;

[FluentApi]
internal sealed class FluentSandwich
{
    [FluentMember(0)]
    [FluentDefault]
    public BreadEnum Bread { get; private set; } = BreadEnum.Black;

    [FluentMember(1)]
    [FluentDefault]
    public DressingEnum Dressing { get; private set; } = DressingEnum.Mayonnaise;

    [FluentMember(2)]
    [FluentDefault]
    public VegetablesEnum Vegetables { get; private set; } = VegetablesEnum.Cucumber | VegetablesEnum.Tomato | VegetablesEnum.Salade;

    /// <inheritdoc />
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Bread {this.Bread:G}");
        sb.AppendLine($"Dressing {this.Dressing:G}");
        sb.AppendLine($"Vegetables {this.Vegetables:F}");

        //..

        return sb.ToString();
    }
}
