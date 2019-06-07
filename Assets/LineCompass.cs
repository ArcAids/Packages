using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCompass : MonoBehaviour
{
    [SerializeField]
    Transform cam;

    [SerializeField]
    List<RectTransform> compassLine;

    float width;
    InfiniteScroller compassScroller;
    private void Start()
    {
        compassScroller = new InfiniteScroller(compassLine);
    }

    private void Update()
    {
        PointNorth();
    }

    void PointNorth()
    {
        Vector3 direction =Vector3.forward - cam.position;
        direction = direction.normalized;
        PointTowards(Vector3.forward);
    }

    void PointTowards(Vector3 direction)
    {
        Vector3 camWithNoHeight= Vector3.ProjectOnPlane(cam.forward,Vector3.up);
        direction.y = 0;
        float angle = Vector3.Angle(direction,camWithNoHeight);
        if(Vector3.Cross(direction,camWithNoHeight).y>0)
        {
            angle = 360 - angle;
        }
        angle = (angle / 360);
        compassScroller.Scroll(angle);
        //Vector3 tempPosition = compassLine.localPosition;
        //tempPosition.x = angle;
        //compassLine.localPosition = tempPosition;
    }
}
