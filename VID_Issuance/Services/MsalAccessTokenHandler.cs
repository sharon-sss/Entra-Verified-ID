using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Security.Cryptography.X509Certificates;

namespace VID_Issuance.Services
{
    public class MsalAccessTokenHandler
    {
        private static X509Certificate2 ReadCertificate(string certificateName)
        {
            if (string.IsNullOrWhiteSpace(certificateName))
            {
                throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json", "certificateName");
            }
            CertificateDescription certificateDescription = CertificateDescription.FromStoreWithDistinguishedName(certificateName);
            DefaultCertificateLoader defaultCertificateLoader = new DefaultCertificateLoader();
            defaultCertificateLoader.LoadIfNeeded(certificateDescription);
            return certificateDescription.Certificate;
        }
        public static async Task<(string token, string error, string error_description)> GetAccessToken(IConfiguration configuration)
        {
            string tenantId = configuration.GetValue("VerifiedID:TenantId", configuration["AzureAd:TenantId"]);
            string authority = $"{configuration["VerifiedID:Authority"]}{tenantId}";
            string clientId = configuration.GetValue("VerifiedID:ClientId", configuration["AzureAd:ClientId"]);
            string clientSecret = configuration.GetValue("VerifiedID:ClientSecret", "");
            // Since we are using application permissions this will be a confidential client application
            IConfidentialClientApplication app;
            if (!string.IsNullOrWhiteSpace(clientSecret))
            {
                app = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri(authority))
                    .Build();
            }
            else
            {
                X509Certificate2 certificate = ReadCertificate(configuration["VerifiedID:CertificateName"]);
                app = ConfidentialClientApplicationBuilder.Create(clientId)
                    .WithCertificate(certificate)
                    .WithAuthority(new Uri(authority))
                    .Build();
            }

            app.AddDistributedTokenCache(services => {
                services.AddDistributedMemoryCache();
                services.AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = Microsoft.Extensions.Logging.LogLevel.Debug);
            });

            string[] scopes = new string[] { configuration["VerifiedID:scope"] };

            AuthenticationResult result = null;
            try
            {
                result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                return (string.Empty, "500", "Scope provided is not supported");
            }
            catch (MsalServiceException ex)
            {
                return (String.Empty, "500", "Something went wrong getting an access token for the client API:" + ex.Message);
                //return BadRequest(new { error = "500", error_description = "Something went wrong getting an access token for the client API:" + ex.Message });
            }

            return (result.AccessToken, String.Empty, String.Empty);
        }
    }
}
