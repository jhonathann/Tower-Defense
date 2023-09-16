using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Timer class that uses coroutines as a countdown timer and executes an action at the end of the timer
/// </summary>
public class Timer
{
     /// <summary>
     /// Tells if the timer is Done
     /// </summary>
     /// <value></value>
     public bool IsDone { get; private set; }
     /// <summary>
     /// Variable that keps the remaining time as a string
     /// </summary>
     public string remainingTimeAsString = "00:00";
     public Timer()
     {
          IsDone = true;
     }
     /// <summary>
     /// Starts the coroutine
     /// </summary>
     /// <param name="time">Time until the action is executed</param>
     /// <param name="OnCountDownOver"> Action to be executed</param>
     /// <param name="usingObject">Monobehaviour object needed to execute the coroutine</param>
     public void StartTimer(float time, Action OnCountDownOver, MonoBehaviour usingObject)
     {
          usingObject.StartCoroutine(CountDown(time, OnCountDownOver));
     }
     /// <summary>
     /// Coroutine that countsdown and executes an action when the time is over
     /// </summary>
     /// <param name="time"time until the action is executed></param>
     /// <param name="OnCountDownOver">Action to be executed</param>
     /// <returns></returns>
     private IEnumerator CountDown(float time, Action OnCountDownOver)
     {
          IsDone = false;
          while (time > 0)
          {
               remainingTimeAsString = RemainingTimeToString(time);
               time -= Time.deltaTime;
               yield return null;
          }
          OnCountDownOver?.Invoke();
          IsDone = true;
     }

     /// <summary>
     /// Turns a float value into a strong representing the value as time
     /// </summary>
     /// <param name="remainingTime">time</param>
     /// <returns>The formated time string</returns>
     private string RemainingTimeToString(float remainingTime)
     {
          int minutes = (int)remainingTime / 60;
          int seconds = (int)remainingTime % 60;
          return $"{minutes}:{seconds}";
     }
}
