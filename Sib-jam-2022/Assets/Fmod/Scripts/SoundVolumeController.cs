using UnityEngine;
using UnityEngine.UI;

public class SoundVolumeController : MonoBehaviour
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    public void MasterVolumeLevel()
    {
        AudioManager.instance.SetMasterVolumeLevel(masterSlider.value);
    }

    public void MusicVolumeLevel()
    {
        AudioManager.instance.SetMusicVolumeLevel(musicSlider.value);
    }

    public void SfxVolumeLevel()
    {
        AudioManager.instance.SetSfxVolumeLevel(sfxSlider.value);
    }
    public void MasterToggle()
    {
        AudioManager.instance.MasterBusMuteToggle();
    }
    public void MusicToggle()
    {
        AudioManager.instance.MusicBusMuteToggle();
    }
    public void SfxToggle()
    {
        AudioManager.instance.SfxBusMuteToggle();
    }
}