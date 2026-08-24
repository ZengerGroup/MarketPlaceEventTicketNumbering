using System;
using System.Collections.Generic;
using System.Text;

namespace MarketPlaceEventTicketNumbering
{
    public class Row
    {
        public string Version;
        public string MailingName;
        public string Prefix;
        public int Records;
        public int TicketsPerRecord;
        public int CalculatedQuantity;
        public double EnteredOvers;
        public int CalculatedOvers;
        public string GroupName;
        public string FirstTicket;
        public string LastTicket;
        public Row(string version, string mailingName, string prefix, string records, string ticketsPerRecord, string overs, string groupName)
        {
            Version = version;
            MailingName = mailingName;
            Prefix = prefix;
            Records = Int32.Parse(records);
            TicketsPerRecord = Int32.Parse(ticketsPerRecord);
            CalculatedQuantity = Records * TicketsPerRecord;
            EnteredOvers = double.Parse(overs);
            CalculatedOvers = (int)Math.Ceiling(Records * (EnteredOvers/100));
            GroupName = groupName;
        }
    }
}
