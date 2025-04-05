namespace OpenRepairManager.MAUI.Extensions;

public static class ORSDictionaries
    {
        private static readonly Dictionary<string, int> ElectricalIdDictionaryStringToIdMap = new()
        {
            { "", 0 },
            { "Aircon/dehumidifier", 1 },
            { "Battery/charger/adapter", 2 },
            { "Decorative or safety lights", 3 },
            { "Desktop computer", 4 },
            { "Digital compact camera", 5 },
            { "DSLR/video camera", 6 },
            { "Fan", 7 },
            { "Flat screen", 8 },
            { "Hair & beauty item", 9 },
            { "Handheld entertainment device", 10 },
            { "Headphones", 11 },
            { "Hi-Fi integrated", 12 },
            { "Hi-Fi separates", 13 },
            { "Kettle", 14 },
            { "Lamp", 15 },
            { "Laptop", 16 },
            { "Large home electrical", 17 },
            { "Misc", 18 },
            { "Mobile", 19 },
            { "Musical instrument", 20 },
            { "Paper shredder", 21 },
            { "PC accessory", 22 },
            { "Portable radio", 23 },
            { "Power tool", 24 },
            { "Printer/scanner", 25 },
            { "Projector", 26 },
            { "Sewing machine", 27 },
            { "Small home electrical", 28 },
            { "Small kitchen item", 29 },
            { "Tablet", 30 },
            { "Toaster", 31 },
            { "Toy", 32 },
            { "TV and gaming-related accessories", 33 },
            { "Vacuum", 34 },
            { "Watch/clock", 35 },
            { "Coffee maker", 36 },
            { "Food processor", 37 },
            { "Games console", 38 },
            { "Hair dryer", 39 },
            { "Iron", 40 }
        };

        private static readonly Dictionary<int, string> ElectricalCategoryIdToStringMap = ElectricalIdDictionaryStringToIdMap
            .ToDictionary(x => x.Value, x => x.Key);

        public static Dictionary<int, string> GetElectricalIdDictionary()
        {
            return ElectricalCategoryIdToStringMap;
        }
        
        public static string GetCatString(int catId)
        {
            return ElectricalCategoryIdToStringMap.TryGetValue(catId, out string catString) ? catString : "";
        }

        public static int GetCatID(string catString)
        {
            return ElectricalIdDictionaryStringToIdMap.TryGetValue(catString, out int id) ? id : 0;
        }

        private static readonly Dictionary<int, string> RepairBarrierIDtoStringMap = new()
        {
            { 0, "" },
            { 1, "Spare parts not available" },
            { 2, "Spare parts too expensive" },
            { 3, "No way to open product" },
            { 4, "Repair information not available" },
            { 5, "Lack of equipment" },
            { 6, "Item too worn out" }
        };
        
        private static readonly Dictionary<string, int> RepairBarrierStringToIDMap = RepairBarrierIDtoStringMap
            .ToDictionary(x => x.Value, x => x.Key);
        
        public static string GetRepairBarrierString(int? barrier)
        {
            return RepairBarrierIDtoStringMap.TryGetValue(barrier ?? 0, out string barrierString) ? barrierString : "";
        }

        public static int GetRepairBarrierID(string barrier)
        {
            return RepairBarrierStringToIDMap.TryGetValue(barrier, out int id) ? id : 0;
        }

        private static readonly Dictionary<int, string> RepairStatusStringToIDMap = new()
        {
            { 0, "" },
            { 1, "Fixed" },
            { 2, "Repairable" },
            { 3, "End of life" }
        };
        
        private static readonly Dictionary<string, int> RepairStatusIDToStringMap = RepairStatusStringToIDMap
            .ToDictionary(x => x.Value, x => x.Key);
        
        public static string GetRepairStatusString(int status)
        {
            return RepairBarrierIDtoStringMap.TryGetValue(status, out string statusString) ? statusString : "";
        }

        public static int GetRepairStatusID(string status)
        {
            return RepairBarrierStringToIDMap.TryGetValue(status, out int id) ? id : 0;
        }
        
    }