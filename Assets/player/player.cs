using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
  public float speed = 0.0f;
  public float gravity = -9.0f;
  public float extraGravity = -10.0f;
  public float terminalVel = -3.0f;
  public float extSpeed = 0.0f;

  public float depthTarget = -4.0f;

  public float verSpeed = 0.0f;
  public float verSpeedDampen = 1.0f;
  public float verSpeedMin = 1.0f;
  public Vector3 jumpImpact = new Vector3();

  public Texture2D regular;
  public Texture2D happy;
  public Texture2D jump;
  public Texture2D dead;
  float regularTimer = 0.0f;
  public Renderer rend;

  GameManager gm;

  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();
    gm.registerPlayer(this);

  }

  float yPer(float offset = 0.0f)
  {
    return Mathf.Clamp01(transform.position.y + depthTarget / (gm.screenXMax() + offset - depthTarget));
  }

  // Update is called once per frame
  void Update()
  {
    switch (gm.state)
    {
      case (GameState.game):
        {
          if (Input.GetButtonDown("Right"))
          {
            JumpRight();
          }
          else if (Input.GetButtonDown("Left"))
          {
            JumpLeft();
          }

          float y = transform.position.y;
          float clampY = Mathf.Clamp(y - depthTarget, 0.0f, 1.0f);
          extSpeed = speed > 0 ? 0 : speed * (1.0f - clampY);
          float locSpeed = speed - extSpeed;

          transform.Translate(verSpeed * Time.deltaTime, locSpeed * Time.deltaTime, 0);

          float locGravity = gravity; //+ (yPer * yPer) * extraGravity;
          if (speed > terminalVel)
          {
            speed = speed + locGravity * Time.deltaTime;
          }

          float absVerSpeed = Mathf.Abs(verSpeed);
          if (absVerSpeed > verSpeedMin)
          {
            verSpeed = Mathf.Sign(verSpeed) * (absVerSpeed - verSpeedDampen * Time.deltaTime);
          }

          if (regularTimer > 0) {
            regularTimer -= Time.deltaTime;
            if (regularTimer < 0) {
              rend.material.mainTexture = regular;
            }
          }

          break;
        }
    }
  }

  public void Jump(float direction)
  {
    float p = yPer(-1.0f);
    this.speed = jumpImpact.y * (0.3f + 0.7f * (1.0f - p * p));

    float verMult = Mathf.Sign(verSpeed) == Mathf.Sign(direction) ? 1.1f : 1.0f; 
    this.verSpeed = jumpImpact.x * direction * verMult;

    rend.material.mainTexture = jump;
    regularTimer = 0.3f;
  }

  public void JumpRight()
  {
    rend.transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
    Jump(-1.0f);
  }

  public void JumpLeft()
  {
    rend.transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
    Jump(1.0f);
  }

  public void MakeHappy(bool veryHappy = false) {
    if (veryHappy || regularTimer <= 0.0f) {
      rend.material.mainTexture = happy;
      regularTimer = 0.2f;
    }
  }

  public void MakeDead() {
    rend.material.mainTexture = dead;
    regularTimer = 0.2f;
  }
}
