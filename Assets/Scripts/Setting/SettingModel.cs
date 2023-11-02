[System.Serializable]
public class SettingModel
{
    public float sfx;
    public float bgm;

    public SettingModel(VolumeSetting setting)
    {
      this.sfx = setting.sfx;
      this.bgm = setting.bgm;
    }
}