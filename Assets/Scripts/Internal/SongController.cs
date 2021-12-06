using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;


public class SongController : MonoBehaviour {
	public int LowsTop = 80;
	public int MidsBottom = 60;
	public int HighsThreshold = 180;

	public float BeefThreshold = 4f;

	public int ThresholdBufferSize = 50;

    [DebugGUIGraph(min: 0, max: 3, r: 0, g: 1, b: 0, autoScale: true)]
	public float Flux = 0f;
    [DebugGUIGraph(min: 0, max: 3, r: 1, g: 0, b: 0, autoScale: true)]
	public float Peak = 0f;
    [DebugGUIGraph(min: 0, max: 3, r: 0, g: 0, b: 1, autoScale: true)]
	public float Threshold = 0f;

	public float[] RealTimeSpectrum;

    [DebugGUIGraph(group: 1, min: 0, max: 0.35f, r: 1, g: 1, b: 0, autoScale: false)]
	public float[] ScaledRealTimeSpectrum;

	[DebugGUIGraph(group: 2, min: 0, max: 2f, r: 1, g: 0, b: 0, autoScale: true)]
	public float LowsSumWeighted = 0;
	[DebugGUIGraph(group: 2, min: 0, max: 2f, r: 1, g: 1, b: 0, autoScale: true)]
	public float MidsSumWeighted = 0;
	[DebugGUIGraph(group: 2, min: 0, max: 2f, r: 1, g: 0, b: 1, autoScale: true)]
	public float HighsSumWeighted = 0;

	[DebugGUIGraph(group: 3, min: 0, max: 5f, r: 1, g: 0, b: 1, autoScale: true)]
	public float Beefiness = 0;
	[DebugGUIGraph(group: 3, min: 0, max: 1f, r: 1, g: 0, b: 0, autoScale: false)]
	public float BeefMode = 0;

	[DebugGUIGraph(group: 3, min: 0, max: 1f, r: 0, g: 1, b: 0, autoScale: false)]
	public float Beatiness = 0;

	[DebugGUIGraph(group: 3, min: 0, max: 1f, r: 0, g: 1, b: 1, autoScale: true)]
	public float Shininess = 0;

	[DebugGUIGraph(group: 3, min: 0, max: 1f, r: 1, g: 1, b: 0, autoScale: true)]
	public float Loudness = 0;

	SpectralFluxAnalyzer realTimeSpectralFluxAnalyzer;

	int sampleRate;

	public bool realTimeSamples = true;

	void Start() {
		// Process audio as it plays
		if (realTimeSamples) {
			RealTimeSpectrum = new float[1024];
			ScaledRealTimeSpectrum = new float[300];
			realTimeSpectralFluxAnalyzer = new SpectralFluxAnalyzer(ThresholdBufferSize);

			this.sampleRate = AudioSettings.outputSampleRate;
		}
	}

	void FixedUpdate() {
		// Real-time
		if (realTimeSamples) {
			AudioListener.GetSpectrumData (RealTimeSpectrum, 0, FFTWindow.BlackmanHarris);
			realTimeSpectralFluxAnalyzer.analyzeSpectrum (RealTimeSpectrum, (float)AudioSettings.dspTime);

			var samples = realTimeSpectralFluxAnalyzer.spectralFluxSamples;
			if (samples.Count >= ThresholdBufferSize) {
				var sample = samples[samples.Count - ThresholdBufferSize];
				Flux = sample.spectralFlux;
				Peak = sample.isPeak ? sample.spectralFlux : 0f;
				Threshold = sample.threshold;
			}

			// calculate derivatives
			for (int i = 0; i < ScaledRealTimeSpectrum.Length; i++)
            {
				ScaledRealTimeSpectrum[i] = RealTimeSpectrum[Mathf.FloorToInt(Mathf.Pow(10, i / 100f))];
			}
			// sum of lows (impulse sticks)
			var newLowsSum = 0f;
			for (int i = 0; i < LowsTop; i++)
            {
				newLowsSum += RealTimeSpectrum[i];
			}
			// beatiness is when lows jumps
			Beatiness = newLowsSum > LowsSumWeighted + .15f || (Beatiness > 0f && newLowsSum > LowsSumWeighted) ? 1f : 0f;
			LowsSumWeighted = Mathf.Max(newLowsSum, Mathf.Lerp(LowsSumWeighted, newLowsSum, Time.deltaTime));
			// sum of mids
			var newMidsSum = 0f;
			for (int i = MidsBottom; i < HighsThreshold; i++)
			{
				newMidsSum += RealTimeSpectrum[i];
			}
			newMidsSum *= 2; // balance with lows i guess
			MidsSumWeighted = Mathf.Lerp(MidsSumWeighted, newMidsSum, Time.deltaTime);
			// sum of highs (another guess...)
			var newHighsSum = 0f;
			for (int i = HighsThreshold; i < 300; i++)
			{
				newHighsSum += RealTimeSpectrum[i];
			}
			newHighsSum *= 3;
			HighsSumWeighted = Mathf.Lerp(HighsSumWeighted, newHighsSum, Time.deltaTime);
			// proportion of lows to others multiplied by intensity
			Beefiness = (LowsSumWeighted / (LowsSumWeighted + (MidsSumWeighted / 2) + HighsSumWeighted)) * Threshold * 3;
			BeefMode = Beefiness > BeefThreshold ? 1 : 0;
			// shininess is proportion of highs to mids, boosted if lows are low
			Shininess = Mathf.Clamp01(HighsSumWeighted / Mathf.Max(MidsSumWeighted, 0.00001f) - (Beefiness / 2));
			Loudness = Mathf.Clamp01((LowsSumWeighted + MidsSumWeighted + HighsSumWeighted) / 6);
		}
	}

	public float getTimeFromIndex(int index) {
		return ((1f / (float)this.sampleRate) * index);
	}
}