using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckWin : MonoBehaviour
{
    public List<TextMeshPro> t_winlist;

    // Método para verificar si los números están en el orden correcto
    public bool IsGameCompleted()
    {
        // Asegurarse de que la lista tenga exactamente 20 elementos
        if (t_winlist.Count != 20)
        {
            Debug.LogError("La lista debe contener exactamente 20 elementos.");
            return false;
        }

        // Revisar el orden de los números en el círculo
        for (int i = 0; i < t_winlist.Count; i++)
        {
            int expectedNumber = (i + 1) % 20;
            if (expectedNumber == 0) expectedNumber = 20; // Para que después del 20 venga el 1

            if (t_winlist[i].text != expectedNumber.ToString())
            {
                return false;
            }
        }

        // Si todos los números están en el orden correcto
        return true;
    }

    // Método que puede ser llamado en cada movimiento o al finalizar
    public void CheckIfGameCompleted()
    {
        if (IsGameCompleted())
        {
            Debug.Log("¡Juego completado! Los números están en el orden correcto.");
            // Aquí puedes agregar lógica adicional para lo que sucede cuando el juego está terminado
        }
        else
        {
            Debug.Log("El juego aún no está completado.");
        }
    }
}
