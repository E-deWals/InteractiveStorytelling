using System.Collections;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioSource AudioSource;

    [SerializeField] private float minWait = 2;
    [SerializeField] private float maxWait = 5;

    //private void Start()
    //{
    //    StartCoroutine(playAudio());
    //}
    private void OnEnable()
    {
        StartCoroutine(playAudio());
    }
    private IEnumerator playAudio()
    {
        float randomWait = Random.Range(minWait, maxWait);
        int audioClipToPlay = Random.Range(0, audioClips.Length);
        AudioSource.PlayOneShot(audioClips[audioClipToPlay]);
        yield return new WaitForSeconds(randomWait);
        StartCoroutine(playAudio());
    }
}
