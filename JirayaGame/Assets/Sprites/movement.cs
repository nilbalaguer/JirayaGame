using UnityEngine;

public class movement : MonoBehaviour
{
    private Rigidbody2D body;
    //public float force;
    public float maxSpeed;
    public bool puedoMoverme = true;
    public Transform puntoSujecion;
    public float distanciaSujecion = 0.1f;
    private Vector2 ultimaDireccion = Vector2.right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!puedoMoverme)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        float angle = Mathf.Atan2(inputY, inputX) * Mathf.Rad2Deg;
        Vector2 direccion = new Vector2(inputX, inputY);

        body.linearVelocity = direccion * maxSpeed;

        if (direccion != Vector2.zero)
        {
            ultimaDireccion = direccion.normalized;
        }

        if (inputX < 0)
        {
            transform.localScale = new Vector3(-5, 5, 5);
        }
        else if (inputX > 0)
        {
            transform.localScale = new Vector3(5, 5, 5);
        }

        //Mover puntoSujecion junto al jugador
        Vector2 offset = ultimaDireccion * distanciaSujecion;
        if (transform.localScale.x < 0)
        {
            offset.x *= -1;
        }
        puntoSujecion.localPosition = offset;   
    }

}
