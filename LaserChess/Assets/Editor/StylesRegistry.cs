using System.Collections.Generic;
using UnityEngine;

public static class StylesRegistry
{
    public static Dictionary<string, ButtonStyle> ButtonStyles;

    public static void Init()
    {
        var icons = Resources.LoadAll<Texture2D>("BoardCompositions/Icons");
        ButtonStyles = new Dictionary<string, ButtonStyle>();
        for (int i = 0; i < icons.Length; i++)
        {
            var name = icons[i].name.Remove(0, 3);
            var style = new ButtonStyle
            {
                ButtonText = name,
                NodeStyle = new GUIStyle()
            };

            style.NodeStyle.normal.background = icons[i];
            ButtonStyles.Add(name, style);
        }
    }
}

public struct ButtonStyle
{
    public string ButtonText;
    public GUIStyle NodeStyle;
}