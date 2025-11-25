using UnityEngine;

public class BucleInfinitoController : MonoBehaviour
{
    [SerializeField] TriggerBucleScript trigger1;
    [SerializeField] TriggerBucleScript trigger2;

    [SerializeField] Transform puntoTp1;
    [SerializeField] Transform puntoTp2;

    private GameObject player;
    private bool activarSegundoTp = false;

    [SerializeField] GameObject fondoNegro;

    private void Start() {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (trigger1.IsActive())
        {
            Vector3 nuevaPos = player.transform.position;
            nuevaPos.x = puntoTp1.position.x;
            player.transform.position = nuevaPos;
            
            activarSegundoTp = true;
        }

        if (trigger2.IsActive() && activarSegundoTp)
        {
            Vector3 nuevaPos = player.transform.position;
            nuevaPos.x = puntoTp2.position.x;
            player.transform.position = nuevaPos;
            Destroy(fondoNegro);
        }
    }
}
