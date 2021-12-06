using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JackAudio;

public class JackSetup : MonoBehaviour
{
    public int inputs = 1;
    public int outputs = 1;
    
    private bool activated;
        
    // Start is called before the first frame update
    void Start()
    {
        int numbuffers;
        int bufferSize;
        AudioSettings.GetDSPBufferSize(out bufferSize, out numbuffers);
        AudioConfiguration config = AudioSettings.GetConfiguration();

        JackLogger.Initialize();
        if (JackWrapper.CreateClient((uint)bufferSize,(uint)config.sampleRate))
        {
          JackWrapper.RegisterPorts((uint)inputs, (uint)outputs);
          activated = JackWrapper.ActivateClient();
        }
        GetComponent<AudioSource>().Play();
    }
    
    void OnDestroy() {
        if (activated) {
            JackWrapper.DestroyClient();
        } else {
            Debug.LogError("JackWrapper was never started!");
        }
    }
}
