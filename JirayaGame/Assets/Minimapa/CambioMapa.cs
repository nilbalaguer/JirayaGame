using UnityEngine;

public class CambioMapa : MonoBehaviour
{
    public GameObject minimapaPequeño;
    public GameObject minimapaGrande;
    public Camera camaraBigMap;
    private bool mapaGrandeActivo = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        minimapaPequeño.SetActive(true);
        minimapaGrande.SetActive(false);
        camaraBigMap.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mapaGrandeActivo = !mapaGrandeActivo;
            minimapaPequeño.SetActive(!mapaGrandeActivo);
            minimapaGrande.SetActive(mapaGrandeActivo);
            camaraBigMap.gameObject.SetActive(mapaGrandeActivo);
            Time.timeScale = mapaGrandeActivo ? 0 : 1;
        }
    }
}
