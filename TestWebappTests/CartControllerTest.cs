using Moq;
using TestWebapp.Controllers;
using TestWebapp.Services;

namespace TestWebappTests;

public class CartControllerTest
{

      private CartController controller;
      private Mock<IPaymentService> paymentServiceMock;
      private Mock<ICartService> cartServiceMock;

      private Mock<IShipmentService> shipmentServiceMock;
      private Mock<Services.ICard> cardMock;
      private Mock<Services.IAddressInfo> addressInfoMock;
      private List<Services.ICartItem> items;

      [SetUp]
      public void Setup()
      {
          
          cartServiceMock = new Mock<ICartService>();
          paymentServiceMock = new Mock<IPaymentService>();
          shipmentServiceMock = new Mock<IShipmentService>();

          // arrange
          cardMock = new Mock<Services.ICard>();
          addressInfoMock = new Mock<Services.IAddressInfo>();

          // 
          var cartItemMock = new Mock<Services.ICartItem>();
          cartItemMock.Setup(item => item.Price).Returns(10);

          items = new List<Services.ICartItem>()
          {
              cartItemMock.Object
          };

          cartServiceMock.Setup(c => c.Items()).Returns(items.AsEnumerable());

          controller = new CartController(cartServiceMock.Object, paymentServiceMock.Object, shipmentServiceMock.Object);
      }

      [Test]
      public void ShouldReturnCharged()
      {
          paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(true);

          // act
          var result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

          // assert
          // myInterfaceMock.Verify((m => m.DoesSomething()), Times.Once());
          shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Once());

          Assert.AreEqual("charged", result);
      }

      [Test]
      public void ShouldReturnNotCharged() 
      {
          paymentServiceMock.Setup(p => p.Charge(It.IsAny<double>(), cardMock.Object)).Returns(false);

          // act
          var result = controller.CheckOut(cardMock.Object, addressInfoMock.Object);

          // assert
          shipmentServiceMock.Verify(s => s.Ship(addressInfoMock.Object, items.AsEnumerable()), Times.Never());
          Assert.AreEqual("not charged", result);
      }
  }
