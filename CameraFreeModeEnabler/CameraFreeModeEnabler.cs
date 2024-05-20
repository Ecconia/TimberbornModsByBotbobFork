namespace CameraFreeModeEnabler
{
  using Bindito.Core;
  using TimberApi.ConfiguratorSystem;
  using TimberApi.SceneSystem;
  using Timberborn.CameraSystem;
  using Timberborn.SingletonSystem;
  using static TimberbornMods.BotBobPluginLogger;

  [Configurator(SceneEntrypoint.InGame)]
  internal class CameraFreeModeEnabler : IConfigurator, IPostLoadableSingleton
  {
    private CameraComponent _cameraComponent = null;

    [method: Inject]
    public void InjectDependancies(CameraComponent cameraComponent)
    {
      _cameraComponent = cameraComponent;
      LogTrace("Injected cameraComponent '{0}'", cameraComponent);
    }

    public void Configure(IContainerDefinition ContainerDefinition)
    {
      ContainerDefinition.Bind<CameraFreeModeEnabler>().AsSingleton();
      LogTrace("CameraFreeModeEnabler alive!");
    }

    public void PostLoad()
    {
      EnableFreeCamera();
    }

    private void EnableFreeCamera()
    {
      LogTrace("Trying to enable free mode with _cameraComponent '{0}'", _cameraComponent);
      if (_cameraComponent && _cameraComponent.FreeMode == false)
      {
        LogMessage("Enabled camera free mode!");
        _cameraComponent.FreeMode = true;
      }
    }
  }
}