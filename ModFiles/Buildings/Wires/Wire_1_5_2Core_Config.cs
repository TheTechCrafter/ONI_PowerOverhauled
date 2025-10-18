using STRINGS;
using TUNING;
using UnityEngine;

namespace PowerOverhauled
{
    // Realistisches 2-adriges Kabel (L1 + N), 1.5 mm²
    public class Wire_1_5_2Core_Config : BaseWireConfig
    {
        public const string ID = "PO_Wire_1_5_2Core";

        public override BuildingDef CreateBuildingDef()
        {
            // Vanilla-Parameter wie im originalen WireConfig
            float[] mass = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
            EffectorValues decor = TUNING.BUILDINGS.DECOR.PENALTY.TIER0;
            EffectorValues noise = NOISE_POLLUTION.NONE;

            // Nutzt eine vorhandene ONI-Kabel-Animation
            BuildingDef def = this.CreateBuildingDef(
                ID,
                "utilities_electric_conduct_kanim",
                3f,           // Bauzeit wie Vanilla
                mass,
                0.05f,        // Schmelz-/Hitze-Parameter wie Vanilla Wire
                decor,
                noise
            );

            // Suchbegriffe – jeweils EIN String pro Aufruf (C# 7.3-kompatibel)
            def.AddSearchTerms((string)SEARCH_TERMS.POWER);
            def.AddSearchTerms((string)SEARCH_TERMS.WIRE);
            def.AddSearchTerms("Kupfer");
            def.AddSearchTerms("1.5mm²");

            // Im normalen Spiel baubar
            def.DebugOnly = false;

            return def;
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            // Aktiviert das vollständige Vanilla-Wire-Verhalten (Netz, Overlay, Verbindungen, Render)
            this.DoPostConfigureComplete(Wire.WattageRating.Max1000, go);

            // Deine Realismus-Daten (ändert NICHT das Vanilla-Netz-Verhalten)
            RealisticWire rWire = go.AddOrGet<RealisticWire>();
            rWire.numConductors = 2;
            rWire.conductorLabels = new string[] { "L1", "N" };
            rWire.crossSection_mm2 = 1.5f;
            rWire.maxCurrent_A = 16f;
            rWire.resistancePerMeter = 0.0121f; // Kupfer 1.5 mm²

            RealisticPowerSystem.Ensure();
        }
    }
}
