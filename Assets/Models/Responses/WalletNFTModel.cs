using System;

namespace DefaultNamespace.Models {
    [Serializable]
    public class WalletResponse {
        public WalletNFTs[] nfts;
    }

    [Serializable]
    public class WalletNFTs {
        public string chain;
        public string contractAddress;
        public string tokenId;
        public Metadata metadata;
        public string locator;
        public string tokenStandard;
    }

    [Serializable]
    public class Metadata {
        public string name;
        public string image;
        public string description;
    }
}