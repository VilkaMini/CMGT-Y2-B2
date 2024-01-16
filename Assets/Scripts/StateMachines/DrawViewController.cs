using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static DataTypes;

public class DrawViewController : ControllerBase
{
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private Camera viewCam;
    [SerializeField] private Transform viewCamPivot;

    private Coroutine drawing;
    private GameObject newLine;
    private LineRenderer lineRenderComp;

    private Touch currentTouch;
    private Vector3 drawOffset;
    private int _signId = 0;
    
    void Update()
    {
        if (stateController.GameState == ControlState.ViewDraw)
        {
            drawOffset = Vector3.Lerp(viewCam.transform.position, new Vector3(0,0,0), 0.1f);
            DetectInput();
        }
    }

    /// <summary>
    /// Method <c>DetectInput</c> is used to detect input and start logic for drawing or finishing drawing.
    /// </summary>
    void DetectInput()
    {
        if (inputManager.IsTouchApplied())
        {
            currentTouch = inputManager.GetTouch();
            if (currentTouch.phase == TouchPhase.Ended || EventSystem.current.IsPointerOverGameObject(0)) {
                FinishLine();
            }
            else if (currentTouch.phase == TouchPhase.Began) {
                StartLine();
            }
        }
    }

    /// <summary>
    /// Method <c>FinishLine</c> is used to stop the coroutine for drawing a line and do the finalization check.
    /// </summary>
    private void FinishLine()
    {
        if (drawing != null) StopCoroutine(drawing);
        if (newLine != null) FinalizeLineHit();
    }

    /// <summary>
    /// Method <c>StartLine</c> is used to start the coroutine for drawing a line.
    /// </summary>
    private void StartLine()
    {
        if (drawing != null) StopCoroutine(drawing);
        drawing = StartCoroutine(DrawLine());
    }

    /// <summary>
    /// Coroutine <c>DrawLine</c> is used to facilitate drawing of the line.
    /// </summary>
    IEnumerator DrawLine()
    {
        newLine = Instantiate(linePrefab, drawOffset, viewCamPivot.rotation);
        lineRenderComp = newLine.GetComponent<LineRenderer>();
        lineRenderComp.positionCount = 0;
        
        // Do not kill me this is needed cause the drawing is controlled by stopping the coroutine
        while (true)
        {
            Vector3 position = viewCam.ScreenToWorldPoint(currentTouch.position);
            lineRenderComp.positionCount++;
            position -= drawOffset * 0.1f;
            lineRenderComp.SetPosition(lineRenderComp.positionCount-1, position);
            yield return null;
        }
    }

    /// <summary>
    /// Method <c>FinalizeLineHit</c> is used to finalize the drawing by casting a ray forward from the position of renderer.
    /// </summary>
    void FinalizeLineHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(lineRenderComp.bounds.center, newLine.transform.forward, out hit, Mathf.Infinity))
        {
            Debug.DrawRay(lineRenderComp.bounds.center, newLine.transform.forward * 1000, Color.yellow, duration:500);
            if (hit.transform.gameObject.tag == "CarPart")
            {
                _networkManagerController.allSigns.Add(hit.collider.gameObject.GetComponentInParent<CarSignManager>()
                    .PlaceSignAtLocation(hit.point, stateController.ActiveCarId, _signId));
            }
        }
        
        Destroy(newLine);
    }

    public void ChangeSignPrefab(int signId)
    {
        _signId = signId;
    }


    // public void TestSign()
    // {
    //     RaycastHit hit;
    //     if (Physics.Raycast(viewCam.transform.position, viewCam.transform.forward, out hit, Mathf.Infinity))
    //     {
    //         Debug.DrawRay(viewCam.transform.position, viewCam.transform.forward * 1000, Color.yellow, duration:500);
    //         if (hit.transform.gameObject.tag == "CarPart")
    //         {
    //             _networkManagerController.allSigns.Add(hit.collider.gameObject.GetComponentInParent<CarSignManager>()
    //                 .PlaceSignAtLocation(hit.point, stateController.ActiveCarId));
    //         }
    //     }
    // }
}
