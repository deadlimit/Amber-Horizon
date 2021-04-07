using System;

public class Timer {
    
    public float RemainingSeconds { get; private set; }
    public event Action OnTimerReachesZero;

    public Timer(float duration) {
        RemainingSeconds = duration;
    }

    public void Tick(float deltatime) {
        if (RemainingSeconds < 0) return;

        RemainingSeconds -= deltatime;

        CheckForTimerEnd();
    }

    private void CheckForTimerEnd() {
        if (RemainingSeconds > 0f) return;

        RemainingSeconds = 0;
        OnTimerReachesZero?.Invoke();
    }
}
