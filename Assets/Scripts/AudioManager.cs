using UnityEngine;
using CSCore.SoundIn;
using CSCore.CoreAudioAPI;
using System;
using System.Linq;

public class AudioManager : MonoBehaviour
{
    //TEMPO FROM THE MUSIC
    //AS LONG AS IT COMES FROM THE SEQUENCE

    //Wrapper for the Windows audio session API (CSCore) 
    private WasapiLoopbackCapture loopBack = new WasapiLoopbackCapture();

    //array of avaible audio devices
    private MMDeviceCollection devices;

    //the array containing the data fetched by CSCore (implementation of Windows audio session API)
    private float[] audioBuffer;

    //how many samples the [Average sample] will be based on 
    public byte LastSamplesBuffer = 64;

    private byte lastSamplesIndex;

    //An array containing the last samples in order to get an average
    private float[] lastSamples;

    //Average of n [LastSamplesBuffer] of samples
    public float AverageSampleFrequency;

    //Most hearable sample in [LastSamplesBuffer]
    public float AverageMaxSampleFrequency;

    //the current most hearable sample 
    public float CurrentMaxFrequency = 0;

    //To Compensate for low volume
    public float CurrentMaxFrequencyMultiplier = 1;

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
            if (maxFrequencyBuffer > 0.0001100110011001101) this.CurrentMaxFrequency = this.audioBuffer.Max() * this.CurrentMaxFrequencyMultiplier;
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

    public void Update()
    {
        this.AddLastSample();
        this.AverageMaxSampleFrequency = this.lastSamples.Max();
        this.AverageSampleFrequency = this.lastSamples.Average();
    }
}
