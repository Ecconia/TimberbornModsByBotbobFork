using System;
using System.Collections.Generic;
using Timberborn.BlockSystem;
using Timberborn.PrefabSystem;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsTool
{
  public class CopySettingsService : ICopySettingsService
  {
    private BlockObject _savedObject;
    private readonly List<ICopyFunctionContainer> _copyFuncContainerList;

    //private readonly Dictionary<ICopyFunctionContainer, List<BlockObject>> _copyableComponents;


    public CopySettingsService()
    {
      _copyFuncContainerList = [];
      _savedObject = null;
      LogDebug("CopySettingsService alive.");
    }

    internal BlockObject GetSavedObject()
    {
      return _savedObject;
    }

    internal bool HasSavedObject() => _savedObject != null;

    internal void SaveObject(BlockObject blockObject)
    {
      _savedObject = blockObject;
      LogDebug(String.Format("Saved object '{0}'", blockObject.GetComponentFast<Prefab>().Name));
    }

    public void AddCopyFunctionContainer(ICopyFunctionContainer container)
    {
      if (container == null)
      {
        LogWarn("Container was null.");
        return;
      }

      _copyFuncContainerList.Add(container);
      LogDebug(String.Format("Added ICopyFuncContainer '{0}' with label: '{1}'", container.GetType(), container.Label));
    }

    internal void CopySavedObjectSettingsTo(BlockObject blockObject)
    {
      if (_savedObject == null || blockObject == null)
      {
        return;
      }

      LogDebug(" ");
      string msg = String.Format("| Copying settings from '{0}' to '{1}'",
          _savedObject.GetComponentFast<Prefab>().Name, blockObject.GetComponentFast<Prefab>().Name);
      string equals = new string('=', Math.Max(0, 100 - msg.Length));
      LogDebug(equals + msg);

      foreach (ICopyFunctionContainer copyFuncContainer in _copyFuncContainerList)
      {
        try
        {

          LogDebug(" ");
          LogDebug(string.Format("Calling '{0}' ... ", copyFuncContainer.Label));
          copyFuncContainer.CopySettings(_savedObject, blockObject);

        }
        catch (Exception ex)
        {
          LogError(
              string.Format("Exception when trying to run copy method in ICopyFuncContainer '{0}'," +
              " container label: '{1}', exception => {2}{3}",
              copyFuncContainer.GetType(), copyFuncContainer.Label, Environment.NewLine, ex.ToString()));
        }
      }
      LogDebug(new string('=', 100));
      LogDebug(" ");
    }
  }
}