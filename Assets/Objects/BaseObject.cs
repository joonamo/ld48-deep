using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ContactJump
{
  left,
  right,
  both,
  none
}

public enum ContactAction
{
  damage,
  score,
  multiplier,
  goal
}

public class BaseObject : MonoBehaviour
{
  GameManager gm;
  public bool wrapping = false;
  public float yExtent = 5.0f;
  public bool stopAtZero = false;

  public ContactJump contactJump = ContactJump.none;
  public ContactAction contactAction = ContactAction.damage;

  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();
  }

  // Update is called once per frame
  void Update()
  {
    switch (gm.state)
    {
      case (GameState.game):
        {
          if (!stopAtZero || transform.position.y < 0.0f)
          {
            transform.Translate(0.0f, gm.generalSpeed * Time.deltaTime, 0.0f, Space.World);
            if (transform.position.y > gm.screenYMax + yExtent)
            {
              if (wrapping)
              {
                this.transform.position =
                    new Vector3(
                        transform.position.x,
                        -gm.screenYMax - yExtent,
                        transform.position.z);
              }
              else
              {
                GameObject.Destroy(gameObject);
              }
            }
          }
          break;
        }
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Player")
    {
      player p = other.transform.GetComponent<player>();
      switch (contactJump)
      {
        case (ContactJump.left):
          {
            p.JumpLeft();
            break;
          }
        case (ContactJump.right):
          {
            p.JumpRight();
            break;
          }
        case (ContactJump.both):
          {
            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
              p.JumpRight();
            }
            else
            {
              p.JumpLeft();
            }
            break;
          }
      }

      switch (contactAction)
      {
        case (ContactAction.score):
          {
            gm.addPoint();
            Destroy(gameObject);
            break;
          }
        case (ContactAction.multiplier):
          {
            gm.addMultiplier();
            Destroy(gameObject);
            break;
          }
        case (ContactAction.damage):
          {
            // TODO
            break;
          }
        case (ContactAction.goal):
          {
            // TODO
            break;
          }
      }
    }
  }
}
