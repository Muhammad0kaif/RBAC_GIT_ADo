namespace AdoApi2.Repositories.Interfaces
{
    public interface IEmailTemplateService
    {
        string BuildNewUserEmail( string name,string email, string temporaryPassword,  string loginUrl);
    }
}
