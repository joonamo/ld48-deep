using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour
{
  public float scrollTime = 10.0f;
  public float scrollDistance = 18.0f;

  GameManager gm;

  Vector3 startPos;
  Vector3 endPos;
  float startTime = 0.0f;

  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();

    this.startPos = transform.position;
    this.endPos = startPos + Vector3.up * scrollDistance;
  }

  // Update is called once per frame
  void Update()
  {
    if (gm.state == GameState.game) {
      float phase = ((Time.time - startTime) % scrollTime) / scrollTime;
      transform.position = Vector3.Lerp(startPos, endPos, phase);
    } else {
      startTime = Time.time;
    }
  }
}
