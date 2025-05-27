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
        // S�tt referensbiblioteket
        trackedImageManager.referenceLibrary = imageLibrary;
        trackedImageManager.requestedMaxNumberOfMovingImages = 3;

        // Registrera h�ndelseanropet f�r bildsp�rning
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        // Delegera till ARManager f�r att hantera bilddetektering
        GetComponent<ARManager>().OnImageDetected(args);
    }
}