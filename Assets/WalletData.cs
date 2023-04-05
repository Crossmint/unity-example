using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DefaultNamespace {
    public class WalletData {
        public static string WalletAddress { get; set; } = "Loading...";
        public static string Id { get; set; }
        public static ObservableCollection<string> PendingMints { get; } = new ObservableCollection<string>();
        public static List<string> ImageURLs { get; set; } = new List<string>();
    }
}