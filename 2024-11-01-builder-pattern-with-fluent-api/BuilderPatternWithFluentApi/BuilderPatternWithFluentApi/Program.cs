using BuilderPatternWithFluentApi;

var naiveSandwich1 = NaiveSandwichBuilder.Create(BreadEnum.GlutenFree)
                                         .WithoutDressing()
                                         .WithSalade()
                                         .Build();

Console.WriteLine(naiveSandwich1);

var steppedSandwich = SteppedSandwichBuilder.Create(BreadEnum.Black)
                                            .WithDressing(DressingEnum.Mayonnaise)
                                            .WithVegetables()
                                            .WithSalade()
                                            .WithCucumber()
                                            .Build();

Console.WriteLine(steppedSandwich);

var retypedSandwich = ((SteppedSandwichBuilder.IBuild)SteppedSandwichBuilder.Create(BreadEnum.White)).Build();

Console.WriteLine(retypedSandwich);

var fluentSandwich = CreateFluentSandwich.WithDefaultBread()
                                         .WithDefaultDressing()
                                         .WithDefaultVegetables();

Console.WriteLine(fluentSandwich);
