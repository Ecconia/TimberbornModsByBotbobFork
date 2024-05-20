using Bindito.Core;
using Timberborn.BlockSystem;
using Timberborn.Emptying;
using Timberborn.InventorySystem;
using Timberborn.StatusSystem;
using Timberborn.StockpilePrioritySystem;
using Timberborn.Stockpiles;
using static TimberbornMods.BotBobPluginLogger;

namespace CopySettingsTool
{
  internal class CopyStockpileSettingsContainer : ICopyFunctionContainer
  {
    public string Label => "Storage Stockpile Settings";

    private readonly CopySettingsService _copySettingsService;

    [Inject]
    public CopyStockpileSettingsContainer(CopySettingsService copySettingsService)
    {
      _copySettingsService = copySettingsService;
      _copySettingsService.AddCopyFunctionContainer(this);
      LogDebug("Stockpile settings added to copy service.");
    }

    public void CopySettings(BlockObject savedObject, BlockObject targetObject)
    {
      LogDebug("Copying stockpile settings...");
      var copy = new CopySettingsUtil.CopyFunctionWrapper(savedObject, targetObject);
      if (!copy.ValidateBlockObjects())
      {
        return;
      }

      bool isSupplier = false;
      bool isObtainer = false;
      bool isEmptier = false;

      // copy hauler settings
      copy.WithComponentType<GoodSupplier>()((saved, target) =>
      {
        if (saved.IsSupplying)
        {
          isSupplier = true;
          target.EnableSupplying();
        }
        else
        {
          target.DisableSupplying();
        }
      });
      copy.WithComponentType<GoodObtainer>()((saved, target) =>
      {
        if (saved.GoodObtainingEnabled)
        {
          isObtainer = true;
          target.EnableGoodObtaining();
        }
        else
        {
          target.DisableGoodObtaining();
        }
      });
      copy.WithComponentType<Emptiable>()((saved, target) =>
      {
        if (!saved.IsMarkedForEmptying)
        {
          target.UnmarkForEmptying();
          return;
        }

        isEmptier = true;
        var status = saved.GetComponentFast<StatusSubject>();
        if (status is not null && status.enabled)
        {
          target.MarkForEmptyingWithStatus();
        }
        else
        {
          target.MarkForEmptyingWithoutStatus();
        }
      });
      // "Accepts goods." => no other options selected
      copy.WithComponentType<Stockpile>()((saved, target) =>
      {
        if (!isSupplier && !isObtainer && !isEmptier)
        {
          target.GetComponentFast<GoodSupplier>()?.DisableSupplying();
          target.GetComponentFast<GoodObtainer>()?.DisableGoodObtaining();
          target.GetComponentFast<Emptiable>()?.UnmarkForEmptying();
        }
      });

      // copy selected stockpile item
      Stockpile targetStockpile = targetObject.GetComponentFast<Stockpile>();
      SingleGoodAllower targetSingleGoodAllower = targetObject.GetComponentFast<SingleGoodAllower>();
      Stockpile savedStockpile = savedObject.GetComponentFast<Stockpile>();
      SingleGoodAllower savedSingleGoodAllower = savedObject.GetComponentFast<SingleGoodAllower>();
      if (targetStockpile == null || targetSingleGoodAllower == null || savedStockpile == null || savedSingleGoodAllower == null)
      {
        return;
      }

      if (!savedStockpile.WhitelistedGoodType.Equals(targetStockpile.WhitelistedGoodType))
      {
        return;
      }

      if (savedSingleGoodAllower.AllowedGood == null)
      {
        targetSingleGoodAllower.Disallow();
        return;
      }

      targetSingleGoodAllower.Allow(savedSingleGoodAllower.AllowedGood);
    }
  }
}