using TUNING;
using UnityEngine;

namespace PowerOverhauled
{
    public class Wire2CoreConfig : IBuildingConfig
    {
        public const string ID = "Wire2Core";

        public override BuildingDef CreateBuildingDef()
        {
            var def = BuildingTemplates.CreateBuildingDef(
                ID,
                1, 1,                                  // Breite, Höhe
                "electrical_wire_kanim",               // Vanilla-KAnim als Platzhalter
                10, 10f,                               // hitpoints, construction_time
                new float[] { 50f },                   // Bau-Masse
                new string[] { "Copper" },             // Baumaterial
                800f,                                  // Schmelzpunkt
                BuildLocationRule.Tile,
                DECOR.NONE,
                NOISE_POLLUTION.NONE
            );

            def.Floodable = false;
            def.Overheatable = false;
            def.AudioCategory = "Metal";
            def.ViewMode = OverlayModes.Power.ID;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            // Optional: Logik für Stromfluss kann hier eingefügt werden
        }
    }
}
