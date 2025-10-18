using HarmonyLib;
using KMod;
using STRINGS;
using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// Hauptklasse der Mod "Power Overhauled"
    /// </summary>
    public class PowerOverhauledMod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("[PowerOverhauled] Mod geladen und Harmony-Patches aktiviert.");
        }
    }

    /// <summary>
    /// Fügt neue Gebäude (z. B. Kabel) in das Baumenü und die Sprachstrings ein
    /// </summary>
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch("LoadGeneratedBuildings")]
    public static class PowerOverhauled_LoadBuildings
    {
        public static void Prefix()
        {
            // === String-Definitionen ===
            string key = Wire_1_5_2Core_Config.ID.ToUpperInvariant();

            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{key}.NAME", " 1.5mm² (L1/N)");
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{key}.DESC", "Ein zweiadriges Stromkabel mit 1.5 mm² Querschnitt – geeignet für Haushaltsstromkreise.");
            Strings.Add($"STRINGS.BUILDINGS.PREFABS.{key}.EFFECT", "Verbindet Stromquellen und Verbraucher über ein realistisches Leitungssystem.");

            // === Registrierung im Baumenü ===
            ModUtil.AddBuildingToPlanScreen("Power", Wire_1_5_2Core_Config.ID);

            Debug.Log("[PowerOverhauled] Wire_1_5_2Core ins Power-Menü eingefügt.");
        }
    }

    /// <summary>
    /// Optional: fügt das Kabel einer Forschung hinzu (entferne das, wenn du es nicht willst)
    /// </summary>
    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch("Initialize")]
    public static class PowerOverhauled_AddToTechTree
    {
        public static void Postfix()
        {
            try
            {
                // In vorhandene Tech-Gruppe "Power Regulation" einfügen
                var tech = Db.Get().Techs.Get("PowerRegulation");
                if (tech != null && !tech.unlockedItemIDs.Contains(Wire_1_5_2Core_Config.ID))
                {
                    tech.unlockedItemIDs.Add(Wire_1_5_2Core_Config.ID);
                    Debug.Log("[PowerOverhauled] Wire_1_5_2Core zu Tech 'PowerRegulation' hinzugefügt.");
                }
            }
            catch
            {
                Debug.LogWarning("[PowerOverhauled] Konnte Tech-Eintrag nicht hinzufügen – evtl. andere ONI-Version.");
            }
        }
    }
}
