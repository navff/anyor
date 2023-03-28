using System.Collections.Generic;

namespace AmoToSheetFunc
{
    public abstract class YaCloudFunctionResponseBase
    {
    }

    public class YaCloudFunctionResponse: YaCloudFunctionResponseBase
    {
        public int StatusCode { get; set; } = 200;
        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> MultiValueHeaders { get; set; } = new Dictionary<string, string>();
        public string Body { get; set; } = string.Empty;
        public bool IsBase64Encoded { get; set; }
    }
    
    public class YaCloudFunctionErrorResponse: YaCloudFunctionResponseBase
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public string ErrorType { get; set; } = string.Empty;
        public string StackTrace { get; set; } = string.Empty;
    }
}