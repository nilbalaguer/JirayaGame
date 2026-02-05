using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerMultiplayerPuzzle : MonoBehaviour
{
    public IndicadorNumeroScriptMultiplayer[] indicadorNumeroScript = new IndicadorNumeroScriptMultiplayer[5];
    private int correctos = 0;
    private int[] resultados = new int[5];
    private bool echo = false;

    // Orden de las runas: 1,7,2,4,8
    void Start() {
        resultados = new int[] { 1, 7, 2, 4, 8 };
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!echo)
        {
            for (int i = 0; i < indicadorNumeroScript.Length; i++)
            {
                if (indicadorNumeroScript[i].numeroActual == resultados[i])
                {
                    correctos += 1;
                    if (correctos == 5)
                    {
                        SceneManager.LoadScene("PantallaFinal");
                        echo = true;
                    }
                }
            }
            correctos = 0;
        }
        
    }
}