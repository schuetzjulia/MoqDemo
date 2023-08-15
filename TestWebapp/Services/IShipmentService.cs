namespace TestWebapp.Services;

public interface IShipmentService
{
    void Ship(Services.IAddressInfo info, IEnumerable<Services.ICartItem> items);
}