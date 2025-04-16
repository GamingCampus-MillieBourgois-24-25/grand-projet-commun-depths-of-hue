using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FramesManager : MonoBehaviour
{
    [SerializeField] private Button upButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private TextMeshProUGUI ZoneText;

    [SerializeField] private Sprite lockedFrame;
    [SerializeField] private Sprite unlockedFrame;


    public static FramesManager Instance;

    [System.Serializable]
    public class Frame
    {
        public string id; // "Main_frame", "cave"...

        public Transform cameraPosition;
        public GameObject[] ActiveProps; // Props being used in a frame

        public RoomStateEnum InitialFrameState;
        public RoomStateEnum FrameState;

        public List<FrameConnection> connections = new List<FrameConnection>();

    }
    [System.Serializable]
    public class FrameConnection
    {
        public DirectionsEnum direction;
 
        public string connectedFrameId;
    }

    public Frame currentFrame;
    public Frame[] frames;
    [SerializeField] private string initalFrame = "main_frame"; //First frame always called main_frame
    [SerializeField] private float cameraSpeed = 2f;

    private Coroutine currentCameraCoroutine;

    private Camera mainCamera;


    void Start()
    {
        mainCamera = Camera.main;
        SwitchFrame(initalFrame);

    }

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

    /// <summary>
    /// Switch frame. It deactivates non used props and active the used ones.
    /// Parameter expecting a string room ID.
    /// </summary>
    /// <param name="newRoomID"></param>
    public void SwitchFrame(string newRoomID)
    {
        if (CheckIfLocked(newRoomID))
        {
            //Target frame reference
            Frame targetFrame = System.Array.Find(frames, s => s.id == newRoomID);

            ZoneText.text = newRoomID;

            //Stop current camera movement
            if (currentCameraCoroutine != null)
            {
                StopCoroutine(currentCameraCoroutine);
                mainCamera.transform.position = currentFrame.cameraPosition.position;
            }


            //Manage props deactivation/activation
            ManageFrameProps(targetFrame);


            // Move camera
            currentCameraCoroutine= StartCoroutine(MoveCamera(targetFrame.cameraPosition.position));
            currentFrame = targetFrame;
            UpdateDirectionButtons();
        }
        else
        {
            Debug.Log("can't acces locked room");
        }

    }


    /// <summary>
    /// Switch camera position to the new target.
    /// Parameter expecting a vector 3 position.
    /// </summary>
    /// <param name="positionCible"></param>
    /// <returns></returns>
    IEnumerator MoveCamera(Vector3 positionCible)
    {
        Debug.Log(positionCible + "target");
        while (Vector3.Distance(mainCamera.transform.position, positionCible) > 0.1f)
        {
            Vector3 targetPos = mainCamera.transform.position;

            Debug.Log(targetPos + "original");

            mainCamera.transform.position = Vector3.Lerp(
                targetPos,
                positionCible,
                cameraSpeed * Time.deltaTime
            );
            yield return null;
        }
        
    }

    /// <summary>
    /// Updates the switching frame buttons display depending on the current's frame connections
    /// </summary>
    void UpdateDirectionButtons()
    {
        Debug.Log("eeoeooeoe");
        // Reset all buttons
        upButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);
        downButton.gameObject.SetActive(false);
        leftButton.gameObject.SetActive(false);

        // Set active buttons based on connections
        foreach (var connection in currentFrame.connections)
        {
            switch (connection.direction)
            {
                case DirectionsEnum.up:
                    upButton.gameObject.SetActive(true);
                    Frame targetFrameUp = System.Array.Find(frames, s => s.id == connection.connectedFrameId);
                    if (targetFrameUp.FrameState == RoomStateEnum.Locked)
                    {
                        upButton.image.sprite = lockedFrame;
                    }
                    upButton.onClick.RemoveAllListeners();
                    upButton.onClick.AddListener(() => SwitchFrame(connection.connectedFrameId));
                    break;

                case DirectionsEnum.down:
                    downButton.gameObject.SetActive(true);
                    Frame targetFrameDown = System.Array.Find(frames, s => s.id == connection.connectedFrameId);
                    if (targetFrameDown.FrameState == RoomStateEnum.Locked)
                    {
                        downButton.image.sprite = lockedFrame;
                    }
                    downButton.onClick.RemoveAllListeners();
                    downButton.onClick.AddListener(() => SwitchFrame(connection.connectedFrameId));
                    break;

                case DirectionsEnum.left:
                    leftButton.gameObject.SetActive(true);
                    Frame targetFrameLeft = System.Array.Find(frames, s => s.id == connection.connectedFrameId);
                    if (targetFrameLeft.FrameState == RoomStateEnum.Locked)
                    {
                        Debug.Log(targetFrameLeft.FrameState);
                        leftButton.image.sprite = lockedFrame;
                    }
                    else
                    {
                        leftButton.image.sprite = unlockedFrame;
                    }
                    leftButton.onClick.RemoveAllListeners();
                    leftButton.onClick.AddListener(() => SwitchFrame(connection.connectedFrameId));

                    break;

                case DirectionsEnum.right:
                    rightButton.gameObject.SetActive(true);
                    Frame targetFrameRight = System.Array.Find(frames, s => s.id == connection.connectedFrameId);
                    if (targetFrameRight.FrameState == RoomStateEnum.Locked)
                    {
                        rightButton.image.sprite = lockedFrame;
                    }
                    rightButton.onClick.RemoveAllListeners();
                    rightButton.onClick.AddListener(() => SwitchFrame(connection.connectedFrameId));
                    break;
                 
            }
        }

    }

    /// <summary>
    /// Checks if a frame is unlocked.
    /// </summary>
    /// <param name="frameId"></param>
    bool CheckIfLocked(string frameId)
    {
        Frame targetFrameUp = System.Array.Find(frames, s => s.id == frameId);

        if (targetFrameUp.FrameState == RoomStateEnum.Locked)
        {
            return false;
        }
        else
        {
            return true;
            
        }
    }

    void ManageFrameProps(Frame targetFrame)
    {
        
        if (targetFrame == null) return;

        // Deactivate all props
        foreach (var frame in frames)
        {
            foreach (var obj in frame.ActiveProps)
            {
                obj.SetActive(false);
            }
        }

        // Activate target frame and current frame props
        foreach (var obj in targetFrame.ActiveProps)
        {
            obj.SetActive(true);
        }
        foreach (var obj in currentFrame.ActiveProps)
        {
            obj.SetActive(true);
        }
    }

    public void UnlockFrame(string frame_id)
    {
        //Target frame reference
        Frame targetFrame = System.Array.Find(frames, s => s.id == frame_id);

        if (targetFrame != null)
        {
            targetFrame.FrameState = RoomStateEnum.Unlocked;
            UpdateDirectionButtons();
        }
        else
        {
            print("no frame");
        }
    }
    
    public void LockFrame(string frame_id)
    {
        //Target frame reference
        Frame targetFrame = System.Array.Find(frames, s => s.id == frame_id);

        if (targetFrame != null)
        {
            targetFrame.FrameState = RoomStateEnum.Locked;
            UpdateDirectionButtons() ;
        }
    }
}
