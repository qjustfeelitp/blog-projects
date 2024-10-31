namespace BuilderPatternWithFluentApi;

public interface ISandwichBuilder
{
    BreadEnum Bread { get; }
    DressingEnum Dressing { get; }
    VegetablesEnum Vegetables { get; }
}
