namespace AutomaticFactoryWithMsDI;

internal sealed class PaymentProcessorWrapper
{
    private readonly UserRepository userRepository;

    public PaymentProcessorWrapper(UserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public bool Process(string serviceId,
                        string userId,
                        string currency,
                        decimal amount)
    {
        var processor = new PaymentProcessor(serviceId,
                                             userId,
                                             currency,
                                             amount,
                                             this.userRepository); // pass everything down

        bool result = processor.Process();

        return result;
    }
}
