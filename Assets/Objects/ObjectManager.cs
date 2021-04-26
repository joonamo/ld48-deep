using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
  GameManager gm;
  public GameObject leftWall;
  public GameObject rightWall;
  public GameObject coin;
  public GameObject megaCoin;

  public float coinInterval = 0.5f;
  public float timeToNextCoin = 0.0f;
  int coinsSpawned = 0;
  public int multiplierTime = 100;
  public EasingFunction.Ease coinRoad = EasingFunction.Ease.EaseInOutQuint;
  EasingFunction.Function coinRoadF;
  public float coinFunctionSpeed = 20.0f;
  public float coinExtentent = 0.7f;
  public float coinRoadOffset = 0.0f;
  public float offsetChance = 0.05f;
  public float offsetAmount = 30.0f;

  public List<GameObject> dangerObjects = new List<GameObject>();
  public float dangerMinTime = 1.0f;
  public float dangerMaxTime = 3.0f;
  public float timeToNextDanger = 5.0f;

  public List<GameObject> cliffObjects = new List<GameObject>();
  public float cliffMinTime = 0.4f;
  public float cliffMaxTime = 4.0f;
  public float timeToNextCliff = 1.0f;
  public float cliffSide = 1.0f;
  public int previousCliff = -1;

  bool hasPressed = false;

  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();
    gm.registerObjectManager(this);

    coinRoadF = EasingFunction.GetEasingFunction(coinRoad);

    for (int i = -1; i <= 1; i++)
    {
      GameObject.Instantiate(leftWall, new Vector3(gm.screenXMax(), gm.screenYMax() * i * 2, 0.0f), Quaternion.identity);
      GameObject.Instantiate(rightWall, new Vector3(-gm.screenXMax(), gm.screenYMax() * i * 2, 0.0f), Quaternion.identity);
    }
  }

  // Update is called once per frame
  void Update()
  {
    switch (gm.state)
    {
      case (GameState.game):
        {
          if (hasPressed) {
            timeToNextCliff = timeToNextCliff - Time.deltaTime;
            timeToNextDanger = timeToNextDanger - Time.deltaTime;
          } else if (Input.GetButtonDown("Right") || Input.GetButtonDown("Left") || gm.timeInState() > 30.0f) {
            hasPressed = true;
          }

          if (timeToNextDanger <= 0.0f)
          {
            GameObject toSpawn = dangerObjects[Random.Range(0, dangerObjects.Count)];
            var newObject = GameObject.Instantiate(toSpawn, new Vector3(0.0f, -999.0f, 0.0f), Quaternion.identity);
            newObject.tag = "RemoveOnRestart";
            
            float xExtent = gm.screenXMax() - newObject.transform.lossyScale.x * 1.5f;
            newObject.transform.position = new Vector3(
                Random.Range(-xExtent, xExtent),
                -gm.screenYMax() - newObject.transform.lossyScale.y,
                -0.1f);
            float r = Random.Range(0.0f, 1.0f);
            timeToNextDanger = Mathf.Lerp(dangerMinTime, dangerMaxTime, r * r);
          }

          if (timeToNextCliff <= 0.0f)
          {
            int rand = Random.Range(0, cliffObjects.Count);
            while (rand == previousCliff) {
              rand = Random.Range(0, cliffObjects.Count);
            }
            previousCliff = rand;
            GameObject toSpawn = cliffObjects[rand];

            var newObject = GameObject.Instantiate(toSpawn, new Vector3(0.0f, -999.0f, 0.0f), Quaternion.identity);
            BaseObject newBase = newObject.GetComponent<BaseObject>();
            newObject.tag = "RemoveOnRestart";

            float side = cliffSide;
            cliffSide *= -1.0f;
            var extent = newObject.transform.lossyScale;
            newObject.transform.position = new Vector3(
                side * gm.screenXMax() - side * extent.x * 0.56f, //Random.Range(0.0f, extent.x * 0.7f),
                -gm.screenYMax() - extent.y * 0.5f,
                -0.2f);
            if (side < 0.0f) {
              newBase.Flippendo();
            }

            float r = Random.Range(0.0f, 1.0f);
            timeToNextCliff = Mathf.Lerp(cliffMinTime, cliffMaxTime, r * r);
          }

          timeToNextCoin = timeToNextCoin - Time.deltaTime;
          if (timeToNextCoin <= 0.0f)
          {
            float phase = Mathf.PingPong(Time.time + coinRoadOffset, coinFunctionSpeed) / coinFunctionSpeed;
            float extent = gm.screenXMax() * coinExtentent;
            Vector3 pos = new Vector3(
              EasingFunction.EaseInOutQuad(-extent, extent, phase),
              -gm.screenYMax() - 1.0f,
              0.0f
            );

            var o = coinsSpawned > 0 && coinsSpawned % multiplierTime == 0 ? megaCoin : coin;
            var newObject = Instantiate(o, pos, Quaternion.identity);
            newObject.tag = "RemoveOnRestart";

            timeToNextCoin = coinInterval;
            coinsSpawned++;

            if (Random.Range(0.0f, 1.0f) < offsetChance)
            {
              coinRoadOffset += Random.Range(offsetAmount * 0.5f, offsetAmount);
            }
          }
          break;
        }
    }
  }

  public void Reset() {
    timeToNextCoin = 0.0f;
    coinsSpawned = 0;
    timeToNextDanger = 3.0f;
    timeToNextCliff = 2.0f;
    hasPressed = false;

    foreach (GameObject go in GameObject.FindGameObjectsWithTag("RemoveOnRestart")) {
      Destroy(go);
    }
  }
}
