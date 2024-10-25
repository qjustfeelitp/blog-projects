namespace AutomaticFactoryWithMsDI;

public interface IPaymentProcessorFactory
{
    PaymentProcessor Create(string serviceId,
                            string userId,
                            string currency,
                            decimal amount);
}
