using UnityEngine;

public class DiscoBall : MonoBehaviour, IHitable
{
    private Flash flash;
    private DiscoBallManager discoBallManager;

    private void Awake()
    {
        flash = GetComponent<Flash>();
        discoBallManager = FindFirstObjectByType<DiscoBallManager>();
    }

    public void TakeHit()
    {
        flash.StartFlash();
        discoBallManager.DiscoBallParty();
        Debug.Log("Disco ball hit");
    }
}
