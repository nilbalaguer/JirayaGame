using UnityEngine;
using UnityEngine.SceneManagement;

public class AumentarTamanyoTexto : MonoBehaviour
{
    private float scaleNow = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scaleNow += Time.deltaTime * 0.5f;
        transform.localScale = new Vector3(scaleNow, scaleNow, scaleNow);

        if (scaleNow > 15)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
