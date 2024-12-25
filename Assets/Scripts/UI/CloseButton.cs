using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    public AudioClip closeClip;

    void Start()
    {
        transform.GetComponent<Button>().onClick.AddListener(Close);
    }

    public void Close()
    {
        AudioManager.instance.soundSource.PlayOneShot(closeClip);
        transform.parent.gameObject.SetActive(false);

    }
}
