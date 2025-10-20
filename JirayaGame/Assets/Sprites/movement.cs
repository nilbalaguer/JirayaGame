using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody2D body;
    //public float force;
    public float maxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float angle = Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg;
        body.linearVelocity = new Vector2(inputX * maxSpeed, inputY * maxSpeed);

        if(inputX < 0){
            transform.localScale = new Vector3(-5, 5, 5);
        }else if(inputX > 0){
            transform.localScale = new Vector3(5, 5, 5);
        }
        
    }

}
