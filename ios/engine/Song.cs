using System;

public interface Song
{
    float Volume { get; set; }
    float Time { get; set; }
    
    void Dispose();
    void Play();
    void Stop();
    void Pause();
    void Resume();

}

