using UnityEngine;
using Mirror;
using System.Collections;

public class TpSeparador : NetworkBehaviour
{
    [SerializeField] Transform punto1;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            other.gameObject.transform.position = punto1.position;
        }
    }
}