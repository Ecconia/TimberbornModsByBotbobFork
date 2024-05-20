using Bindito.Core;
using System;
using Timberborn.BlockSystem;
using Timberborn.BuilderPrioritySystem;
using Timberborn.BuildingsBlocking;
using Timberborn.Fields;
using Timberborn.Forestry;
using Timberborn.Hauling;
using Timberborn.Planting;
using Timberborn.WaterBuildings;
using Timberborn.Workshops;
using Timberborn.WorkSystem;
using static CopySettingsTool.CopySettingsUtil;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsTool
{
  internal class CopyGeneralSettingsContainer : ICopyFunctionContainer
  {
    public string Label => "General Settings";

    private readonly CopySettingsService _copySettingsService;

    [Inject]
    public CopyGeneralSettingsContainer(CopySettingsService copySettingsService)
    {
      _copySettingsService = copySettingsService;
      _copySettingsService.AddCopyFunctionContainer(this);
      LogDebug("General settings added to copy service.");
    }


    // copy other building settings
    public void CopySettings(BlockObject savedObject, BlockObject targetObject)
    {
      LogDebug("Copying general settings...");
      var copy = new CopyFunctionWrapper(savedObject, targetObject);
      if(!copy.ValidateBlockObjects())
      {
        return;
      }

      // other
      copy.WithComponentType<WorkplacePriority>()((s, t) => t.SetPriority(s.Priority));
      copy.WithComponentType<HaulPrioritizable>()((s, t) => t.Prioritized = s.Prioritized);
      copy.WithComponentType<BuilderPrioritizable>()((s, t) => t.SetPriority(s.Priority));
      copy.WithComponentType<WorkplaceWorkerType>()((s, t) => t.SetWorkerType(s.WorkerType));
      copy.WithComponentType<Forester>()((s, t) => t.SetReplantDeadTrees(s.ReplantDeadTrees));
      copy.WithComponentType<HaulPrioritizable>()((s, t) => t.Prioritized = s.Prioritized);
      copy.WithComponentType<FarmHouse>()((s, t) => { if (s.PlantingPrioritized) t.PrioritizePlanting(); else t.UnprioritizePlanting(); });
      copy.WithComponentType<Floodgate>()((s, t) => { t.SetHeight(s.Height); t.ToggleSynchronization(s.IsSynchronized); });

      // manufactory
      copy.WithComponentType<Manufactory>()((savedComponent, targetComponent) => {
        if(!targetComponent.ProductionRecipes.Contains(savedComponent.CurrentRecipe))
        {
          return;
        }
        targetComponent.SetRecipe(savedComponent.CurrentRecipe);
      });

      // plantable
      copy.WithComponentType<PlantablePrioritizer>()((savedComponent, targetComponent) => {
        if(savedComponent.PrioritizedPlantable == null)
        {
          targetComponent.PrioritizePlantable(null);
          return;
        }

        PlanterBuilding targetPlanterBuilding = targetComponent.GetComponentFast<PlanterBuilding>();
        if(targetPlanterBuilding == null || !targetPlanterBuilding.CanPlant(savedComponent.PrioritizedPlantable.PrefabName))
        {
          return;
        }

        targetComponent.PrioritizePlantable(savedComponent.PrioritizedPlantable);
      });

      // pausable
      copy.WithComponentType<PausableBuilding>()((savedObject, targetObject) => {

        PausableBuilding targetComponent = targetObject.GetEnabledComponent<PausableBuilding>();
        if (targetComponent == null || !targetComponent.IsPausable())
        {
          return;
        }
        PausableBuilding savedComponent = savedObject.GetEnabledComponent<PausableBuilding>();
        if(savedComponent == null) {
          return;
        }
        
        if (savedComponent.Paused) targetComponent.Pause(); else targetComponent.Resume();
      });


      // workplace
      copy.WithComponentType<Workplace>()(
          (s, t) =>
          {
            var increase = s.IncreaseDesiredWorkers;
            var decrease = s.DecreaseDesiredWorkers;

            if (t.DesiredWorkers == s.DesiredWorkers)
            {
              return;
            }
            Action increaseDecrease = (t.DesiredWorkers < s.DesiredWorkers) ? t.IncreaseDesiredWorkers : t.DecreaseDesiredWorkers;
            var tOld = -1;
            while (t.DesiredWorkers != s.DesiredWorkers
              || t.DesiredWorkers == tOld)
            {
              tOld = t.DesiredWorkers;
              increaseDecrease();
            }
          });
    }
  }
}