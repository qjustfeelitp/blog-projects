namespace BuilderPatternWithFluentApi;

internal sealed class NaiveSandwichBuilder : ISandwichBuilder
{
    public BreadEnum Bread { get; }
    public DressingEnum Dressing { get; private set; }
    public VegetablesEnum Vegetables { get; private set; }

    private NaiveSandwichBuilder(BreadEnum bread)
    {
        this.Bread = bread;
    }

    public static NaiveSandwichBuilder Create(BreadEnum bread)
    {
        return new NaiveSandwichBuilder(bread);
    }

    public NaiveSandwichBuilder WithoutDressing()
    {
        this.Dressing = DressingEnum.None;

        return this;
    }

    public NaiveSandwichBuilder WithDressing(DressingEnum desiredDressing)
    {
        this.Dressing = desiredDressing;

        return this;
    }

    public NaiveSandwichBuilder WithoutVegetables()
    {
        this.Vegetables = VegetablesEnum.None;

        return this;
    }

    public NaiveSandwichBuilder WithTomato()
    {
        this.Vegetables |= VegetablesEnum.Tomato;

        return this;
    }

    public NaiveSandwichBuilder WithCucumber()
    {
        this.Vegetables |= VegetablesEnum.Cucumber;

        return this;
    }

    public NaiveSandwichBuilder WithSalade()
    {
        this.Vegetables |= VegetablesEnum.Salade;

        return this;
    }

    //..

    public NaiveSandwich Build()
    {
        var sandwich = new NaiveSandwich(this);

        return sandwich;
    }
}
