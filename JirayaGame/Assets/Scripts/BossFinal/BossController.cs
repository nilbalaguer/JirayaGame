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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizarSaltos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DispararBombarda()
    {
        RandomizarSaltos();
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
    }
}
