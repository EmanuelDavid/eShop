using CatalogMicroS.Controllers;
using Moq;
using NUnit.Framework;

namespace CatalogMicroS.DL
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public void TestBaseClass()
        {

            var expected = new Site { Name = "It s working" };
            Mock<ISiteRepository> siteRepo = new Mock<ISiteRepository>();
            siteRepo.Setup(M => M.GetAllFromBase()).Returns(new Site { Name = "It s working"});

            SchimmerController controller = new SchimmerController(siteRepo.Object);

            Assert.AreSame(expected, controller.Index());
        }
    }
}
