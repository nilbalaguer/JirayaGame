using UnityEngine;

public class ButtonPuzzleController : MonoBehaviour
{
    [SerializeField] FloorButonScript boton1;
    [SerializeField] FloorButonScript boton2;
    [SerializeField] FloorButonScript boton3;
    [SerializeField] FloorButonScript boton4;
    [SerializeField] FloorButonScript boton5;

    [SerializeField] GameObject partituraPrefab;

    private void FixedUpdate() 
    {
        //El jugador tinen que acertar el numero 19
        if (boton1.activado && !boton2.activado && !boton3.activado && boton4.activado && boton5.activado)
        {
            GameObject tempPartitura = Instantiate(partituraPrefab, transform.position, Quaternion.identity);
            PartituraItemScript tempPartituraItemScript = tempPartitura.GetComponent<PartituraItemScript>();
            tempPartituraItemScript.claveTPdesactivar = "mazzmorraBotones";
            Destroy(gameObject);
        }
    }
}