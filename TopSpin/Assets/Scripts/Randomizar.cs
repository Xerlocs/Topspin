using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Randomizar : MonoBehaviour
{
    public List<TextMeshPro> t_randomList;

    // Método para randomizar las posiciones de los textos
    public void RandomizarPosicion()
    {
        // Crear una lista para almacenar los textos actuales
        List<string> texts = new List<string>();

        // Guardar los textos actuales en la lista
        foreach (var textMesh in t_randomList)
        {
            texts.Add(textMesh.text);
        }

        // Mezclar los textos de la lista
        texts = ShuffleList(texts);

        // Asignar los textos mezclados a los TextMeshPro en el nuevo orden
        for (int i = 0; i < t_randomList.Count; i++)
        {
            t_randomList[i].text = texts[i];
        }
    }

    // Método auxiliar para mezclar la lista
    private List<string> ShuffleList(List<string> list)
    {
        // Crear una instancia de Random
        System.Random random = new System.Random();

        // Algoritmo de Fisher-Yates para mezclar la lista
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            string temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        return list;
    }
}
