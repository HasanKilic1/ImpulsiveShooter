using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects / Sound Collection")]
public class SoundsCollectionSO : ScriptableObject
{
    [Header("Music")]
    public SoundSO[] FightMusics;
    public SoundSO[] DiscoBallMusics;

    [Header("SFX")]
    public SoundSO[] GunShoot;
    public SoundSO[] Jump;
    public SoundSO[] Splat;
    public SoundSO[] Jetpack;
    public SoundSO[] Grenade_Explosion;
    public SoundSO[] Grenade_Beep;
}
