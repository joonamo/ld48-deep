using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
  GameManager gm;
  public TMPro.TextMeshPro text;
  // Start is called before the first frame update
  void Start()
  {
    gm = GameObject.FindObjectOfType<GameManager>();
    gm.registerScoreDisplay(this);
  }

  public void ShowScore (int score) {
    text.text = score.ToString("#0000");
  }
}
