using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
     private GameObject menu;
     // Start is called before the first frame update
     void Start()
     {
          menu = GameObject.Find("Menu");
     }

     // Update is called once per frame
     void Update()
     {
          if (Input.GetKeyDown(KeyCode.Escape))
          {
               menu.SetActive(!menu.activeSelf);
          }
     }
}
