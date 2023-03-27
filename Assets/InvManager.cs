using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InvManager : MonoBehaviour {
    public Canvas canvas;

    // Start is called before the first frame update
    void Start() {
        Web3Requests.GetInstance(this).GetNFTs(WalletData.WalletAddress, RenderNFTs);
    }

    // Update is called once per frame
    void Update() {

    }

    void RenderNFTs() {
        // Download and render all images
        foreach (var imgUrl in WalletData.ImageURLs) {
            var routine = LoadImageFromURL(imgUrl);
            StartCoroutine(routine);
        }
    }
    
    IEnumerator LoadImageFromURL(string url) {
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
        trans.transform.SetParent(canvas.transform);
        trans.localScale = Vector3.one;
        trans.anchoredPosition = new Vector2(0f, 0f);
        trans.sizeDelta = new Vector2(150, 200);

        Image image = imgObject.AddComponent<Image>();
        image.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        imgObject.transform.SetParent(canvas.transform);
    }
}