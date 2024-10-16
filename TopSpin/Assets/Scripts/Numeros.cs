using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Numeros : MonoBehaviour
{
    public int numero;
    [SerializeField] private TextMeshPro t_numero;
    // Start is called before the first frame update
    void Start()
    {
        t_numero = GetComponentInChildren<TextMeshPro>();
        if (t_numero != null)
        {
            t_numero.text = "" + numero;
            CenterText(); // Asegurarse de que el texto esté centrado
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateText(string newText)
    {
        if (t_numero != null)
        {
            t_numero.text = newText;
            CenterText();  // Asegurarte de que el texto esté centrado
        }
    }

    void CenterText()
    {
        // Centrar el texto en el círculo ajustando la posición
        t_numero.rectTransform.anchoredPosition = Vector2.zero;
    }
}
