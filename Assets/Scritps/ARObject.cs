using UnityEngine;

public class ARObject : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private float environmentalScore; // Milj�po�ng (0-100)

    // Roteringsparametrar f�r objektanimation
    [SerializeField] private bool autoRotate = true;
    [SerializeField] private float rotationSpeed = 30f;

    private bool isSelected = false;
    private Vector3 initialScale;

    public string Title => title;
    public string Description => description;
    public Sprite Icon => icon;
    public float EnvironmentalScore => environmentalScore;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (autoRotate)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void Select()
    {
        isSelected = true;
        // F�rstora objektet n�r det v�ljs
        transform.localScale = initialScale * 1.2f;
    }

    public void Deselect()
    {
        isSelected = false;
        // �terst�ll storleken
        transform.localScale = initialScale;
    }

    // F�r anv�ndarinteraktion
    private void OnMouseDown()
    {
        // Visa information om objektet vid klick
        UIManager.Instance.ShowObjectInfo(gameObject);
    }
}