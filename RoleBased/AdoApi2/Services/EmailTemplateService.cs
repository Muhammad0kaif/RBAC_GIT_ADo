using AdoApi2.Repositories.Interfaces;


namespace AdoApi2.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        public string BuildNewUserEmail(
            string name,
            string email,
            string temporaryPassword,
            string loginUrl)
        {
            return $@"
<div style='font-family: Arial, sans-serif; background-color:#f4f6f9; padding:30px;'>

    <div style='max-width:600px; margin:auto; background:white; padding:30px; border-radius:10px; box-shadow:0 2px 8px rgba(0,0,0,0.1);'>

        <h2 style='color:#0d6efd; text-align:center;'>
            Welcome to RBAC System
        </h2>

        <p>Hello <strong>{name}</strong>,</p>

        <p>
            Your account has been created successfully by the administrator.
        </p>

        <div style='background:#f8f9fa; padding:15px; border-radius:8px; margin:20px 0;'>

            <p style='margin:5px 0;'>
                <strong>Email:</strong> {email}
            </p>

            <p style='margin:5px 0;'>
                <strong>Temporary Password:</strong>
                <span style='color:#dc3545; font-weight:bold;'>{temporaryPassword}</span>
            </p>

        </div>

        <p style='color:#dc3545;'>
            For security reasons, you must change your password after first login.
        </p>

        <div style='text-align:center; margin:30px 0;'>

            <a href='{loginUrl}'
               style='background:#0d6efd; color:white; padding:12px 25px; text-decoration:none; border-radius:6px; font-weight:bold; display:inline-block;'>
                Login Now
            </a>

        </div>

        <p style='font-size:13px; color:#6c757d;'>
            If the button does not work, copy and paste this link into your browser:
        </p>

        <p style='font-size:13px; word-break:break-all;'>
            <a href='{loginUrl}'>{loginUrl}</a>
        </p>

        <hr />

        <p style='font-size:12px; color:#6c757d; text-align:center;'>
            This is an automated email. Please do not reply.
        </p>

    </div>

</div>";
        }
    }
}