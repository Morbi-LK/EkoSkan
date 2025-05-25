using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    [SerializeField] private ARSession arSession;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private GameObject[] objectPrefabs;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private Vector2 touchPosition;

    private bool canPlaceObject = false;

    void Start()
    {
        placementIndicator.SetActive(false);
    }

    void Update()
    {
        // Uppdatera markören för objektplacering
        UpdatePlacementIndicator();

        // Hantera användarinput för objektplacering
        HandleTouch();
    }

    private void UpdatePlacementIndicator()
    {
        touchPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        if (raycastManager.Raycast(touchPosition, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            canPlaceObject = true;
            Pose hitPose = raycastHits[0].pose;

            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(hitPose.position, hitPose.rotation);
        }
        else
        {
            canPlaceObject = false;
            placementIndicator.SetActive(false);
        }
    }

    private void HandleTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (canPlaceObject)
            {
                SpawnObject(raycastHits[0].pose);
            }
            else
            {
                // Raycast för att kontrollera interaktion med befintliga objekt
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.CompareTag("ARObject"))
                    {
                        // Visa information om objektet
                        UIManager.Instance.ShowObjectInfo(hit.collider.gameObject);
                    }
                }
            }
        }
    }

    public void SpawnObject(Pose pose)
    {
        int randomIndex = Random.Range(0, objectPrefabs.Length);
        GameObject instantiatedObject = Instantiate(objectPrefabs[randomIndex], pose.position, pose.rotation);
        instantiatedObject.tag = "ARObject";
    }

    public void OnImageDetected(ARTrackedImagesChangedEventArgs args)
    {
        foreach (ARTrackedImage trackedImage in args.added)
        {
            string imageName = trackedImage.referenceImage.name;
            // Hitta prefab baserat på markörens namn
            GameObject prefab = GetPrefabForImage(imageName);

            if (prefab != null)
            {
                GameObject newObject = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation);
                spawnedObjects.Add(imageName, newObject);
            }
        }

        // Uppdatera position för redan spårade bilder
        foreach (ARTrackedImage trackedImage in args.updated)
        {
            string imageName = trackedImage.referenceImage.name;

            if (spawnedObjects.ContainsKey(imageName))
            {
                spawnedObjects[imageName].transform.position = trackedImage.transform.position;
                spawnedObjects[imageName].transform.rotation = trackedImage.transform.rotation;
            }
        }

        // Ta bort objekt kopplade till förlorade markörer
        foreach (ARTrackedImage trackedImage in args.removed)
        {
            string imageName = trackedImage.referenceImage.name;

            if (spawnedObjects.ContainsKey(imageName))
            {
                Destroy(spawnedObjects[imageName]);
                spawnedObjects.Remove(imageName);
            }
        }
    }

    private GameObject GetPrefabForImage(string imageName)
    {
        // Här kan du implementera logik för att matcha bilder med rätt prefabs
        foreach (GameObject prefab in objectPrefabs)
        {
            if (prefab.name == imageName)
            {
                return prefab;
            }
        }

        return objectPrefabs[0]; // Returnera standardprefab om ingen matchning hittas
    }

    public void TogglePlaneVisualization(bool enabled)
    {
        planeManager.enabled = enabled;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(enabled);
        }
    }
}