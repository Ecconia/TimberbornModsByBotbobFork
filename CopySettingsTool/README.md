### **CopySettingsTool**

---

Adds a tool to copy settings. [More info here](https://mod.io/g/timberborn/m/copy-settings-tool "More info here").

To extend the settings that are copied, inject the `CopySettingsService` with TimberAPI and add a `ICopyFuncContainer` with `AddCopyFuncContainer(...)`.
The added container will be called when the settings are being applied, with the saved building and a single building to copy settings to.

You can see an example of how i copied the settings in `CopyGeneralSettingsContainer` and `CopyStockpileSettingsContainer`. You can use the util class `CopySettingsUtil`, or use any of your own  or other methods.

---

For an example of how i extended the settings you can check out the [CopySettingsToolSimpleFloodGateTriggersSettings](https://github.com/botbob5981/timberborn-mods/tree/main/CopySettingsToolSimpleFloodGateTriggersSettings "CopySettingsToolSimpleFloodGateTriggersSettings") project.

---

Verbose logs can be enabled in the mod's config folder.

