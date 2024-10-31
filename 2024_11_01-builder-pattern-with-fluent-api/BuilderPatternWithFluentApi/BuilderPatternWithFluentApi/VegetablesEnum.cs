namespace BuilderPatternWithFluentApi;

[Flags]
public enum VegetablesEnum
{
    None = 0,
    Tomato = 1 << 0,
    Cucumber = 1 << 2,

    Salade = 1 << 3
    // etc
}
