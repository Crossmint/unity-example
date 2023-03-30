using DefaultNamespace;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public GameObject ScrollContent;
    public Button GoBack;

    public int imageWidth = 150;
    public int imageHeight = 200;
    public int spacing = 10;

    private const int startXPos = -400;
    private const int startYPos = 100;
    private const int maxPerRow = 6;

    // Start is called before the first frame update
    void Start()
    {
        GoBack.onClick.AddListener(GoBackToMenu);

        Web3Requests.GetInstance(this).GetNFTs(WalletData.WalletAddress, RenderNFTs);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GoBackToMenu()
    {
        SceneManager.LoadScene(1);
    }

    void RenderNFTs()
    {
        // Download and render all images
        for (var index = 0; index < WalletData.ImageURLs.Count; index++)
        {
            var imgUrl = WalletData.ImageURLs[index];
            var routine = LoadImageFromURL(imgUrl, index);
            StartCoroutine(routine);
        }
    }

    IEnumerator LoadImageFromURL(string url, int index)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        var result = www.result;
        if (result == UnityWebRequest.Result.ConnectionError || result == UnityWebRequest.Result.ProtocolError)
        {
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