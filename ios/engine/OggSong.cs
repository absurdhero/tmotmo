using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using NVorbis;

// https://gist.github.com/nickgravelyn/5580531
class OggSong : Song, IDisposable
{
    private VorbisReader reader;
    private DynamicSoundEffectInstance effect;
    private byte[] buffer;
    private float[] nvBuffer;

    public SoundState State
    {
        get { return effect.State; }
    }

    public float Volume
    {
        get { return effect.Volume; }
        set { effect.Volume = MathHelper.Clamp(value, 0, 1); }
    }
    
    public float Time
    {
        get;
        set;
    }

    public bool IsLooped { get; set; }

    public OggSong(string oggFile)
    {
        reader = new VorbisReader(oggFile);
        effect = new DynamicSoundEffectInstance(reader.SampleRate, (AudioChannels)reader.Channels);
        buffer = new byte[effect.GetSampleSizeInBytes(TimeSpan.FromMilliseconds(500))];
        nvBuffer = new float[buffer.Length / 2];

        // when a buffer is needed, set our handle so the helper thread will read in more data
        effect.BufferNeeded += (s, e) => readNextBuffer();
    }

    ~OggSong()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected void Dispose(bool isDisposing)
    {
        effect.Dispose();
    }

    public void Play()
    {
        Stop();
        effect.Play();
    }

    public void Pause()
    {
        effect.Pause();
    }

    public void Resume()
    {
        effect.Resume();
    }

    public void Stop()
    {
        if (!effect.IsDisposed)
        {
            effect.Stop();
        }

        reader.DecodedTime = TimeSpan.Zero;
    }

    private void readNextBuffer()
    {
        // read the next chunk of data
        int samplesRead = reader.ReadSamples(nvBuffer, 0, nvBuffer.Length);

        // out of data and looping? reset the reader and read again
        if (samplesRead == 0 && IsLooped)
        {
            reader.DecodedTime = TimeSpan.Zero;
            samplesRead = reader.ReadSamples(nvBuffer, 0, nvBuffer.Length);
        }

        if (samplesRead > 0)
        {
            for (int i = 0; i < samplesRead; i++)
            {
                short sValue = (short)Math.Max(Math.Min(short.MaxValue * nvBuffer[i], short.MaxValue), short.MinValue);
                buffer[i * 2] = (byte)(sValue & 0xff);
                buffer[i * 2 + 1] = (byte)((sValue >> 8) & 0xff);
            }

            effect.SubmitBuffer(buffer, 0, samplesRead);
            effect.SubmitBuffer(buffer, samplesRead, samplesRead);
            
            Time += (float) effect.GetSampleDuration(samplesRead * 2).TotalSeconds;
        }
    }
}
