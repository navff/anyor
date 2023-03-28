using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;

namespace AmoToSheetFunc
{
    public class GoogleSheetsHelper
    {
        public SheetsService Service { get; set; }
        const string APPLICATION_NAME = "BalanceChecker";
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string _gooleCredsJson;
    
        public GoogleSheetsHelper(string gooleCredsJson)
        {
            _gooleCredsJson = gooleCredsJson;
            var credential = GetCredentialsFromToken();
        
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }

        private GoogleCredential GetCredentialsFromToken()
        {
            var credential = GoogleCredential.FromJson(_gooleCredsJson);
            return credential;
        }
    }
}

