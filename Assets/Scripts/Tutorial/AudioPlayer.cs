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

        public void PlaySound(int i = 0, bool isRandom = false)
        {
            audioSource.pitch = 1;
            // 0 buat placement, 1 buat spawn structure, 2 buat remove
            if(sfx.Length >= i+1)
            {
                if (isRandom)
                {
                    switch (Random.Range(0,9))
                    {
                    case 0:
                        audioSource.pitch = 1f; break;
                    case 1:
                        audioSource.pitch = 1.1f; break;
                    case 2:
                        audioSource.pitch = 1.2f; break;
                    case 3:
                        audioSource.pitch = 1.3f; break;
                    case 4:
                        audioSource.pitch = 1.4f; break;
                    case 6:
                        audioSource.pitch = 1.5f; break;
                    case 7:
                        audioSource.pitch = 1.6f; break;
                    case 8:
                        audioSource.pitch = 1.7f; break;
                    default: 
                        audioSource.pitch = 1.8f; break;
                    }
                }   
                audioSource.PlayOneShot(sfx[i]);
            }
        }
    }
}