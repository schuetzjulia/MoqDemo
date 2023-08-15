namespace TestWebapp.Services;

public interface ICartService
{
    double Total();
    IEnumerable<Services.ICartItem> Items();
}