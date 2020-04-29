using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFunc : MonoBehaviour
{

    public Transform RoomTarget;
    public Transform MainTarget;
    public Transform InfoTarget;
    public Transform ShowRoom;
    public Transform Demo1;

    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.1f;

    public float RotationsSpeed = 1.0f;
    public float MovementSpeed = 0.02f;//移動速度

    private GUIStyle largeFont;
    private string info;



    private int cameraState = 0;
    private Vector3 shift;
    private Vector3 roomMovement;

    // Use this for initialization
    void Start()
    {
        largeFont = new GUIStyle();
        largeFont.fontSize = 72;
        largeFont.normal.textColor = Color.white;
        Cursor.visible = true;
        roomMovement = new Vector3(0,3.0f,0);
    }
    void OnGUI()
    {
        //GUI.Window(0, new Rect(10, 10, 300, 150), infoWindow, "");
    }

    void infoWindow(int windowID)
    {
        //GUI.Label(new Rect(10, 10, 100, 20), "link: " + Convert.ToString(ws.IsConnected()));

    }



    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        switch (cameraState)
        {
            case 0:
                WsInput();
                MouseInput();
                break;
            case 1:
                Transition();
                break;
            case 2:
                Transition2();
                break;
            case 3:
                CheckMouseClick();
                break;
            case 4:
                Transition3();
                break;
            case 5:
                MainShow();
                break;
        }
    }

    void MainShow()
    {
            return;
    }

    void Transition3()
    {
        Vector3 movement = shift * (MovementSpeed * 5 * Time.deltaTime);
        if (transform.position.y >= MainTarget.position.y + 0.3)
            transform.Translate(movement, Space.World);
        print(transform.position);
        print(MainTarget.eulerAngles);
        print(transform.eulerAngles);
        if (MainTarget.eulerAngles.y >= transform.eulerAngles.y + 3)
        {
            //transform.Rotate(MainTarget.eulerAngles * Time.deltaTime, Space.World);
           transform.rotation = Quaternion.Lerp(transform.rotation, MainTarget.rotation, 3 * Time.deltaTime);
        }
        if (transform.position.y <= MainTarget.position.y + 0.3 && MainTarget.eulerAngles.y <= transform.eulerAngles.y + 3)
        {
            transform.localPosition = new Vector3(MainTarget.position.x, MainTarget.position.y, MainTarget.position.z);

            //transform.LookAt(ShowRoom);
            print("move to state 5");
            cameraState = 5;
            return;
        }
    }
    void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            RaycastHit hit;

            Ray ray;

            if (Input.GetMouseButtonDown(0))
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            else
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                print(hit.transform);
                if (hit.transform != null )
                {
                    cameraState = 4;

                    shift = MainTarget.position - transform.position;
                }
            }

        }
    }

    void Transition2()
    {
        Vector3 movement = roomMovement * (MovementSpeed * Time.deltaTime);

        ShowRoom.Translate(movement, Space.World);

        print(ShowRoom.position);
        if (ShowRoom.position.y + 0.1 >= 0)
        {
            //return;
            //ShowRoom.localPosition = new Vector3(ShowRoom.position.x, 0, ShowRoom.position.z);
            cameraState = 3;
            return;
        }
    }
    void Transition()
    {
        Vector3 movement = shift * (MovementSpeed * Time.deltaTime);

        transform.Translate(movement, Space.World);

        print(transform.position);
        if (RoomTarget.eulerAngles.x >= transform.eulerAngles.x + 3)
            transform.Rotate(RoomTarget .eulerAngles* Time.deltaTime, Space.World);
        if (transform.position.y <= RoomTarget.position.y + 0.05)
        {
            transform.localPosition = new Vector3(RoomTarget.position.x, RoomTarget.position.y, RoomTarget.position.z);
            transform.eulerAngles = RoomTarget.eulerAngles;
            print("move to state 2");
            print(transform.position);
            //transform.LookAt(ShowRoom);
            cameraState = 2;
            return;
        }
    }

    void MouseInput()
    {

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 1)
        {
            print("move to state 1");
            cameraState = 1;

            shift = RoomTarget.position - transform.position;
            return;
        }
    }

    void WsInput()
    {

    }
}
