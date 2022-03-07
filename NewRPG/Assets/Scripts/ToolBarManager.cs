using UnityEngine;
using System.Collections;

public class ToolBarManager : MonoBehaviour
{ 
    public void OnStatusButtonClick()
    {
        StatusManager._instance.OnStatusShowTransform();
    }

    public void OnEquipButtonClick()
    {
        EquipManager._instance.OnEquipShowTransform();
    }

    public void OnBagButtonClick()
    {
        BagManager._instance.OnBagShowTransform();
    }

    public void OnSkillButtonClick()
    {
        SkillManager._instance.OnSkillShowTransform();
    }

    public void OnQuestButtonClick()
    {
        QuestManager._instance.OnQuestShowTransform();
    }
    public void OnSettingButtonClick()
    {
        SettingManager._instance.OnSettingShowTransform();
    }

}
