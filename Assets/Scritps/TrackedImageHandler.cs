using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class TrackedImageHandler : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;

    [System.Serializable]
    public class ARImagePrefabEntry
    {
        public string imageName;
        public GameObject prefab; // Prefab som innehåller både 3D-objekt + ARObject
    }

    [SerializeField] private List<ARImagePrefabEntry> trackedPrefabs;

    private Dictionary<string, GameObject> prefabLookup;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        prefabLookup = new Dictionary<string, GameObject>();
        foreach (var entry in trackedPrefabs)
        {
            prefabLookup[entry.imageName] = entry.prefab;
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
            HandleTrackedImage(trackedImage);

        foreach (var trackedImage in args.updated)
            HandleTrackedImage(trackedImage);

        foreach (var trackedImage in args.removed)
        {
            if (spawnedPrefabs.TryGetValue(trackedImage.referenceImage.name, out var spawned))
            {
                Destroy(spawned);
                spawnedPrefabs.Remove(trackedImage.referenceImage.name);
                UIManager.Instance.HideObjectInfo(); // Dölj panel när bilden försvinner
            }
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState != TrackingState.Tracking)
            return;

        string imageName = trackedImage.referenceImage.name;

        if (spawnedPrefabs.ContainsKey(imageName))
            return; // Redan spånad

        if (!prefabLookup.TryGetValue(imageName, out var prefab))
            return;

        // Spawna prefab vid bilden
        GameObject spawned = Instantiate(prefab, trackedImage.transform.position, trackedImage.transform.rotation, trackedImage.transform);
        spawnedPrefabs[imageName] = spawned;

        // Visa info från ARObject
        ARObject arObject = spawned.GetComponentInChildren<ARObject>();
        if (arObject != null)
        {
            UIManager.Instance.ShowObjectInfo(arObject.gameObject);
        }
    }
}
