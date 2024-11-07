using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopSpinAStarSolver : MonoBehaviour
{
    public List<TextMeshPro> textMeshProList; // Lista de TextMeshPro con la configuración inicial
    public Button solveButton; // Referencia al botón en la UI
    public TextMeshProUGUI info;

    private void Start()
    {
        solveButton.onClick.AddListener(StartAStarCoroutine);
    }

    public void StartAStarCoroutine()
    {
        int[] initialConfig = new int[textMeshProList.Count];
        for (int i = 0; i < textMeshProList.Count; i++)
        {
            int.TryParse(textMeshProList[i].text, out initialConfig[i]);
        }

        StartCoroutine(FindSolutionCoroutine(initialConfig));
    }

    private IEnumerator FindSolutionCoroutine(int[] initialConfig)
    {
        // Inicializar estadísticas de rendimiento
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int nodesVisited = 0;
        int nodesProcessed = 0;

        // Configurar estructuras para el algoritmo A*
        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        HashSet<int> visited = new HashSet<int>();
        Dictionary<int, int> costSoFar = new Dictionary<int, int>();

        int initialHash = GetHash(initialConfig);
        Node startNode = new Node(initialConfig, null, 0, Heuristic(initialConfig), "Inicio");
        openSet.Enqueue(startNode, startNode.F);
        costSoFar[initialHash] = 0;
        visited.Add(initialHash);

        while (openSet.Count > 0)
        {
            nodesProcessed++;
            Node current = openSet.Dequeue();
            int currentHash = GetHash(current.Config);

            // Verificar si se alcanzó el objetivo
            if (IsGoal(current.Config))
            {
                stopwatch.Stop();
                UnityEngine.Debug.Log("¡Solución encontrada!");

                List<string> solutionPath = GetSolutionPath(current);
                foreach (string step in solutionPath)
                {
                    UnityEngine.Debug.Log(step);
                }

                // Imprimir estadísticas
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
                info.text = $"Tiempo de ejecución: {elapsedSeconds} segundos, Nodos visitados: {nodesVisited}, Nodos procesados: {nodesProcessed}, Nodos procesados por segundo: {nodesProcessed / elapsedSeconds}";

                yield break;
            }

            // Marcar el nodo actual como visitado
            nodesVisited++;

            // Generar y procesar los vecinos
            foreach (var (neighborConfig, move) in GetNeighbors(current.Config))
            {
                int neighborHash = GetHash(neighborConfig);
                int newCost = current.G + 1;

                if (!costSoFar.ContainsKey(neighborHash) || newCost < costSoFar[neighborHash])
                {
                    costSoFar[neighborHash] = newCost;
                    int priority = newCost + Heuristic(neighborConfig);
                    Node neighborNode = new Node(neighborConfig, current, newCost, priority, move);
                    openSet.Enqueue(neighborNode, priority);

                    // Agregar al conjunto de visitados
                    visited.Add(neighborHash);
                }
            }

            // Pausar para el próximo cuadro
            yield return null;
        }

        // Si no se encontró solución
        stopwatch.Stop();
        UnityEngine.Debug.Log("No se encontró solución.");
        double finalElapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        info.text = $"Tiempo de ejecución: {finalElapsedSeconds} segundos, Nodos visitados: {nodesVisited}, Nodos procesados: {nodesProcessed}, Nodos procesados por segundo: {nodesProcessed / finalElapsedSeconds}";
    }

    private int GetHash(int[] configuration)
    {
        int hash = 17;
        foreach (var val in configuration)
        {
            hash = hash * 31 + val;
        }
        return hash;
    }

    private bool IsGoal(int[] configuration)
    {
        for (int i = 0; i < configuration.Length - 1; i++)
        {
            if (configuration[i] > configuration[i + 1])
                return false;
        }
        return true;
    }

    private List<(int[], string)> GetNeighbors(int[] configuration)
    {
        List<(int[], string)> neighbors = new List<(int[], string)>();
        int k = 3;

        for (int i = 0; i < configuration.Length - k; i++)
        {
            int[] newConfig = (int[])configuration.Clone();
            Array.Reverse(newConfig, i, k);
            neighbors.Add((newConfig, $"Revertir del índice {i} al índice {i + k - 1}"));
        }
        return neighbors;
    }

    private int Heuristic(int[] configuration)
    {
        int heuristicValue = 0;
        for (int i = 0; i < configuration.Length - 1; i++)
        {
            if (configuration[i] + 1 != configuration[i + 1])
            {
                heuristicValue += Mathf.Abs(configuration[i] - (i + 1));
            }
        }
        return heuristicValue;
    }


    private List<string> GetSolutionPath(Node node)
    {
        List<string> path = new List<string>();
        while (node != null)
        {
            path.Add(node.Move);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }

    private class Node
    {
        public int[] Config;
        public Node Parent;
        public int G;
        public int F;
        public string Move;

        public Node(int[] config, Node parent, int g, int f, string move)
        {
            Config = config;
            Parent = parent;
            G = g;
            F = f;
            Move = move;
        }
    }
}
