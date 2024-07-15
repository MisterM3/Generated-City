using UnityEngine;

public static class Logger
{

    private const bool LOGGER_ACTIVE = true;

    public static void Log(string log, Object sender = null)
    {
        if (LOGGER_ACTIVE) 
            Debug.Log(log, sender);
    }

    public static void Warning(string warning, Object sender = null)
    {
        if (LOGGER_ACTIVE)
            Debug.LogWarning(warning, sender);
    }

    public static void Error(string error, Object sender = null)
    {
        if (LOGGER_ACTIVE)
            Debug.LogError(error, sender);
    }
}
