using UnityEngine;

public class ARObject : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private float environmentalScore; // Miljöpoäng (0-100)

    // Roteringsparametrar för objektanimation
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
        // Förstora objektet när det väljs
        transform.localScale = initialScale * 1.2f;
    }

    public void Deselect()
    {
        isSelected = false;
        // Återställ storleken
        transform.localScale = initialScale;
    }

    // För användarinteraktion
    private void OnMouseDown()
    {
        // Visa information om objektet vid klick
        UIManager.Instance.ShowObjectInfo(gameObject);
    }
}