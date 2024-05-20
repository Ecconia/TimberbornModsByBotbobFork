namespace TimberbornMods
{
  using System;

  public interface IBotBobPluginLogger
  {
    public static void LogTrace(string message, params object[] args) => BotBobPluginLogger.LogTrace(message, args);

    public static void LogDebug(string message, params object[] args) => BotBobPluginLogger.LogDebug(message, args);

    public static void LogInfo(string message, params object[] args) => BotBobPluginLogger.LogInfo(message, args);

    public static void LogWarn(string message, params object[] args) => BotBobPluginLogger.LogWarn(message, args);

    public static void LogError(string message, params object[] args) => BotBobPluginLogger.LogError(message, args);



    public static void LogException(Exception ex, string message, params object[] args) => BotBobPluginLogger.LogException(ex, message, args);


  }
}