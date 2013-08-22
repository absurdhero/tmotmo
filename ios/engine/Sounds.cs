using System;
using System.Collections.Generic;

public class Sounds {
	const float trackStartingOffset = 8.0f;
	
	public Song guitar1;
	public Song keys;
	public Song bass_beatsfx;
	public Song guitars2_beats;
	public Song lead_bgvocals;
	public Song vocalsfx_livedrums;

	public Song[] orderedStems;
	private Song playingStem; // we need this to get playingStem.Time
	public static string playingStems = "??";

	public Sounds() {
        guitar1 = new OggSong(soundPath("GUITARS 1 STEM"));
        keys = new OggSong(soundPath("KEYS STEM_01"));
        bass_beatsfx = new OggSong(soundPath("bass_beatsfx"));
        guitars2_beats = new OggSong(soundPath("guitars2_beats"));
        lead_bgvocals = new OggSong(soundPath("lead_bgvocals"));
        vocalsfx_livedrums = new OggSong(soundPath("vocalsfx_livedrums"));
        
        orderedStems = new Song[]
      { bass_beatsfx, vocalsfx_livedrums, guitars2_beats, lead_bgvocals, guitar1, keys };
	}
	
    private string soundPath(string stemName) {
        return "Content/Sounds/" + stemName + ".ogg";
    }

	public void playAudio () {
		foreach (var stem in orderedStems) {
			stem.Play ();
		}
	}

	public void setAudioTime (float when) {
		foreach (var stem in orderedStems) {
			setTime (stem, when);
		}
	}

	private void setTime (Song stem, float when) {
		stem.Time = when + trackStartingOffset;
	}

	public float getAudioTime () {
		if (playingStem == null) {
			return 0f;
		}
		return playingStem.Time - trackStartingOffset;
	}

	// Strategy: Round 1: Play all stems
	// Then remove the last (highest array index) pair of stems each round...
	// ...until only the first and second are left, after that start adding in the highest numbers again...
	// ...until we're back to playing all again, and then we start over
	public void pickStemsFor (int repetition) {
		var toPlay = new List<Song> ();
		
		toPlay.Add (orderedStems[0]);
		
		int cycle = Math.Min(1, orderedStems.Length - 2);
		int n = repetition % cycle;
		
		if (n < cycle / 2) {
			for (int i = 1; i <= cycle / 2 - n; i++) {
				toPlay.Add (orderedStems[i*2-1]);
				toPlay.Add (orderedStems[i*2]);
			}
		} else {
			for (int i = 1; i <= n - cycle / 2; i++) {
				toPlay.Add (orderedStems[orderedStems.Length - i*2 + 1]);
				toPlay.Add (orderedStems[orderedStems.Length - i*2]);
			}
		}
		stemStatus (toPlay);
		//Debug.Log(playingStems);
		playStems (toPlay);
	}

	public void startPlaying () {
		foreach (var stem in orderedStems) {
			stem.Stop ();
			stem.Time = 0f;
			stem.Play ();
		}
		playingStem = bass_beatsfx;
	}

	public void playAllStems () {
		playStems (new List<Song> (orderedStems));
	}

	public void playStems (List<Song> toPlay) {
		logStems (toPlay);
		
		foreach (var stem in orderedStems) {
			stem.Volume = (toPlay.Contains (stem) ? 1f : 0f);
		}
	}

	private void logStems (List<Song> toPlay) {
		string msg = playingStems + " - ";
		foreach (var stem in toPlay) {
			msg += print (stem);
		}
		//Debug.Log(msg);
	}

	private string print (Song stem) {
		if (stem == guitar1)
			return "guitar1 ";
		if (stem == keys)
			return "keys ";
		if (stem == bass_beatsfx)
			return "bass_beatsfx ";
		if (stem == guitars2_beats)
			return "guitars2_beats ";
		if (stem == lead_bgvocals)
			return "lead_bgvocals ";
		if (stem == vocalsfx_livedrums)
			return "vocalsfx_livedrums ";

		return "SOMETHING'S MISSING!!";
	}

	private void stemStatus (List<Song> toPlay) {
		string result = "";
		foreach (var stem in orderedStems) {
			result += (toPlay.Contains (stem) ? ":" : ".");
		}
		playingStems = result;
	}
}
