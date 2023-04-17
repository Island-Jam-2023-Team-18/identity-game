using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxPlayaround : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        soundManager.PlayGameMusic();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
