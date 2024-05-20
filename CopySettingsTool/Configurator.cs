using Automation.Utils;
using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsTool
{
  [Configurator(SceneEntrypoint.InGame)]
  internal sealed class Configurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      LogDebug("BotBobPlugin CopySettingsTool Configurator alive!");
      containerDefinition.Bind<CopySettingsService>().AsSingleton();
      CustomToolSystem.BindTool<CopySettingsTool>(containerDefinition, null);
      containerDefinition.Bind<CopyStockpileSettingsContainer>().AsSingleton();
      containerDefinition.Bind<CopyGeneralSettingsContainer>().AsSingleton();
    }
  }
}