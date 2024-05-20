namespace TimberbornMods
{
  using TimberApi.ConfigSystem;
  using TimberApi.ConsoleSystem;
  using TimberApi.ModSystem;

  public class BotBobPluginConfig : IModEntrypoint
  {
    public static bool DebugLogs { get; set; } = false;

    public class BotBobPluginConfigFile : IConfig
    {
      public BotBobPluginConfigFile()
      {
        DebugLogs = false;
      }

      public string ConfigFileName => "config";

      public bool DebugLogs { get; set; }
    }

    public void Entry(IMod mod, IConsoleWriter consoleWriter)
    {
      var config = mod.Configs.Get<BotBobPluginConfigFile>();
      bool _debugLogs = config.DebugLogs;

      BotBobPluginLogger._isDebug = _debugLogs;
      BotBobPluginConfig.DebugLogs = _debugLogs;
    }
  }
}