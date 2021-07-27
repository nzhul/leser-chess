using UnityEngine;

public class Node
{
    private Rect _rect;

    private GUIStyle _style;

    public string Name
    {
        get
        {
            return _style.normal.background.name.Remove(0, 3);
        }

        private set { }
    }

    public Node(Vector2 position, float width, float height, GUIStyle defaultStyle)
    {
        _rect = new Rect(position.x, position.y, width, height);
        _style = defaultStyle;
    }

    public void Drag(Vector2 delta)
    {
        _rect.position += delta;
    }

    public void Draw()
    {
        GUI.Box(_rect, "", _style);
    }

    public void SetStyle(GUIStyle style)
    {
        _style = style;
    }
}