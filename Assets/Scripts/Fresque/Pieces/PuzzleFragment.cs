using UnityEngine;

public class PuzzleFragment:MonoBehaviour
{
    public int row;
    public int col;
    public string id;
    public Sprite icon;

    private void OnClick()
    {
        testInventory.Instance.AddFragment(this);
   
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clic gauche
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Debug.Log("yolo");
            Debug.Log(this.gameObject.name);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
               

                if (hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log("yolo2");
                    OnClick();
                }
            }
        }
    }
}
