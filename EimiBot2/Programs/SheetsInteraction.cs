using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace EimiBot2.Programs
{
    class SheetsInteraction
    {
        private string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private string _applicationName = "EimiBot Project";
        private string _spreadsheetId = "1h0z0V8gZYQUtCT7Kd70nHHfSfCHvd58RyM7ZaRKKkXA";
        private SheetsService _sheetsService;

        private void GoogleConnection()
        {
            GoogleCredential credential;

            using (var stream = new FileStream(Path.Combine(HttpRuntime.BinDirectory, "credentials.json"), FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            // Create Google Sheets API service.
            _sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });
        }

        public string UpdateTab()
        {
            GoogleConnection();

            // Add new Sheet
            string sheetName = string.Format("{0}/{1}/{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            var addSheetRequest = new AddSheetRequest();
            addSheetRequest.Properties = new SheetProperties();
            addSheetRequest.Properties.Title = sheetName;
            BatchUpdateSpreadsheetRequest batchUpdateSpreadsheetRequest = new BatchUpdateSpreadsheetRequest();
            batchUpdateSpreadsheetRequest.Requests = new List<Request>();
            batchUpdateSpreadsheetRequest.Requests.Add(new Request
            {
                AddSheet = addSheetRequest
            });

            var batchUpdateRequest = _sheetsService.Spreadsheets.BatchUpdate(batchUpdateSpreadsheetRequest, _spreadsheetId);

            Google.Apis.Sheets.v4.Data.BatchUpdateSpreadsheetResponse response = batchUpdateRequest.Execute();

            return JsonConvert.SerializeObject(response);
        }

        public string EditData(List<Messages> msgList)
        {
            return "";
        }
    }
}
