using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginMenuManager : MonoBehaviour {
    public Button LoginBtn;
    public TMP_InputField Input;
    
    // Start is called before the first frame update
    void Start() {
        LoginBtn.onClick.AddListener(Login);
    }

    // Update is called once per frame
    void Update() {

    }

    void Login() {
        string inputText = Input.text;
        if (string.IsNullOrWhiteSpace(inputText) || inputText.Length < 1) {
            return;
        }
        
        var buttonText = LoginBtn.GetComponentInChildren<TMP_Text>();
        
        Input.enabled = false;
        LoginBtn.enabled = false;
        buttonText.text = "Loading...";
        
        WalletData.Id = inputText;

        var req = Web3Requests.GetInstance(this);
        req.FindOrCreateWallet(WalletData.Id, () => {
            SceneManager.LoadScene(1);
        });
    }
}