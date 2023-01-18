using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scriptable Object Used to store and change the state of the game
/// </summary>
[CreateAssetMenu]
public class GameState : ScriptableObject
{
     /// <summary>
     /// Propperty of the state
     /// </summary>
     public GameStateType State { get; private set; }
     /// <summary>
     /// Variable that stores whether the game enter in the paused state while in placingtower state or not
     /// </summary>
     private bool pausedWhilePlacingTower = false;

     private void OnEnable()
     {
          SetUnstartedState(); //Initialization of the state variable
     }
     /// <summary>
     /// Changes the state of the game if it is possible 
     /// </summary>
     /// <param name="newState">new State to be evaluated</param>
     public void TrySetState(GameStateType newState)
     {
          switch (State)
          {
               case GameStateType.Unstarted:
                    switch (newState)
                    {
                         case GameStateType.Unstarted:
                              break;
                         case GameStateType.Running:
                              SetRunningState();
                              break;
                         case GameStateType.Paused:
                              break;
                         case GameStateType.PlacingTower:
                              break;
                         case GameStateType.GameOver:
                              break;
                    }
                    break;
               case GameStateType.Running:
                    switch (newState)
                    {
                         case GameStateType.Unstarted:
                              break;
                         case GameStateType.Running:
                              break;
                         case GameStateType.Paused:
                              SetPausedState(false);
                              break;
                         case GameStateType.PlacingTower:
                              SetPlacingTowerState();
                              break;
                         case GameStateType.GameOver:
                              SetGameOverState();
                              break;
                    }
                    break;
               case GameStateType.Paused:
                    switch (newState)
                    {
                         case GameStateType.Unstarted:
                              break;
                         case GameStateType.Running:
                              if (pausedWhilePlacingTower)
                              {
                                   SetPlacingTowerState();
                              }
                              else
                              {
                                   SetRunningState();
                              }
                              break;
                         case GameStateType.Paused:
                              break;
                         case GameStateType.PlacingTower:
                              break;
                         case GameStateType.GameOver:
                              break;
                    }
                    break;
               case GameStateType.PlacingTower:
                    switch (newState)
                    {
                         case GameStateType.Unstarted:
                              break;
                         case GameStateType.Running:
                              SetRunningState();
                              break;
                         case GameStateType.Paused:
                              SetPausedState(true);
                              break;
                         case GameStateType.PlacingTower:
                              break;
                         case GameStateType.GameOver:
                              SetGameOverState();
                              break;
                    }
                    break;
               case GameStateType.GameOver:
                    switch (newState)
                    {
                         case GameStateType.Unstarted:
                              break;
                         case GameStateType.Running:
                              SetRunningState();
                              break;
                         case GameStateType.Paused:
                              break;
                         case GameStateType.PlacingTower:
                              break;
                         case GameStateType.GameOver:
                              break;
                    }
                    break;
          }
     }
     //Functions to the executed when each of the states is going changed
     private void SetUnstartedState()
     {
          State = GameStateType.Unstarted;
     }
     private void SetRunningState()
     {
          State = GameStateType.Running;
          Time.timeScale = 1;
     }
     private void SetPausedState(bool pausedWhilePlacingTower)
     {
          State = GameStateType.Paused;
          this.pausedWhilePlacingTower = pausedWhilePlacingTower;
          Time.timeScale = 0;
     }
     private void SetPlacingTowerState()
     {
          State = GameStateType.PlacingTower;
          Time.timeScale = 1;
     }
     private void SetGameOverState()
     {
          State = GameStateType.GameOver;
          Time.timeScale = 0;
     }
}

public enum GameStateType
{
     Unstarted,
     Running,
     Paused,
     PlacingTower,
     GameOver
}