using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckWin : MonoBehaviour
{
    public List<TextMeshPro> t_winlist;

    // M�todo para verificar si los n�meros est�n en el orden correcto
    public bool IsGameCompleted()
    {
        // Asegurarse de que la lista tenga exactamente 20 elementos
        if (t_winlist.Count != 20)
        {
            Debug.LogError("La lista debe contener exactamente 20 elementos.");
            return false;
        }

        // Revisar el orden de los n�meros en el c�rculo
        for (int i = 0; i < t_winlist.Count; i++)
        {
            int expectedNumber = (i + 1) % 20;
            if (expectedNumber == 0) expectedNumber = 20; // Para que despu�s del 20 venga el 1

            if (t_winlist[i].text != expectedNumber.ToString())
            {
                return false;
            }
        }

        // Si todos los n�meros est�n en el orden correcto
        return true;
    }

    // M�todo que puede ser llamado en cada movimiento o al finalizar
    public void CheckIfGameCompleted()
    {
        if (IsGameCompleted())
        {
            Debug.Log("�Juego completado! Los n�meros est�n en el orden correcto.");
            // Aqu� puedes agregar l�gica adicional para lo que sucede cuando el juego est� terminado
        }
        else
        {
            Debug.Log("El juego a�n no est� completado.");
        }
    }
}
