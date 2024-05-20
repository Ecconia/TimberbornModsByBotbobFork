using Bindito.Core;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsToolSimpleFloodgateTriggersSettings
{
  [Configurator(SceneEntrypoint.InGame)]
  internal sealed class Configurator : IConfigurator
  {
    public void Configure(IContainerDefinition containerDefinition)
    {
      LogDebug("CopySettingsToolSimpleFloodgateTriggersSettings Configurator alive!");
      containerDefinition.Bind<CopySimpleFloodgateTriggersSettingsContainer>().AsSingleton();
    }
  }
}