using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine;

public class MovHaciaIzquierda : MonoBehaviour
{
    [SerializeField] public List<TextMeshPro> list;
    private CheckWin checkWin;

    // Start is called before the first frame update
    void Start()
    {
        checkWin = FindAnyObjectByType<CheckWin>();
    }

    public void MoverIzquierda()
    {
        if (list.Count > 1)
        {
            string primerNum = list[0].text;

            for (int i = 0; i < list.Count - 1; i++)
            {
                list[i].text = list[i + 1].text;
            }

            list[list.Count - 1].text = primerNum;
        }

        checkWin.CheckIfGameCompleted();
    }
}
