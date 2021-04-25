using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    public float interval = 1.0f;
    public float maximum = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        interval = interval + Random.Range(-interval * 0.1f, interval * 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        float phase = Mathf.PingPong( Time.time, interval) / interval;
        transform.eulerAngles = new Vector3(0.0f, 0.0f, EasingFunction.EaseInOutCubic(-maximum, maximum, phase));
    }
}
