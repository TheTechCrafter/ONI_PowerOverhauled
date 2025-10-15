using HarmonyLib;
using KMod;
using UnityEngine;

namespace PowerOverhauled
{
    public class PowerOverhauledMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("[PO] PowerOverhauled loaded (Creative-only)");

            // Manager sicherstellen
            RealisticPowerSystem.Ensure();

            // Baumenü-Einträge (nur sichtbar, wenn das Building DebugOnly=true hat und Sandbox aktiv ist)
            ModUtil.AddBuildingToPlanScreen("Power", Wire_1_5_2Core_Config.ID);
            ModUtil.AddBuildingToPlanScreen("Power", PO_DebugGenerator_Config.ID);
            ModUtil.AddBuildingToPlanScreen("Power", PO_DebugConsumer_Config.ID);

            // KEINE Forschungseinträge (Creative/Sandbox-only)
            // -> Keine ModUtil.AddBuildingToTech(...) Aufrufe hier.
        }
    }
}
