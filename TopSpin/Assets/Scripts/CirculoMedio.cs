using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CirculoMedio : MonoBehaviour
{
    private float targetRotation;
    public float rotationSpeed = 200f;
    private CheckWin checkWin;
    [SerializeField] private List<TextMeshPro> t_list;
    [SerializeField] private bool rotando;
    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.eulerAngles.z;
        checkWin = FindAnyObjectByType<CheckWin>();
    }

    void OnMouseDown()
    {
        // Solo iniciar la rotación si no se está rotando actualmente
        if (!rotando)
        {
            targetRotation += 180f;  // Incrementar 180 grados al objetivo de rotación
            rotando = true;
        }

        if (t_list.Count == 4)
        {
            string text1 = t_list[0].text;
            string text2 = t_list[1].text;
            string text3 = t_list[2].text;
            string text4 = t_list[3].text;

            t_list[0].text = text4;
            t_list[1].text = text3;
            t_list[2].text = text2;
            t_list[3].text = text1;
        }

        checkWin.CheckIfGameCompleted();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotando)
        {
            float step = rotationSpeed * Time.deltaTime;
            float newRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetRotation, step);
            transform.eulerAngles = new Vector3(0, 0, newRotation);

            // Si la rotación ha alcanzado el objetivo, detener la rotación
            if (Mathf.Abs(newRotation - targetRotation) < 0.01f)
            {
                rotando = false;
            }
        }
    }

}
