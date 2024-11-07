using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopSpinIDASolver : MonoBehaviour
{
    public List<TextMeshPro> textMeshProList; // Lista de TextMeshPro con la configuración inicial
    public Button solveButton; // Referencia al botón en la UI
    public int windowSize = 4; // Tamaño de la ventana de reversión
    public TextMeshProUGUI info;

    private void Start()
    {
        solveButton.onClick.AddListener(StartIDAS);
    }

    public void StartIDAS()
    {
        int[] initialConfig = new int[textMeshProList.Count];
        for (int i = 0; i < textMeshProList.Count; i++)
        {
            int.TryParse(textMeshProList[i].text, out initialConfig[i]);
        }

        List<string> solutionPath = IDAStar(initialConfig);
        if (solutionPath != null)
        {
            UnityEngine.Debug.Log("¡Solución encontrada!");
            foreach (string step in solutionPath)
            {
                UnityEngine.Debug.Log(step);
            }
        }
        else
        {
            UnityEngine.Debug.Log("No se encontró solución.");
        }
    }

    private List<string> IDAStar(int[] initialConfig)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int nodesVisited = 0;
        int threshold = Heuristic(initialConfig);

        while (true)
        {
            HashSet<int> visited = new HashSet<int>();
            var (result, path, nodesProcessed) = Search(initialConfig, 0, threshold, visited, null, ref nodesVisited);

            if (result == 0) // Solución encontrada
            {
                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                info.text = $"Tiempo de ejecución: {elapsedSeconds} segundos, Nodos visitados: {nodesVisited}, Nodos procesados: {nodesProcessed}, Nodos procesados por segundo: {nodesProcessed / elapsedSeconds}";

                return path;
            }
            if (result == int.MaxValue) // No se encontró solución dentro del límite
            {
                stopwatch.Stop();
                UnityEngine.Debug.Log("No se encontró solución.");
                double finalElapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                info.text = $"Tiempo de ejecución: {finalElapsedSeconds} segundos, Nodos visitados: {nodesVisited}, Nodos procesados: {nodesProcessed}, Nodos procesados por segundo: {nodesProcessed / finalElapsedSeconds}";

                return null;
            }

            // Incrementar el umbral de profundidad
            threshold = result;
        }
    }

    private (int, List<string>, int) Search(int[] config, int g, int threshold, HashSet<int> visited, string move, ref int nodesVisited)
    {
        int f = g + Heuristic(config);
        if (f > threshold)
        {
            return (f, null, 0); // Supera el límite actual
        }
        if (IsGoal(config))
        {
            return (0, new List<string> { move }, 1); // Solución encontrada
        }

        visited.Add(GetHash(config));
        nodesVisited++;

        int min = int.MaxValue;
        List<string> bestPath = null;
        int nodesProcessed = 1;

        foreach (var (neighborConfig, neighborMove) in GetNeighbors(config))
        {
            int neighborHash = GetHash(neighborConfig);
            if (visited.Contains(neighborHash)) continue;

            var (t, path, subNodesProcessed) = Search(neighborConfig, g + 1, threshold, visited, neighborMove, ref nodesVisited);
            nodesProcessed += subNodesProcessed;

            if (t == 0) // Solución encontrada en un camino
            {
                if (path == null) path = new List<string>();
                path.Insert(0, move); // Agregar el movimiento actual
                return (0, path, nodesProcessed);
            }
            if (t < min) min = t;

            if (path != null && (bestPath == null || path.Count < bestPath.Count))
            {
                bestPath = new List<string>(path);
                bestPath.Insert(0, move);
            }
        }

        visited.Remove(GetHash(config));
        return (min, bestPath, nodesProcessed);
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

        int n = configuration.Length;
        for (int i = 0; i < n; i++)
        {
            int[] newConfig = (int[])configuration.Clone();

            // Realizar la reversión circular
            for (int j = 0; j < windowSize / 2; j++)
            {
                int index1 = (i + j) % n;
                int index2 = (i + windowSize - 1 - j) % n;
                (newConfig[index1], newConfig[index2]) = (newConfig[index2], newConfig[index1]);
            }

            neighbors.Add((newConfig, $"Revertir del índice {i} al índice {(i + windowSize - 1) % n}"));
        }
        return neighbors;
    }

    private int Heuristic(int[] configuration)
    {
        int distanceSum = 0;
        for (int i = 0; i < configuration.Length; i++)
        {
            int expectedValue = (i + 1) % configuration.Length;
            if (expectedValue == 0) expectedValue = configuration.Length;

            distanceSum += Mathf.Abs(configuration[i] - expectedValue);
        }
        return distanceSum;
    }
}
