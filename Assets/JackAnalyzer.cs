using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using JackAudio;

  [RequireComponent(typeof(AudioSource))]
  public class JackAnalyzer : MonoBehaviour
  {

    public int port;
    private bool started = false;
    private int sampleRate = 48000;
    private int bufferSize = 0;

    private float[] monodata;
    GCHandle monodataHandler;
	
    void Start()
    {
      StartCoroutine(Initialize());
    }
    
    IEnumerator Initialize() {
    int frames = 0;
    	do {
	      yield return null;
	      bufferSize = JackWrapper.GeBufferSize();
	      frames++;
	      if (frames % 100 == 0) {
	      	Debug.Log("still initializing...");
	      	}
	      } while (bufferSize <= 0);
      monodata = new float[bufferSize]; //this has to be set from options
      monodataHandler = GCHandle.Alloc(monodata, GCHandleType.Pinned);
      started = true;
}
     
    void OnDestroy()
    {      
        System.Array.Clear(monodata, 0, monodata.Length);
        monodataHandler.Free();
    }

      float[] buffer = new float[1024];
    void Update ()
    {
      if (!started) { return; }
      int bufferSize = 1024;
      JackWrapper.ReadBuffer(port, buffer, bufferSize);
      float sum = 0;
	foreach(var f in buffer) {
	sum += f;
	}
	Debug.Log($"sum of buffer: {sum}");
    }
  }
