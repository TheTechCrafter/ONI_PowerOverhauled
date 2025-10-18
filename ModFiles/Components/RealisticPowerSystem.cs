using System.Collections.Generic;
using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// Verwalter für das gesamte realistische Stromsystem.
    /// </summary>
    public class RealisticPowerSystem : KMonoBehaviour, ISim1000ms
    {
        public static RealisticPowerSystem Instance { get; private set; }
        private readonly List<RealisticWire> wires = new List<RealisticWire>();

        public static void Ensure()
        {
            if (Instance != null)
                return;

            Game game = Game.Instance;
            GameObject host = (game != null) ? game.gameObject : null;
            if (host == null)
            {
                Debug.LogWarning("[PO][RPS] Game.Instance not ready – cannot spawn manager yet.");
                return;
            }

            GameObject go = new GameObject("PO_RealisticPowerSystem");
            go.transform.SetParent(host.transform);
            go.AddComponent<RealisticPowerSystem>();
            Debug.Log("[PO][RPS] Manager created.");
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            Debug.Log("[PO][RPS] Active and running.");
        }

        protected override void OnCleanUp()
        {
            if (Instance == this)
                Instance = null;

            base.OnCleanUp();
        }

        public void RegisterWire(RealisticWire wire)
        {
            if (wire == null) return;
            if (!wires.Contains(wire))
                wires.Add(wire);
        }

        public void UnregisterWire(RealisticWire wire)
        {
            if (wire == null) return;
            wires.Remove(wire);
        }

        public void Sim1000ms(float dt)
        {
            // Derzeit nur Durchreichen; hier kommt später die Netzlogik rein.
            int count = wires.Count;
            for (int i = 0; i < count; i++)
            {
                RealisticWire w = wires[i];
                if (w != null)
                    w.Sim1000ms(dt);
            }
        }
    }
}
