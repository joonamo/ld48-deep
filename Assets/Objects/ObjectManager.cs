﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
  GameManager gm;
  public GameObject leftWall;
  public GameObject rightWall;

  public List<GameObject> dangerObjects = new List<GameObject>();
  public float dangerMinTime = 1.0f;
  public float dangerMaxTime = 3.0f;
  public float timeToNextDanger = 5.0f;

  public List<GameObject> cliffObjects = new List<GameObject>();
  public float cliffMinTime = 0.4f;
  public float cliffMaxTime = 4.0f;
  public float timeToNextCliff = 1.0f;


  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();

    for (int i = -1; i <= 1; i++)
    {
      GameObject.Instantiate(leftWall, new Vector3(gm.screenXMax, gm.screenYMax * i * 2, 0.0f), Quaternion.identity);
      GameObject.Instantiate(rightWall, new Vector3(-gm.screenXMax, gm.screenYMax * i * 2, 0.0f), Quaternion.identity);
    }
  }

  // Update is called once per frame
  void Update()
  {
    timeToNextDanger = timeToNextDanger - Time.deltaTime;
    if (timeToNextDanger <= 0.0f)
    {
      GameObject toSpawn = dangerObjects[Random.Range(0, dangerObjects.Count)];
      var newObject = GameObject.Instantiate(toSpawn, new Vector3(0.0f, -999.0f, 0.0f), Quaternion.identity);
      float xExtent = gm.screenXMax - newObject.transform.lossyScale.x * 0.5f;
      newObject.transform.position = new Vector3(
          Random.Range(-xExtent, xExtent),
          -gm.screenYMax - newObject.transform.lossyScale.y,
          0.0f);
      float r = Random.Range(0.0f, 1.0f);
      timeToNextDanger = Mathf.Lerp(dangerMinTime, dangerMaxTime, r * r);
    }

    timeToNextCliff = timeToNextCliff - Time.deltaTime;
    if (timeToNextCliff <= 0.0f)
    {
      GameObject toSpawn = cliffObjects[Random.Range(0, cliffObjects.Count)];
      var newObject = GameObject.Instantiate(toSpawn, new Vector3(0.0f, -999.0f, 0.0f), Quaternion.identity);
      float side = Random.Range(-1.0f, 1.0f) < 0.0f ? -1.0f : 1.0f;
      var extent = newObject.transform.lossyScale;
      newObject.transform.position = new Vector3(
          side * gm.screenXMax - side * extent.x * 0.5f, //Random.Range(0.0f, extent.x * 0.7f),
          -gm.screenYMax - extent.y * 0.5f,
          0.0f);

      float r = Random.Range(0.0f, 1.0f);
      timeToNextCliff = Mathf.Lerp(cliffMinTime, cliffMaxTime, r * r);
    }
  }
}