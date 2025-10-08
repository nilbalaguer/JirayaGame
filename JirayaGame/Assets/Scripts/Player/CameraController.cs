using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject playerGameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(playerGameObject.transform.position.x, playerGameObject.transform.position.y, -10);

        transform.position = movement;
    }
}
