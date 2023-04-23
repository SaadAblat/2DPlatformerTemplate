using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipController : MonoBehaviour
{

    internal Rigidbody2D rb;
    [SerializeField] float maxSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float ThrustForce;
    private float yAxis;
    private float xAxis;
    [SerializeField] GameObject SpaceShipFire;
    internal bool canRotate;

    //[SerializeField] Animator SpaceShipFireAnimator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    private void ClampVelocity()
    {
        float x = Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed);
        float y = Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed);

        rb.velocity = new Vector2(x, y);
    }
    void ThrustForwatd(float amount)
    {
        Vector2 force = transform.up * amount;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
            if (Input.GetKey(KeyCode.Space))
            {
                yAxis = 1;
                SpaceShipFire.SetActive(true);

            }
            else
            {
                yAxis = 0;
                SpaceShipFire.SetActive(false);
            }
            //yAxis = Input.GetAxis("Vertical");
            xAxis = Input.GetAxis("Horizontal");


    }
    private void FixedUpdate()
    {
            ClampVelocity();
            ThrustForwatd(yAxis * ThrustForce);
            rotatingFunction(transform, xAxis * -rotationSpeed);

    }
    private void rotatingFunction(Transform t, float amount)
    {
        if (canRotate)
        {
            t.Rotate(0, 0, amount*Time.fixedDeltaTime);
        }
    }
    private void OnEnable()
    {
    transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canRotate = false;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        canRotate = true;
    }
}
