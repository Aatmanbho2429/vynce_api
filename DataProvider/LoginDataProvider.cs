using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Data.SqlClient;
using MimeKit;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class LoginDataProvider: BaseDataProvider, ILoginDataProvider
    {
        private IDataProviderHelper _dataProviderHelper;
        public LoginDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region reader
        private async Task<MemberAuthResponse> ReaderMemberGet(SqlDataReader dataReader)
        {
            MemberAuthResponse response = new MemberAuthResponse();

            while (await dataReader.ReadAsync())
            {
                response = (new MemberAuthResponse
                {
                    member_id = ConvertString(dataReader, "member_id"),
                    email = ConvertString(dataReader, "email"),
                    name = ConvertString(dataReader, "name"),
                    role_id = ConvertString(dataReader, "role_id"),
                    membership_id = ConvertString(dataReader, "membership_id"),
                    o_output_status = ConvertIntiger(dataReader, "o_output_status"),
                    o_output_message = ConvertString(dataReader, "o_output_message"),
                });
            }
            return response;
        }

        private async Task<MemberGetEmailPasswordResponse> ReaderMemberGetEmailPassword(SqlDataReader reader)
        {
            MemberGetEmailPasswordResponse response = new MemberGetEmailPasswordResponse();
            while (await reader.ReadAsync())
            {
                response.member_id = ConvertString(reader, "member_id");
                response.name = ConvertString(reader, "name");
                response.email = ConvertString(reader, "email");
                response.phone = ConvertString(reader, "phone");
                response.password = ConvertString(reader, "password");
                response.role_id = ConvertIntiger(reader, "role_id");
                response.status = ConvertIntiger(reader, "status");
                response.membership_id = ConvertIntiger(reader, "membership_id");
                response.created_date = ConvertToDate(reader, "created_date");
                response.modified_date = ConvertToDate(reader, "modified_date");
                response.membership_start_date = ConvertToDate(reader, "membership_start_date");
                response.membership_end_date = ConvertToDate(reader, "membership_end_date");
            }
            return response;
        }
        #endregion
        public async Task<BaseResponse<MemberAuthResponse>> MemberAuth(string email, string password)
        {
            string decrypt_pass=CryptoJsAes.Decrypt(password, ApplicationConfigurations.encryptionKey);
            string decrypted_password = System.Text.Json.JsonSerializer.Deserialize<string>(decrypt_pass);
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_email",email),
                new SqlParameter("i_password",decrypted_password),
            };

            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.LOGIN_V1_PROC, ReaderMemberGet, sqlParameters.ToArray());

            return new BaseResponse<MemberAuthResponse>()
            {
                data = response,
                status = 1
            };
        }

        public async Task<BaseResponse<int>> Register(RegistrationRequest request)
        {
            string newPassword = GenerateRandomPassword();
            var created_date = DateTime.UtcNow;
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_email",request.email),
                new SqlParameter("i_phone",request.phone),
                new SqlParameter("i_password",newPassword),
                new SqlParameter("i_role_id",2),
                new SqlParameter("i_status",1),
                new SqlParameter("i_membership_id","0"),
                new SqlParameter("i_created_date",created_date),
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.REGISTER_V1_PROC, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public string GenerateRandomPassword()
        {
            const int length = 8;

            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string allChars = lowerCase + upperCase + numbers;

            var password = new StringBuilder();
            var rng = RandomNumberGenerator.Create();

            byte[] buffer = new byte[1];

            rng.GetBytes(buffer);
            password.Append(lowerCase[buffer[0] % lowerCase.Length]);

            rng.GetBytes(buffer);
            password.Append(upperCase[buffer[0] % upperCase.Length]);

            rng.GetBytes(buffer);
            password.Append(numbers[buffer[0] % numbers.Length]);

            while (password.Length < length)
            {
                rng.GetBytes(buffer);
                password.Append(allChars[buffer[0] % allChars.Length]);
            }

            char[] chars = password.ToString().ToCharArray();
            for (int i = chars.Length - 1; i > 0; i--)
            {
                rng.GetBytes(buffer);
                int j = buffer[0] % (i + 1);

                char temp = chars[i];
                chars[i] = chars[j];
                chars[j] = temp;
            }

            return new string(chars);
        }

        public async Task<BaseResponse<int>> RegistrationMail(RegistrationMailRequest request)
        {
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_email",request.email)
            };

            var responseFromDb = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_GET_EMAIL_PASSWORD_V1, ReaderMemberGetEmailPassword, sqlParameters.ToArray());

            var response = await SendEmail(request.email, responseFromDb.password, responseFromDb.name);

            return new BaseResponse<int>()
            {
                data = response.data,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> SendEmail(string email, string password, string receiverName)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Test Sender", "aatman.bhoraniya@stridelysolutions.com"));
                message.To.Add(MailboxAddress.Parse("aatman.bhoraniya@stridelysolutions.com"));
                message.Subject = "Login Credentials";

                string htmlMessage = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
    <title>Login Credentials</title>
</head>
<body style=""margin:0; padding:0; background-color:#f4f6f8; font-family:Arial, Helvetica, sans-serif;"">

    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f4f6f8; padding:30px 0;"">
        <tr>
            <td align=""center"">
                <table width=""600"" cellpadding=""0"" cellspacing=""0""
                       style=""background-color:#ffffff; border-radius:8px; overflow:hidden; box-shadow:0 2px 8px rgba(0,0,0,0.08);"">

                    <!-- Header -->
                    <tr>
                        <td style=""background-color:#0d6efd; padding:20px; text-align:center; color:#ffffff;"">
                            <h2 style=""margin:0; font-size:22px;"">
                                [#title#]
                            </h2>
                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style=""padding:30px; color:#333333;"">
                            <p style=""margin:0 0 15px 0; font-size:15px;"">
                                Hello [#recipientName#],
                            </p>

                            <p style=""margin:0 0 20px 0; font-size:15px;"">
                                [#description#]
                            </p>
                            <p style=""margin:0 0 20px 0; font-size:15px;color:red;text-transform:uppercase"">
                                [#warning#]
                            </p>

                            <table width=""100%"" cellpadding=""0"" cellspacing=""0""
                                   style=""background-color:#f8f9fa; border:1px solid #dee2e6; border-radius:6px; padding:15px;"">
                                <tr>
                                    <td style=""padding:8px 0; font-size:14px;"">
                                        <strong>Email:</strong> [#email#]
                                    </td>
                                </tr>
                                <tr>
                                    <td style=""padding:8px 0; font-size:14px;"">
                                        <strong>Password:</strong> [#password#]
                                    </td>
                                </tr>
                            </table>

                            <p style=""margin:20px 0 0 0; font-size:14px; color:#555555;"">
                                For security reasons, please change your password after logging in.
                            </p>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style=""background-color:#f1f3f5; padding:15px; text-align:center; font-size:12px; color:#6c757d;"">
                            © [#year#] Your Company Name. All rights reserved.
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>

</body>
</html>";
                htmlMessage = htmlMessage
     .Replace("[#title#]", "Welcome to Vynce – Your Login Credentials")
     .Replace("[#description#]",
         "Welcome to Vynce. Your account has been successfully created.<br/><br/>" +
         "Below are your login credentials to access the system. Please use these details to sign in for the first time.")
     .Replace("[#warning#]", "This information is confidential. Please do not share your login credentials with anyone.")
     .Replace("[#recipientName#]", receiverName)
     .Replace("[#email#]", email)
     .Replace("[#password#]", password)
     .Replace("[#year#]", DateTime.Now.Year.ToString());

                message.Body = new TextPart("html")
                {
                    Text = htmlMessage
                };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(
                        "aatmanbhoraniya12@gmail.com",
                        "xnlhypclblvdqvgc"
                    );

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return new BaseResponse<int>()
                {
                    data = 1,
                    message = "Password send in your email"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    data = 0,
                    message = "Technical error occured!"
                };
            }
        }
    }
}
