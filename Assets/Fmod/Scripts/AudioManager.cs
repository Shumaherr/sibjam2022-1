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

    [SerializeField]
    private FMODUnity.EventReference ambienceEvent;
    private FMOD.Studio.EventInstance ambienceInstance;

    [SerializeField]
    private FMODUnity.EventReference buttonClick;

    [SerializeField]
    private FMODUnity.EventReference buttonHover;

    [SerializeField]
    private FMODUnity.EventReference purchase;

    [SerializeField]
    private FMODUnity.EventReference mapOpen;

    [SerializeField]
    private FMODUnity.EventReference mapClose;

    [SerializeField]
    private FMODUnity.EventReference carMopedDriveAway;

    [SerializeField]
    private FMODUnity.EventReference carPickupDriveAway;

    [SerializeField]
    private FMODUnity.EventReference carMinivanDriveAway;

    [SerializeField]
    private FMODUnity.EventReference carTruckDriveAway;

    private void Awake()
    {
        instance = this;

        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        pauseSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Pause");

        // Create fmod instances here.
        mainMusicInstance = FMODUnity.RuntimeManager.CreateInstance(mainMusicEvent.Path);

        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(ambienceEvent.Path);
    }

    private void Start()
    {
        PlayMainMusic();
        PlayAmbience();
    }

    #region MUSIC
    public void PlayMainMusic()
    {
        if (PlaybackState(mainMusicInstance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            mainMusicInstance.start();
    }

    public void StopMainMusic()
    {
        if (mainMusicInstance.isValid())
            mainMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
    #endregion

    #region SFX
    public void PlayAmbience()
    {
        if (PlaybackState(ambienceInstance) != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            ambienceInstance.start();
    }

    public void StopAmbience()
    {
        if (ambienceInstance.isValid())
            ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public void PlayButtonClick()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonClick.Path);
    }

    public void PlayButtonHover()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonHover.Path);
    }

    public void PlayPurchase()
    {
        FMODUnity.RuntimeManager.PlayOneShot(purchase.Path);
    }
    public void PlayMapOpen()
    {
        FMODUnity.RuntimeManager.PlayOneShot(mapOpen.Path);
    }
    public void PlayMapClose()
    {
        FMODUnity.RuntimeManager.PlayOneShot(mapClose.Path);
    }

    public void PlayDriveAway(string transportID)
    {
        switch (transportID)
        {
            case "Moped":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carMopedDriveAway.Path);
                    break;
                }

            case "Pickup":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carTruckDriveAway.Path);
                    break;
                }

            case "Minivan":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carPickupDriveAway.Path);
                    break;
                }

            case "Truck":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carPickupDriveAway.Path);
                    break;
                }

            default:
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carMinivanDriveAway.Path);
                    break;
                }
        }

    }
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