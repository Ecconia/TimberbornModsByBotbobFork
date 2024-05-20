using Bindito.Core;
using CopySettingsTool;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction;
using Hytone.Timberborn.Plugins.Floodgates.EntityAction.WaterPumps;
using System.Linq;
using Timberborn.BlockSystem;
using Timberborn.Common;
using UnityEngine.InputSystem;
using static CopySettingsTool.CopySettingsUtil;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsToolSimpleFloodgateTriggersSettings
{
  public class CopySimpleFloodgateTriggersSettingsContainer : ICopyFunctionContainer
  {

    public string Label => "Floodgate Triggers Settings";

    private CopySettingsService _copySettingsService;

    [Inject]
    public void InjectDependancies(CopySettingsService copySettingsService)
    {
      _copySettingsService = copySettingsService;
      _copySettingsService.AddCopyFunctionContainer(this);
      LogDebug("Flood gate settings added to copy service.");
    }

    public void CopySettings(BlockObject savedObject, BlockObject targetObject)
    {
      LogDebug("Copying flood gate trigger settings...");
      var copy = new CopyFunctionWrapper(savedObject, targetObject);
      if (!copy.ValidateBlockObjects())
      {
        return;
      }
      // if ctrl also change which streamgauge is linked
      // else only copy link attributes
      bool copyLinks = Keyboard.current.ctrlKey.isPressed;

      copy.WithComponentType<WaterPumpMonobehaviour>()(
          (saved, target) =>
          {
            target.PauseOnDroughtStart = saved.PauseOnDroughtStart;
            target.UnpauseOnDroughtStart = saved.UnpauseOnDroughtStart;
            target.PauseOnTemperateStarted = saved.PauseOnTemperateStarted;
            target.UnpauseOnTemperateStarted = saved.UnpauseOnTemperateStarted;
            target.PauseOnBadtideStarted = saved.PauseOnBadtideStarted;
            target.UnpauseOnBadtideStarted = saved.UnpauseOnBadtideStarted;
            target.ScheduleEnabled = saved.ScheduleEnabled;
            target.DisableScheduleOnDrought = saved.DisableScheduleOnDrought;
            target.DisableScheduleOnTemperate = saved.DisableScheduleOnTemperate;
            target.DisableScheduleOnBadtide = saved.DisableScheduleOnBadtide;
            target.PauseOnScheduleTime = saved.PauseOnScheduleTime;
            target.ResumeOnScheduleTime = saved.ResumeOnScheduleTime;

            LogDebug("Copying water pump link settings...");
            WaterPumpStreamGaugeLink savedLink = null;
            System.Collections.ObjectModel.ReadOnlyCollection<WaterPumpStreamGaugeLink> savedWaterPumpLinks = saved.WaterPumpLinks;
            if (savedWaterPumpLinks != null && !savedWaterPumpLinks.IsEmpty())
            {
              savedLink = saved.WaterPumpLinks.First();
            }
            WaterPumpStreamGaugeLink targetLink = null;
            System.Collections.ObjectModel.ReadOnlyCollection<WaterPumpStreamGaugeLink> targetWaterPumpLinks = target.WaterPumpLinks;
            if (targetWaterPumpLinks != null && !targetWaterPumpLinks.IsEmpty())
            {
              targetLink = saved.WaterPumpLinks.First();
            }

            if (copyLinks)
            {
              target.DetachAllLinks();
              target.AttachLink(target, savedLink.StreamGauge);
              if (target.WaterPumpLinks.Count > 0)
              {
                targetLink = target.WaterPumpLinks[0];
              }
              else
              {
                targetLink = null;
              }
            }

            if (savedLink != null && targetLink != null)
            {
              targetLink.Threshold1 = savedLink.Threshold1;
              targetLink.Threshold2 = savedLink.Threshold2;
              targetLink.Threshold3 = savedLink.Threshold3;
              targetLink.Threshold4 = savedLink.Threshold4;
              targetLink.Enabled1 = savedLink.Enabled1;
              targetLink.Enabled2 = savedLink.Enabled2;
              targetLink.Enabled3 = savedLink.Enabled3;
              targetLink.Enabled4 = savedLink.Enabled4;
              targetLink.DisableDuringDrought = savedLink.DisableDuringDrought;
              targetLink.DisableDuringTemperate = savedLink.DisableDuringTemperate;
              targetLink.DisableDuringBadtide = savedLink.DisableDuringBadtide;
              targetLink.ContaminationPauseBelowEnabled = savedLink.ContaminationPauseBelowEnabled;
              targetLink.ContaminationPauseAboveEnabled = savedLink.ContaminationPauseAboveEnabled;
              targetLink.ContaminationUnpauseBelowEnabled = savedLink.ContaminationUnpauseBelowEnabled;
              targetLink.ContaminationUnpauseAboveEnabled = savedLink.ContaminationUnpauseAboveEnabled;
              targetLink.ContaminationPauseBelowThreshold = savedLink.ContaminationPauseBelowThreshold;
              targetLink.ContaminationPauseAboveThreshold = savedLink.ContaminationPauseAboveThreshold;
              targetLink.ContaminationUnpauseBelowThreshold = savedLink.ContaminationUnpauseBelowThreshold;
              targetLink.ContaminationUnpauseAboveThreshold = savedLink.ContaminationUnpauseAboveThreshold;
              targetLink.DisableDuringBadtide = savedLink.DisableDuringBadtide;
              targetLink.DisableDuringDrought = savedLink.DisableDuringDrought;
              targetLink.DisableDuringTemperate = savedLink.DisableDuringTemperate;
            }
          }
      );

      copy.WithComponentType<FloodgateTriggerMonoBehaviour>()(
          (saved, target) =>
          {
            target.DroughtEndedEnabled = saved.DroughtEndedEnabled;
            target.DroughtEndedHeight = saved.DroughtEndedHeight;
            target.DroughtStartedEnabled = saved.DroughtStartedEnabled;
            target.DroughtStartedHeight = saved.DroughtStartedHeight;
            target.FirstScheduleTime = saved.FirstScheduleTime;
            target.FirstScheduleHeight = saved.FirstScheduleHeight;
            target.SecondScheduleTime = saved.SecondScheduleTime;
            target.SecondScheduleHeight = saved.SecondScheduleHeight;
            target.ScheduleEnabled = saved.ScheduleEnabled;
            target.DisableScheduleOnDrought = saved.DisableScheduleOnDrought;
            target.DisableScheduleOnTemperate = saved.DisableScheduleOnTemperate;
            target.DisableScheduleOnBadtide = saved.DisableScheduleOnBadtide;
            target.BadtideEndedEnabled = saved.BadtideEndedEnabled;
            target.BadtideEndedHeight = saved.BadtideEndedHeight;
            target.BadtideStartedEnabled = saved.BadtideStartedEnabled;
            target.BadtideStartedHeight = saved.BadtideStartedHeight;

            LogDebug("Copying flood gate link settings...");
            StreamGaugeFloodgateLink savedLink = null;

            System.Collections.ObjectModel.ReadOnlyCollection<StreamGaugeFloodgateLink> savedFloodgateLinks = saved.FloodgateLinks;
            if (savedFloodgateLinks != null && !savedFloodgateLinks.IsEmpty())
            {
              savedLink = saved.FloodgateLinks.First();
            }
            StreamGaugeFloodgateLink targetLink = null;
            System.Collections.ObjectModel.ReadOnlyCollection<StreamGaugeFloodgateLink> targetFloodgateLinks = target.FloodgateLinks;
            if (targetFloodgateLinks != null && !targetFloodgateLinks.IsEmpty())
            {
              targetLink = target.FloodgateLinks.First();
            }

            if (copyLinks)
            {
              if (savedLink == null && targetLink != null)
              {
                target.DetachAllLinks();
                targetLink = null;
              }
              if (savedLink != null)
              {
                target.DetachAllLinks();
                target.AttachLink(target, savedLink.StreamGauge);
                if (target.FloodgateLinks.Count > 0)
                {
                  targetLink = target.FloodgateLinks[0];
                }
                else
                {
                  targetLink = null;
                }
              }
            }

            if (savedLink != null && targetLink != null)
            {
              targetLink.Threshold1 = savedLink.Threshold1;
              targetLink.Threshold2 = savedLink.Threshold2;
              targetLink.Height1 = savedLink.Height1;
              targetLink.Height2 = savedLink.Height2;
              targetLink.ContaminationThresholdLow = savedLink.ContaminationThresholdLow;
              targetLink.ContaminationThresholdHigh = savedLink.ContaminationThresholdHigh;
              targetLink.ContaminationHeight1 = savedLink.ContaminationHeight1;
              targetLink.ContaminationHeight2 = savedLink.ContaminationHeight2;
              targetLink.EnableContaminationLow = savedLink.EnableContaminationLow;
              targetLink.EnableContaminationHigh = savedLink.EnableContaminationHigh;
              targetLink.DisableDuringBadtide = savedLink.DisableDuringBadtide;
              targetLink.DisableDuringDrought = savedLink.DisableDuringDrought;
              targetLink.DisableDuringTemperate = savedLink.DisableDuringTemperate;
            }
          }
      );

      // special case, if saved object is stream gauge and target object can be linked to it, link it
      // have to hold shift to allow copying between different type objects
      bool linkSavedStreamGaugeToTarget = Keyboard.current.shiftKey.isPressed;
      FloodgateTriggerMonoBehaviour targetFloodGate = targetObject.GetComponentFast<FloodgateTriggerMonoBehaviour>();
      WaterPumpMonobehaviour targetWaterPump = targetObject.GetComponentFast<WaterPumpMonobehaviour>();
      StreamGaugeMonoBehaviour savedStreamGauge = savedObject.GetComponentFast<StreamGaugeMonoBehaviour>();
      if (savedStreamGauge != null && targetFloodGate != null)
      {
        targetFloodGate.DetachAllLinks();
        targetFloodGate.AttachLink(targetFloodGate, savedStreamGauge);
      }
      if (savedStreamGauge != null && targetWaterPump != null)
      {
        targetWaterPump.DetachAllLinks();
        targetWaterPump.AttachLink(targetWaterPump, savedStreamGauge);
      }
    }
  }
}