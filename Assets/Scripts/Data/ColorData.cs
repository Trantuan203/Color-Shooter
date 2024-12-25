using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Color/ColorData")]
public class ColorData : ScriptableObject
{
    public List<Color> colorList = new List<Color>
    {
        Color.red,
        new Color(255 / 255.0f, 127 / 255.0f, 0 / 255.0f),
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(75 / 255.0f, 0 / 255.0f, 130 / 255.0f),
        new Color(148 / 255.0f, 0 / 255.0f, 211 / 255.0f)
    };

    //public Color grey = Color.grey;
    //public Color green = Color.green;
    //public Color blue = Color.blue;
    //public Color red = Color.red;
    //public Color yellow = Color.yellow;

    //public Color red = Color.red;
    //public Color orange = new Color(255 / 255.0f, 127 / 255.0f, 0 / 255.0f);
    //public Color yellow = Color.yellow;
    //public Color green = Color.green;
    //public Color blue = Color.blue;
    //public Color indigo = new Color(75 / 255.0f, 0 / 255.0f, 130 / 255.0f);
    //public Color violet = new Color(148 / 255.0f, 0 / 255.0f, 211 / 255.0f);

    public static CharacterColor GetRandomColor()
    {
        CharacterColor[] colors = (CharacterColor[])System.Enum.GetValues(typeof(CharacterColor));

        return colors[Random.Range(1, colors.Length)];
    }

    public static Color GetColor(CharacterColor characterColor)
    {
        Color color;

        switch (characterColor)
        {
            case CharacterColor.GRAY:
                color = Color.gray;
                break;
            case CharacterColor.RED:
                color = Color.red;
                break;
            case CharacterColor.ORANGE:
                color = new Color(255 / 255.0f, 127 / 255.0f, 0 / 255.0f);
                break;
            case CharacterColor.YELLOW:
                color = Color.yellow;
                break;
            case CharacterColor.GREEN:
                color = Color.green;
                break;
            case CharacterColor.BLUE:
                color = Color.blue;
                break;
            case CharacterColor.INDIGO:
                color = new Color(75 / 255.0f, 0 / 255.0f, 130 / 255.0f);
                break;
            case CharacterColor.VIOLET:
                color = new Color(148 / 255.0f, 0 / 255.0f, 211 / 255.0f);
                break;
            default:
                color = Color.gray;
                break;
        }
        return color;
    }

}
public enum CharacterColor
{
    GRAY,
    RED,
    ORANGE,
    YELLOW,
    GREEN,
    BLUE,
    INDIGO,
    VIOLET,
}

