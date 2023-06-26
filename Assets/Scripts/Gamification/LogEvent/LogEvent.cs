using System;

[Serializable]
public class LogEvent
{
    public LogEventType eventType;
    public string message;
    public int sharpenedKnifeForTrigger;
    public bool isApplied;
}
