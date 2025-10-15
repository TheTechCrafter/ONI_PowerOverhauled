using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// Einfache Stromquelle (Debug): stellt MaxCurrent_A bereit.
    /// Muss im selben Cell wie ein Wire stehen, um ins Cluster zu zählen.
    /// </summary>
    public class RealisticSource : KMonoBehaviour, ISim1000ms
    {
        [SerializeField] public float MaxCurrent_A = 8f; // z.B. kleine Quelle

        public int Cell { get; private set; }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Cell = Grid.PosToCell(this);
            RealisticPowerSystem.Ensure();
            RealisticPowerSystem.Instance.RegisterSource(this);
        }

        protected override void OnCleanUp()
        {
            RealisticPowerSystem.Instance?.UnregisterSource(this);
            base.OnCleanUp();
        }

        public void Sim1000ms(float dt) { /* Quelle ist konstant */ }
    }
}
