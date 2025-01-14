using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashTime = 0.15f;

    private SpriteRenderer[] spriteRenderers;
    private ColorChanger colorChanger;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        colorChanger = GetComponent<ColorChanger>();
    }

    public void StartFlash()
    {
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material = flashMaterial;

            if(colorChanger != null)
            {
                colorChanger.SetColor(Color.white);
            }
        }

        yield return new WaitForSeconds(flashTime);

        SetDefaultMaterial();
    }

    private void SetDefaultMaterial()
    {
        foreach (var spriteRenderer in spriteRenderers)
        {
            spriteRenderer.material = defaultMaterial;
            
            if (colorChanger != null) colorChanger.SetColor(colorChanger.DefaultColor);
        }
    }
}
