namespace TestBulkEmailSender {
    public class SendMailMock : IEmailSender.IEmailSender {
        private bool returnValue;
        public int Calls { get; set; }
        public Dictionary<string,int> CallMap { get; set; }
        public SendMailMock(bool b) {
            returnValue = b;
            Calls = 0;
            CallMap = new Dictionary<string, int>();
        }
        public bool SendEmail(string to, string body) {
            Calls++;
            if (!CallMap.TryAdd(to,1)) CallMap[to]++;
            return returnValue;
        }
    }
    public class SendMailStub : IEmailSender.IEmailSender {
        private bool returnValue;
        public SendMailStub(bool b) {
            returnValue = b;
        }
        public bool SendEmail(string to, string body) {
            return returnValue;
        }
    }
    public class Tests {


        [Test]
        public void SendEmail_WhenEmailSenderFails_ThrowsException() {
            var bulk = new BulkEmailSender.BulkEmailSender(new SendMailStub(false), string.Empty);
            var addresses = new List<string> { "a@a.com" };
            Assert.That(() => bulk.SendEmail(addresses, string.Empty), Throws.TypeOf<Exception>());
        }
        [Test]
        public void SendEmail_Passing3Addresses_Sends3Mails() {
            var sendMailMock = new SendMailMock(true);
            var bulk = new BulkEmailSender.BulkEmailSender( sendMailMock, string.Empty);
            var addresses = new List<string> { "a@a.com", "b@b.org", "c@c.info" };
            bulk.SendEmail(addresses, string.Empty);
            Assert.That(sendMailMock.Calls,Is.EqualTo(3));
        }
    }
}