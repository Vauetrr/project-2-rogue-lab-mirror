using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform follow;
    public float FollowHeight = 1.0f;
    // Update is called once per frame
    void Update()
    {
        this.transform.position = follow.position + new Vector3(0.0f, FollowHeight, 0.0f);
    }
}
