using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim : MonoBehaviour
{
    Vector3 playerPos;
    Vector3 gunPosition;
    Transform player;
    internal Vector2 direction;
    float gunAngle;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = new Vector2 (player.position.x, player.position.y + 0.2f);
        gunPosition = transform.position;
        playerPos.x -= gunPosition.x;
        playerPos.y -= gunPosition.y;
        
        gunAngle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 180, gunAngle));
        direction = new Vector2(Mathf.Cos(gunAngle*Mathf.PI/180), Mathf.Sin(gunAngle*Mathf.PI / 180)/2);


        //RaycastHit2D hit2D = Physics2D.Raycast(player.position, direction);
        //Debug.Log(hit2D.transform.name);
        transform.rotation = Quaternion.Euler(new Vector3(180f, 0f, gunAngle));


        if (player.position.x < transform.position.x )
        {
            transform.rotation = Quaternion.Euler(new Vector3(180f, 180f, gunAngle));


        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180f, -gunAngle));


        }

    }
}
