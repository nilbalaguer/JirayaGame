using UnityEngine;
using Mirror;
using TMPro;

public class NetworkHUD : MonoBehaviour
{
    public NetworkManager networkManager;
    public TMP_InputField addressInput;
    public TextMeshProUGUI statusText;

    void Update()
    {
        if (NetworkServer.active && NetworkClient.isConnected)
            statusText.text = "Host activo";
        else if (NetworkClient.isConnected)
            statusText.text = "Conectado como cliente";
        else
            statusText.text = "Desconectado";

        gameObject.SetActive(!NetworkClient.isConnected);
    }

    public void StartHost()
    {
        networkManager.StartHost();
    }

    public void StartClient()
    {
        networkManager.networkAddress = addressInput.text;
        networkManager.StartClient();
    }

    public void Disconnect()
    {
        if (NetworkServer.active)
            networkManager.StopHost();
        else
            networkManager.StopClient();
    }
}
