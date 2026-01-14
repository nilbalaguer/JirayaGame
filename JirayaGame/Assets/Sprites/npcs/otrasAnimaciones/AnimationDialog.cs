using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimationDialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //efecto puntos suspensivos
        float t = Mathf.PingPong(Time.time, 1f);
        dialogText.text = "...".Substring(0, (int)(t * 3) + 1);
    }
}
