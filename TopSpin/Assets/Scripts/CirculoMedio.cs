using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirculoMedio : MonoBehaviour
{
    private float targetRotation;
    public float rotationSpeed = 100f;
    [SerializeField] private bool rotando;
    // Start is called before the first frame update
    void Start()
    {
        targetRotation = transform.eulerAngles.z;
    }

    void OnMouseDown()
    {
        // Solo iniciar la rotaci�n si no se est� rotando actualmente
        if (!rotando)
        {
            targetRotation += 180f;  // Incrementar 180 grados al objetivo de rotaci�n
            rotando = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotando)
        {
            float step = rotationSpeed * Time.deltaTime;
            float newRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.z, targetRotation, step);
            transform.eulerAngles = new Vector3(0, 0, newRotation);

            // Si la rotaci�n ha alcanzado el objetivo, detener la rotaci�n
            if (Mathf.Abs(newRotation - targetRotation) < 0.01f)
            {
                rotando = false;
            }
        }
    }
}
