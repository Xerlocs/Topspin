using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Linq;

public class TopSpinSolver : MonoBehaviour
{
    public List<TextMeshPro> textMeshProList; // Lista de los TextMeshPro que contienen los números
    public int windowSize = 4; // Tamaño de la ventana que se puede revertir
    public CirculoMedio rotar;
    public MovHaciaDerecha derecha;
    public MovHaciaIzquierda izquierda;
    public TextMeshProUGUI info;

    // Método para ejecutar BFS y buscar solución después de la randomización
    public void FindSolutionAfterRandomization()
    {
        List<string> solution = SolvePuzzle();
        if (solution != null)
        {
            UnityEngine.Debug.Log("¡Solución encontrada!");
            foreach (string move in solution)
            {
                UnityEngine.Debug.Log(move);
            }
        }
        else
        {
            UnityEngine.Debug.Log("No se encontró solución.");
        }
    }

    // Método para resolver el puzzle usando BFS
    private List<string> SolvePuzzle()
    {
        // Estadísticas de rendimiento
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        int nodesVisited = 0;

        List<int> startState = GetCurrentState();
        List<int> goalState = GenerateGoalState();

        Queue<PuzzleState> queue = new Queue<PuzzleState>();
        HashSet<PuzzleState> visited = new HashSet<PuzzleState>();

        PuzzleState initialState = new PuzzleState(startState);
        queue.Enqueue(initialState);
        visited.Add(initialState);

        while (queue.Count > 0)
        {
            PuzzleState currentState = queue.Dequeue();
            nodesVisited++;

            if (currentState.Numbers.SequenceEqual(goalState))
            {
                stopwatch.Stop();
                double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

                // Imprimir estadísticas de rendimiento
                info.text = $"Tiempo de ejecución: {elapsedSeconds} segundos, Nodos visitados: {nodesVisited}, Nodos procesados por segundo: {nodesVisited / elapsedSeconds}";

                return GetSolutionPath(currentState);
            }

            foreach (var neighbor in GetNeighbors(currentState))
            {
                if (!visited.Contains(neighbor))
                {
                    queue.Enqueue(neighbor);
                    visited.Add(neighbor);
                }
            }
        }

        stopwatch.Stop();
        double finalElapsedSeconds = stopwatch.Elapsed.TotalSeconds;

        // Imprimir estadísticas de rendimiento si no se encontró solución
        UnityEngine.Debug.Log("No se encontró solución.");
        info.text = $"Tiempo de ejecución: {finalElapsedSeconds} segundos, Nodos visitados: {nodesVisited}, Nodos procesados por segundo: {nodesVisited / finalElapsedSeconds}";

        return null; // No se encontró solución
    }

    private List<int> GetCurrentState()
    {
        List<int> state = new List<int>();
        foreach (var textMesh in textMeshProList)
        {
            state.Add(int.Parse(textMesh.text));
        }
        return state;
    }

    private List<int> GenerateGoalState()
    {
        List<int> goalState = new List<int>();
        for (int i = 1; i <= textMeshProList.Count; i++)
        {
            goalState.Add(i);
        }
        return goalState;
    }

    private List<PuzzleState> GetNeighbors(PuzzleState state)
    {
        List<PuzzleState> neighbors = new List<PuzzleState>();

        List<int> rightRotation = new List<int>(state.Numbers);
        RotateRight(rightRotation);
        neighbors.Add(new PuzzleState(rightRotation, state, "Rotar a la derecha"));

        List<int> leftRotation = new List<int>(state.Numbers);
        RotateLeft(leftRotation);
        neighbors.Add(new PuzzleState(leftRotation, state, "Rotar a la izquierda"));

        List<int> reversedWindow = new List<int>(state.Numbers);
        ReverseWindow(reversedWindow);
        neighbors.Add(new PuzzleState(reversedWindow, state, "Revertir ventana"));

        return neighbors;
    }

    private void RotateRight(List<int> list)
    {
        derecha.MoverDerecha();
        int last = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        list.Insert(0, last);
    }

    private void RotateLeft(List<int> list)
    {
        izquierda.MoverIzquierda();
        int first = list[0];
        list.RemoveAt(0);
        list.Add(first);
    }

    private void ReverseWindow(List<int> list)
    {
        rotar.Rotar();
        int start = 0;
        int end = Mathf.Min(windowSize - 1, list.Count - 1);
        while (start < end)
        {
            int temp = list[start];
            list[start] = list[end];
            list[end] = temp;
            start++;
            end--;
        }
    }

    private List<string> GetSolutionPath(PuzzleState state)
    {
        List<string> path = new List<string>();
        while (state.Parent != null)
        {
            path.Add(state.Move);
            state = state.Parent;
        }
        path.Reverse();
        return path;
    }

    // Clase para representar un estado del puzzle
    private class PuzzleState
    {
        public List<int> Numbers;
        public PuzzleState Parent;
        public string Move;

        public PuzzleState(List<int> numbers, PuzzleState parent = null, string move = "")
        {
            Numbers = new List<int>(numbers);
            Parent = parent;
            Move = move;
        }

        public override bool Equals(object obj)
        {
            if (obj is PuzzleState other)
                return Numbers.SequenceEqual(other.Numbers);
            return false;
        }

        public override int GetHashCode()
        {
            return string.Join(",", Numbers).GetHashCode();
        }
    }
}


