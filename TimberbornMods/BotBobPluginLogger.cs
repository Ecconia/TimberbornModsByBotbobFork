namespace TimberbornMods
{
  using System;
  using System.Collections.Generic;
  using System.Diagnostics;

  public static class BotBobPluginLogger
  {
    internal static bool _isDebug = false;


    private static Dictionary<LogColor, Action<string>> LogColors = new Dictionary<LogColor, Action<string>>{
    { LogColor.White, UnityEngine.Debug.Log },
    { LogColor.Yellow, UnityEngine.Debug.LogWarning },
    { LogColor.Red, UnityEngine.Debug.LogError }};


    private static Action<string> GetLogMethod(LogColor logColor)
    {
      Action<string> result = null;
      LogColors.TryGetValue(logColor, out result);
      return result ?? UnityEngine.Debug.Log;
    }

    enum LogColor
    {
      White,
      Yellow,
      Red
    }

    public static void LogTrace(string message, params object[] args)
    {
      if (!_isDebug)
      {
        return;
      }

      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Trace: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.White);

      log(messageWithClassInfo);
    }

    public static void LogDebug(string message, params object[] args)
    {
      if (!_isDebug)
      {
        return;
      }

      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Debug: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.White);

      log(messageWithClassInfo);
    }

    public static void LogInfo(string message, params object[] args)
    {
      if (!_isDebug)
      {
        return;
      }

      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Info: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.White);

      log(messageWithClassInfo);
    }

    public static void LogMessage(string message, params object[] args)
    {

      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Message: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.White);

      log(messageWithClassInfo);
    }

    public static void LogWarn(string message, params object[] args)
    {
      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Warn: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.Yellow);

      log(messageWithClassInfo);
    }

    public static void LogError(string message, params object[] args)
    {
      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Error: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.Red);

      log(messageWithClassInfo);
    }

    public static void LogException(Exception ex, string message, params object[] args)
    {
      var methodInfo = new StackTrace().GetFrame(1).GetMethod();
      var className = methodInfo.ReflectedType.Name;

      var formattedMessage = string.Format(message, args);
      var messageWithClassInfo = string.Format("<{0}> Exception: {1}", className, formattedMessage);

      var log = GetLogMethod(LogColor.White);

      log(messageWithClassInfo);
      
      UnityEngine.Debug.LogException(ex);
    }
  }
}