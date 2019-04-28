using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FloatLabel : MonoBehaviour
{
    public float Speed = 5;
    public float Lifetime = 2;

    private float counter = 0;
    private TextMeshProUGUI label;
    private Color labelStartColor;

    void Start()
    {
        label = GetComponent<TextMeshProUGUI>();
        labelStartColor = label.color;
    }

    // rise up until lifetime is reached
    void Update()
    {
        counter += Time.deltaTime;
        label.color = new Color(labelStartColor.r, labelStartColor.g, labelStartColor.b, labelStartColor.a * (1 - counter / Lifetime));
        transform.localPosition += Time.deltaTime * Speed * Vector3.up;
        if (counter > Lifetime) Destroy(gameObject);
    }
}
