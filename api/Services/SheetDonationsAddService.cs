using AmoToSheetFunc;
using AmoToSheetFunc.Dtos;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Api.Services
{
    public class SheetDonationsAddService
    {
        const string SPREADSHEET_ID = "1_7EVuWmnobK6jDQDUUKh7a1zJwW4_6DrhH4VKNiNwDE";
        const string SHEET_NAME = "Регулярные пожертвования";

        GoogleSheetsHelper _googleSheetsHelper;

        public SheetDonationsAddService(string googleCredsJson)
        {
            _googleSheetsHelper = new GoogleSheetsHelper(googleCredsJson);
        }

        public string GetValues()
        {
            var range = $"{SHEET_NAME}!A:B";
            var googleSheetValues = _googleSheetsHelper.Service.Spreadsheets.Values;
            var request = googleSheetValues.Get(SPREADSHEET_ID, range);
            var response = request.Execute();
            var values = response.Values;

            return values.Count.ToString();
        }

        public void AddNewRow(Donation donation)
        {

            // var updateRange = $"{SHEET_NAME}!A{2}:E{2}";
            var appendRange = $"{SHEET_NAME}!A:E";
            var valueRange = new ValueRange
            {
                Values = new List<IList<object>> { new List<object>
                {
                    donation.AmoLeadId, 
                    donation.ContactName, 
                    donation.Amount ?? "", 
                    donation.Date.ToString("dd.MM.yyyy HH:mm:ss")
                } }
            };
            var googleSheetValues = _googleSheetsHelper.Service.Spreadsheets.Values;
            var appendRequest = googleSheetValues.Append(valueRange, SPREADSHEET_ID, appendRange);
            // var updateRequest = googleSheetValues.Update(valueRange, SPREADSHEET_ID, updateRange);
            appendRequest.ValueInputOption =
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            
            appendRequest.Execute();
        }
    }
}