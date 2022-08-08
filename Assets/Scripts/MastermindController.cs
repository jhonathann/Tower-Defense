using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controlls the mastermind gameobject in charge of making things in the game happen (idk how yet)
public class MastermindController : MonoBehaviour
{
     private GameObject menu;

     private void Start()
     {
          findMenu();
          menu.SetActive(false); //starts the game with the menu deactivated
     }

     private void Update()
     {
          toggleMenu();
     }
     //Finds the menu object with the name "Menu"
     private void findMenu()
     {
          menu = GameObject.Find("Menu");
     }
     //Activates or deactivates the menu when esc is pressed (and pause or depause the game accordingly)
     private void toggleMenu()
     {
          if (Input.GetKeyDown(KeyCode.Escape))
          {
               menu.SetActive(!menu.activeSelf);
               Time.timeScale = System.Convert.ToSingle(!menu.activeSelf);
          }
     }
}
