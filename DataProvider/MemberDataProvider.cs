using Azure;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using MimeKit;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Data;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using vynce_api.DataProvider.Interface;
using vynce_api.Model;
using vynce_api.Model.Request;
using vynce_api.Model.Response;

namespace vynce_api.DataProvider
{
    public class MemberDataProvider: BaseDataProvider,IMemberDataProvider
    {
        IDataProviderHelper _dataProviderHelper;
        public MemberDataProvider(IDataProviderHelper dataProviderHelper)
        {
            _dataProviderHelper = dataProviderHelper;
        }

        #region reader 
        public async Task<MemberListResponse> MemberListReader(SqlDataReader reader)
        {
            MemberListResponse response = new MemberListResponse();
            var members = new List<Member>();

            while (await reader.ReadAsync())
            {
                members.Add(new Member()
                {
                    member_id = ConvertString(reader, "member_id"),
                    name = ConvertString(reader, "name"),
                    email = ConvertString(reader, "email"),
                    phone = ConvertString(reader, "phone"),
                    role_id = ConvertIntiger(reader, "role_id"),
                    status = ConvertIntiger(reader, "status"),
                    membership_id = ConvertIntiger(reader, "membership_id"),
                    created_date = ConvertToDate(reader, "created_date"),
                    modified_date = ConvertToDate(reader, "modified_date"),
                    membership_start_date = ConvertToDate(reader, "membership_start_date"),
                    membership_end_date = ConvertToDate(reader, "membership_end_date")
                });
            }

            if (reader.NextResult())
            {
                while (await reader.ReadAsync())
                {
                    response.total_count = ConvertIntiger(reader, "total_record");
                }
            }

            response.list = members;
            return response;
        }

        public async Task<MemberGetResponse> MemberGetReader(SqlDataReader reader)
        {
            MemberGetResponse response = new MemberGetResponse();
            while (await reader.ReadAsync())
            {
                response.member_id = ConvertString(reader, "member_id");
                response.name = ConvertString(reader, "name");
                response.email = ConvertString(reader, "email");
                response.phone = ConvertString(reader, "phone");
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

        public async Task<MemberGetProfileResponse> MemberGetProfileReader(SqlDataReader reader)
        {
            MemberGetProfileResponse response = new MemberGetProfileResponse();
            while (await reader.ReadAsync())
            {
                response.member_id = ConvertString(reader, "member_id");
                response.name = ConvertString(reader, "name");
                response.email = ConvertString(reader, "email");
                response.phone = ConvertString(reader, "phone");
                response.membership_name = ConvertString(reader, "membership_name");
                response.role_id = ConvertIntiger(reader, "role_id");
                response.status = ConvertIntiger(reader, "status");
                response.membership_id = ConvertIntiger(reader, "membership_id");
                response.membership_start_date = ConvertToDate(reader, "membership_start_date");
                response.membership_end_date = ConvertToDate(reader, "membership_end_date");
                response.created_date = ConvertToDate(reader, "created_date");
                response.modified_date = ConvertToDate(reader, "modified_date");
            }
            return response;
        }

        public async Task<MemberGetFreeSearchResponse> MemberGetFreeSearchReader(SqlDataReader reader)
        {
            MemberGetFreeSearchResponse response = new MemberGetFreeSearchResponse();
            while (await reader.ReadAsync())
            {
                response.members_free_search_id = ConvertIntiger(reader, "members_free_search_id");
                response.member_id = ConvertIntiger(reader, "member_id");
                response.free_search_remaining = ConvertIntiger(reader, "free_search_remaining");
                response.created_date = ConvertToDate(reader, "created_date");
                response.modified_date = ConvertToDate(reader, "modified_date");
            }
            return response;
        }
        public async Task<MemberForgetPasswordResponse> ForgetPasswordReader(SqlDataReader reader)
        {
            MemberForgetPasswordResponse response = new MemberForgetPasswordResponse();
            while (await reader.ReadAsync())
            {
                response.member_id = ConvertIntiger(reader, "member_id");
                response.password = ConvertString(reader, "password");
                response.name = ConvertString(reader, "name");
            }
            return response;
        }
        #endregion

        public async Task<BaseResponse<int>> AddMember(MemberAddRequest request)
        {
            string decrypt_pass = CryptoJsAes.Decrypt(request.password, ApplicationConfigurations.encryptionKey);
            string decrypted_password = System.Text.Json.JsonSerializer.Deserialize<string>(decrypt_pass);
            var created_date = DateTime.UtcNow;
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_email",request.email),
                new SqlParameter("i_phone",request.phone),
                new SqlParameter("i_password",decrypted_password),
                new SqlParameter("i_role_id",request.role_id),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_membership_id",request.membership_id),
                new SqlParameter("i_created_date",created_date),
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_ADD_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<MemberListResponse>> MemberList(MemberListRequest request)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("page_size",request.page_size),
                new SqlParameter("page_no",request.page_no),
                new SqlParameter("sorting_by",request.sorting_by),
                new SqlParameter("sorting_column",request.sorting_column)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_LIST_V1, MemberListReader, sqlParameters.ToArray());

            return new BaseResponse<MemberListResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<MemberGetResponse>> MemberGet(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_GET_V1, MemberGetReader, sqlParameters.ToArray());

            return new BaseResponse<MemberGetResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<MemberGetProfileResponse>> MemberGetProfile(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_GET_PROFILE_V1, MemberGetProfileReader, sqlParameters.ToArray());

            return new BaseResponse<MemberGetProfileResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<MemberGetFreeSearchResponse>> MemberGetFreeSearch(string id)
        {
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id)
            };
            var response = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_FREE_SEARCH_GET_V1, MemberGetFreeSearchReader, sqlParameters.ToArray());

            return new BaseResponse<MemberGetFreeSearchResponse>()
            {
                data = response
            };
        }

        public async Task<BaseResponse<int>> MemberDelete(string id)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id),
                new SqlParameter("i_modified_date",modified_date)
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });

            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_DELETE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> MemberUpdate(string id, MemberUpdateRequest request)
        {
            var modified_date = DateTime.Now;
            var sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("i_member_id",id),
                new SqlParameter("i_name",request.name),
                new SqlParameter("i_email",request.email),
                new SqlParameter("i_phone",request.phone),
                new SqlParameter("i_role_id",request.role_id),
                new SqlParameter("i_status",request.status),
                new SqlParameter("i_membership_id",request.membership_id),
                new SqlParameter("i_modified_date",modified_date),
            };
            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_UPDATE_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> MemberExist(MemberExistEmailRequest request)
        {
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_email",request.email)
            };

            sqlParameters.Add(new SqlParameter("o_output_message", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output });
            sqlParameters.Add(new SqlParameter("o_output_status", SqlDbType.Int) { Direction = ParameterDirection.Output });
            var response = await _dataProviderHelper.ExecuteNonQueryAsync(Procedures.MEMBER_EXIST_EMAIL_V1, sqlParameters.ToArray());

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> MemberSendEmail(MemberSendEmailRequest request)
        {
            string newPassword = GenerateRandomPassword();
            var sqlParameters = new List<SqlParameter>() {
                new SqlParameter("i_email",request.email),
                new SqlParameter("i_new_password",newPassword)
            };
            var responseFromDb = await _dataProviderHelper.ExecuteReaderAsync(Procedures.MEMBER_FORGET_PASSWORD_V1, ForgetPasswordReader, sqlParameters.ToArray());

            var response = await SendEmail(request.email, responseFromDb.password,responseFromDb.name);

            return new BaseResponse<int>()
            {
                data = response.status,
                message = response.message
            };
        }

        public async Task<BaseResponse<int>> SendEmail(string email,string password,string receiverName)
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
                    .Replace("[#title#]","Password Changed")
                    .Replace("[#description#]", "Your password changed successfully. Below are your login cridentials")
                .Replace("[#warning#]", "Please do not share with anyone.")
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
    }
}
