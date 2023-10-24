//	Created by: Sunny Valley Studio 
//	https://svstudio.itch.io

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SVS
{

    public class AudioPlayer : MonoBehaviour
    {
        public AudioClip[] sfx;
        public AudioSource audioSource;

        public static AudioPlayer instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this.gameObject);

        }

        public void PlaySound(int i = 0)
        {
            // 0 buat placement, 1 buat spawn structure, 2 buat remove
            if(sfx.Length >= i+1)
            {
                audioSource.PlayOneShot(sfx[i]);
            }
        }
    }
}