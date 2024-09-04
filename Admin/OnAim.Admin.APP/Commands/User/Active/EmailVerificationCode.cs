namespace OnAim.Admin.APP.Commands.User.Active
{
    public class EmailVerificationCode
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }

        public DateTime SentAt { get; set; }
    }
}
