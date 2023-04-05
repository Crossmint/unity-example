using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DefaultNamespace.Models;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace {
    public class Web3Requests : MonoBehaviour {
        private static Web3Requests _instance;

        public static Web3Requests GetInstance(MonoBehaviour mb) {
            if (_instance == null) {
                _instance = mb.gameObject.AddComponent<Web3Requests>();
            }

            return _instance;
        }

        public void FindOrCreateWallet(string id, Action onFinished, Action onFailed = null) {
            var user = new UserModel() {
                userId = id
            };
            string json = JsonUtility.ToJson(user);

            bool ParseResponse(string data) {
                var wallet = JsonUtility.FromJson<WalletModel>(data);
                WalletData.WalletAddress = wallet.publicKey;
                return true;
            }
            
            var routine = PostRequest("/api/wallet", json, ParseResponse, onFinished, onFailed);
            StartCoroutine(routine);
        }

        public void GetNFTs(string walletAddress, Action onFinished) {
            bool ParseResponse(string data) {
                WalletData.ImageURLs = new List<string>();
                
                WalletNFTs[] mints = JsonUtility.FromJson<WalletResponse>("{\"nfts\":" + data + "}").nfts;
                foreach (var mint in mints) {
                    WalletData.ImageURLs.Add(mint.metadata.image);
                }
                
                return true;
            }
            
            var routine = GetRequest($"/api/mint?address={walletAddress}", ParseResponse, onFinished);
            StartCoroutine(routine);
        }

        public void Mint(string walletAddress, Action onFinished) {
            var user = new AddressModel() {
                address = walletAddress
            };
            string json = JsonUtility.ToJson(user);

            bool ParseResponse(string data) {
                var mint = JsonUtility.FromJson<MintModel>(data);
                WalletData.PendingMints.Add(mint.id);
                return true;
            }
            
            var routine = PostRequest("/api/mint", json, ParseResponse, onFinished);
            StartCoroutine(routine);
        }

        public void MintStatus(string id, Action onFinished) {
            bool ParseResponse(string data) {
                var mint = JsonUtility.FromJson<MintModel>(data);
                if (mint.onChain.status == "success") {
                    var url = $"https://nftstorage.link/ipfs/{mint.metadata.image.Substring(7)}";
                    WalletData.ImageURLs.Add(url);
                    return true;
                }
                
                return false;
            }
            
            var routine = GetRequest($"/api/mint/status?id={id}", ParseResponse, onFinished);
            StartCoroutine(routine);
        }
        
        IEnumerator GetRequest(string path, Func<string, bool> postLogic, Action onFinished) {
            using (UnityWebRequest request = UnityWebRequest.Get(Config.BASEURL + path)) {
                yield return request.SendWebRequest();

                var result = request.result;
                if (result == UnityWebRequest.Result.ConnectionError ||
                    result == UnityWebRequest.Result.ProtocolError) {
                    yield break;
                }

                var response = request.downloadHandler.text;
                
                Debug.Log(response);
                
                if (!postLogic(response)) {
                    yield break;
                }
                
                onFinished();
            }
        }

        IEnumerator PostRequest(string path, string bodyJsonString, Func<string, bool> postLogic, Action onFinished, Action onFailed = null) {
            Debug.Log(bodyJsonString);
            
            using (var request = new UnityWebRequest(Config.BASEURL + path, "POST")) {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();

                var response = request.downloadHandler.text;
                Debug.Log(response);

                if (response.Contains("error")) {
                    if (onFailed != null) {
                        onFailed();
                    }

                    throw new Exception("Server has thrown an error.");
                }
                
                if (!postLogic(response)) {
                    yield break;
                }
                
                onFinished();
            }
        }
    }
}