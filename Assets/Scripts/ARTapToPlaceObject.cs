using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

// This script is inspired by:
// https://www.dropbox.com/s/n1i5v200npx1mf1/AutoPlaceItem.cs from video 7 of udemy course:
// https://www.udemy.com/create-ar-placement-app-and-full-template-for-photo-app/
// same video available on youtube:
// https://www.youtube.com/watch?v=x08UU-I8eZ8&list=PLw3UgsOGHn4loDyxHG75eJxSnxxVgB-Yb&index=5&t=0s
// arfoundation-examples PlaceOnPlace.cs with GetMouseButtonDown instead of GetMouseButton (was spawning 10 cubes with one click)
// video Getting Started With ARFoundation in Unity (ARKit, ARCore): https://www.youtube.com/watch?v=Ml2UakwRxjk&list=WL&index=4

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlaceObject : MonoBehaviour
{
    public GameObject objectToPlace1;
    public GameObject objectToPlace12;
    public GameObject objectToPlace13;
    public GameObject objectToPlace14;
    public GameObject objectToPlace2;
    public GameObject objectToPlace22;
    public GameObject objectToPlace23;
    public GameObject objectToPlace24;

    public GameObject placementIndicator;
    private ARRaycastManager m_RaycastManager;
    private Pose m_PlacementPose;
    private bool m_PlacementPoseIsValid = false;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    public LayerMask layerMask;

    public GameObject[] TestingGround;
    public Text text;

    public AudioClip sound1;
    AudioSource audioSource;

    public int playercount = 0;
  

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        

    }

    void Awake()
    {
#if UNITY_EDITOR
        for (int i = 0; i < TestingGround.Length; i++)
        {
            TestingGround[i].SetActive(true);
        }
#else
        for (int i = 0; i < TestingGround.Length; i++)
        {
            TestingGround[i].SetActive(false);
        }
#endif

        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(sound1);
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    void Update()
    {
        TryUpdatePlacementPose();
        UpdatePlacementIndicator();

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        // we actually ignore touchPosition, and always use screenCenter for the ray

        if (m_PlacementPoseIsValid)
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        audioSource.PlayOneShot(sound1);

        if (playercount % 2 == 0)
        {
            Instantiate(objectToPlace1, m_PlacementPose.position, m_PlacementPose.rotation);
            playercount += 1;
            text.text = "2P";
         

        }
        else
        {
            int objectcount = Random.Range(0, 3);
            if (objectcount == 0)
            {
                Instantiate(objectToPlace22, m_PlacementPose.position, m_PlacementPose.rotation);
                playercount += 1;
                
                text.text = "1P";
            }
            if (objectcount == 1)
            {
                Instantiate(objectToPlace23, m_PlacementPose.position, m_PlacementPose.rotation);
                playercount += 1;
                
                text.text = "1P";
            }
            if (objectcount == 2)
            {
                Instantiate(objectToPlace24, m_PlacementPose.position, m_PlacementPose.rotation);
                playercount += 1;
                
                text.text = "1P";
            }
            if (objectcount  == 3)
            {
                Instantiate(objectToPlace2, m_PlacementPose.position, m_PlacementPose.rotation);
                playercount += 1;
                
                text.text = "1P";
            }
        
        }
            
    }

    private void UpdatePlacementIndicator()
    {
        if (m_PlacementPoseIsValid)
        {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(m_PlacementPose.position, m_PlacementPose.rotation);
        }
        else
        {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose(Vector3 hitPoint)
    {
        m_PlacementPose.position = hitPoint;
        var cameraForward = Camera.main.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        m_PlacementPose.rotation = Quaternion.LookRotation(cameraBearing);
    }

    private void TryUpdatePlacementPose()
    {
#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth * 0.5f, Camera.main.pixelHeight * 0.5f, 0f));
        RaycastHit hit;

        m_PlacementPoseIsValid = Physics.Raycast(ray, out hit, 500f, layerMask);
        if (m_PlacementPoseIsValid)
        {
            UpdatePlacementPose(hit.point);
        }
#else
        var screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        m_RaycastManager.Raycast(screenCenter, s_Hits, TrackableType.Planes);
        m_PlacementPoseIsValid = s_Hits.Count > 0;
        if (m_PlacementPoseIsValid)
        {
            UpdatePlacementPose(s_Hits[0].pose.position);
        }
#endif
    }
}