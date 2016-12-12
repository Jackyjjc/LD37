using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

    private Transform hourHand;
    private Transform minuteHand;

    private float hour;
    public float Hour
    {
        get { return hour; }
        set { hour = value; Render(hour, minute); }
    }
    private float minute;
    public float Minute
    {
        get { return minute; }
        set { minute = value; Render(hour, minute); }
    }

    void Awake()
    {
        hourHand = transform.FindChild("HourHand");
        minuteHand = transform.FindChild("MinuteHand");
        hour = 23;
        minute = 58;
    }
	
    public void Tick(float delta)
    {
        minute += delta;
        while (minute >= 60)
        {
            minute -= 60;
            hour += 1;
            while (hour >= 24)
            {
                hour -= 24;
            }
        }

        Render(hour + minute / 60, minute);
    }

    private void Render(float hour, float minute)
    {
        hourHand.transform.rotation = Quaternion.AngleAxis(-360 * (hour / 12), Vector3.forward);
        minuteHand.transform.rotation = Quaternion.AngleAxis(-360 * (minute / 60), Vector3.forward);
    }
}
