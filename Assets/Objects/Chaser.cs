using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
  GameManager gm;

  public float interval = 10.0f;
  public float maxExtent = 0.5f;
  float extent = 0.0f;
  bool lastDirection = true;
  float lastX = 999.0f;
  BaseObject myBase;

  // Start is called before the first frame update
  void Start()
  {
    interval = interval + Random.Range(-interval * 0.1f, interval * 0.1f);
    gm = GameObject.FindObjectOfType<GameManager>();
    extent = gm.screenXMax * maxExtent;
    myBase = GetComponent<BaseObject>();
  }

  // Update is called once per frame
  void Update()
  {
    if (gm.state == GameState.game)
    {
      float p = Mathf.PingPong(Time.time, interval);
      float phase = p / interval;
      Vector3 newPos = new Vector3(
          EasingFunction.EaseInOutSine(-extent, extent, phase),
          transform.position.y,
          transform.position.z
      );
      transform.position = newPos;

      bool direction = lastX < newPos.x;
      if (direction != lastDirection)
      {
        if (direction)
        {
          myBase.Flippendo();
        }
        else
        {
          myBase.FlippendoMaximus();
        }
        lastDirection = direction;
      }

      lastX = newPos.x;
    }
  }
}
