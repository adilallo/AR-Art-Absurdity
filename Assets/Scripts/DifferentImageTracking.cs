using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
 
[RequireComponent(typeof(ARTrackedImageManager))]
public class DifferentImageTracking : MonoBehaviour
{
    [Header("The length of this list must match the number of images in Reference Image Library")]
    public List<GameObject> ObjectsToPlace;
    public Text debug;
    public GameObject preloadedImage;
    private int refImageCount;
    private Dictionary<string, GameObject> allObjects;
    private ARTrackedImageManager arTrackedImageManager;
    private IReferenceImageLibrary refLibrary;
 
    void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
        debug.text = "Awake";
    }
 
    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
 
    }
 
    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }
 
    private void Start()
    {
        refLibrary = arTrackedImageManager.referenceLibrary;
        refImageCount = refLibrary.count;
        LoadObjectDictionary();
        debug.text = "start";
    }
 
    void LoadObjectDictionary()
    {
        allObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < refImageCount; i++)
        {
            allObjects.Add(refLibrary[i].name, ObjectsToPlace[i]);
            ObjectsToPlace[i].SetActive(false);
            debug.text = "Dictionary loaded";
        }
    }
 
    void ActivateTrackedObject(string _imageName)
    {
        allObjects[_imageName].SetActive(true);
        debug.text = "scanned" + _imageName;
        if (preloadedImage != allObjects[_imageName])
        { preloadedImage.SetActive(false);
            debug.text = "scannedandreplaced with" + allObjects[_imageName];
         
        };
        preloadedImage = allObjects[_imageName];
       
 
    }
 
 
    public void OnImageChanged(ARTrackedImagesChangedEventArgs _args)
    {
        foreach (var addedImage in _args.added)
        {
            ActivateTrackedObject(addedImage.referenceImage.name);
           
        }
 
        foreach (var updated in _args.updated)
        {
            allObjects[updated.referenceImage.name].transform.position = updated.transform.position;
            allObjects[updated.referenceImage.name].transform.rotation = updated.transform.rotation;
        }
    }
}