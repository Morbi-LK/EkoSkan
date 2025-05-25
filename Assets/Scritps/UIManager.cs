using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI infoTitle;
    [SerializeField] private TextMeshProUGUI infoDescription;
    [SerializeField] private Image infoImage;
    [SerializeField] private Button closeInfoButton;
    [SerializeField] private Toggle planeVisualizationToggle;

    [SerializeField] private ARManager arManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        closeInfoButton.onClick.AddListener(HideObjectInfo);
        planeVisualizationToggle.onValueChanged.AddListener(arManager.TogglePlaneVisualization);

        mainMenu.SetActive(true);
        infoPanel.SetActive(false);
    }

    public void ShowObjectInfo(GameObject arObject)
    {
        // Hämta information från objektet
        ARObject objectInfo = arObject.GetComponent<ARObject>();

        if (objectInfo != null)
        {
            infoTitle.text = objectInfo.Title;
            infoDescription.text = objectInfo.Description;

            if (objectInfo.Icon != null)
            {
                infoImage.sprite = objectInfo.Icon;
                infoImage.gameObject.SetActive(true);
            }
            else
            {
                infoImage.gameObject.SetActive(false);
            }

            infoPanel.SetActive(true);
        }
    }

    public void HideObjectInfo()
    {
        infoPanel.SetActive(false);
    }

    public void ToggleMainMenu()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}