using HarmonyLib;

namespace PowerOverhauled
{
    // Patches zum Entfernen der einzelnen Kabel und Brücken aus dem Baumenü
    public static class RemoveEachWire
    {
        // Normales Kabel
        [HarmonyPatch(typeof(WireConfig), nameof(WireConfig.CreateBuildingDef))]
        public static class PatchWire
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        // Starkstromkabel
        [HarmonyPatch(typeof(WireHighWattageConfig), nameof(WireHighWattageConfig.CreateBuildingDef))]
        public static class PatchWireHighWattage
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        // Leiterkabel
        [HarmonyPatch(typeof(WireRefinedConfig), nameof(WireRefinedConfig.CreateBuildingDef))]
        public static class PatchWireRefined
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        // Starkes Leiterkabel
        [HarmonyPatch(typeof(WireRefinedHighWattageConfig), nameof(WireRefinedHighWattageConfig.CreateBuildingDef))]
        public static class PatchWireRefinedHighWattage
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        // Brücken
        [HarmonyPatch(typeof(WireBridgeConfig), nameof(WireBridgeConfig.CreateBuildingDef))]
        public static class PatchWireBridge
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        [HarmonyPatch(typeof(WireBridgeHighWattageConfig), nameof(WireBridgeHighWattageConfig.CreateBuildingDef))]
        public static class PatchWireBridgeHighWattage
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        [HarmonyPatch(typeof(WireRefinedBridgeConfig), nameof(WireRefinedBridgeConfig.CreateBuildingDef))]
        public static class PatchWireRefinedBridge
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }

        [HarmonyPatch(typeof(WireRefinedBridgeHighWattageConfig), nameof(WireRefinedBridgeHighWattageConfig.CreateBuildingDef))]
        public static class PatchWireRefinedBridgeHighWattage
        {
            public static void Postfix(BuildingDef __result)
            {
                __result.ShowInBuildMenu = false;
                __result.Deprecated = true;
            }
        }
    }
}
