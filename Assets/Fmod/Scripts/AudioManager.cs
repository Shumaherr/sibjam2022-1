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

    private FMOD.Studio.EventInstance mainMusicInstance;
    private FMOD.Studio.EventInstance ambienceInstance;

    private string buttonClick = "event:/SFX/button click";
    private string buttonHover = "event:/SFX/button hover";
    private string purchase = "event:/SFX/purchase";
    private string mapOpen = "event:/SFX/map open";
    private string mapClose = "event:/SFX/map close";
    private string carMopedDriveAway = "event:/SFX/car send scooter";
    private string carPickupDriveAway = "event:/SFX/car send light";
    private string carMinivanDriveAway = "event:/SFX/car send van";
    private string carTruckDriveAway = "event:/SFX/car send truck";
    private string boxTake = "event:/SFX/box take";
    private string boxPut = "event:/SFX/box put";
    private string cashIncome = "event:/SFX/cash income";

    private void Awake()
    {
        instance = this;

        MasterBus = FMODUnity.RuntimeManager.GetBus("bus:/");
        MusicBus = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SfxBus = FMODUnity.RuntimeManager.GetBus("bus:/SFX");

        pauseSnapshot = FMODUnity.RuntimeManager.CreateInstance("snapshot:/Pause");

        // Create fmod instances here.
        mainMusicInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Main Music");

        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/ambience");
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
        FMODUnity.RuntimeManager.PlayOneShot(buttonClick);
    }

    public void PlayButtonHover()
    {
        FMODUnity.RuntimeManager.PlayOneShot(buttonHover);
    }

    public void PlayPurchase()
    {
        FMODUnity.RuntimeManager.PlayOneShot(purchase);
    }
    public void PlayMapOpen()
    {
        FMODUnity.RuntimeManager.PlayOneShot(mapOpen);
    }
    public void PlayMapClose()
    {
        FMODUnity.RuntimeManager.PlayOneShot(mapClose);
    }

    public void PlayBoxTake()
    {
        FMODUnity.RuntimeManager.PlayOneShot(boxTake);
    }

    public void PlayBoxPut()
    {
        FMODUnity.RuntimeManager.PlayOneShot(boxPut);
    }
    
    public void PlayCashIncome()
    {
        FMODUnity.RuntimeManager.PlayOneShot(cashIncome);
    }

    public void PlayDriveAway(string transportID)
    {
        switch (transportID)
        {
            case "Moped":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carMopedDriveAway);
                    break;
                }

            case "Pickup":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carTruckDriveAway);
                    break;
                }

            case "Minivan":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carPickupDriveAway);
                    break;
                }

            case "Truck":
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carPickupDriveAway);
                    break;
                }

            default:
                {
                    FMODUnity.RuntimeManager.PlayOneShot(carMinivanDriveAway);
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