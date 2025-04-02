using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramesManager : MonoBehaviour
{
    [System.Serializable]
    public class Frame
    {
        public string id; // "Main_frame", "cave"...
        public Transform cameraPosition;
        public GameObject[] ActiveProps; // Props being used in a frame
    }

    [SerializeField] private Frame[] frames;
    [SerializeField] private string initalFrame = "main_frame"; //First frame always called main_frame
    [SerializeField] private float cameraSpeed = 2f;

    private Camera mainCamera;
    private string currentFrame;

    void Start()
    {
        mainCamera = Camera.main;
        SwitchFrame(initalFrame);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");
            SwitchFrame(frames[1].id);
        }
    }

    /// <summary>
    /// Switch frame. It deactivates non used props and active the used ones.
    /// Parameter expecting a string room ID.
    /// </summary>
    /// <param name="newRoomID"></param>
    public void SwitchFrame(string newRoomID)
    {
        Frame targetFrame = System.Array.Find(frames, s => s.id == newRoomID);
        if (targetFrame == null) return;

        // Deactivate currently used props
        foreach (var salle in frames)
        {
            foreach (var obj in salle.ActiveProps)
            {
                obj.SetActive(false);
            }
        }

        // Activate future props
        foreach (var obj in targetFrame.ActiveProps)
        {
            obj.SetActive(true);
        }

        // Déplace la caméra
        StartCoroutine(MoveCamera(targetFrame.cameraPosition.position));
        currentFrame = newRoomID;
    }


    /// <summary>
    /// Switch camera position to the new target.
    /// Parameter expecting a vector 3 position.
    /// </summary>
    /// <param name="positionCible"></param>
    /// <returns></returns>
    IEnumerator MoveCamera(Vector3 positionCible)
    {
        while (Vector3.Distance(mainCamera.transform.position, positionCible) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                positionCible,
                cameraSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}
