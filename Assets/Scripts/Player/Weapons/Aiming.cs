using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour
{
    Vector3 mouseposition;
    Vector3 gunPosition;
    [SerializeField] Transform player;
    Camera maincamera;
    internal Vector2 direction;
    float gunAngle;
    // Start is called before the first frame update
    void Start()
    {
        maincamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        mouseposition = Input.mousePosition;
        gunPosition = Camera.main.WorldToScreenPoint(transform.position);
        mouseposition.x -= gunPosition.x;
        mouseposition.y -= gunPosition.y;
        
        gunAngle = Mathf.Atan2(mouseposition.y, mouseposition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, gunAngle));
        direction = new Vector2(Mathf.Cos(gunAngle*Mathf.PI/180), Mathf.Sin(gunAngle*Mathf.PI / 180)/2);


        //RaycastHit2D hit2D = Physics2D.Raycast(player.position, direction);
        //Debug.Log(hit2D.transform.name);


        if (maincamera.ScreenToWorldPoint(Input.mousePosition).x < player.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, -gunAngle));


        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0f, gunAngle));


        }
    }
}
