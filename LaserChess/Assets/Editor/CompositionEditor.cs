using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class CompositionEditor : EditorWindow
{
    private const int CELL_SIZE = 30;
    private const int GRID_SIZE = 8;

    private static CompositionEditor _instance;
    private BoardComposition _selectedComposition;

    private Vector2 _offset;
    private Vector2 _drag;
    private Node[,] _nodes;
    private ButtonStyle _currentStyle;
    private bool _isErasing;

    [MenuItem("Window/Composition Editor")]
    public static void ShowEditorWindow()
    {
        _instance = GetWindow<CompositionEditor>(false, "Composition Editor");
    }

    [OnOpenAsset(1)]
    public static bool OpenDialogue(int instanceId, int line)
    {
        var composition = EditorUtility.InstanceIDToObject(instanceId) as BoardComposition;

        if (composition != null)
        {
            ShowEditorWindow();
            _instance.SetupNodes(composition);
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;
        StylesRegistry.Init();
        _currentStyle = StylesRegistry.ButtonStyles["empty"];
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnGUI()
    {
        if (_selectedComposition == null)
        {
            EditorGUILayout.LabelField("Please select board composition object!");
            return;
        }

        DrawGrid();
        DrawNodes();
        DrawMenuBar();
        ProcessNodes(Event.current);
        ProcessGrid(Event.current);
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void OnSelectionChanged()
    {
        var newComposition = Selection.activeObject as BoardComposition;
        if (newComposition != null)
        {
            _instance.SetupNodes(newComposition);
            Repaint();
        }
    }

    private void ProcessNodes(Event e)
    {
        int row = (int)((e.mousePosition.x - _offset.x) / CELL_SIZE);
        int col = (int)((e.mousePosition.y - _offset.y) / CELL_SIZE);
        if ((e.mousePosition.x - _offset.x) < 0
            || (e.mousePosition.x - _offset.x) > (CELL_SIZE * GRID_SIZE)
            || (e.mousePosition.y - _offset.y) < 0
            || (e.mousePosition.y - _offset.y) > (CELL_SIZE * GRID_SIZE))
        {
            return;
        }

        if (e.type == EventType.MouseDown)
        {
            if (_nodes[row, col].Name == "empty")
            {
                _isErasing = false;
            }
            else
            {
                _isErasing = true;
            }

            PaintNodes(row, col);
        }

        if (e.type == EventType.MouseDrag)
        {
            PaintNodes(row, col);
            e.Use();
        }
    }

    private void PaintNodes(int row, int col)
    {
        if (row >= GRID_SIZE || col >= GRID_SIZE)
        {
            return;
        }

        if (_isErasing)
        {
            _nodes[row, col].SetStyle(StylesRegistry.ButtonStyles["empty"].NodeStyle);
            RemoveCompositionEntity(row, col);
            GUI.changed = true;
        }
        else
        {
            _nodes[row, col].SetStyle(_currentStyle.NodeStyle);
            CreateOrUpdateCompositionEntity(row, col);
            GUI.changed = true;
        }
    }

    private void RemoveCompositionEntity(int row, int col)
    {
        if (_selectedComposition.Entities == null)
        {
            _selectedComposition.Entities = new List<CompositionEntity>();
        }

        var existingEntity = _selectedComposition.Entities.FirstOrDefault(entity => entity.X == row && entity.Y == col);

        if (existingEntity != null)
        {
            _selectedComposition.Entities.Remove(existingEntity);
        }
    }

    private void CreateOrUpdateCompositionEntity(int row, int col)
    {
        if (_selectedComposition.Entities == null)
        {
            _selectedComposition.Entities = new List<CompositionEntity>();
        }

        var type = (PieceType)Enum.Parse(typeof(PieceType), _currentStyle.ButtonText, true);
        var existingEntity = _selectedComposition.Entities.FirstOrDefault(entity => entity.X == row && entity.Y == col);

        if (existingEntity != null)
        {
            existingEntity.Type = type;
        }
        else
        {
            _selectedComposition.Entities.Add(new CompositionEntity
            {
                Type = type,
                X = row,
                Y = col
            });
        }
    }

    private void ProcessGrid(Event e)
    {
        _drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnMouseDrag(e.delta);
                }
                break;
        }
    }

    private void OnMouseDrag(Vector2 delta)
    {
        _drag = delta;
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                _nodes[i, j].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void DrawMenuBar()
    {
        var menuBar = new Rect(0, 0, position.width, 20);
        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        foreach (var key in StylesRegistry.ButtonStyles.Keys)
        {
            var style = StylesRegistry.ButtonStyles[key];
            var enabled = _currentStyle.ButtonText == style.ButtonText;

            if (GUILayout.Toggle(enabled, new GUIContent(style.ButtonText), EditorStyles.toolbarButton, GUILayout.Width(80)))
            {
                _currentStyle = style;
            }
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawNodes()
    {
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                _nodes[i, j].Draw();
            }
        }
    }

    private void DrawGrid()
    {
        int columns = Mathf.CeilToInt(position.width / CELL_SIZE);
        int rows = Mathf.CeilToInt(position.height / CELL_SIZE);

        Handles.BeginGUI();
        Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
        _offset += _drag;
        var newOffset = new Vector3(_offset.x % CELL_SIZE, _offset.y % CELL_SIZE, 0);

        for (int i = 0; i < columns; i++)
        {
            Handles.DrawLine(new Vector3(CELL_SIZE * i, -CELL_SIZE, 0) + newOffset, new Vector3(CELL_SIZE * i, position.height, 0) + newOffset);
        }

        for (int i = 0; i < rows; i++)
        {
            Handles.DrawLine(new Vector3(-CELL_SIZE, CELL_SIZE * i, 0) + newOffset, new Vector3(position.width, CELL_SIZE * i, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void SetupNodes(BoardComposition composition)
    {
        if (_selectedComposition == composition)
        {
            return;
        }

        _selectedComposition = composition;

        _offset = new Vector2();
        _drag = new Vector2();

        _nodes = new Node[GRID_SIZE, GRID_SIZE];
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                var nodePos = new Vector2(i * CELL_SIZE, j * CELL_SIZE);
                var iconStyle = ResolveIconStyle(new Vector2(i, j), composition);
                _nodes[i, j] = new Node(nodePos, CELL_SIZE, CELL_SIZE, iconStyle);
            }
        }

        OnMouseDrag(new Vector2(CELL_SIZE, CELL_SIZE * 2 - 10));
    }

    private GUIStyle ResolveIconStyle(Vector2 nodePos, BoardComposition composition)
    {
        var style = StylesRegistry.ButtonStyles["empty"].NodeStyle;
        var entity = composition.Entities?.FirstOrDefault(entity => entity.X == nodePos.x && entity.Y == nodePos.y);

        if (entity != null)
        {
            style = StylesRegistry.ButtonStyles[entity.Type.ToString().ToLower()].NodeStyle;
        }

        return style;
    }
}