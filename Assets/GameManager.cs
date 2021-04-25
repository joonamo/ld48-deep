using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float screenYMax = 5.0f;
    public float screenXMax = 5.0f;

    public float generalSpeed = 10.0f;

    public player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void registerPlayer(player newPlayer) {
        player = newPlayer;
    }
}
