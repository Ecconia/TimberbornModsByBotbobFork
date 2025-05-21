using Timberborn.ModManagerScene;
using UnityEngine;

namespace CameraFreeModeEnabler;

using Bindito.Core;
using Timberborn.CameraSystem;
using Timberborn.SingletonSystem;

public class CameraFreeModeEnabler : IModStarter
{
	public void StartMod()
	{
	}
}

[Context("Game")]
public class CameraFreeModeEnablerConfigurator : IConfigurator
{
	public void Configure(IContainerDefinition ContainerDefinition)
	{
		ContainerDefinition.Bind<CameraFreeModeEnablerService>().AsSingleton();
	}
}

public class CameraFreeModeEnablerService : IPostLoadableSingleton
{
	private CameraService _cameraService;
	
	public CameraFreeModeEnablerService(CameraService cameraService)
	{
		_cameraService = cameraService;
	}
	
	public void PostLoad()
	{
		if (_cameraService.FreeMode == false)
		{
			Debug.Log("[Mod/CameraFreeModeEnablerEcconiaUpdate] Enabled camera free mode!");
			_cameraService.FreeMode = true;
		}
	}
}
