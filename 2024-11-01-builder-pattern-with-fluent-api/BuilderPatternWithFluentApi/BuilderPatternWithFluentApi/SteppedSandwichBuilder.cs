using static BuilderPatternWithFluentApi.SteppedSandwichBuilder;

namespace BuilderPatternWithFluentApi;

internal sealed class SteppedSandwichBuilder : ISandwichBuilder, IAddDressing, IWithVegetables, IAddVegetables
{
    public BreadEnum Bread { get; }
    public DressingEnum Dressing { get; private set; }
    public VegetablesEnum Vegetables { get; private set; }

    private SteppedSandwichBuilder(BreadEnum bread)
    {
        this.Bread = bread;
    }

    public static IAddDressing Create(BreadEnum bread)
    {
        return new SteppedSandwichBuilder(bread);
    }

    public IWithVegetables WithoutDressing()
    {
        this.Dressing = DressingEnum.None;

        return this;
    }

    public IWithVegetables WithDressing(DressingEnum desiredDressing)
    {
        this.Dressing = desiredDressing;

        return this;
    }

    public IBuild WithoutVegetables()
    {
        this.Vegetables = VegetablesEnum.None;

        return this;
    }

    public IAddVegetables WithVegetables()
    {
        return this;
    }

    public IAddVegetables WithTomato()
    {
        this.Vegetables |= VegetablesEnum.Tomato;

        return this;
    }

    public IAddVegetables WithCucumber()
    {
        this.Vegetables |= VegetablesEnum.Cucumber;

        return this;
    }

    public IAddVegetables WithSalade()
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

    //public static explicit operator IBuild(SteppedSandwichBuilder builder)
    //{
    //    throw new NotSupportedException();
    //}

    public interface IAddDressing
    {
        IWithVegetables WithoutDressing();
        IWithVegetables WithDressing(DressingEnum desiredDressing);
    }

    public interface IWithVegetables
    {
        IBuild WithoutVegetables();
        IAddVegetables WithVegetables();
    }

    public interface IAddVegetables : IBuild
    {
        IAddVegetables WithTomato();
        IAddVegetables WithCucumber();
        IAddVegetables WithSalade();
    }

    public interface IBuild
    {
        NaiveSandwich Build();
    }
}
