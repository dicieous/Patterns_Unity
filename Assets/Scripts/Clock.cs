using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hourHand, minuteHand, secondHand;
    private const float HoursToDegree = -30f, MinutesToDegree = -6f, SecondsToDegree = -6f;


    private void Update()
    {
        TimeSpan time = DateTime.Now.TimeOfDay;
        hourHand.localRotation = Quaternion.Euler(0,0,HoursToDegree*(float)time.TotalHours);
        minuteHand.localRotation = Quaternion.Euler(0,0,MinutesToDegree*(float)time.TotalMinutes);
        secondHand.localRotation = Quaternion.Euler(0,0,SecondsToDegree*(float)time.TotalSeconds);
    }
}