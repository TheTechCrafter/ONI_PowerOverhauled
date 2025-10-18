using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// Einfache Last (Debug): fordert konstanten Strom an.
    /// Muss im selben Cell wie ein Wire stehen, um ins Cluster zu zählen.
    /// </summary>
    public class RealisticSink : KMonoBehaviour, ISim1000ms
    {
        [SerializeField] public float RequestCurrent_A = 6f;

        public int Cell { get; private set; }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            Cell = Grid.PosToCell(this);
            RealisticPowerSystem.Ensure();
            RealisticPowerSystem.Instance.RegisterSink(this);
        }

        protected override void OnCleanUp()
        {
            RealisticPowerSystem.Instance?.UnregisterSink(this);
            base.OnCleanUp();
        }

        public void Sim1000ms(float dt) { /* konstante Last */ }
    }
}
