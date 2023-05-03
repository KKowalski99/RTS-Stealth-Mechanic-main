using UnityEngine;

namespace Tools
{
    public static class Logger
    {
        public static void LogMessage(object message, Object owner)
        {
            Debug.Log(message, owner);
        }
        public static void LogWarning(object message, Object owner)
        {
            Debug.LogWarning(message, owner);
        }
        public static void LogError(object message, Object owner)
        {
            Debug.LogError(message, owner);
        }


    }
}


