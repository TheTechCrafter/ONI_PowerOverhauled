using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// Simuliert elektrisches Verhalten eines realistischen Kabels
    /// </summary>
    public class RealisticWire : KMonoBehaviour, ISim1000ms
    {
        [SerializeField] public int numConductors = 2;
        [SerializeField] public string[] conductorLabels = new string[] { "L1", "N" };
        [SerializeField] public float crossSection_mm2 = 1.5f;
        [SerializeField] public float resistancePerMeter = 0.0121f;
        [SerializeField] public float maxCurrent_A = 16f;

        private float current_A;
        private float voltDrop_V;
        private bool overloaded;
        public int Cell { get; private set; }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Cell = Grid.PosToCell(this);

            // Manager sicherstellen
            RealisticPowerSystem.Ensure();

            if (RealisticPowerSystem.Instance != null)
            {
                RealisticPowerSystem.Instance.RegisterWire(this);
                Debug.Log("[PO][Wire] Spawned at cell " + Cell);
            }
            else
            {
                Debug.LogWarning("[PO][Wire] RealisticPowerSystem not available!");
            }
        }

        protected override void OnCleanUp()
        {
            if (RealisticPowerSystem.Instance != null)
                RealisticPowerSystem.Instance.UnregisterWire(this);

            base.OnCleanUp();
        }

        public void Sim1000ms(float dt)
        {
            const float length_m = 1f;
            voltDrop_V = current_A * resistancePerMeter * length_m;

            overloaded = current_A > maxCurrent_A;
            if (overloaded)
                Debug.LogWarning("[PO][Wire " + Cell + "] Überlast: " + current_A.ToString("F1") + "A > " + maxCurrent_A.ToString("F1") + "A");
        }

        public void SetCurrent(float amps)
        {
            current_A = Mathf.Max(amps, 0f);
        }

        public float GetCurrent() { return current_A; }
        public float GetVoltDrop() { return voltDrop_V; }
        public bool IsOverloaded() { return overloaded; }

        public string GetInfoString()
        {
            return crossSection_mm2 + "mm² " + numConductors + "-adrig [" + string.Join("/", conductorLabels) + "]: " +
                   current_A.ToString("F1") + "A / " + maxCurrent_A.ToString("F1") + "A, ΔU=" + voltDrop_V.ToString("F2") + "V";
        }
    }
}
