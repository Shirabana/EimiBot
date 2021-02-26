using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EimiBot2.Programs
{
    class SheetsInteraction
    {
        private string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private string _applicationName = "EimiBot Project";
        private string _spreadsheetId = "1h0z0V8gZYQUtCT7Kd70nHHfSfCHvd58RyM7ZaRKKkXA";
        private string _sheetname = "";
        private SheetsService _sheetsService;

        private void GoogleConnection()
        {
            //GoogleCredential credential;

            //using (var stream = new FileStream(Path.Combine(HttpRuntime.BinDirectory, @"C:\Users\Amagi\source\repos\EimiBot\credentials.json"), FileMode.Open, FileAccess.Read))
            //{
            //    credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            //}

            var credential = GoogleCredential.FromStream(new FileStream(@"C:\Users\Amagi\source\repos\EimiBot\credentials.json", FileMode.Open)).CreateScoped(_scopes);

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
            _sheetname = sheetName;
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

            BatchUpdateSpreadsheetResponse response = batchUpdateRequest.Execute();

            return JsonConvert.SerializeObject(response);
        }

        public string EditData(List<Messages> msgList)
        {
            string resp = "";

            foreach (Messages m in msgList)
            {
                IList<Object> obj = new List<Object>();
                obj.Add(m.Timestamp);
                obj.Add(m.Author);
                obj.Add(m.MessageContent);
                IList<IList<Object>> values = new List<IList<Object>>();
                values.Add(obj);

                // The A1 notation of a range to search for a logical table of data.
                // Values will be appended after the last row of the table.
                string range = _sheetname + "!A1:Y";

                // How the input data should be interpreted.
                SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum valueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;

                // How the input data should be inserted.
                SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum insertDataOption = SpreadsheetsResource.ValuesResource.AppendRequest.InsertDataOptionEnum.INSERTROWS;
                ValueRange requestBody = new ValueRange();
                requestBody.Range = range;
                requestBody.Values = values;

                SpreadsheetsResource.ValuesResource.AppendRequest request = _sheetsService.Spreadsheets.Values.Append(requestBody, _spreadsheetId, range);
                request.ValueInputOption = valueInputOption;
                request.InsertDataOption = insertDataOption;

                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                AppendValuesResponse response = request.Execute();
                // Data.AppendValuesResponse response = await request.ExecuteAsync();

                // TODO: Change code below to process the `response` object:
                // Console.WriteLine(JsonConvert.SerializeObject(response));

                resp = JsonConvert.SerializeObject(response);
            }

            return resp;
        }
    }
}
