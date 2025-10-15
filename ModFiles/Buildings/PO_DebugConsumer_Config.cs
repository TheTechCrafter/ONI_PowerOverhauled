using TUNING;
using UnityEngine;

namespace PowerOverhauled
{
    /// <summary> 1x1 Debug-Verbraucher, der RealisticSink nutzt. </summary>
    public class PO_DebugConsumer_Config : IBuildingConfig
    {
        public const string ID = "PO_DebugConsumer";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID, 1, 1, "lightbug_statue_kanim", // Platzhalter-Anim
                10, 5f,
                BUILDINGS.CONSTRUCTION_MASS_KG.TIER1,
                MATERIALS.REFINED_METALS,
                800f,
                BuildLocationRule.OnFloor,
                BUILDINGS.DECOR.PENALTY.TIER0,
                NOISE_POLLUTION.NONE
            );
            def.Overheatable = false;
            def.Floodable = false;
            def.ViewMode = OverlayModes.Power.ID;
            return def;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<RealisticSink>().RequestCurrent_A = 6f; // 6A Last
        }

        public override void DoPostConfigureComplete(GameObject go) { }
    }
}
