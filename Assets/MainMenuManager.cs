using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    public TMPro.TMP_Text TextField;
    public Button ViewInv;
    public Button Mint;

    // Start is called before the first frame update
    void Start() {
        TextField.text = $"Wallet address: {WalletData.WalletAddress}";
        
        ViewInv.onClick.AddListener(HandleViewInventory);
        Mint.onClick.AddListener(HandleMint);
    }

    // Update is called once per frame
    void Update() {

    }

    void HandleViewInventory() {
        Debug.Log("Opening inv...");
        SceneManager.LoadScene(2);
    }

    void HandleMint() {
        // Mint nft
        Debug.Log("Minting nft to wallet...");
        TextField.text = "Minting...";
        Mint.enabled = false;
        
        Web3Requests.GetInstance(this).Mint(WalletData.WalletAddress, () => {
            TextField.text = "Minting complete!";
            Mint.enabled = true;
        });
    }
}
