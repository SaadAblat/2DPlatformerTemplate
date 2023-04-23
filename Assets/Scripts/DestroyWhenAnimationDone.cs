using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenAnimationDone : MonoBehaviour
{

    public void DestroyGameobjectEvent()
    {
        Destroy(gameObject);
    }
}
