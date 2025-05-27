using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ImageLibraryManager : MonoBehaviour
{
    [SerializeField] private ARTrackedImageManager trackedImageManager;
    [SerializeField] private XRReferenceImageLibrary imageLibrary;

    void Awake()
    {
        // Sätt referensbiblioteket
        trackedImageManager.referenceLibrary = imageLibrary;
        trackedImageManager.requestedMaxNumberOfMovingImages = 3;

        // Registrera händelseanropet för bildspårning
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Delegera till ARManager för att hantera bilddetektering
        GetComponent<ARManager>().OnImageDetected(args);
    }
}