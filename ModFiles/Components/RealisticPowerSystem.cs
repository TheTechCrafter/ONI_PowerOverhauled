using System.Collections.Generic;
using UnityEngine;

namespace PowerOverhauled
{
    /// <summary>
    /// Bildet Netze (Cluster) aus benachbarten RealisticWire-Zellen und
    /// verteilt einfachen Gleichstrom: I_flow = min(Supply, Demand).
    /// Quellen/Senken müssen im selben Cell wie ein Wire stehen (Test-Setup).
    /// </summary>
    public class RealisticPowerSystem : KMonoBehaviour, ISim1000ms
    {
        public static RealisticPowerSystem Instance { get; private set; }

        private readonly List<RealisticWire> wires = new List<RealisticWire>();
        private readonly Dictionary<int, RealisticWire> cellToWire = new Dictionary<int, RealisticWire>();

        private readonly List<RealisticSource> sources = new List<RealisticSource>();
        private readonly Dictionary<int, List<RealisticSource>> cellToSources = new Dictionary<int, List<RealisticSource>>();

        private readonly List<RealisticSink> sinks = new List<RealisticSink>();
        private readonly Dictionary<int, List<RealisticSink>> cellToSinks = new Dictionary<int, List<RealisticSink>>();

        private static readonly CellOffset[] CARDINALS = new[]
        {
            new CellOffset(-1,0), new CellOffset(1,0),
            new CellOffset(0,1),  new CellOffset(0,-1)
        };

        // --- lifecycle ---
        public static void Ensure()
        {
            if (Instance != null) return;
            var host = Game.Instance ? Game.Instance.gameObject : null;
            if (host == null) return;

            var go = new GameObject("PO_RealisticPowerSystem");
            go.transform.SetParent(host.transform);
            go.AddComponent<RealisticPowerSystem>();
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
            Debug.Log("[PO][RPS] started");
        }

        protected override void OnCleanUp()
        {
            if (Instance == this) Instance = null;
            base.OnCleanUp();
        }

        // --- registry: wires ---
        public void RegisterWire(RealisticWire w)
        {
            if (w == null) return;
            if (!wires.Contains(w)) wires.Add(w);
            cellToWire[w.Cell] = w;
        }

        public void UnregisterWire(RealisticWire w)
        {
            if (w == null) return;
            wires.Remove(w);
            if (cellToWire.TryGetValue(w.Cell, out var cur) && cur == w)
                cellToWire.Remove(w.Cell);
        }

        // --- registry: sources ---
        public void RegisterSource(RealisticSource s)
        {
            if (s == null) return;
            if (!sources.Contains(s)) sources.Add(s);
            if (!cellToSources.TryGetValue(s.Cell, out var list))
            {
                list = new List<RealisticSource>();
                cellToSources[s.Cell] = list;
            }
            if (!list.Contains(s)) list.Add(s);
        }

        public void UnregisterSource(RealisticSource s)
        {
            if (s == null) return;
            sources.Remove(s);
            if (cellToSources.TryGetValue(s.Cell, out var list))
            {
                list.Remove(s);
                if (list.Count == 0) cellToSources.Remove(s.Cell);
            }
        }

        // --- registry: sinks ---
        public void RegisterSink(RealisticSink s)
        {
            if (s == null) return;
            if (!sinks.Contains(s)) sinks.Add(s);
            if (!cellToSinks.TryGetValue(s.Cell, out var list))
            {
                list = new List<RealisticSink>();
                cellToSinks[s.Cell] = list;
            }
            if (!list.Contains(s)) list.Add(s);
        }

        public void UnregisterSink(RealisticSink s)
        {
            if (s == null) return;
            sinks.Remove(s);
            if (cellToSinks.TryGetValue(s.Cell, out var list))
            {
                list.Remove(s);
                if (list.Count == 0) cellToSinks.Remove(s.Cell);
            }
        }

        // --- sim tick ---
        public void Sim1000ms(float dt)
        {
            // 1) Cluster bilden
            var visited = new HashSet<int>();
            foreach (var w in wires)
            {
                if (w == null) continue;
                if (visited.Contains(w.Cell)) continue;

                var clusterCells = new List<int>();
                var clusterWires = new List<RealisticWire>();
                BFSBuildCluster(w.Cell, visited, clusterCells, clusterWires);

                // 2) Quelle/Senke je Cluster sammeln
                float clusterSupplyA = 0f;
                float clusterDemandA = 0f;

                foreach (var c in clusterCells)
                {
                    if (cellToSources.TryGetValue(c, out var srcs))
                        for (int i = 0; i < srcs.Count; i++)
                            clusterSupplyA += Mathf.Max(0f, srcs[i].MaxCurrent_A);

                    if (cellToSinks.TryGetValue(c, out var snks))
                        for (int i = 0; i < snks.Count; i++)
                            clusterDemandA += Mathf.Max(0f, snks[i].RequestCurrent_A);
                }

                float flowA = Mathf.Min(clusterSupplyA, clusterDemandA);

                // 3) Simple Verteilung: gleichmäßig über alle Leitungssegmente
                // (später: Stromdichte/Querschnitt/Länge berücksichtigen)
                float perWireA = (clusterWires.Count > 0) ? flowA / clusterWires.Count : 0f;
                foreach (var cw in clusterWires)
                    cw.SetCurrent(perWireA);
            }
        }

        private void BFSBuildCluster(int startCell, HashSet<int> visited, List<int> clusterCells, List<RealisticWire> clusterWires)
        {
            var q = new Queue<int>();
            q.Enqueue(startCell);
            visited.Add(startCell);

            while (q.Count > 0)
            {
                int cell = q.Dequeue();
                clusterCells.Add(cell);

                if (cellToWire.TryGetValue(cell, out var wire))
                    clusterWires.Add(wire);

                for (int i = 0; i < CARDINALS.Length; i++)
                {
                    int n = Grid.OffsetCell(cell, CARDINALS[i]);
                    if (n < 0 || n >= Grid.CellCount) continue;
                    if (!cellToWire.ContainsKey(n)) continue; // nur weiter, wenn dort auch Wire liegt
                    if (visited.Contains(n)) continue;

                    visited.Add(n);
                    q.Enqueue(n);
                }
            }
        }
    }
}
