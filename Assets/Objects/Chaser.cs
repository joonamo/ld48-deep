using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
  GameManager gm;

  public float interval = 10.0f;
  public float maxExtent = 0.7f;
  float extent = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    interval = interval + Random.Range(-interval * 0.1f, interval * 0.1f);
    gm = GameObject.FindObjectOfType<GameManager>();
    extent = gm.screenXMax * maxExtent;
  }

  // Update is called once per frame
  void Update()
  {
      float p = Mathf.PingPong(Time.time, interval);
      float phase = p / interval;
      transform.position = new Vector3(
          EasingFunction.EaseInOutSine(-extent, extent, phase),
          transform.position.y,
          transform.position.z
      );
  }
}
