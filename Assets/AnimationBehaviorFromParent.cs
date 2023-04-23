using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviorFromParent : MonoBehaviour
{
    [SerializeField] Chest parentScript;
    // Start is called before the first frame update

    public void CallEventFromParentScript()
    {
        transform.parent.GetComponent<Chest>().GiveReward();
    }
}
