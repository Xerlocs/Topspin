using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

public class MovHaciaDerecha : MonoBehaviour
{
    [SerializeField] public List<TextMeshPro> list;
    private CheckWin checkWin;

    // Start is called before the first frame update
    void Start()
    {
        checkWin = FindAnyObjectByType<CheckWin>();
    }

    public void MoverDerecha()
    {
        if (list.Count > 1)
        {
            string ultimoNum = list[list.Count - 1].text;

            for (int i = list.Count - 1; i > 0; i--)
            {
                list[i].text = list[i - 1].text;
            }

            list[0].text = ultimoNum;

        }

        checkWin.CheckIfGameCompleted();
    }
}
