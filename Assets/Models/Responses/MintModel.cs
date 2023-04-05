using System;

namespace DefaultNamespace.Models {
    [Serializable]
    public class MintModel {
        public string id;
        public Metadata metadata;
        public OnChain onChain;
    }

    [Serializable]
    public class OnChain {
        public string status;
        public string tokenId;
        public string owner;
        public string txId;
        public string contractAddress;
        public string chain;
    }
}