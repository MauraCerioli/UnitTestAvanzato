using Moq;
using IEmailSender;
namespace TestProjectWithMoq {
    public class Tests {
        [Test]
        public void SendEmail_WhenEmailSenderFails_ThrowsException() {
            var moq = new Mock<IEmailSender.IEmailSender>();
            moq.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            var bulk = new BulkEmailSender.BulkEmailSender(moq.Object, string.Empty);
            var addresses = new List<string> { "a@a.com" };
            Assert.That(() => bulk.SendEmail(addresses, string.Empty), Throws.TypeOf<Exception>());
        }
        [Test]
        public void SendEmail_Passing3Addresses_Sends3Mails() {
            var moq = new Mock<IEmailSender.IEmailSender>();
            moq.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            var sendMailMock = moq.Object;
            var bulk = new BulkEmailSender.BulkEmailSender(sendMailMock, string.Empty);
            var addresses = new List<string> { "a@a.com", "b@b.org", "c@c.info" };
            bulk.SendEmail(addresses, string.Empty);
            moq.Verify(m=> m.SendEmail(It.IsAny<string>(), It.IsAny<string>()),Times.Exactly(3));
        }
        [TestCase("a@a.com", "b@b.org", "c@c.info")]
        public void SendEmail_PassingNAddresses_SendsNMailsToCorrectAddresses(params string[] addresses) {
            var moq = new Mock<IEmailSender.IEmailSender>();
            foreach (var a in addresses) moq.Setup(m => m.SendEmail(a, It.IsAny<string>())).Returns(true);
            /*
             var address1 = "a@a.com";
            moq.Setup(m => m.SendEmail(address1, It.IsAny<string>())).Returns(true);
            var address2 = "b@b.org";
            moq.Setup(m => m.SendEmail(address2, It.IsAny<string>())).Returns(true);
            var address3 = "c@c.info";
             var addresses = new List<string> { address1, address2, address3 };
           moq.Setup(m => m.SendEmail(address3, It.IsAny<string>())).Returns(true);
            var sendMailMock = moq.Object;
            var bulk = new BulkEmailSender.BulkEmailSender(sendMailMock, string.Empty);
            */
            var bulk = new BulkEmailSender.BulkEmailSender(moq.Object, string.Empty);
            bulk.SendEmail(addresses.ToList(), string.Empty);
            moq.Verify(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(addresses.Count));
            foreach (var a in addresses) {
                moq.Verify(m => m.SendEmail(a, It.IsAny<string>()), Times.Exactly(1));
            }
            /*
            moq.Verify(m => m.SendEmail(address1, It.IsAny<string>()), Times.Exactly(1));
            moq.Verify(m => m.SendEmail(address2, It.IsAny<string>()), Times.Exactly(1));
            moq.Verify(m => m.SendEmail(address3, It.IsAny<string>()), Times.Exactly(1));
            */
        }
    }
}