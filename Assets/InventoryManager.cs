using DefaultNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour {
    public GameObject ScrollContent;
    public Button GoBack;
    public GameObject PlaceholderPrefab;

    const int imageWidth = 150;
    const int imageHeight = 200;
    const int spacing = 10;

    const int startXPos = -400;
    const int startYPos = 100;
    const int maxPerRow = 6;

    bool initialized = false;

    Dictionary<int, GameObject> _placeholders = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start() {
        GoBack.onClick.AddListener(GoBackToMenu);

        Web3Requests.GetInstance(this).GetNFTs(WalletData.WalletAddress, RenderNFTs);

        StartCoroutine(CheckPendingMints());
    }

    // Update is called once per frame
    void Update() {

    }

    void GoBackToMenu() {
        SceneManager.LoadScene(1);
    }

    void RenderNFTs() {
        // Download and render all images
        for (var index = 0; index < WalletData.ImageURLs.Count; index++) {
            var imgUrl = WalletData.ImageURLs[index];
            var routine = LoadImageFromURL(imgUrl, index);
            StartCoroutine(routine);
        }
        
        // Create placeholders for pending NFTs
        for (var index = 0; index < WalletData.PendingMints.Count; index++) {
            if (!_placeholders.ContainsKey(index + WalletData.ImageURLs.Count)) {
                CreatePlaceholder(index + WalletData.ImageURLs.Count);
            }
        }

        initialized = true;
    }

    void ReplacePlaceholderWithImage(string imgUrl, int index) {
        if (_placeholders.ContainsKey(index)) {
            Destroy(_placeholders[index]);
            _placeholders.Remove(index);

            var routine = LoadImageFromURL(imgUrl, index);
            StartCoroutine(routine);
        }
    }

    void CheckMintStatus(string mintId, int index) {
        Web3Requests.GetInstance(this).MintStatus(mintId, () => {
            if (WalletData.ImageURLs.Count > index) {
                WalletData.PendingMints.Remove(mintId);
                ReplacePlaceholderWithImage(WalletData.ImageURLs[index], index);
            }
        });
    }

    IEnumerator CheckPendingMints() {
        while (WalletData.PendingMints.Count > 0) {
            if (initialized) {
                for (int i = 0; i < WalletData.PendingMints.Count; i++) {
                    string mintId = WalletData.PendingMints[i];
                    CheckMintStatus(mintId, WalletData.ImageURLs.Count + i);
                }
            }

            yield return new WaitForSeconds(10);
        }
    }

    void CreatePlaceholder(int index) {
        // Instantiate the placeholder prefab and add it to the scene
        GameObject placeholder = Instantiate(PlaceholderPrefab);
        // Position the placeholder as required
        // placeholder.transform.position = ...
        _placeholders.Add(index, placeholder);

        RectTransform trans = placeholder.GetComponent<RectTransform>();
        trans.transform.SetParent(ScrollContent.transform);
        trans.localScale = Vector3.one;

        var startPos = (index * imageWidth) % (imageWidth * maxPerRow);
        var row = index / maxPerRow;

        trans.anchoredPosition = new Vector2(startXPos + startPos, startYPos - (row * imageHeight + spacing));
        trans.sizeDelta = new Vector2(imageWidth, imageHeight);
    }

    IEnumerator LoadImageFromURL(string url, int index) {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        var result = www.result;
        if (result == UnityWebRequest.Result.ConnectionError || result == UnityWebRequest.Result.ProtocolError) {
            Debug.LogError(www.error);
            yield break;
        }

        Texture2D tex = ((DownloadHandlerTexture)www.downloadHandler).texture;

        GameObject imgObject = new GameObject(url);

        RectTransform trans = imgObject.AddComponent<RectTransform>();
        trans.transform.SetParent(ScrollContent.transform);
        trans.localScale = Vector3.one;

        var startPos = (index * imageWidth) % (imageWidth * maxPerRow);
        var row = index / maxPerRow;

        trans.anchoredPosition = new Vector2(startXPos + startPos, startYPos - (row * imageHeight + spacing));
        trans.sizeDelta = new Vector2(imageWidth, imageHeight);

        Image image = imgObject.AddComponent<Image>();
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        imgObject.transform.SetParent(ScrollContent.transform);
    }
}