using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float slowdownFactor;
    private float slowdownLength;

    public bool slowTime;

    private void Start()
    {
        slowTime = false;
        slowdownFactor = 0.02f;
        slowdownLength = 2f;
    }

    private void Update()
    {
        if (PlayerInputController.instance.Current.AimInput)
        {
            slowTime = true;
        }
        else
        {
            slowTime = false;
        }

        TimeMonitor();
    }

    public void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void TimeSpeedUp()
    {
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    private void TimeMonitor()
    {
        if (slowTime)
        {
            DoSlowmotion();
        }
        else if (!slowTime)
        {
            TimeSpeedUp();
        }
    }
}
