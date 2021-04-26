using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellUI : MonoBehaviour
{
  public int shellAmount = 0;
  public Renderer rend;
  // Start is called before the first frame update
  void Start()
  {
     rend.enabled = false;
  }

  public void turnOn()
  {
    rend.enabled = true;
  }

  public void turnOff()
  {
    rend.enabled = false;
  }
}
