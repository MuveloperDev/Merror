using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform Arrow_Hour = null;
    [SerializeField] private Transform Arrow_Minute = null;
    [SerializeField] private Transform Arrow_Second = null;

    private int Hour = 0;
    private int Minute = 0;
    private int Second = 0;

    private const int Degree_Hour = 30;
    private const int Degree_Minute = 6;
    private const int Degree_Second = 6;
    private void Start()
    {
        StartCoroutine(UpdateClock());
    }
    /// <summary>
    /// Update clock's arrows by system datetime
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateClock()
    {
        GetSystemTime();
        SetArrow();
        float deltaSecond = 0f;

        while (true)
        {
            deltaSecond += Time.deltaTime;
            if (deltaSecond >= 1.0f) // When one second passed,
            {
                deltaSecond = 0f;
                GetSystemTime(); // Update values
                SetArrow(); // Update rotations
            }
            yield return null;
        }
    }
    /// <summary>
    /// Get hour, minute and second from system
    /// </summary>
    private void GetSystemTime()
    {
        Hour = DateTime.Now.Hour;
        Minute = DateTime.Now.Minute;
        Second = DateTime.Now.Second;
    }
    /// <summary>
    /// Set arrow's angles by value get from system
    /// </summary>
    private void SetArrow()
    {
        if (Hour > 12) // If AM / PM mode on, 
        {
            Hour -= 12;
        }
        Arrow_Hour.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Hour * Degree_Hour));
        Arrow_Minute.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Minute * Degree_Minute));
        Arrow_Second.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Second * Degree_Second));
    }
}
