using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Misions : MonoBehaviour
{
    public TextMeshProUGUI texto;
    public GameObject panelMision;
    public static bool[] Mision;
    public string textoMision, textoFinalizarMision;
    public int MisionActual;
    public static Misions misiones;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mision[MisionActual] = false;
        //texto.gameObject.SetActive(false);
        panelMision.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
