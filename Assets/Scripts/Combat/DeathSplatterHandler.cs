using UnityEngine;

public class DeathSplatterHandler : MonoBehaviour
{
    private void OnEnable()
    {
        Health.OnDeath += SpawnDeathSplatter;
        Health.OnDeath += SpawnDeathVfx;
    }

    private void OnDisable()
    {
        Health.OnDeath -= SpawnDeathSplatter;
        Health.OnDeath -= SpawnDeathVfx;
    }

    private void SpawnDeathSplatter(Health sender)
    {
        GameObject newSplatter = Instantiate(sender.SplatterPrefab, sender.transform.position, sender.transform.rotation);
        SpriteRenderer splatterSpriteRenderer = newSplatter.GetComponent<SpriteRenderer>();

        if (sender.TryGetComponent(out ColorChanger colorChanger))
        {
            splatterSpriteRenderer.color = colorChanger.DefaultColor;
        }

        newSplatter.transform.SetParent(this.transform);
    }

    private void SpawnDeathVfx(Health sender)
    {
        GameObject vfx = Instantiate(sender.DeathVfx, sender.transform.position, sender.transform.rotation);

        ParticleSystem.MainModule ps = vfx.GetComponent<ParticleSystem>().main;

        if (sender.TryGetComponent(out ColorChanger colorChanger))
        {
            ps.startColor = colorChanger.DefaultColor;
        }
    }
}
