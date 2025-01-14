using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor {  get; private set; }

    [SerializeField] Color[] colors;
    [SerializeField] private SpriteRenderer fillSpriteRenderer;

    public void SetDefaultColor(Color color)
    {
        DefaultColor = color;
        SetColor(DefaultColor);
    }

    public void SetColor(Color color)
    {
        fillSpriteRenderer.color = color;
    }

    public void SetRandomColor()
    {
        DefaultColor = colors[Random.Range(0, colors.Length)];
        SetColor(DefaultColor);
    }

}
