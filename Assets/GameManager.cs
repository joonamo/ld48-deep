using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
  menu,
  intro,
  game,
  outro,
  gameOver
}

public class GameManager : MonoBehaviour
{
  public float screenYMax() { return 9.0f; }
  public float screenXMax() { return 5.5f; }

  public float generalSpeed() { return 8.0f; }

  public player player;
  public ObjectManager objectManager;

  public int score = 0;
  public int multiplier = 1;
  public int multiplierGoal = 4;

  public GameObject goal;

  Score scoreDisplay;

  public GameState state = GameState.menu;
  public float stateChangeTime = 0.0f;

  public List<ShellUI> shellUis = new List<ShellUI>();

  public AudioSource music;
  public AudioSource collect;
  public AudioSource collectBig;
  public AudioSource death;

  Intro intro;

  // Start is called before the first frame update
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {
    switch (state)
    {
      case (GameState.menu):
        {
          if (Input.GetButtonDown("Button"))
          {
            changeState(GameState.intro);
            intro.Reset();
          }
          break;
        }
      case (GameState.intro): {
        break;
      }
      case (GameState.outro):
      case (GameState.gameOver): {
          if (Input.GetButtonDown("Button"))
          {
            ResetGame();
          }
        break;
      }
    }
  }

  public void changeState(GameState newState)
  {
    state = newState;
    stateChangeTime = Time.time;
  }

  public float timeInState() {
    return Time.time - stateChangeTime;
  }

  public void registerPlayer(player newPlayer)
  {
    player = newPlayer;
  }

  public void registerObjectManager(ObjectManager om) {
    objectManager = om;
  }

  public void registerScoreDisplay(Score newDisplay)
  {
    scoreDisplay = newDisplay;
    scoreDisplay.ShowScore(score);
  }

  public void registerIntro(Intro newIntro) {
    intro = newIntro;
  }

  public void addPoint()
  {
    if (state != GameState.game) return;

    collect.Play();

    score += multiplier;
    scoreDisplay.ShowScore(score);
  }

  public void addMultiplier()
  {
    if (state != GameState.game) return;

    collectBig.Play();

    multiplier += 1;
    if (multiplier == multiplierGoal)
    {
      Instantiate(goal, new Vector3(0.0f, -screenYMax() - 5.0f, 0.0f), Quaternion.identity);
    }

    foreach (ShellUI shell in shellUis) {
      if (shell.shellAmount == multiplier) {
        shell.turnOn();
      }
    }

    player.MakeHappy();
  }

  public void reportGoal() {
    if (state != GameState.game) return;

    collectBig.Play();

    changeState(GameState.outro);
    player.MakeHappy(true);
    intro.ReportHappy();
  }

  public void reportDeath() {
    if (state != GameState.game) return;

    death.Play();

    changeState(GameState.outro);
    player.MakeDead();
    intro.ReportSad();
  }

  public void ResetGame() {
    player.Reset();
    score = 0;
    scoreDisplay.ShowScore(score);
    multiplier = 1;
    foreach (ShellUI shell in shellUis) {
      shell.turnOff();
    }

    objectManager.Reset();
    intro.Reset();

    changeState(GameState.intro);
  }
}
