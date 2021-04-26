using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
  public GameObject bg;
  public GameObject comic1;
  public GameObject comic2;
  public GameObject comic3;

  public float comicDelay = 3.0f;
  public float comicAppearTime = 0.5f;
  public float appearDistance = 25.0f;

  Vector3 comic1TargetPos;
  Vector3 comic2TargetPos;
  Vector3 comic3TargetPos;

  bool audio1Played = false;
  bool audio2Played = false;
  bool audio3Played = false;
  public AudioSource sound;
  public float audioPhase = 0.8f;

  public float outroDelay = 5.0f;

  GameManager gm;
  
  public Texture2D startScreen;
  public Texture2D comicBackground;
  public Texture2D happyEnd;
  public Texture2D sadEnd;
  public Renderer bgRend;

  public TMPro.TextMeshPro text; 

  // Start is called before the first frame update
  void Start()
  {
    comic1TargetPos = comic1.transform.position;
    setComic(0.0f, 0.0f, Vector3.zero, comic1.transform);
    comic2TargetPos = comic2.transform.position;
    setComic(0.0f, 0.0f, Vector3.zero, comic2.transform);
    comic3TargetPos = comic3.transform.position;
    setComic(0.0f, 0.0f, Vector3.zero, comic3.transform);

    gm = GameObject.FindObjectOfType<GameManager>();
    gm.registerIntro(this);

    text.text = "";
  }

  // Update is called once per frame
  void Update()
  {
    if (gm.state == GameState.intro)
    {
      float t = gm.timeInState();

      float comic1AppearPhase = (Mathf.Clamp(t, 0.0f, comicAppearTime)) / comicAppearTime;
      float comicDisappearPhase = (Mathf.Clamp(t, comicDelay * 3, comicDelay * 3 + comicAppearTime) - comicDelay * 3) / comicAppearTime;

      if (t > 0.1f && Input.GetButtonDown("Button")) {
        comicDisappearPhase = 1.0f;
      }

      setComic(comic1AppearPhase, comicDisappearPhase, comic1TargetPos, comic1.transform);

      float comic2AppearPhase = (Mathf.Clamp(t, comicDelay, comicDelay + comicAppearTime) - comicDelay) / comicAppearTime;
      setComic(comic2AppearPhase, comicDisappearPhase, comic2TargetPos, comic2.transform);

      float comic3AppearPhase = (Mathf.Clamp(t, comicDelay * 2, comicDelay * 2 + comicAppearTime) - comicDelay * 2) / comicAppearTime;
      setComic(comic3AppearPhase, comicDisappearPhase, comic3TargetPos, comic3.transform);
      
      bg.transform.localPosition = new Vector3(
        0.0f,
        EasingFunction.EaseOutQuart(0.0f, 18.0f, comicDisappearPhase),
        0.0f
      );


      if (comicDisappearPhase >= 1.0f)
      {
        if (!gm.music.isPlaying)
        {
          gm.music.Play();
        }
        gm.changeState(GameState.game);
      }

      if (!audio1Played && comic1AppearPhase > audioPhase)
      {
        sound.pitch = 1.0f;
        sound.Play();
        audio1Played = true;
      }
      if (!audio2Played && comic2AppearPhase > audioPhase)
      {
        sound.pitch = 1.1f;
        sound.Play();
        audio2Played = true;
      }
      if (!audio3Played && comic3AppearPhase > audioPhase)
      {
        sound.pitch = 1.2f;
        sound.Play();
        audio3Played = true;
      }
    }
    else if (gm.state == GameState.outro)
    {
      float t = gm.timeInState();
      if (t < 0.1) {
        text.text = gm.score.ToString("#0000");
      }

      float phase = (Mathf.Clamp(t, outroDelay, outroDelay + comicAppearTime) - outroDelay) / comicAppearTime;

      bg.transform.localPosition = new Vector3(
        0.0f,
        EasingFunction.EaseOutQuart(18.0f, 0.0f, phase),
        0.0f
      );

      if (phase >= 1.0f) {
        gm.changeState(GameState.gameOver);
      }
    }
  }

  void setComic(float appearPhase, float disappearPhase, Vector3 targetPos, Transform t)
  {
    if (disappearPhase == 0.0f)
    {
      t.localPosition = new Vector3(
        EasingFunction.EaseOutQuart(-appearDistance, targetPos.x, appearPhase),
        targetPos.y,
        targetPos.z
      );
    }
    else
    {
      t.localPosition = new Vector3(
        EasingFunction.EaseOutQuart(targetPos.x, appearDistance, disappearPhase),
        targetPos.y,
        targetPos.z
      );
    }
  }

  public void ReportHappy() {
    bgRend.material.mainTexture = happyEnd;
  }

  public void ReportSad() {
    bgRend.material.mainTexture = sadEnd;
  }

  public void Reset() {
    bgRend.material.mainTexture = comicBackground;
    audio1Played = false;
    audio2Played = false;
    audio3Played = false;

    text.text = "";
  }
}
