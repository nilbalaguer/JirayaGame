using UnityEngine;

public class habilidaddeintercambio : MonoBehaviour
{

    public Transform rotation;
    public GameObject player;
    public GameObject selectedObject;
    public Collider2D prefabHitboxRadius;
    public SpriteRenderer abilityUI; // asignar en el Inspector: la UI que se muestra mientras se mantiene E

    void Start()
    {
        
            prefabHitboxRadius.enabled = false;
            abilityUI.enabled = false;
    }

    void Update()
    {
        // Mantener E: mostrar UI y activar área de detección
        if (Input.GetKey(KeyCode.E))
        {
            prefabHitboxRadius.enabled = true;

            abilityUI.enabled = true;
            if (selectedObject != null)
            {
                selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow; // Cambiar color para indicar selección
            }
        }

        // Al soltar E: ocultar UI, desactivar área y, si hay objeto seleccionado, intercambiar posiciones
        if (Input.GetKeyUp(KeyCode.E) && selectedObject != null)
        {
            SwapPositionsWithSelected();
            prefabHitboxRadius.enabled = false;

            abilityUI.enabled = false;

            
            
            
        } else if (Input.GetKeyUp(KeyCode.E))
        {
            prefabHitboxRadius.enabled = false;

            abilityUI.enabled = false;
        }
    }

    void SwapPositionsWithSelected()
    {
        Vector2 tempPosition = player.transform.position;
        player.transform.position = selectedObject.transform.position;
        selectedObject.transform.position = tempPosition;

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Solo guardamos la referencia si el hitbox está activo y el objeto tiene la etiqueta correcta
        if (other != null && other.CompareTag("intObject") && prefabHitboxRadius != null && prefabHitboxRadius.enabled)
        {
            selectedObject = other.gameObject;
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("intObject") && other.gameObject == selectedObject)
        {
            selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;

            selectedObject = null;
        }
    }
}

