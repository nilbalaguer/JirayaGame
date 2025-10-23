using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollPanel : MonoBehaviour
{
    public GameObject text;
    private Animator animator;
    public npcReputacion reputacion;
    public bool hasTalked = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnEnable()
    {
        text.SetActive(false);
    }
    public void ShowText()
    {
        text.SetActive(true);
    }

    public void ClosePanel()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
        }
    }

    public void DisablePanel()
    {
        gameObject.SetActive(false);
    }

    public void HideText()
    {
        text.SetActive(false);
    }

    public void botonSi()
    {
        reputacion.RespuestaPositiva();
        animator.SetTrigger("Close");
        hasTalked = true;
    }
    
    public void botonNo()
    {
        reputacion.RespuestaNegativa();
        animator.SetTrigger("Close");
        hasTalked = true;
    }
}
