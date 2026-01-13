using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject cajaSalto1;
    [SerializeField] GameObject cajaSalto2;
    [SerializeField] GameObject nenufarSalto1;
    [SerializeField] GameObject nenufarSalto2;

    [SerializeField] GameObject prefabBola;
    [SerializeField] GameObject prefabPolvora;
    [SerializeField] Transform punto1;
    [SerializeField] Transform punto2;

    [SerializeField] Transform spawnEnemigo1;
    [SerializeField] Transform spawnEnemigo2;
    [SerializeField] GameObject enemigo;
    [SerializeField] GameObject enemigo2;

    [SerializeField] CabezaSerpiente serpienteScript;

    [SerializeField] int dificultad = 0;

    [SerializeField] AudioClip bossMusic;
    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizarSaltos();

        audioSource = gameObject.GetComponent<AudioSource>();

        serpienteScript.vida = 10;
        serpienteScript.speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (serpienteScript.vida < 4)
        {
            dificultad = 1;
        }
    }

    public void DispararBombarda()
    {
        RandomizarSaltos();

        serpienteScript.speed += 1.5f;
    }

    private void RandomizarSaltos()
    {
        Debug.Log("RandomizarSaltosEjecutado");
        float random1 = Random.Range(0f,1f);
        if (random1 >= 0.5f)
        {
            cajaSalto1.SetActive(true);
            cajaSalto2.SetActive(false);
            nenufarSalto1.SetActive(true);
            nenufarSalto2.SetActive(false);

            Instantiate(prefabBola, punto1.position, Quaternion.identity);
            Instantiate(prefabPolvora, punto2.position, Quaternion.identity);
        } else
        {
            cajaSalto1.SetActive(false);
            cajaSalto2.SetActive(true);
            nenufarSalto1.SetActive(false);
            nenufarSalto2.SetActive(true);
            Instantiate(prefabBola, punto2.position, Quaternion.identity);
            Instantiate(prefabPolvora, punto1.position, Quaternion.identity);
        }

        if (dificultad > 0)
        {
            if (Random.Range(0f, 10f) > 0f)
            {
                GameObject tempEnemigo1 = Instantiate(enemigo, spawnEnemigo1.position, Quaternion.identity);
                Enemigo1Script enemigo1Script = tempEnemigo1.GetComponent<Enemigo1Script>();

                enemigo1Script.puntoA = punto1;
                enemigo1Script.puntoB = spawnEnemigo1;

                GameObject tempEnemigo2 = Instantiate(enemigo, spawnEnemigo2.position, Quaternion.identity);
                Enemigo1Script enemigo2Script = tempEnemigo2.GetComponent<Enemigo1Script>();

                enemigo2Script.puntoA = punto2;
                enemigo2Script.puntoB = spawnEnemigo2;
            }
            else
            {
                GameObject tempEnemigo1 = Instantiate(enemigo2, spawnEnemigo1.position, Quaternion.identity);
                Enemigo2DistanciaScript enemigo1Script = tempEnemigo1.GetComponent<Enemigo2DistanciaScript>();

                enemigo1Script.puntoA = punto1;
                enemigo1Script.puntoB = spawnEnemigo1;

                GameObject tempEnemigo2 = Instantiate(enemigo2, spawnEnemigo2.position, Quaternion.identity);
                Enemigo2DistanciaScript enemigo2Script = tempEnemigo2.GetComponent<Enemigo2DistanciaScript>();

                enemigo2Script.puntoA = punto2;
                enemigo2Script.puntoB = spawnEnemigo2;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            audioSource.clip = bossMusic;
            audioSource.Play();
        }
    }
}
