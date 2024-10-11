﻿using System.Globalization;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Parsing {
    public class CsvLineParser {
        public static MachineDataItem[] Parse(string[] csvlines) {
            var machineDataItems = new List<MachineDataItem>();

            foreach (var csvLine in csvlines) {
                if (string.IsNullOrWhiteSpace(csvLine)) {
                    continue;
                }
                var machineDataItem = Parse(csvLine);

                machineDataItems.Add(machineDataItem);
            }

            return machineDataItems.ToArray();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S112:General or reserved exceptions should never be thrown", Justification = "<Pending>")]
        private static MachineDataItem Parse(string csvLine) {
            var lineItems = csvLine.Split(';');

            if (lineItems.Length != 2) {
                throw new Exception($"Invalid csv line: {csvLine}");
            }

            if (!DateTime.TryParse(lineItems[1], CultureInfo.InvariantCulture, result: out DateTime datetime)) {
                throw new Exception($"Invalid datetime in csv line: {csvLine}");
            }

            return new MachineDataItem(lineItems[0], datetime);
        }
    }
}
