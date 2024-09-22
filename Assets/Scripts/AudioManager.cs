using UnityEngine;
using CSCore.SoundIn;
using CSCore.CoreAudioAPI;
using System;
using System.Linq;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{

    //Wrapper for the Windows audio session API (CSCore) 
    private WasapiLoopbackCapture loopBack = new WasapiLoopbackCapture();

    //array of avaible audio devices
    private MMDeviceCollection devices;

    //the array containing the data fetched by CSCore (implementation of Windows audio session API)
    private float[] audioBuffer;

    //last n [CurrentMaxFrequency]
    //the higher this value, the more precised will the environment be(works better with songs that change beat frequently)
    [Range(16, 1024)]
    public short LastSamplesBuffer = 128;

    private byte lastSamplesIndex;

    //An array containing the last samples in order to get an average
    private float[] lastSamples;

    //Average of n [LastSamplesBuffer] of samples
    public float LastSamplesAverage;

    //Most hearable sample in [LastSamplesBuffer]
    public float LastSamplesMaxFrequency;

    //The "Tempo" of [lastSamples]
    //Indicates how many times in average the frequencies are equivalent (more exactly in the same range)
    public int LastSamplesTempo = 1;

    //What is considered "Equivalent" (ex: 0.33 -> 0.34 isn't a noticable change), the n percent of the initial value (40 - 10 and 40 + 10 = (30 - 50))
    [Range(0.1f, 1f)]
    public float LastSamplesTempoDifferentialPercentage = 0.20f;

    //the current most hearable sample 
    public float CurrentMaxFrequency = 0;

    //To Compensate for low volume
    public float CurrentMaxFrequencyMultiplier = 1;

    //the result of (CurrentMaxFrequency*100)/LastSamplesMaxFrequency
    //signifies the frequency relative to Max() of lastSamples (to fix low volume issues that would only return 0 - 0.20 frequencies)
    public float CurrentToMaxFrequencyPercentage;


    //the index in the list of currently active audio devices
    //Should be second element of the array(1) by default?
    public byte AudioEndPoint = 1;

    public DataFlow DataFlow = DataFlow.All;

    public DeviceState DeviceState = DeviceState.Active;

    public void Start()
    {
        this.devices = MMDeviceEnumerator.EnumerateDevices(this.DataFlow, this.DeviceState);
        this.loopBack.Device = this.devices[this.AudioEndPoint];
        this.lastSamples = new float[this.LastSamplesBuffer];
        this.InitializeLoopback();
    }

    public void InitializeLoopback()
    {
        loopBack.Initialize();
        float maxFrequencyBuffer;
        loopBack.DataAvailable += (sender, _event) =>
        {
            this.audioBuffer = new float[_event.Data.Length / 4];
            //BlockCopy takes blocks of bytes (4 bytes for floats) in the byte data array
            //returned by [_event.Data], assembles them in binary and converts the results into floats
            //by diving it by Int32.Max() -> 2 147 483 647
            Buffer.BlockCopy(_event.Data, 0, this.audioBuffer, 0, _event.Data.Length);
            //Getting the loudest sample in the float[]
            maxFrequencyBuffer = this.audioBuffer.Max();
            //to handle E notation floats -> 9.040459E-06 returned when no sound is being played
            if (maxFrequencyBuffer > 0.01) this.CurrentMaxFrequency = this.audioBuffer.Max() * this.CurrentMaxFrequencyMultiplier;
            else this.CurrentMaxFrequency = 0f;
        }; 
        loopBack.Start();
    }

    private float[] Displace(float[] initialArray, float nSample)
    {
        //Is Size of initial array in order to be able add the new sample at the end
        float[] temp = new float[initialArray.Length];
        for (int elem = 0; elem < initialArray.Length - 1; elem++)
            temp[elem] = initialArray[elem + 1];    
        temp[initialArray.Length - 1] = nSample;
        return temp;
    }

    private void AddLastSample()
    {
        if (this.lastSamplesIndex < this.lastSamples.Length) this.lastSamples[this.lastSamplesIndex++] = this.CurrentMaxFrequency;
        else this.lastSamples = this.Displace(this.lastSamples, this.CurrentMaxFrequency);
    }

    public void ListDevicesDebug()
    {
        foreach (MMDevice device in devices) Debug.Log(device);
    }

    public int GetLastSamplesTempo(float[] samples)
    {
        List<int> tempoValues = new List<int>();
        float percentagePart;
        int tempoValue = 0;
        for (int elem = 0; elem < samples.Length - 1; elem++)
        {
            percentagePart = samples[elem] * this.LastSamplesTempoDifferentialPercentage;
            if (samples[elem + 1] >= samples[elem] - percentagePart && samples[elem + 1] <= samples[elem] + percentagePart) tempoValue++;
            else if (tempoValue > 0)
            {
                tempoValues.Add(tempoValue);
                tempoValue = 0;
            }

        }
        return tempoValues.Count > 0 ? (int)tempoValues.Average() : 1;
    }

    public void Update()
    {
        this.AddLastSample();
        this.LastSamplesMaxFrequency = this.lastSamples.Max();
        this.LastSamplesAverage = this.lastSamples.Average();
        this.CurrentToMaxFrequencyPercentage = ((this.CurrentMaxFrequency * 100)/this.LastSamplesMaxFrequency);
        this.LastSamplesTempo = GetLastSamplesTempo(this.lastSamples);
    }
}
