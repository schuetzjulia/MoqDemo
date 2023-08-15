namespace TestWebapp.Services;

public interface IPaymentService
{
    bool Charge(double total, Services.ICard card);
}