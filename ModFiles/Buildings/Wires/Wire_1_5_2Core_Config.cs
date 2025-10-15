using ModFiles.Components;
using TUNING;
using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// 2-adriges Kabel (L1 + N), 1,5 mm².
    /// BuildingConfig legt das Bauteil an und hängt RealisticWire dran.
    /// </summary>
    public class Wire_1_5_2Core_Config : IBuildingConfig
    {
        public const string ID = "PO_Wire_1_5_2Core";

        public override BuildingDef CreateBuildingDef()
        {
            // 1x1, Standard-Wire-Anim als Platzhalter (kannst du später ersetzen)
            const int width = 1;
            const int height = 1;
            const string anim = "wire_kanim";
            const int hitpoints = 10;
            const float construction_time = 3f;
            float[] mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER_TINY;
            string[] materials = MATERIALS.REFINED_METALS;
            const float melting_point = 1600f;

            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                width,
                height,
                anim,
                hitpoints,
                construction_time,
                mass,
                materials,
                melting_point,
                BuildLocationRule.Anywhere,
                BUILDINGS.DECOR.PENALTY.TIER0,
                NOISE_POLLUTION.NONE
            );

            // Darstellung/Layer wie ein normales Kabel
            def.SceneLayer = Grid.SceneLayer.Wires;
            def.ObjectLayer = ObjectLayer.Wire;
            def.ViewMode = OverlayModes.Power.ID;

            // Kleinkram
            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Metal";
            def.BaseTimeUntilRepair = -1f;
            def.CanMove = true;
            def.ConnectsToBuilding = true;
            def.RequiresPowerInput = false;

            // Baukosten/Materialien / Repair / etc. kannst du später feinjustieren
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            // Realistisches Kabel-Behaviour anhängen und konfigurieren
            var wire = go.AddOrGet<RealisticWire>();

            wire.numConductors = 2;
            wire.conductorLabels = new[] { "L1", "N" };
            wire.crossSection_mm2 = 1.5f;

            // typische Eckdaten für Cu 1,5 mm²
            wire.maxCurrent_A = 16f;     // Standard-Absicherung 16A
            wire.resistancePerMeter = 0.0121f; // Ω/m (ungefähr für Cu 1,5 mm²)

            // Sicherstellen, dass der Manager existiert, sobald das erste Kabel spawnt
            RealisticPowerSystem.Ensure();
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            // Platz für VFX/Overlay-Hooks etc.
        }
    }
}