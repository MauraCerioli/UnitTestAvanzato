namespace IEmailSender {
    public interface IEmailSender {
        bool SendEmail(string to, string body);
    }
}
