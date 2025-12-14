using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    [Header("Varios")]
    [SerializeField] Animator animator;

    [SerializeField] Rigidbody2D rigidBody;
    public float maxSpeed = 5;

    [SerializeField] string state = "idle";
    public bool human = true;

    [Header("HUD")]
    private Image indicadorParry;

    private SpriteRenderer spriteRendererPlayer;

    [Header("Armas")]
    [SerializeField] GameObject katanaObject;
    [SerializeField] PolygonCollider2D colliderKatana;
    [SerializeField] SpriteRenderer spriteRendererKatana;
    //Tonge
    // Tonge collider max extension 1.84581
    [SerializeField] GameObject toatTonge;
    private SpriteRenderer toatTongeTonge;
    private Animator tongeAnimator;
    [SerializeField] GameObject toatTongeColliderObject;
    private BoxCollider2D tongeCollider;

    private GameObject objectPicked = null;
    [SerializeField] Transform objectTravelPosition;
    

    private float cooldownMele = 0;
    private float cooldownTonge = 0.375f;
    [SerializeField] float cooldownForMele = 0.5f;
    private int lastMove;

    [Header("Vida i Habilidades")]

    public GameManager gameManager;

    [SerializeField] LayerMask nenufarLayerMask;
    private bool estaSaltando = false;

    [Header("Parry")]
    private float staminaParry = 4;
    [SerializeField] float staminaDuration = 4;

    [Header("Sonido")]
    [SerializeField] AudioSource fuenteSonido;
    [SerializeField] AudioClip sonidoBlandirKatana;

    [SerializeField] AudioClip sonidoFootstepFrog;
    [SerializeField] AudioClip sonidoFootstep;
    private float cooldownFootStep = 0.9f;

    [SerializeField] AudioClip sonidoLengua;
    [SerializeField] AudioClip sonidoDamage;
    [SerializeField] AudioClip sonidoParry;

    [Header("Sprites Bloqueo")]
    [SerializeField] Sprite[] spritesBloqueo;
    public Transform puntoSujecion;
    private Objeto objetoCercano;
    public Objeto objetoSujeto;
    public float fuerzaLanzamiento = 10f;
    [HideInInspector]
    public Inventario inventario;

    public GameObject CanvasInfo;
    public GameObject tsunadePanel;
    public GameObject tsunadePanel2;
    public bool tsunadeCerca = false;

    //public ScrollPanel scrollPanel;
    private GameObject tsunade;
    //public panelErmitaño panelScript;
    public GameObject mensajePocion;
    private Vector2 ultimaDireccion = Vector2.right;
    public float distanciaSujecion = 0.1f;
    public bool puedoMoverme = true;
    [HideInInspector]
    public bool timelineMostrado = false;
    public float ultimoDialogo = 0f;
    public float cooldownDialogo = 2f;

    public bool puedeTransformarse = false;

    [Header("HUmo")]
    [SerializeField] Animator focusAnimator;
    [SerializeField] SpriteRenderer spriterendersmoke;
    public Sprite iconoRana1;
    public Sprite iconoRana2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colliderKatana.enabled = false;
        spriteRendererKatana.enabled = false;

        //Lengua
        toatTongeTonge = toatTonge.GetComponentInChildren<SpriteRenderer>();
        tongeAnimator = toatTonge.GetComponentInChildren<Animator>();
        tongeCollider = toatTonge.GetComponentInChildren<BoxCollider2D>();
        toatTongeTonge.enabled = false;
        tongeAnimator.SetFloat("Blend", 0);
        tongeCollider.enabled = false;

        //Obtener indicadores
        GameObject parryObj = GameObject.Find("parryIndicator");
        indicadorParry = parryObj.GetComponent<Image>();

        //Obtener GameManager
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Obtener sprite renderer
        spriteRendererPlayer = gameObject.GetComponentInChildren<SpriteRenderer>();

        inventario = GetComponent<Inventario>();
        CanvasInfo.SetActive(false);
        if (tsunade != null)
        {
            tsunadePanel.SetActive(false);
            tsunade = GameObject.FindWithTag("Tsunade");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Sistema Katana para el cooldown i para activar i desactivar el katana collider solo por 0.1 segundos
        if (cooldownMele > 0)
        {
            if (cooldownMele == cooldownForMele)
            {
                colliderKatana.enabled = true;
                spriteRendererKatana.enabled = true;

            }
            else if (colliderKatana.enabled && cooldownMele < (cooldownForMele - 0.1))
            {
                colliderKatana.enabled = false;
            }

            cooldownMele -= Time.deltaTime;

            if (cooldownMele <= 0)
            {
                spriteRendererKatana.enabled = false;
            }
        }

        //Sistema de cooldown lengua.
        if (toatTongeTonge.enabled)
        {
            cooldownTonge -= Time.deltaTime;

            if (cooldownTonge <= 0)
            {
                toatTongeTonge.enabled = false;
                tongeCollider.enabled = false;
                cooldownTonge = 0.375f;
            }
            else
            {
                rigidBody.linearVelocity = new Vector2(0, 0);

                //NO DEJA MOVER NI HACER NADA MIENTRAS SE LANZA LA LENGUA!!!
                //No colocar nada por debajo de este return que quieres que se ejecute absolutamente siempre
                return;
            }
        }

        if (objectPicked != null)
        {
            objectPicked.transform.position = objectTravelPosition.position;
        }

        //Sistema movimiento
        if (!puedoMoverme)
        {
            rigidBody.linearVelocity = Vector2.zero;
            state = "idle";
            return;
        }

        if (state == "Parry")
        {
            if (Input.GetButtonUp("Fire2"))
            {
                state = "idle";
            }

            staminaParry -= Time.deltaTime;
            indicadorParry.fillAmount = staminaParry / staminaDuration;

            rigidBody.linearVelocity = Vector2.zero;
            return;
        }

        float forceX = Input.GetAxis("Horizontal");
        float forceY = Input.GetAxis("Vertical");

        if (forceY > 0)
        {
            lastMove = 1;
        }
        else if (forceY < 0)
        {
            lastMove = 2;
        }
        else if (forceX > 0)
        {
            lastMove = 3;
        }
        else if (forceX < 0)
        {
            lastMove = 4;
        }

        Vector2 movimiento = new Vector2(forceX, forceY) * maxSpeed;

        rigidBody.linearVelocity = movimiento;

        //Mover punto de sujeción junto al jugador
        Vector2 direccion = new Vector2(forceX, forceY);

        if (direccion != Vector2.zero)
            ultimaDireccion = direccion.normalized;

        // Punto de sujeción
        Vector2 offset = ultimaDireccion * distanciaSujecion;

        // Si está flippeado horizontalmente, invierte el offset
        if (transform.localScale.x < 0)
            offset.x *= -1;

        puntoSujecion.localPosition = offset;


        //Maquina de estados
        switch (state)
        {
            default:
            case "idle":
            case "MoveRight":
            case "MoveLeft":
            case "MoveUp":
            case "MoveDown":

                if (rigidBody.linearVelocity.x > 0)
                {
                    state = "MoveRight";
                }
                else if (rigidBody.linearVelocity.x < 0)
                {
                    state = "MoveLeft";
                }

                if (rigidBody.linearVelocity.y > 0)
                {
                    state = "MoveUp";
                }
                else if (rigidBody.linearVelocity.y < 0)
                {
                    state = "MoveDown";
                }

                if (rigidBody.linearVelocity.x == 0 && rigidBody.linearVelocity.y == 0 && state != "Attack")
                {
                    state = "idle";
                }

                if (objetoSujeto != null && (objetoSujeto.nombreObjeto == "Pocion1" || objetoSujeto.nombreObjeto == "Pocion2" || 
                objetoSujeto.nombreObjeto == "Pocion3")  
                && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("X")))
                {
                    //BeberPocion();
                    state = "BeberPocion";
                }

                if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("X"))
                {
                    Debug.Log("x pulsada");
                    CogerObjeto();
                }

                //si el objeto esta cogido puedo lanzarlo o guardarlo en el inventario
                if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("B")) && objetoSujeto != null)
                {
                    LanzarObjeto();
                }
                else if ((Input.GetKeyDown(KeyCode.G) || Input.GetButtonDown("Y")) && objetoSujeto != null)
                {
                    GuardarObjeto();
                }
                //Soltar el objeto en caso de no necesitarlo
                else if (Input.GetKeyDown(KeyCode.Z) && objetoSujeto != null)
                {
                    SoltarObjeto();
                }

                //Mostrar paneles tsunade al acercarme a ella
                //if (!tsunadePanel.activeSelf && tsunadeInRange() && objetoSujeto != null && !objetoSujeto.esRecompensa)
                if (tsunadePanel != null && !tsunadePanel.activeSelf && tsunadeInRange() && ObjetoTsunadeExiste() && !inventario.modoEntrega)
                {
                    if (Time.time - ultimoDialogo >= cooldownDialogo)
                    {
                        tsunadePanel.SetActive(true);
                        //puedoMoverme = false;
                    }
                }
                //Beber pocion si la tiene equipada y mostrar mensaje HUD
                MostrarMensajePocion();

                if (Input.GetButtonDown("Fire1") && cooldownMele <= 0)
                {
                    if (objectPicked != null)
                    {
                        BoxCollider2D tempBoxCollider = objectPicked.GetComponent<BoxCollider2D>();
                        tempBoxCollider.enabled = true;
                        objectPicked = null;
                    }
                    else
                    {
                        state = "Attack";
                    }

                }

                if (Input.GetButtonDown("Fire3") && GameManager.Instance.puedeTransformarse)
                {
                    human = !human;

                    SmokeEfect();

                    if (objectPicked != null)
                    {
                        BoxCollider2D tempBoxCollider = objectPicked.GetComponent<BoxCollider2D>();
                        tempBoxCollider.enabled = true;
                        objectPicked = null;
                    }

                    if (human)
                    {
                        Timeline2.Instance.habilidadRana.sprite = iconoRana2;
                    }
                    else
                    {
                        Timeline2.Instance.habilidadRana.sprite = iconoRana1;
                    }
                }

                if (Input.GetButtonDown("Jump"))
                {
                    saltarSapo();
                }

                if (Input.GetButton("Fire2") && staminaParry > 0 && human)
                {
                    state = "Parry";
                }
                else if (staminaParry < staminaDuration)
                {
                    staminaParry += Time.deltaTime;
                    indicadorParry.fillAmount = staminaParry / staminaDuration;
                }

                break;

        }

        /*
            State 0 = idle
            State 1 = Walk right
            State 2 = Walk left
            State 3 = Walk Up
            State 4 = Walk Down
            State 5 = Attack
        */

        switch (state)
        {
            default:
            case "idle":
                animator.SetFloat("State", 0);
                animator.SetInteger("State-int", 0);

                break;

            case "MoveRight":

                animator.SetFloat("State", 1);
                animator.SetInteger("State-int", 1);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, 0);
                toatTonge.transform.localPosition = new Vector2(0, 0.11f);

                break;

            case "MoveLeft":

                animator.SetFloat("State", 2);
                animator.SetInteger("State-int", 2);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, 180);
                toatTonge.transform.localPosition = new Vector2(0, 0.11f);

                break;

            case "MoveUp":

                animator.SetFloat("State", 3);
                animator.SetInteger("State-int", 3);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, 90);
                toatTonge.transform.localPosition = new Vector2(0, 0);

                break;

            case "MoveDown":

                animator.SetFloat("State", 4);
                animator.SetInteger("State-int", 4);

                toatTonge.transform.rotation = Quaternion.Euler(0, 0, -90);
                toatTonge.transform.localPosition = new Vector2(0, 0);

                break;

            case "Attack":
                if (human)
                {
                    //Lanzar ataque katana
                    //Rotacion de las armas siempre igual que el ultimo movimiento del jugador
                    //Setea el cooldown para que empieze el ataque y retrase para el siguiente
                    fuenteSonido.PlayOneShot(sonidoBlandirKatana);

                    animator.SetFloat("State", 5);
                    animator.SetInteger("State-int", 5);

                    cooldownMele = cooldownForMele;

                    switch (lastMove)
                    {
                        case 1:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                            break;
                        case 2:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                        case 3:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, -90);
                            break;
                        case 4:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 90);
                            break;

                        default:
                            katanaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
                            break;
                    }

                    state = "idle";
                }
                else
                {
                    //Lanzar lengua
                    fuenteSonido.PlayOneShot(sonidoLengua);

                    animator.SetFloat("State", 5);
                    animator.SetInteger("State-int", 5);

                    toatTongeTonge.enabled = true;
                    tongeCollider.enabled = true;
                    tongeAnimator.Play("Lengua-Right_Clip", 0, 0f);

                    state = "idle";
                }

                break;

            case "Parry":

                staminaParry -= Time.deltaTime;

                indicadorParry.fillAmount = staminaParry / staminaDuration;

                break;
            case "BeberPocion":
                //animator.SetInteger("State-int", 7);
                animator.SetTrigger("Beber");
                //BeberPocion();
                //Animacion beber pocion 
                break;

        }

        //Sincronizar variables animator
        animator.SetFloat("LastDirection", lastMove);
        animator.SetBool("Human", human);

        if (state == "Parry")
        {
            animator.enabled = false;
            spriteRendererPlayer.sprite = spritesBloqueo[lastMove - 1];
        } else
        {
            animator.enabled = true;
        }
    }
    
    void FixedUpdate()
    {
        //Sonido pasos
        switch (state)
        {
            case "MoveUp":
            case "MoveDown":
            case "MoveRight":
            case "MoveLeft":
                cooldownFootStep -= Time.deltaTime;

                if (cooldownFootStep <= 0)
                {
                    if (human)
                    {
                        cooldownFootStep = 0.25f;
                        fuenteSonido.PlayOneShot(sonidoFootstep);
                    } else
                    {
                        cooldownFootStep = 0.25f;
                        fuenteSonido.PlayOneShot(sonidoFootstepFrog);
                    }
                    
                }

                break;
        }
    }

    private void MostrarMensajePocion()
    {
        if (objetoSujeto != null && (objetoSujeto.nombreObjeto == "Pocion1" || objetoSujeto.nombreObjeto == "Pocion2" || objetoSujeto.nombreObjeto == "Pocion3"))
        {
            mensajePocion.SetActive(true);
            CanvasInfo.SetActive(false);
        }
        else
        {
            mensajePocion.SetActive(false);
        }
    }

    public void BeberPocion()
    {
        Debug.Log("Has bebido la poción");
        if (objetoSujeto.nombreObjeto == "Pocion1")
        {
            if (gameManager.vidaPlayer >= 10)
            {
                Debug.Log("Vida al máximo, no puedes beber esta poción");
                return;
            }
            gameManager.RecuperarVida(2f);
        }
        else if (objetoSujeto.nombreObjeto == "Pocion2")
        {
            gameManager.AumentarVelocidad(3f);
        }
        else if (objetoSujeto.nombreObjeto == "Pocion3")
        {
            if (gameManager.vidaPlayer > 6)
            {
                Debug.Log("Vida demasiado alta, no puedes beber esta poción");
                return;
            }
            gameManager.RecuperarVida(4f);
        }
        Destroy(objetoSujeto.gameObject);
        objetoSujeto = null;
        mensajePocion.SetActive(false);
    }

    //Coger objeto cercano
    public void CogerObjeto()
    {
        if (objetoSujeto == null && objetoCercano != null)
        {
            float distancia = Vector2.Distance(transform.position, objetoCercano.transform.position);
            float rangoDeteccion = 2f;
            if (distancia <= rangoDeteccion)
            {
                /*objetoSujeto = objetoCercano;
                objetoSujeto.Coger(puntoSujecion);
    
                CanvasInfo.SetActive(true);
                Transform light = objetoSujeto.transform.Find("Light");
                if (light != null)
                {
                    light.gameObject.SetActive(false);
                }*/
                inventario.AñadirObjeto(objetoCercano);
                if ((objetoCercano.nombreObjeto == "PergaminoSagrado" || objetoCercano.nombreObjeto == "CollarShizune" || objetoCercano.nombreObjeto == "Flor")
                && objetoCercano.yaRecogido == false)
                {
                    GameManager.Instance.objetosRecogidos += 1;
                    GameManager.Instance.ActualizarContadorObjetos();
                    objetoCercano.yaRecogido = true;
                    if (!timelineMostrado)
                    {
                        GameManager.Instance.ReproducirTimelineTsunade();
                        timelineMostrado = true;
                    }
                    objetoCercano.gameObject.SetActive(false);
                }
                else
                {
                    objetoCercano.gameObject.SetActive(false);
                }
            }
            else
            {
                objetoCercano = null;
                Debug.Log("El objeto está fuera de rango para ser cogido.");
            }
        }
    }

    //Lanzar objeto sujeto
    public void LanzarObjeto()
    {
            if (objetoSujeto == null)
            return;

            Vector2 direccion = ultimaDireccion;

        Objeto objetoLanzado = objetoSujeto;
        objetoSujeto = null;

        objetoLanzado.transform.SetParent(null);
        objetoLanzado.gameObject.SetActive(true);
        objetoLanzado.Lanzar(direccion, fuerzaLanzamiento);

        // Consumir 1 del inventario
        Inventario.InventoryEntry entrada =
            inventario.objetos.Find(e => e.nombre == objetoLanzado.nombreObjeto);

        if (entrada != null)
            inventario.EliminarObjeto(entrada);

        // Equipar siguiente si queda alguno
        Invoke(nameof(EquiparSiguienteShuriken), 0.01f);
    }

    //Equipar siguiente shuriken 
    public void EquiparSiguienteShuriken()
    {
        if (objetoSujeto != null)
            return;

        Inventario.InventoryEntry entrada =
            inventario.objetos.Find(e => e.nombre == "Shuriken");

        if (entrada == null)
            return;

        GameObject nueva = Instantiate(entrada.prefab, puntoSujecion.position, puntoSujecion.rotation);
        Objeto nuevoObj = nueva.GetComponent<Objeto>();
        nueva.transform.localScale = entrada.escalaOriginal;

        if (nuevoObj != null)
            EquiparObjeto(nuevoObj);
            inventario.EliminarObjeto(entrada);
    }

    //Guardar objeto en inventario
    public void GuardarObjeto()
    {
        inventario.AñadirObjeto(objetoSujeto);
        objetoSujeto.CogerObjeto();
        objetoSujeto = null;
        Debug.Log("Objeto guardado en inventario.");
        CanvasInfo.SetActive(false);
    }

    //Equipar objeto desde inventario
    public void EquiparObjeto(Objeto objetoCercano)
    {
        if (objetoSujeto != null)
        {
            objetoSujeto.transform.SetParent(null);
            objetoSujeto.gameObject.SetActive(false);
        }
        objetoSujeto = objetoCercano;
        objetoSujeto.gameObject.SetActive(true);
        //Resetar valores del rididbody cada vez que se equipe un objeto
        Rigidbody2D rb = objetoSujeto.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.gravityScale = 0;
        }
        objetoSujeto.transform.position = puntoSujecion.position;
        objetoSujeto.transform.rotation = puntoSujecion.rotation;

        objetoSujeto.Coger(puntoSujecion);
        objetoSujeto.transform.localScale = Vector3.one;
        CanvasInfo.SetActive(true);
    }

    //Soltar objeto
    public void SoltarObjeto()
    {
        if (objetoSujeto != null)
        {
            Objeto objetoDejado = objetoSujeto;
            objetoDejado.transform.position = transform.position;
            objetoSujeto.Soltar();
            objetoSujeto = null;
            Transform light = objetoDejado.transform.Find("Light");
            if (light != null)
            {
                light.gameObject.SetActive(true);
            }
            CanvasInfo.SetActive(false);
        }
    }

    public void SoltarObjetoInventario(Objeto obj)
    {
        if (obj == null)
        {
            return;
        }

        obj.SoltarInventario(transform.position);
        Transform light = obj.transform.Find("Light");
        if (light != null)
        {
            light.gameObject.SetActive(true);
        }

        CanvasInfo.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("intObject") && toatTongeTonge.enabled)
        {
            objectPicked = other.gameObject;
            BoxCollider2D colliderTemp = objectPicked.GetComponent<BoxCollider2D>();
            colliderTemp.enabled = false;

        }

        if (other.CompareTag("KatanaEnemigo"))
        {
            if (staminaParry < 4 && staminaParry > 0.1 && Input.GetButton("Fire2"))
            {
                staminaParry -= 0.3f;
                fuenteSonido.PlayOneShot(sonidoParry);
            }
            else
            {
                gameManager.ReducirVida(1);

                fuenteSonido.PlayOneShot(sonidoDamage);

            }


        }

        if (other.CompareTag("nenufar"))
        {
            other.gameObject.layer = 0;
        }

        if (other.CompareTag("deathArea"))
        {
            gameManager.PlayerDie();
        }

        if (other.CompareTag("intObject"))
        {
            objetoCercano = other.GetComponent<Objeto>();
        }
        if (other.CompareTag("Tsunade"))
        {
            Debug.Log("Entraste en el rango de Tsunade");
            tsunadeCerca = true;
        }
        if (other.CompareTag("Moneda"))
        {
            GameManager.Instance.RecolectarMonedas();
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("nenufar"))
        {
            other.gameObject.layer = 3;
        }

        if (other.CompareTag("intObject"))
        {
            if (objetoCercano != null && other.gameObject == objetoCercano.gameObject)
            {
                objetoCercano = null;
            }
        }
        if (other.CompareTag("Tsunade"))
        {
            Debug.Log("Saliste del rango de Tsunade");
            tsunadeCerca = false;
        }
    }

    void saltarSapo()
    {
        if (human || estaSaltando)
        {
            return;
        }

        float rayDistance = 7f;
        Vector2 rayDirection = Vector2.right;

        switch (lastMove)
        {
            case 3:
                rayDirection = Vector2.right;
                break;
            case 4:
                rayDirection = Vector2.left;
                break;
            case 1:
                rayDirection = Vector2.up;
                break;
            case 2:
                rayDirection = Vector2.down;
                break;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, rayDistance, nenufarLayerMask);

        Debug.DrawRay(transform.position, rayDirection * rayDistance, Color.red);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("nenufar"))
            {
                StartCoroutine(SaltoPlano(hit.collider.transform.position));
            }
        }
    }

    void SmokeEfect()
    {
        StartCoroutine(SmokeEffectCorutine());
    }

    IEnumerator SmokeEffectCorutine()
    {
        focusAnimator.enabled = true;
        spriterendersmoke.enabled = true;
        focusAnimator.Play("humocambiodeposicion_Clip", 0, 0f);

        yield return new WaitForSeconds(0.4f);

        focusAnimator.enabled = false;
        spriterendersmoke.enabled = false;
        
    }
    
    //Prueva de corrutina para salto de sapo
    private IEnumerator SaltoPlano(Vector3 destino)
    {
        estaSaltando = true;

        float duracion = 0.70f;
        float tiempo = 0f;
        Vector3 inicio = transform.position;

        Vector3 escalaInicial = transform.localScale;
        Vector3 escalaMax = escalaInicial * 1.8f; //Efecto de salto (escala)

        rigidBody.linearVelocity = Vector2.zero;
        rigidBody.simulated = false; // pausa fisica

        fuenteSonido.PlayOneShot(sonidoFootstepFrog);

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            //Interpolacion
            transform.position = Vector3.Lerp(inicio, destino, t);

            if (t < 0.5f)
                transform.localScale = Vector3.Lerp(escalaInicial, escalaMax, t * 2);
            else
                transform.localScale = Vector3.Lerp(escalaMax, escalaInicial, (t - 0.5f) * 2);

            yield return null;
        }

        transform.position = destino;
        transform.localScale = escalaInicial;

        rigidBody.simulated = true;

        estaSaltando = false;
    }

    bool tsunadeInRange()
    {
        Vector2 direccion = ultimaDireccion;
        float distancia = 1f;
        Vector2[] direcciones = 
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        int layerMask = LayerMask.GetMask("tsunade");

        foreach (Vector2 dir in direcciones)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, distancia, layerMask);
            Debug.DrawRay(transform.position, dir * distancia, Color.yellow);
            if (hit.collider != null && hit.collider.CompareTag("Tsunade"))
            {
                return true;
            }

        }return false;
    }

    //Funciones para aceptar entrega y entregar objetos tsunade 
    public void AceptarEntrega()
    {
        List<Inventario.InventoryEntry> entregables = inventario.ObtenerEntregables();

        if (entregables.Count == 0)
        {
            return; 
        }

        if (entregables.Count == 1)
        {
           
            AceptarEntregaDirecta(entregables[0]);
        }
        else
        {
            // hay varios por lo tanto se entra en el modo de seleccion
            inventario.modoEntrega = true;
            inventario.indiceSeleccionEntrega = 0;
        }
    }

    public void AceptarEntregaDirecta(Inventario.InventoryEntry entry)
    {
        tsunade tsunadeScript = GameObject.FindWithTag("Tsunade").GetComponent<tsunade>();
        GameObject objeto = Instantiate(entry.prefab);
        Objeto objInstanciado = objeto.GetComponent<Objeto>();
        tsunadeScript.objetoRecibido = objInstanciado;

        inventario.EliminarObjeto(entry);
    }

    public void RecibirRecompensa(Objeto recompensa)
    {
        inventario.AñadirObjeto(recompensa);
    }

    public bool ObjetoTsunadeExiste()
    {
        foreach (var entry in inventario.objetos){
            if (entry.tipo == Objeto.TipoObjeto.PergaminoSagrado || 
            entry.tipo == Objeto.TipoObjeto.Flor || 
            entry.tipo == Objeto.TipoObjeto.CollarShizune)
            {
                return true;
            }
        }
        return false;
    }

}
