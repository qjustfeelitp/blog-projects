namespace AutomaticFactoryWithMsDI;

public sealed class PaymentProcessor
{
    private readonly string serviceId;
    private readonly string userId;
    private readonly string currency;
    private readonly decimal amount;
    private readonly UserRepository userRepository;

    public PaymentProcessor(string serviceId,
                            string userId,
                            string currency,
                            decimal amount,
                            UserRepository userRepository) // imagine a lot of other dependencies
    {
        this.serviceId = serviceId;
        this.userId = userId;
        this.currency = currency;
        this.amount = amount;
        this.userRepository = userRepository;
    }

    public bool Process()
    {
        var user = this.userRepository.GetUser(this.userId);

        // process something out of scope of this example

        return (Random.Shared.Next() % 2) == 0;
    }
}
