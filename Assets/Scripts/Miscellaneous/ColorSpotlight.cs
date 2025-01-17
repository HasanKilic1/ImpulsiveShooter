using System.Collections;
using UnityEngine;

public class ColorSpotlight : MonoBehaviour
{
    [SerializeField] private GameObject spotlightHead;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float discoRotSpeed = 120f;
    [SerializeField] private float maxRotation = 45f;
    private float currentRotation;

    public float GetRotationSpeed => rotationSpeed;
    public void SetRotationSped(float rotationSpeed) => this.rotationSpeed = rotationSpeed;

    private void Start()
    {
        RandomStartingRotation();
    }

    private void Update()
    {
        RotateHead();
    }

    private void RotateHead()
    {
        currentRotation += Time.deltaTime * rotationSpeed;
        float z = Mathf.PingPong(currentRotation, maxRotation);
        spotlightHead.transform.localRotation = Quaternion.Euler(0f, 0f, z);
    }

    private void RandomStartingRotation()
    {
        float randomRotation = Random.Range(-maxRotation, maxRotation);
        spotlightHead.transform.rotation = Quaternion.Euler(0f, 0f, randomRotation);
        currentRotation = randomRotation + maxRotation;
    }

    public IEnumerator SpotlightDiscoParty(float time)
    {
        float defaultRotSpeed = rotationSpeed;
        rotationSpeed = discoRotSpeed;

        yield return new WaitForSeconds(time);

        rotationSpeed = defaultRotSpeed;
    }
}
