namespace BulkEmailSender {
    public class BulkEmailSender {
        private readonly IEmailSender.IEmailSender _emailSender;
        private readonly string _footer;

        public BulkEmailSender(IEmailSender.IEmailSender emailSender, string footer) {
            this._emailSender = emailSender;
            this._footer = footer;
        }

        public void SendEmail(List<string> addresses, string body) {
            if (addresses == null) throw new ArgumentNullException("addresses");
            if (body == null) throw new ArgumentNullException("body");
            foreach (var a in addresses) {
                if (!this._emailSender.SendEmail(a, body + this._footer))
                    throw new Exception("Cannot send email");
            }
        }
    }

}
