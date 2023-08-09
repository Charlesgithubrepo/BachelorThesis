using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using LSL;
using System;

namespace LSL4Unity.Samples.SimplePhysicsEvent
{
    public class LSLOutletFlash : MonoBehaviour
    {
        public string StreamName = "ImageEnabled";
        public string StreamType = "Markers";
        private StreamOutlet outlet;
        private string[] sample = new string[2]; // Modified to include timestamp
        private Image image;

        void Start()
        {
            var hash = new Hash128();
            hash.Append(StreamName);
            hash.Append(StreamType);
            hash.Append(gameObject.GetInstanceID());

            // Updated StreamInfo to include timestamps
            StreamInfo streamInfo = new StreamInfo(StreamName, StreamType, 2, LSL.LSL.IRREGULAR_RATE,
                channel_format_t.cf_string, hash.ToString());

            outlet = new StreamOutlet(streamInfo);
            image = GetComponent<Image>();
        }

        void Update()
        {
            if (outlet != null)
            {
                if (image.enabled)
                {
                    sample[0] = "ImageEnabled: " + image.enabled.ToString();
                    sample[1] = Time.realtimeSinceStartup.ToString(); // Using Unity's time as timestamp
                    Debug.Log(sample[0] + " Timestamp: " + sample[1]);
                    outlet.push_sample(sample);
                }
            }
        }
    }
}
