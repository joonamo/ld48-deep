using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    GameManager gm;
    public GameObject leftWall;
    public GameObject rightWall;

    public List<GameObject> dangerObjects = new List<GameObject>();
    public float minTime = 1.0f;
    public float maxTime = 3.0f;
    public float timeToNext = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

        for (int i = -1; i <= 1; i++) {
            GameObject.Instantiate(leftWall, new Vector3(gm.screenXMax, gm.screenYMax * i * 2, 0.0f), Quaternion.identity);
            GameObject.Instantiate(rightWall, new Vector3(-gm.screenXMax, gm.screenYMax * i * 2, 0.0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeToNext = timeToNext - Time.deltaTime;
        if (timeToNext <= 0.0f) {
            GameObject toSpawn = dangerObjects[Random.Range(0, dangerObjects.Count - 1)];
            var newObject = GameObject.Instantiate(toSpawn, new Vector3(0.0f, -999.0f, 0.0f), Quaternion.identity);
            float xExtent = gm.screenXMax - newObject.transform.lossyScale.x * 0.5f;
            newObject.transform.position = new Vector3(
                Random.Range(-xExtent, xExtent),
                -gm.screenYMax - newObject.transform.lossyScale.y * 0.5f,
                0.0f);

            timeToNext = Random.Range(minTime, maxTime);
        }
    }
}
