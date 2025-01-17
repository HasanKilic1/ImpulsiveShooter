using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Range(0f, 2f)][SerializeField] float masterVolume = 1f;

    [SerializeField] SoundsCollectionSO soundsCollectionSO;
    [SerializeField] AudioMixerGroup sfxMixerGroup;
    [SerializeField] AudioMixerGroup musicMixerGroup;
    
    private AudioSource currentMusicSource;

    private void OnEnable()
    {
        Gun.OnShoot += Gun_OnShoot;
        PlayerController.OnJump += PlayerController_OnJump;
        PlayerController.OnJetpack += PlayerController_OnJetpack;
        Health.OnDeath += Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit += DiscoBallMusic;
        Grenade.OnExplosion += Grenade_Explosion;
        Grenade.OnBeep += Grenade_Beep;
    }

    private void OnDisable()
    {
        Gun.OnShoot -= Gun_OnShoot;
        PlayerController.OnJump -= PlayerController_OnJump;
        PlayerController.OnJetpack -= PlayerController_OnJetpack;
        Health.OnDeath -= Health_OnDeath;
        DiscoBallManager.OnDiscoBallHit -= DiscoBallMusic;
        Grenade.OnExplosion -= Grenade_Explosion;
        Grenade.OnBeep -= Grenade_Beep;
    }

    private void Start()
    {
        FightMusic();
    }

    private void PlayRandomSound(SoundSO[] sounds)
    {
        if (sounds != null && sounds.Length > 0)
        {
            SoundSO randomSoundSO = sounds[Random.Range(0, sounds.Length)];
            AdjustSoundAndPlay(randomSoundSO);
        }
    }


    private void AdjustSoundAndPlay(SoundSO soundSO)
    {
        AudioClip clip = soundSO.Clip;
        float pitch = soundSO.Pitch;
        float volume = soundSO.Volume;
        bool loop = soundSO.Loop;

        pitch = soundSO.RandomizePitch ? soundSO.Pitch + Random.Range(-soundSO.RandomPitchRangeModifier, +soundSO.RandomPitchRangeModifier) :
                                         soundSO.Pitch;

        AudioMixerGroup audioMixerGroup = soundSO.AudioType == SoundSO.AudioTypes.SFX ? sfxMixerGroup : musicMixerGroup;

        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }


    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        GameObject soundObject = new GameObject("Temp Audio Source");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.pitch = pitch;
        audioSource.volume = volume * masterVolume;
        audioSource.loop = loop;
        audioSource.outputAudioMixerGroup = audioMixerGroup;

        audioSource.Play();
        if (!loop) Destroy(soundObject, clip.length);

        if(audioMixerGroup == musicMixerGroup)
        {
            if(currentMusicSource != null)
            {
                currentMusicSource.Stop();
            }

            currentMusicSource = audioSource;
        }
    }

    private void Gun_OnShoot()
    {
        PlayRandomSound(soundsCollectionSO.GunShoot);
    }
    private void PlayerController_OnJump()
    {
        PlayRandomSound(soundsCollectionSO.Jump);
    }

    private void PlayerController_OnJetpack()
    {
        PlayRandomSound(soundsCollectionSO.Jetpack);
    }

    private void Health_OnDeath(Health health)
    {
        PlayRandomSound(soundsCollectionSO.Splat);
    }

    private void Grenade_Explosion()
    {
        PlayRandomSound(soundsCollectionSO.Grenade_Explosion);
    }

    private void Grenade_Beep()
    {
        PlayRandomSound(soundsCollectionSO.Grenade_Beep);
    }

    private void FightMusic()
    {
        PlayRandomSound(soundsCollectionSO.FightMusics);
    }

    private void DiscoBallMusic()
    {
        PlayRandomSound(soundsCollectionSO.DiscoBallMusics);
        float soundLength = soundsCollectionSO.DiscoBallMusics[0].Clip.length;
        Utils.RunAfterDelay(this, soundLength, FightMusic, out _);
    }

}
