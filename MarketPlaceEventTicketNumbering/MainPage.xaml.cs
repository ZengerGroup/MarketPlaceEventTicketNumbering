using CommunityToolkit.Maui.Storage;
using Microsoft.Extensions.Configuration;

namespace MarketPlaceEventTicketNumbering
{
    public partial class MainPage : ContentPage
    {
        List<Row> DataRows;
        ProcessHandler Process;
        string OutputPath;
        public MainPage()
        {
            InitializeComponent();
            DataRows = new List<Row>();
            Process = new ProcessHandler();
        }
        private void UpdateData()
        {
            VersionEntry.Text = "";
            MailingEntry.Text = "";
            PrefixEntry.Text = "";
            RecordEntry.Text = "";
            TicketsEntry.Text = "";
            OversEntry.Text = "";
            GroupEntry.Text = "";
            RowTable.Clear();
            for(int i = 0; i < DataRows.Count; i++) RowTable.Children.Add(new RowView(DataRows[i]));
        }
        private void UpdateData(Row lastRow)
        {
            VersionEntry.Text = lastRow.Version;
            MailingEntry.Text = lastRow.MailingName;
            PrefixEntry.Text = lastRow.Prefix;
            RecordEntry.Text = lastRow.Records.ToString();
            TicketsEntry.Text = lastRow.TicketsPerRecord.ToString();
            OversEntry.Text = lastRow.EnteredOvers.ToString();
            GroupEntry.Text = lastRow.GroupName;
            RowTable.Clear();
            for (int i = 0; i < DataRows.Count; i++) RowTable.Children.Add(new RowView(DataRows[i]));
        }
        private void EditButton_Clicked(object sender, EventArgs e)
        {
            Row row = DataRows[^1];
            DataRows.RemoveAt(DataRows.Count - 1);
            UpdateData(row);
        }
        private async void ProcessButton_Clicked(object sender, EventArgs e)
        {
            
            if (DataRows.Count == 0) await DisplayAlertAsync("Data Error", "No data rows entered.", "Okay");
            else
            {
                var result = await FolderPicker.PickAsync(@"Z:\", new CancellationToken());
                if (result.IsSuccessful) OutputPath = result.Folder.Path;
                else OutputPath = @"Z:\_Error_TicketingNumbering";
                Directory.CreateDirectory(OutputPath);
                if (Process.RunProcess(DataRows, OutputPath).Result) await DisplayAlertAsync("Processing Complete", "Please close the program before running again.", "Okay");
                else await DisplayAlertAsync("Processing Error", "Processing could not be completed.", "Okay");
            }
        }
        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            if (!Int32.TryParse(RecordEntry.Text, out _)) await DisplayAlertAsync("Process Error", "Quantity entered is not a valid integer.", "Okay");
            else if (!double.TryParse(OversEntry.Text, out _) && OversEntry.Text != "") await DisplayAlertAsync("Process Error", "Overs percent entered is not a valid value.", "Okay");
            else if (!Int32.TryParse(TicketsEntry.Text, out _) && TicketsEntry.Text != "") await DisplayAlertAsync("Process Error", "Tickets per record entered is not a valid integer.", "Okay");
            else
            {
                string ticketString = (TicketsEntry.Text == "") ? "1" : TicketsEntry.Text;
                string oversString = (OversEntry.Text == "") ? "0" : OversEntry.Text;
                DataRows.Add(new Row(VersionEntry.Text, MailingEntry.Text, PrefixEntry.Text, RecordEntry.Text, ticketString, oversString, GroupEntry.Text));
                UpdateData();
            }
        }
    }
}
