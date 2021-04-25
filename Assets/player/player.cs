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

    public float verSpeed = 0.0f;
    public float verSpeedDampen = 1.0f;
    public float verSpeedMin = 1.0f;
    public Vector3 jumpImpact = new Vector3();

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();
        gm.registerPlayer(this);
    }

    float yPer(float offset = 0.0f) {
        return transform.position.y / (gm.screenXMax + offset);
    }

    // Update is called once per frame
    void Update()
    {
        float y = transform.position.y;
        float clampY = Mathf.Clamp(y, 0.0f, 1.0f);
        extSpeed = speed > 0 ? 0: speed * (1.0f - clampY);
        float locSpeed = speed - extSpeed;

        transform.Translate(verSpeed * Time.deltaTime, locSpeed * Time.deltaTime, 0);

        float locGravity = gravity; //+ (yPer * yPer) * extraGravity;
        if (speed > terminalVel) {
            speed = speed + locGravity * Time.deltaTime;
        }

        float absVerSpeed = Mathf.Abs(verSpeed);
        if (absVerSpeed > verSpeedMin) {
            verSpeed = Mathf.Sign(verSpeed) * (absVerSpeed - verSpeedDampen * Time.deltaTime );
        }

        if (Input.GetButtonDown("Right")) {
            JumpRight();
        } else if (Input.GetButtonDown("Left")) {
            JumpLeft();
        }
    }

    public void Jump(float direction) {
        float p = yPer(-1.0f);
        this.speed = jumpImpact.y * (0.5f + 0.5f * (1.0f - p * p));
        this.verSpeed = jumpImpact.x * direction;
    }

    public void JumpRight() {
        Jump(-1.0f);
    }
     
    public void JumpLeft() {
        Jump(1.0f);
    }
}
