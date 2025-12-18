using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;

public class SorpresaFinal : MonoBehaviour
{
    private Transform player;
    [SerializeField] Transform serpiente;
    [SerializeField] Rigidbody2D rigidbodySerpiente;
    [SerializeField] GameObject serpienteGameObject;
    [SerializeField] GameObject camera;
    [SerializeField] GameObject cosaNegra;
    [SerializeField] Light2D luzSol;
    private AudioSource audioSource;

    private bool rele = true;
    private bool mover = false;
    private Vector3 playerFixedPos;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        serpienteGameObject.SetActive(false);

        player = GameObject.Find("Player").GetComponent<Transform>();
        audioSource = gameObject.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.position.x < -70.00f && rele)
        {
            rele = false;
            StartCoroutine(disparar());
            playerFixedPos = player.position;
            audioSource.Play();
        }

        if (mover)
        {
            rigidbodySerpiente.MovePosition(Vector2.MoveTowards(serpiente.position, player.position, 20 * Time.fixedDeltaTime));
            player.position = playerFixedPos;
            
        }
    }

    private void LateUpdate() {
        if (!rele)
        {
            camera.transform.position = playerFixedPos + new Vector3(3, 0, -1);
        }
    }

    IEnumerator disparar()
    {
        StartCoroutine(AumentarRojo(3f));

        yield return new WaitForSeconds(2f);

        serpienteGameObject.SetActive(true);

        mover = true;

        yield return new WaitForSeconds(0.65f);

        cosaNegra.SetActive(true);

        SceneManager.LoadScene("PantallaFinal");
    }

    IEnumerator AumentarRojo(float duracion)
    {
        Color colorInicial = luzSol.color;
        Color colorFinal = new Color(1f, 0f, 0f);

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            luzSol.color = Color.Lerp(colorInicial, colorFinal, t);
            yield return null;
        }

        luzSol.color = colorFinal;
    }
}
