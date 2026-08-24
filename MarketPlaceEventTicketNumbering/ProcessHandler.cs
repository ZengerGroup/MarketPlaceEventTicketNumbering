using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Storage;

namespace MarketPlaceEventTicketNumbering
{
    internal class ProcessHandler
    {
        List<List<Row>> SortedRows;
        string OutputPath;
        int TicketNumber;
        public ProcessHandler()
        {
            SortedRows = new List<List<Row>>();
            TicketNumber = 1;
            OutputPath = @"C:\Code\TestingFS";
        }
        public async Task<bool> RunProcess(List<Row> dataTable, string outPath)
        {
            try
            {
                OutputPath = outPath;
                SortRows(dataTable);
                for (int i = 0; i < SortedRows.Count; i++) BuildTicketList(SortedRows[i]);
                BuildProofList();
                BuildSummaryTable();
                return true;
            }
            catch { return false; }
        }
        private void SortRows(List<Row> dataTable)
        {
            for(int i = 0; i < dataTable.Count; i++)
            {
                bool found = false;
                for (int ii = 0; ii < SortedRows.Count; ii++)
                    if (dataTable[i].GroupName == SortedRows[ii][0].GroupName)
                    {
                        found = true;
                        SortedRows[ii].Add(dataTable[i]);
                        break;
                    }
                if (!found) SortedRows.Add(new List<Row> { dataTable[i] });
            }
        }
        private void BuildTicketList(List<Row> dataTable)
        {
            string filePath = Path.Combine(OutputPath, String.Format("{0}.csv", dataTable[0].GroupName));
            StreamWriter sWriter = new StreamWriter(filePath);
            sWriter.WriteLine("Ticket #,Version,Mailing Name");
            foreach(Row row in dataTable)
            {
                int LastTicket = (TicketNumber == 1) ? row.CalculatedQuantity + row.CalculatedOvers : TicketNumber + row.CalculatedQuantity + row.CalculatedOvers - 1;
                row.FirstTicket = String.Format("{0}{1}", row.Prefix, TicketNumber.ToString("000000"));
                row.LastTicket = String.Format("{0}{1}", row.Prefix, LastTicket.ToString("000000"));
                while (TicketNumber <= LastTicket) 
                {
                    string ticketString = String.Format("{0}{1}", row.Prefix, TicketNumber.ToString("000000"));
                    sWriter.WriteLine($"{ticketString},{row.Version},{row.MailingName}");
                    TicketNumber++;
                }
            }
            sWriter.Close();
        }
        private void BuildProofList()
        {
            string filePath = Path.Combine(OutputPath, String.Format("{0}_Proofs.csv", Path.GetFileName(OutputPath.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))));
            StreamWriter sWriter = new StreamWriter(filePath);
            sWriter.WriteLine("Ticket #,Version");
            for(int i = 0; i < SortedRows.Count; i++) for(int ii = 0; ii < SortedRows[i].Count; ii++)
                {
                sWriter.WriteLine($"{SortedRows[i][ii].FirstTicket},{SortedRows[i][ii].Version}");
                sWriter.WriteLine($"{SortedRows[i][ii].LastTicket},{SortedRows[i][ii].Version}");
                }
            sWriter.Close();
        }
        private void BuildSummaryTable()
        {
            string filePath = Path.Combine(OutputPath, String.Format("{0}_Summary.csv", Path.GetFileName(OutputPath.TrimEnd(
                Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))));
            StreamWriter sWriter = new StreamWriter(filePath);
            sWriter.WriteLine("Version,Mailing Name,Ticket Prefix,Tickets Needed,Overs Added, Ticket Start, Ticket End");
            for (int i = 0; i < SortedRows.Count; i++) for (int ii = 0; ii < SortedRows[i].Count; ii++)
            {
                string line = $"{SortedRows[i][ii].Version},{SortedRows[i][ii].MailingName},{SortedRows[i][ii].Prefix},{SortedRows[i][ii].CalculatedQuantity.ToString()},"
                    + $"{SortedRows[i][ii].CalculatedOvers.ToString()},{SortedRows[i][ii].FirstTicket},{SortedRows[i][ii].LastTicket}";
                sWriter.WriteLine(line);
            }
            sWriter.Close();
        }
    }
}
