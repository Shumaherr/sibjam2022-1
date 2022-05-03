using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private FMOD.Studio.Bus MasterBus { get; set; }
    private FMOD.Studio.Bus MusicBus { get; set; }
    private FMOD.Studio.Bus SfxBus { get; set; }

    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;

    private FMOD.Studio.EventInstance pauseSnapshot;

    [SerializeField]
    private FMODUnity.EventReference mainMusicEvent;
    private FMOD.Studio.EventInstance mainMusicInstance;

    //[SerializeField]
    //private FMODUnity.EventReference footstepsEvent;

    private void Awake()
    {
        instance = this;

        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        pauseSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Pause");

        // Create fmod instances here.
        mainMusicInstance = FMODUnity.RuntimeManager.CreateInstance(mainMusicEvent.Path);
    }

    private void Start()
    {
        PlayMainMusic();
    }

    #region MUSIC
    public void PlayMainMusic()
    {
        if (mainMusicInstance.isValid())
            mainMusicInstance.start();
    }

    public void StopMainMusic()
    {
        if (mainMusicInstance.isValid())
            mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    #endregion

    #region SFX
    //public void PlayFootsteps()
    //{
    //    FMODUnity.RuntimeManager.PlayOneShot(footstepsEvent.Path);
    //}
    #endregion

    #region BUS VOLUME
    ////// BUS VOLUME //////
    public void SetMasterVolumeLevel(float newMasterLevel)
    {
        masterVolume = newMasterLevel;
        MasterBus.setVolume(masterVolume);
    }
    public void SetMusicVolumeLevel(float newMusicLevel)
    {
        musicVolume = newMusicLevel;
        MusicBus.setVolume(musicVolume);
    }
    public void SetSfxVolumeLevel(float newSfxLevel)
    {
        sfxVolume = newSfxLevel;
        SfxBus.setVolume(sfxVolume);
    }
    #endregion

    #region MUTE TOGGLES
    ////// MUTE TOGGLES //////
    public void MasterBusMuteToggle()
    {
        bool mute;
        MasterBus.getMute(out mute);

        FMODUnity.RuntimeManager.MuteAllEvents(!mute);
    }
    public void MusicBusMuteToggle()
    {
        BusMuteControll(MusicBus);
    }
    public void SfxBusMuteToggle()
    {
        BusMuteControll(SfxBus);
    }

    public void BusMuteControll(FMOD.Studio.Bus bus)
    {
        bool mute;
        bus.getMute(out mute);

        if (mute)
            bus.setMute(false);
        else
            bus.setMute(true);
    }
    #endregion

    #region PAUSE
    ////// PAUSE SNAPSHOT & LOGIC //////
    public void SetPauseSoundState()
    {
        pauseSnapshot.start();

        // Additional pause logic.
        SfxBus.setPaused(true);
    }

    public void UnsetPauseSoundState()
    {
        pauseSnapshot.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        // Additional unpause logic.
        SfxBus.setPaused(false);
    }
    #endregion

    #region PLAYBACK STATE
    ////// Return FMOD event playback state. //////
    FMOD.Studio.PLAYBACK_STATE PlaybackState(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE playbackState;
        instance.getPlaybackState(out playbackState);
        return playbackState;
    }
    #endregion
}