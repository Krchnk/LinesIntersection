using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    [SerializeField] private GameObject intersectPointPrefab;
    [SerializeField] private GameObject segmentLine;
    private LineRenderer _lineRenderer;
    private Vector2 _startMousePos;
    private Vector2 _endMousePos;
    private Point startPoint;
    private Point endPoint;
    private List<Segment> segments = new List<Segment>();
    private List<Segment> dividedSegments = new List<Segment>();
    private List<GameObject> segmentLines = new List<GameObject>();
    private List<Vector2> intersectPoints = new List<Vector2>();
    private List<GameObject> intersectPointsDraw = new List<GameObject>();


    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            startPoint = new Point(_startMousePos.x, _startMousePos.y);

        }

        if (Input.GetMouseButtonUp(0))
        {
            _endMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endPoint = new Point(_endMousePos.x, _endMousePos.y);

            var segment = new Segment(startPoint, endPoint);
            segments.Add(segment);

            DrawLines();
        }      
    }

    public void PushButton()
    {
        DivideSegments(segments);

        foreach (var item in dividedSegments)
        {
            var newSegmentLine = Instantiate(segmentLine);

            Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);

            newSegmentLine.GetComponent<LineRenderer>().startColor = newColor;
            newSegmentLine.GetComponent<LineRenderer>().endColor = newColor;

            newSegmentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(item.Start.x, item.Start.y, 0f));
            newSegmentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(item.End.x, item.End.y, 0f));
         
        }

        foreach (var item in segmentLines)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }

    }

    public void DivideSegments (List<Segment> segments)
    {

        for (int i = 0; i < segments.Count ; i++)
        {
            for (int j = 1; j < segments.Count ; j++)
            {
                var Start1 = segments[i].Start;
                var End1 = segments[i].End;

                var Start2 = segments[j].Start;
                var End2 = segments[j].End;

                float A1 = End1.y - Start1.y;
                float B1 = Start1.x - End1.x;
                float C1 = A1 * Start1.x + B1 * Start1.y;

                float A2 = End2.y - Start2.y;
                float B2 = Start2.x - End2.x;
                float C2 = A2 * Start2.x + B2 * Start2.y;

                float denominator = A1 * B2 - A2 * B1;

                float intersectX = (B2 * C1 - B1 * C2) / denominator;
                float intersectY = (A1 * C2 - A2 * C1) / denominator;
                float rx0 = (intersectX - Start1.x) / (End1.x - Start1.x);
                float ry0 = (intersectY - Start1.y) / (End1.y - Start1.y);

                if ((rx0 >= 0 && rx0 <= 1) || (ry0 >= 0 && ry0 <= 1))
                {
                    var intersect = new Vector2(intersectX, intersectY);

                    var intersectPoint = new Point(intersectX, intersectY);

                    var newSegment1 = new Segment(segments[i].Start, intersectPoint);
                    var newSegment2 = new Segment(intersectPoint, segments[i].End);

                    var newSegment3 = new Segment(segments[j].Start, intersectPoint);
                    var newSegment4 = new Segment(intersectPoint, segments[j].End);

                    dividedSegments.Add(newSegment1);
                    dividedSegments.Add(newSegment2);
                    dividedSegments.Add(newSegment3);
                    dividedSegments.Add(newSegment4);
                 
                }
            }
        }
    }
   
    private void DrawLines()
    {
        foreach (var item in segmentLines)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }

        foreach (var item in segments)
        {
            var newSegmentLine = Instantiate(segmentLine);

            Color newColor = new Color(Random.value, Random.value, Random.value, 1.0f);

            newSegmentLine.GetComponent<LineRenderer>().startColor = newColor;
            newSegmentLine.GetComponent<LineRenderer>().endColor = newColor;

            newSegmentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(item.Start.x, item.Start.y, 0f));
            newSegmentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(item.End.x, item.End.y, 0f));

            segmentLines.Add(newSegmentLine);
        }
    }


}
