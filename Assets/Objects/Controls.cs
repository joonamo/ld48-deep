using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Right") || Input.GetButtonDown("Left")) {
            Destroy(gameObject, 3.0f);
        }
    }
}
