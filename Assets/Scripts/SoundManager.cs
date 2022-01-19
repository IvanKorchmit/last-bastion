using UnityEngine;

public static class SoundManager
{
    public static GameObject soundSource;
    public static void PlaySound(AudioClip sound)
    {
        AudioSource temp = GameObject.Instantiate(soundSource, Camera.main.transform).GetComponent<AudioSource>();
        temp.clip = sound;
        temp.Play();
        Object.Destroy(temp.gameObject, temp.clip.length);
    }
    public static void PlaySound(AudioClip[] sound)
    {
        AudioClip randSound = sound[Random.Range(0, sound.Length)];
        PlaySound(randSound);
    }
    public static void PlaySound(AudioClip sound, Vector2 source)
    {
        AudioSource temp = GameObject.Instantiate(soundSource, source, Quaternion.identity).GetComponent<AudioSource>();
        temp.clip = sound;
        temp.Play();
        Object.Destroy(temp.gameObject, temp.clip.length);
    }
    public static void PlaySound(AudioClip[] sound, Vector2 source)
    {
        AudioSource temp = GameObject.Instantiate(soundSource, source, Quaternion.identity).GetComponent<AudioSource>();
        AudioClip randSound = sound[Random.Range(0, sound.Length)];
        PlaySound(randSound, source);
    }
}
