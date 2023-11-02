[System.Serializable]
public class SettingModel
{
    public float sfx;
    public float bgm;

    public SettingModel(SettingMenu setting)
    {
      this.sfx = setting.sfx;
      this.bgm = setting.bgm;
    }
}