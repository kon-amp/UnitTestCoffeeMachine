using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing
{
    public class CsvLineParser
    {
        public static MachineDataItem[] Parse(string[] csvlines)
        {
            var machineDataItems = new List<MachineDataItem>();

            foreach (var csvLine in csvlines)
            {
                if (string.IsNullOrWhiteSpace(csvLine)) {
                    continue;
                }
                var machineDataItem = Parse(csvLine);

                machineDataItems.Add(machineDataItem);
            }

            return machineDataItems.ToArray();
        }

        private static MachineDataItem Parse(string csvLine)
        {
            var lineItems = csvLine.Split(';');

            if (lineItems.Length !=2) {
#pragma warning disable S112 // General or reserved exceptions should never be thrown
                throw new Exception();
#pragma warning restore S112 // General or reserved exceptions should never be thrown
            }

            return new MachineDataItem(lineItems[0], DateTime.Parse(lineItems[1], CultureInfo.InvariantCulture));
        }
    }
}
