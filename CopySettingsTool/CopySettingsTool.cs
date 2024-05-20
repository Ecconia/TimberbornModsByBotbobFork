using Automation.Tools;
using Automation.Utils;
using Bindito.Core;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.SingletonSystem;
using Timberborn.ToolSystem;
using UnityEngine;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsTool
{
  public class CopySettingsTool : AbstractLockingTool, ICopyTool
  {
    private static Color SavedBuildingHighlightColor = Color.red * 0.5f;

    private CopySettingsService _copySettingsService;
    private EventBus _eventBus;
    private Highlighter _highlighter;

    protected override string CursorName => null;

    public override string WarningText() => "";

    [Inject]
    public void InjectDependancies(Highlighter highlighter, EventBus eventBus, CopySettingsService copySettingsService)
    {
      _highlighter = highlighter;
      _eventBus = eventBus;
      _copySettingsService = copySettingsService;
    }

    protected override void Initialize()
    {
      SetColorSchema(Color.magenta, Color.red, Color.white, Color.white);
      _eventBus.Register(this);
      base.Initialize();
      DescriptionBullets = null;
      LogDebug("CopySettingsTool alive!");
    }

    [OnEvent]
    public void OnToolEntered(ToolEnteredEvent toolEnteredEvent)
    {
      {
        if (toolEnteredEvent.Tool is not ICopyTool)
          return;
      }
      BlockObject savedObject = _copySettingsService.GetSavedObject();
      if (savedObject != null)
      {
        _highlighter.HighlightSecondary(savedObject, SavedBuildingHighlightColor);
      }
      LogDebug("Tool opened.");
    }

    [OnEvent]
    public void OnToolExited(ToolExitedEvent toolExitedEvent)
    {
      if (toolExitedEvent.Tool is not ICopyTool)
      {
        return;
      }
      BlockObject savedObject = _copySettingsService.GetSavedObject();
      if (savedObject != null)
      {
        _highlighter.ResetAllHighlights(savedObject);
      }
      LogDebug("Tool closed.");
    }

    protected override void OnSelectionModeChange(bool isSelectionMode)
    {
      base.OnSelectionModeChange(isSelectionMode);
      if (isSelectionMode && base.SelectedObjects != null && base.SelectedObjects.Count > 0)
      {
        foreach (BlockObject selectedObject in SelectedObjects)
        {
          if (!CheckCanLockOnComponent(selectedObject)) _highlighter.ResetAllHighlights(selectedObject);
        }
      }
    }

    protected override bool CheckCanLockOnComponent(BlockObject obj)
    {

      if (SelectionModeActive && IsAltHeld && SelectedObjects != null && SelectedObjects.First() == obj)
      {
        return true;
      }

      return false;
    }

    protected override bool ObjectFilterExpression(BlockObject blockObject)
    {
      if(!SelectionModeActive)
      {
        return true;
      }

      // hold alt to select object from which to copy from
      if (IsAltHeld)
      {
        BlockObject firstObjectInSelection = SelectedObjects.First();
        bool isFirstObjectInSelection = firstObjectInSelection == blockObject;
        return isFirstObjectInSelection; // select only the first object
      }

      // hold shift to allow copying component settings to objects of a different type
      if (!IsShiftHeld && _copySettingsService.HasSavedObject())
      {
        //
        string savedName = _copySettingsService.GetSavedObject().GetComponentFast<Prefab>().Name;
        return blockObject.GetComponentFast<Prefab>().IsNamed(savedName);
      }

      return true;
      // check map for components
    }

    protected override void OnObjectAction(BlockObject blockObject)
    {
      // hold alt and click an object to save it
      if (IsAltHeld)
      {
        if (blockObject == null)
        {
          return;
        }
        SetSavedObject(blockObject);
        return;
      }

      _copySettingsService.CopySavedObjectSettingsTo(blockObject);
    }

    private void SetSavedObject(BlockObject objectToSave)
    {
      if (objectToSave == null)
      {
        return;
      }
      BlockObject savedObject = _copySettingsService.GetSavedObject();
      if (savedObject != null)
      {
        _highlighter.ResetAllHighlights(savedObject);
      }
      _copySettingsService.SaveObject(objectToSave);
      _highlighter.HighlightSecondary(objectToSave, SavedBuildingHighlightColor);
    }
  }
}