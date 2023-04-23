using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
     Camera cam;


    [SerializeField] float parallaxFactorX;
    [SerializeField] float parallaxFactorY;

    Vector2 CamerastartPos;
    Vector2 startPos;
    Vector2 travel => (Vector2)cam.transform.position - CamerastartPos;

    [SerializeField] Transform levelStartpos;

    void Start()
    {
        cam = Camera.main;
        startPos = transform.position;
        CamerastartPos = levelStartpos.position;
    }

    void Update()
    {
        Vector2 newPos = startPos + (new Vector2(travel.x * parallaxFactorX, travel.y * parallaxFactorY));
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
