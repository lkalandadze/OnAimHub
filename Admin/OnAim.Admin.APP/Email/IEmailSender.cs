namespace OnAim.Admin.APP.Email
{
    public interface IEmailSender
    {
        Task SendAsync(EmailObject emailObject);
    }
}
