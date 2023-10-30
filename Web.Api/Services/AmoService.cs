using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Api.Services
{
    public class AmoService: IDisposable
    {
        private readonly string _amoAccessToken;
        private readonly HttpClient _httpClient;

        public AmoService(string amoAccessToken, HttpClient? httpClient = null)
        {
            _amoAccessToken = amoAccessToken;
            _httpClient = httpClient ?? new HttpClient();
        }

        public async Task<AmoLead> GetAmoLead(long leadId)
        {
            var url = $"https://anyor.amocrm.ru/api/v4/leads/{leadId}?with=contacts";
            
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _amoAccessToken);
            
            var response =  await httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) 
                throw new Exception("Error while getting Amo lead");
            
            var responseString = response.Content.ReadAsStringAsync().Result;
            var amoLead = JsonConvert.DeserializeObject<AmoLead>(responseString);
            if (amoLead == null)
                throw new Exception("Error while parsing Amo lead");
            
            return amoLead;
        }
        
        public AmoContact GetAmoContact(long contactId)
        {
            var url = $"https://anyor.amocrm.ru/api/v4/contacts/{contactId}";
            
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _amoAccessToken);
            
            using var response =  httpClient.GetAsync(url).Result;
            if (!response.IsSuccessStatusCode) 
                throw new Exception("Error while getting Amo lead");
            
            var responseString = response.Content.ReadAsStringAsync().Result;
            var amoLead = JsonConvert.DeserializeObject<AmoContact>(responseString);
            if (amoLead == null)
                throw new Exception("Error while parsing Amo contact");
            
            return amoLead;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }

    #region Entities

    public class AmoLead
    {
        [JsonProperty("id")] 
        public long Id { get; set; }

        [JsonProperty("name")] 
        public string Name { get; set; } = string.Empty;
        
        [JsonProperty("price")] 
        public decimal Price { get; set; }
        
        [JsonProperty("responsible_user_id")] 
        public long ResponsibleUserId { get; set; }
        
        [JsonProperty("group_id")] 
        public long GroupId { get; set; }
        
        [JsonProperty("status_id")] 
        public long StatusId { get; set; }
        
        [JsonProperty("pipeline_id")] 
        public long PipelineId { get; set; }
        
        [JsonProperty("created_by")] 
        public long CreatedBy { get; set; }
        
        [JsonProperty("updated_by")] 
        public long UpdatedBy { get; set; }
        
        [JsonProperty("created_at")] 
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")] 
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("closed_at")] 
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime ClosedAt { get; set; }

        [JsonProperty("is_deleted")] 
        public bool IsDeleted { get; set; }

        [JsonProperty("custom_fields_values")]
        public IEnumerable<AmoCustomField> CustomFieldsValues { get; set; } = new List<AmoCustomField>();
        
        [JsonProperty("account_id")] 
        public long AccountId { get; set; }

        [JsonProperty("_embedded")] 
        public AmoLeadEmbedded Embedded { get; set; } = new AmoLeadEmbedded();
    }

    public class AmoLeadEmbedded
    {
        [JsonProperty("contacts")]
        public IEnumerable<AmoLeadContactShortItem> Contacts { get; set; } = new List<AmoLeadContactShortItem>();
    }
    
    public class AmoLeadContactShortItem
    {
        [JsonProperty("id")] 
        public long Id { get; set; }
        
        [JsonProperty("is_main")] 
        public bool IsMain { get; set; }
    }

    public class AmoContact
    {
        [JsonProperty("id")] 
        public long Id { get; set; }

        [JsonProperty("name")] 
        public string Name { get; set; } = string.Empty;

        [JsonProperty("first_name")] 
        public string FirstName { get; set; } = string.Empty;

        [JsonProperty("last_name")] 
        public string LastName { get; set; } = string.Empty;

        [JsonProperty("responsible_user_id")] 
        public long ResponsibleUserId { get; set; }
        
        [JsonProperty("group_id")] 
        public long GroupId { get; set; }
        
        [JsonProperty("created_by")] 
        public long CreatedBy { get; set; }
        
        [JsonProperty("updated_by")] 
        public long UpdatedBy { get; set; }
        
        [JsonProperty("created_at")] 
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }
        
        [JsonProperty("updated_at")] 
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime UpdatedAt { get; set; }
        
        [JsonProperty("account_id")] 
        public long AccountId { get; set; }
        
        [JsonProperty("is_deleted")] 
        public bool IsDeleted { get; set; }

        [JsonProperty("custom_fields_values")]
        public IEnumerable<AmoCustomField> CustomFieldsValues { get; set; } = new List<AmoCustomField>();
    }

    
    
    public class AmoCustomField
    {
        [JsonProperty("field_id")] 
        public string FieldId { get; set; } = string.Empty;

        [JsonProperty("field_name")] 
        public string FieldName { get; set; } = string.Empty;

        [JsonProperty("field_code")] 
        public string FieldCode { get; set; } = string.Empty;

        [JsonProperty("field_type")] 
        public string FieldType { get; set; } = string.Empty;

        [JsonProperty("values")] 
        public IEnumerable<AmoCustomFieldValue> Values { get; set; } = new List<AmoCustomFieldValue>();
    }

    public class AmoCustomFieldValue
    {
        [JsonProperty("value")] 
        public string Value { get; set; } = "";

        [JsonProperty("enum_id")] 
        public int EnumId { get; set; }

        [JsonProperty("enum_code")] 
        public string EnumCode { get; set; } = string.Empty;
    }
    
    #endregion // Entities


    #region Helpings

    public class MicrosecondEpochConverter : DateTimeConverterBase
    {
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(((DateTime)value - _epoch).TotalMilliseconds + "000");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return _epoch.AddMilliseconds((long)reader.Value / 1000d);
        }
    }

    #endregion
    
}