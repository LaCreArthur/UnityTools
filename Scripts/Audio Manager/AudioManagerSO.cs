using UnityEngine;
using UnityReusables.Managers.Audio_Manager;

[CreateAssetMenu(menuName = "Scriptable Objects/Managers/Audio Manager")]
public class AudioManagerSO : ScriptableObject
{
    public AudioSingletonMono m;
    public void Play(string sfx) => m.Play(sfx);
}