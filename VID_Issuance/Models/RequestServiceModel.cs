using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace VID_Issuance.Models
{
    public class IssuanceRequest
    {
        public string authority { get; set; }
        public bool includeQRCode { get; set; }
        public Registration registration { get; set; }
        public Callback callback { get; set; }
        public string type { get; set; }
        public string manifest { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Pin pin { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> claims;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string expirationDate { get; set; } // format "2024-10-20T14:52:39.043Z"
    }

    public class PresentationRequest
    {
        public string authority { get; set; }
        public bool includeQRCode { get; set; }
        public Registration registration { get; set; }
        public Callback callback { get; set; }
        //public Presentation presentation { get; set; }
        public bool includeReceipt { get; set; }
        public List<RequestedCredential> requestedCredentials { get; set; }
    }

    public class Configuration
    {
        public Validation validation { get; set; }
    }

    public class Validation
    {
        public bool allowRevoked { get; set; } // default false
        public bool validateLinkedDomain { get; set; } // default false
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public FaceCheck faceCheck { get; set; }
    }

    public class FaceCheck
    {
        public string sourcePhotoClaimName { get; set; }
        public int matchConfidenceThreshold { get; set; }
    }

    public class Registration
    {
        public string clientName { get; set; }
        public string purpose { get; set; }
    }
    public class Callback
    {
        public string url { get; set; }
        public string state { get; set; }
        public Dictionary<string, string> headers { get; set; }
    }

    public class Pin
    {
        public string value { get; set; }
        public int length { get; set; }
    }

    public class RequestedCredential
    {
        public string type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> acceptedIssuers { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Configuration configuration { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Constraint> constraints { get; set; }

    }

    public class Constraint
    {
        public string claimName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> values { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string contains { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string startsWith { get; set; }
    }

    public class CallbackEvent
    {
        public string requestId { get; set; }
        public string requestStatus { get; set; }
        public Error error { get; set; }
        public string state { get; set; }
        public string subject { get; set; }
        public ClaimsIssuer[] verifiedCredentialsData { get; set; }
        public Receipt receipt { get; set; }
        public string photo { get; set; }

    }

    public class Receipt
    {
        //public string vp_token { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(VpTokenJsonConverter<string>))]
        public List<string> vp_token { get; set; }
    }

    internal class VpTokenJsonConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(List<T>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);
            if (token.Type == JTokenType.Array)
                return token.ToObject<List<T>>();
            return new List<T> { token.ToObject<T>() };
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    public class Error
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ClaimsIssuer
    {
        public string issuer { get; set; }
        public string domain { get; set; }
        public string verified { get; set; }
        public string[] type { get; set; }
        public IDictionary<string, string> claims { get; set; }
        public CredentialState credentialState { get; set; }
        public FaceCheckResult faceCheck { get; set; }
        public DomainValidation domainValidation { get; set; }
        public string expirationDate { get; set; }
        public string issuanceDate { get; set; }
    }

    public class CredentialState
    {
        public string revocationStatus { get; set; }
        [JsonIgnore]
        public bool isValid { get { return revocationStatus == "VALID"; } }
    }

    public class DomainValidation
    {
        public string url { get; set; }
    }

    public class FaceCheckResult
    {
        public double matchConfidenceScore { get; set; }
    }

}
