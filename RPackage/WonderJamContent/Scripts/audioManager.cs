using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rezoskour.Content
{
    public class audioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        // Start is called before the first frame update
        void Start()
        {
            audioSource.Play();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
