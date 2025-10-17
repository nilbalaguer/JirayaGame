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
        //body.AddForce(new Vector2(inputX * force, inputY * force));

        //body.linearVelocity = Vector2.ClampMagnitude(body.linearVelocity, maxSpeed);
        float angle = Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        body.linearVelocity = new Vector2(inputX * maxSpeed, inputY * maxSpeed);

        if(inputX < 0){
            transform.localScale = new Vector3(-5, 5, 5);
        }else if(inputX > 0){
            transform.localScale = new Vector3(5, 5, 5);
        }
        
    }

}
