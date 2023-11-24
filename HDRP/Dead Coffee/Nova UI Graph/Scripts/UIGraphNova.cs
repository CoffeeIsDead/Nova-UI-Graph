using CodeMonkey.Utils;
using System.Collections.Generic;
using UnityEngine;
using Nova;

public class UIGraphNova : MonoBehaviour
{

    [Header("UIBlock Refs")]
    [SerializeField] UIBlock2D loadingIcon;
    [SerializeField] List<UIBlock> dots;
    [SerializeField] List<UIBlock> lines;
    [SerializeField] List<UIBlock> sections;

    [Header("Fill Mesh Generator Ref")]
    [SerializeField] UIGraphMeshNova meshNova;

    [Header("Sprites")]
    [SerializeField] Sprite dotSprite;
    [SerializeField] Sprite tileSprite;

    [Header("Sizes")]
    [SerializeField] float dotSize;
    [SerializeField] float lineWidth;
    [SerializeField] float sectionWidth;

    [Header("Colors")]
    [SerializeField] Color dotColor;
    [SerializeField] Color lineColor;
    [SerializeField] Color sectionColor;

    [Header("Points")]
    [SerializeField] public List<int> graphPoints;
    [SerializeField] List<string> graphPointsName;

    private float graphWidth;
    private float graphHeight;
    private float graphY;
    private float graphX;

    private void Awake()
    {

        graphHeight = this.gameObject.GetComponent<UIBlock>().Size.Y.Value;
        graphWidth = this.gameObject.GetComponent<UIBlock>().Size.X.Value;

    }

    private void UpdateGraph()
    {

        UIBlock lastDot = null;

        for (int i = 0; i < graphPoints.Count; i++)
        {

            float newY = graphY * graphPoints[i];
            UIBlock dotHolder = dots[i];
            UIBlock sectionHolder = sections[i];
            UIBlock2D section = sectionHolder.GetChild(0).GetComponent<UIBlock2D>();

            dotHolder.Position.Y = newY;

            sectionHolder.Position.Y = newY;
            section.Size.Y = newY + 10;

            if (lastDot != null)
            {

                UIBlock lineHolder = lines[i - 1];
                UIBlock2D line = lineHolder.GetChild(0).GetComponent<UIBlock2D>();
                Vector2 direction = (dotHolder.Position.XY.Value - lastDot.Position.XY.Value).normalized;
                float distance = Vector2.Distance(lastDot.Position.XY.Value, dotHolder.Position.XY.Value);
                Length2 position = lastDot.Position.XY.Value + direction * distance * 0.5f;
                lineHolder.Position = new Length3(position.X.Value, position.Y.Value, 0);
                lineHolder.transform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(direction));
                line.Size.X = distance;

            }

            lastDot = dotHolder;

        }

        meshNova.GetVertices();

    }

    public void BuildGraph()
    {

        ResetGraph();

        graphY = graphHeight / 100f;
        graphX = graphWidth / graphPoints.Count;

        UIBlock lastDot = null;

        for (int i = 0; i < graphPoints.Count; i++)
        {

            float dotX;
            float dotY = graphY * graphPoints[i];

            if (graphPoints[i] == 0)
            {

                dotX = 0.5f * graphX;

            }
            else
            {

                dotX = ((i + 1) * graphX) - (graphX * .5f);

            }

            UIBlock newDot = CreateGraphDot(new Length3(dotX, dotY, 0), i);

            if (lastDot != null)
            {

                MakeGraphLine(lastDot.Position.XY.Value, newDot.Position.XY.Value, i);

            }
            else
            {

                MakeSectionLine(newDot.Position.XY.Value, i);

            }

            lastDot = newDot;

        }

        meshNova.GetVertices();
        loadingIcon.Visible = false;

    }


    private void ResetGraph()
    {

        foreach (UIBlock dotHolder in dots)
        {

            UIBlock2D dot = dotHolder.transform.GetChild(0).GetComponent<UIBlock2D>();
            dot.Visible = false;

        }

        foreach (UIBlock lineHolder in lines)
        {

            UIBlock2D line = lineHolder.transform.GetChild(0).GetComponent<UIBlock2D>();
            line.Visible = false;

        }

        foreach(UIBlock sectionHolder in sections)
        {

            UIBlock2D section = sectionHolder.GetChild(0).GetComponent<UIBlock2D>();
            TextBlock sectionValue = sectionHolder.GetChild(1).GetComponent<TextBlock>();
            TextBlock sectionName = section.GetChild(0).GetComponent<TextBlock>();
            section.Visible = false;
            sectionValue.Visible = false;
            sectionName.Visible = false;

        }

        loadingIcon.Visible = true;

    }

    private UIBlock CreateGraphDot(Length3 position, int dotNum)
    {

        UIBlock dotHolder = dots[dotNum];
        UIBlock2D dot = dotHolder.transform.GetChild(0).GetComponent<UIBlock2D>();
        dotHolder.Position = position;
        dot.Color = dotColor;
        dot.SetImage(dotSprite);
        dot.Size = new Length3(dotSize, dotSize, 0);
        dot.Visible = true;

        return dotHolder;

    }

    private void MakeGraphLine(Vector2 dotPositionA, Vector2 dotPositionB, int dotNum)
    {

        UIBlock lineHolder = lines[dotNum - 1];
        UIBlock2D line = lineHolder.GetChild(0).GetComponent<UIBlock2D>();
        Vector2 direction = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        Length2 position = dotPositionA + direction * distance * 0.5f;
        lineHolder.Position = new Length3(position.X.Value, position.Y.Value, 0);
        lineHolder.transform.localEulerAngles = new Vector3(1, 1, UtilsClass.GetAngleFromVectorFloat(direction));
        line.Color = lineColor;
        line.SetImage(tileSprite);
        line.Size = new Length3(distance, lineWidth, 0);
        line.Visible = true;

        if (dotNum < graphPoints.Count)
        {

            MakeSectionLine(dotPositionB, dotNum);

        }
    }

    private void MakeSectionLine(Vector2 postition, int dotNum)
    {

        UIBlock sectionHolder = sections[dotNum];
        UIBlock2D section = sectionHolder.GetChild(0).GetComponent<UIBlock2D>();
        TextBlock sectionValue = sectionHolder.GetChild(1).GetComponent<TextBlock>();
        TextBlock sectionName = section.GetChild(0).GetComponent<TextBlock>();
        sectionHolder.Position = new Length3(postition.x, postition.y, 0);
        section.Color = sectionColor;
        section.SetImage(tileSprite);
        section.Size = new Length3(sectionWidth, postition.y, 0);
        sectionValue.Text = graphPoints[dotNum].ToString();
        sectionName.Text = graphPointsName[dotNum].ToString();
        section.Visible = true;
        sectionValue.Visible = true;
        sectionName.Visible = true;

    }
}
