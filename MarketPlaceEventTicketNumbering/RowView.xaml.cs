namespace MarketPlaceEventTicketNumbering;

public partial class RowView : ContentView
{
	public RowView(Row row)
	{
		InitializeComponent();
		VersionData.Text = row.Version;
		MailingData.Text = row.MailingName;
		PrefixData.Text = row.Prefix;
		QuantityData.Text = row.CalculatedQuantity.ToString();
		OversData.Text = row.CalculatedOvers.ToString();
		GroupData.Text = row.GroupName;
    }
}