using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    menu,
    intro,
    game,
    outro
}

public class GameManager : MonoBehaviour
{
    public float screenYMax = 5.0f;
    public float screenXMax = 5.0f;

    public float generalSpeed = 10.0f;

    public player player;

    public int score = 0;
    public int multiplier = 1;
    public int targetmultiplier = 3;

    public GameObject goal;

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

    public void addPoint() {
        score += multiplier;
    }

    public void addMultiplier() {
        multiplier += 1;
        if (multiplier == targetmultiplier) {
            Instantiate(goal, new Vector3(0.0f, screenYMax + 5.0f, 0.0f), Quaternion.identity);
        }
    }
}
