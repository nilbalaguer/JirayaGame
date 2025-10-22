using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollPanel : MonoBehaviour
{
    public GameObject text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnEnable()
    {
        text.SetActive(false);
    }
    public void ShowText()
    {
        text.SetActive(true);
    }
}
