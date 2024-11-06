using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopSpinAStarSolver : MonoBehaviour
{
    public int[] initialConfiguration;
    public int segmentSize = 4;

    private HashSet<string> visited = new HashSet<string>();
    public List<TextMeshPro> textMeshProList; // Lista de TextMeshPro con la configuración inicial
    public Button solveButton; // Referencia al botón en la UI

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
        PriorityQueue<Node> openSet = new PriorityQueue<Node>();
        HashSet<string> visited = new HashSet<string>();
        Dictionary<string, int> costSoFar = new Dictionary<string, int>();

        Node startNode = new Node(initialConfig, null, 0, Heuristic(initialConfig));
        openSet.Enqueue(startNode, startNode.F);
        costSoFar[ArrayToString(initialConfig)] = 0;

        while (openSet.Count > 0)
        {
            Node current = openSet.Dequeue();

            if (IsGoal(current.Config))
            {
                Debug.Log("¡Solución encontrada!");
                // Procesa el resultado aquí, como actualizar la UI o mostrar la solución
                yield break;
            }

            foreach (int[] neighborConfig in GetNeighbors(current.Config))
            {
                string neighborString = ArrayToString(neighborConfig);
                int newCost = current.G + 1;

                if (!costSoFar.ContainsKey(neighborString) || newCost < costSoFar[neighborString])
                {
                    costSoFar[neighborString] = newCost;
                    int priority = newCost + Heuristic(neighborConfig);
                    Node neighborNode = new Node(neighborConfig, current, newCost, priority);
                    openSet.Enqueue(neighborNode, priority);
                }
            }

            // Pausa el bucle para el próximo cuadro
            yield return null;
        }

        Debug.Log("No se encontró solución.");
    }

    private bool IsGoal(int[] configuration)
    {
        // Implementa la verificación de la meta aquí
        // Por ejemplo, verificar si el arreglo está ordenado
        for (int i = 0; i < configuration.Length - 1; i++)
        {
            if (configuration[i] > configuration[i + 1])
                return false;
        }
        return true;
    }

    private List<int[]> GetNeighbors(int[] configuration)
    {
        // Implementa la lógica para obtener vecinos aquí
        List<int[]> neighbors = new List<int[]>();
        int k = 3;

        for (int i = 0; i < configuration.Length - k; i++)
        {
            int[] newConfig = (int[])configuration.Clone();
            Array.Reverse(newConfig, i, k);
            neighbors.Add(newConfig);
        }
        return neighbors;
    }

    private int Heuristic(int[] configuration)
    {
        // Define la heurística aquí
        int misplacedCount = 0;
        for (int i = 0; i < configuration.Length - 1; i++)
        {
            if (configuration[i] + 1 != configuration[i + 1])
                misplacedCount++;
        }
        return misplacedCount;
    }

    private string ArrayToString(int[] configuration)
    {
        return string.Join(",", configuration);
    }

    private class Node
    {
        public int[] Config;
        public Node Parent;
        public int G;
        public int F;

        public Node(int[] config, Node parent, int g, int f)
        {
            Config = config;
            Parent = parent;
            G = g;
            F = f;
        }
    }
}