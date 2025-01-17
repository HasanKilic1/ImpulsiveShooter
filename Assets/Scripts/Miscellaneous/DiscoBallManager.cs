using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DiscoBallManager : MonoBehaviour
{
    public static Action OnDiscoBallHit;

    [SerializeField] private float discoBallPartyTime = 2f;
    [SerializeField] private float discoGlobalLightIntensity = 2f;
    [SerializeField] private Light2D globalLight;

    private float defaultGlobalLightIntensity;
    private ColorSpotlight[] allSpotlights;
    private Coroutine discoCoroutine;

    private void OnEnable()
    {
        OnDiscoBallHit += DimTheLights;
    }

    private void OnDisable()
    {        
        OnDiscoBallHit -= DimTheLights;
    }

    private void Start()
    {
        defaultGlobalLightIntensity = globalLight.intensity;
        allSpotlights = FindObjectsByType<ColorSpotlight>(FindObjectsSortMode.None);
    }

    public void DiscoBallParty()
    {
        if(discoCoroutine != null)
        {
            return;
        }
        OnDiscoBallHit?.Invoke();
    }

    private void DimTheLights()
    {
        foreach (var spotlight in allSpotlights)
        {
            StartCoroutine(spotlight.SpotlightDiscoParty(discoBallPartyTime));
        }

        discoCoroutine = StartCoroutine(GlobalLightResetRoutine());
    }

    private IEnumerator GlobalLightResetRoutine()
    {
        globalLight.intensity = discoGlobalLightIntensity;
        Time.timeScale = 1.3f;

        yield return new WaitForSeconds(discoBallPartyTime);

        Time.timeScale = 1f;
        globalLight.intensity = defaultGlobalLightIntensity;
        discoCoroutine = null;
    }
}
