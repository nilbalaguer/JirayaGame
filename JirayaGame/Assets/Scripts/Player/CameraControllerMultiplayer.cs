// using UnityEngine;
// using Mirror;

// public class CameraControllerMultiplayer : NetworkBehaviour
// {
//     private GameObject playerGameObject;

//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         if (isLocalPlayer)
//         {
//             FindPlayer();
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if (playerGameObject == null)
//         {
//             if (isLocalPlayer)
//             {
//                 FindPlayer();
//             }
//         }

//         if (isLocalPlayer)
//         {
//             Vector3 movement = new Vector3(playerGameObject.transform.position.x, playerGameObject.transform.position.y, -10);

//             transform.position = movement;
//         }
//     }

//     void FindPlayer()
//     {
//         playerGameObject = GameObject.FindGameObjectWithTag("Player");
//     }
// }
using UnityEngine;
using Mirror;

public class CameraControllerMultiplayer : NetworkBehaviour
{
    public Transform target;

    void Start()
    {
        // Si no es el jugador local, desactivamos la cámara
        if (!isLocalPlayer)
        {
            GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
            return;
        }

        // Si es local, buscamos al jugador
        target = transform.parent; // si la cámara es hija del jugador
    }

    void LateUpdate()
    {
        if (!isLocalPlayer || target == null) return;

        transform.position = new Vector3(
            target.position.x,
            target.position.y,
            -10f
        );
    }
}
