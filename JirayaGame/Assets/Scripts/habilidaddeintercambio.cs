using UnityEngine;
using System.Collections.Generic;
using System.Collections; 

public class habilidaddeintercambio : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject player;
    public GameObject selectedObject;
    public Collider2D prefabHitboxRadius;
    public Animator focusAnimator;
    public SpriteRenderer abilityUI; // asignar en el Inspector: la UI que se muestra mientras se mantiene E
    public SpriteRenderer spriterendersmoke;
    public float cooldownTime = 3f;
    public bool isOnCooldown = false;
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        focusAnimator.enabled = false;

        prefabHitboxRadius.enabled = false;
        abilityUI.enabled = false;
    }


    void Update()
    {
        
        if ((Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.JoystickButton0)) && gameManager.swapHabilidad == true)
        {
            prefabHitboxRadius.enabled = true;

            abilityUI.enabled = true;
            if (selectedObject != null)
            {
                selectedObject.GetComponentInChildren<SpriteRenderer>().color = Color.yellow; 
            }
        }


        if ((Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.JoystickButton0)) && selectedObject != null && gameManager.swapHabilidad == true)
        {
            SwapPositionsWithSelected();
            prefabHitboxRadius.enabled = false;
            abilityUI.enabled = false;



        } else if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.JoystickButton0))
        {
            prefabHitboxRadius.enabled = false;

            abilityUI.enabled = false;
        }
    }

    void SwapPositionsWithSelected()
    {
        Vector2 tempPosition = player.transform.position;
        player.transform.position = selectedObject.transform.position;

        focusAnimator.enabled = true;
        spriterendersmoke.enabled = true;
        focusAnimator.Play("humocambiodeposicion_Clip", 0, 0f);
        StartCoroutine(Cooldown());
        selectedObject.transform.position = tempPosition;


    }

    IEnumerator Cooldown()
    {
        isOnCooldown = true;

        float elapsed = 0f;

        while (elapsed < cooldownTime)
        {
            elapsed += Time.deltaTime; // suma el tiempo que pasa cada frame
            yield return null;         // espera al siguiente frame
        }

        if (elapsed >= cooldownTime)
        {
            focusAnimator.enabled = false;
            spriterendersmoke.enabled = false;
            isOnCooldown = false;
            yield return null;         // espera al siguiente frame
        }

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