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
  goal,
  bounce
}

public class BaseObject : MonoBehaviour
{
  GameManager gm;
  public bool wrapping = false;
  public float yExtent = 5.0f;
  public bool stopAtZero = false;
  public bool staticOnly = false;

  public ContactJump contactJump = ContactJump.none;
  public ContactAction contactAction = ContactAction.damage;

  public MeshFilter myMesh;

  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();
  }

  // GameManager getGm() {
  //   if (!gm) {
  //     gm = GameObject.FindObjectOfType<GameManager>();
  //   }
  //   return gm;
  // }

  // Update is called once per frame
  void Update()
  {

    if (!staticOnly && gm.state == GameState.game)
    {
      if (!stopAtZero || transform.position.y < 0.0f)
      {
        transform.Translate(0.0f, gm.generalSpeed() * Time.deltaTime, 0.0f, Space.World);
        if (transform.position.y > gm.screenYMax() + yExtent)
        {
          if (wrapping)
          {
            transform.position =
                new Vector3(
                    transform.position.x,
                    -gm.screenYMax() - yExtent,
                    transform.position.z);
          }
          else
          {
            GameObject.Destroy(gameObject);
          }
        }
      }

    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.transform.tag == "Player")
    {
      player p = other.transform.GetComponent<player>();

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
            gm.reportDeath();
            break;
          }
        case (ContactAction.goal):
          {
            gm.reportGoal();
            break;
          }
        case (ContactAction.bounce):
          {
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
            break;
          }
      }
    }
  }

  Transform getRendable()
  {
    if (!myMesh)
    {
      myMesh = GetComponentInChildren<MeshFilter>();
    }
    return myMesh.transform;
  }

  public void Flippendo()
  {
    // print("flip");
    transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
  }

  public void FlippendoMaximus()
  {
    // print("flop");
    transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
  }
}
