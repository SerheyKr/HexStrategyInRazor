namespace HexStrategyInRazor.EmailService
{
    public class EmailServiceSettings : Singleton<EmailServiceSettings>
    {
        public string EmailForSendingMessages { get; set; }

        public EmailServiceSettings()
        {
            Program.Configuration.GetSection("emailSettings").Bind(this);
        }
    }
}
