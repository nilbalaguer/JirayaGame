using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenCinematic : MonoBehaviour
{
    public static ScreenCinematic Instance;
    public GameObject cinematicContainer;
    public Animator animator;

    void Awake()
    {
        if (Instance == null){
            Instance = this;
        }
        else{
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cinematicContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarCinematic()
    {
        cinematicContainer.SetActive(true);
    }

    public void HideBars()
    {
        if (cinematicContainer.activeSelf)
        {
            StartCoroutine(HideBarsAndDisable());
        }
    }

    IEnumerator HideBarsAndDisable()
    {
        animator.SetTrigger("HideBars");
        yield return new WaitForSeconds(0.5f); 
        cinematicContainer.SetActive(false);
    }
}
