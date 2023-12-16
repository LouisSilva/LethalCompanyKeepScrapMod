using GameNetcodeStuff;
using HarmonyLib;
using TMPro;

namespace LethalCompanyKeepScrap.Patches
{ 
  [HarmonyPatch(typeof(HUDManager))]
  [HarmonyPatch("FillEndGameStats")]
  internal class FillEndGameStatsPatch
  { 
    static bool Prefix(ref HUDManager __instance, EndOfGameStats stats)
    { 
      int num1 = 0;
      int num2 = 0;
      for (int index1 = 0; index1 < __instance.playersManager.allPlayerScripts.Length; ++index1)
      {
        PlayerControllerB allPlayerScript = __instance.playersManager.allPlayerScripts[index1];
        __instance.statsUIElements.playerNamesText[index1].text = "";
        __instance.statsUIElements.playerStates[index1].enabled = false;
        __instance.statsUIElements.playerNotesText[index1].text = "Notes: \n";
        if (allPlayerScript.disconnectedMidGame || allPlayerScript.isPlayerDead || allPlayerScript.isPlayerControlled)
        {
          if (allPlayerScript.isPlayerDead)
            ++num1;
          else if (allPlayerScript.isPlayerControlled)
            ++num2;
          __instance.statsUIElements.playerNamesText[index1].text = __instance.playersManager.allPlayerScripts[index1].playerUsername;
          __instance.statsUIElements.playerStates[index1].enabled = true;
          __instance.statsUIElements.playerStates[index1].sprite = !__instance.playersManager.allPlayerScripts[index1].isPlayerDead ? __instance.statsUIElements.aliveIcon : (__instance.playersManager.allPlayerScripts[index1].causeOfDeath != CauseOfDeath.Abandoned ? __instance.statsUIElements.deceasedIcon : __instance.statsUIElements.missingIcon);
          for (int index2 = 0; index2 < 3 && index2 < stats.allPlayerStats[index1].playerNotes.Count; ++index2)
          {
            TextMeshProUGUI textMeshProUgui = __instance.statsUIElements.playerNotesText[index1];
            textMeshProUgui.text = textMeshProUgui.text + "* " + stats.allPlayerStats[index1].playerNotes[index2] + "\n";
          }
        }
        else
          __instance.statsUIElements.playerNotesText[index1].text = "";
      }
      __instance.statsUIElements.quotaNumerator.text = RoundManager.Instance.scrapCollectedInLevel.ToString();
      __instance.statsUIElements.quotaDenominator.text = RoundManager.Instance.totalScrapValueInLevel.ToString();
      
      // The below code is taken out so the allPlayersDeadOverlay does not show, because players keep their scrap even when everyone is dead
      
      // if (StartOfRound.Instance.allPlayersDead)
      // {
      //   __instance.statsUIElements.allPlayersDeadOverlay.enabled = true;
      //   __instance.statsUIElements.gradeLetter.text = "F";
      // }
      // else
      // {
      
      __instance.statsUIElements.allPlayersDeadOverlay.enabled = false;
      int num3 = 0;
      float num4 = (float) RoundManager.Instance.scrapCollectedInLevel / RoundManager.Instance.totalScrapValueInLevel;
      if (num2 == StartOfRound.Instance.connectedPlayersAmount + 1)
        ++num3;
      else if (num1 > 1)
        --num3;
      if ((double) num4 >= 0.9900000095367432)
        num3 += 2;
      else if ((double) num4 >= 0.6000000238418579)
        ++num3;
      else if ((double) num4 <= 0.25)
        --num3;
      switch (num3)
      {
        case -1:
          __instance.statsUIElements.gradeLetter.text = "D";
          break;
        case 0:
          __instance.statsUIElements.gradeLetter.text = "C";
          break;
        case 1:
          __instance.statsUIElements.gradeLetter.text = "B";
          break;
        case 2:
          __instance.statsUIElements.gradeLetter.text = "A";
          break;
        case 3:
          __instance.statsUIElements.gradeLetter.text = "S";
          break;
      }
      // } -> This bracket is part of the commented out if-else chain

      return false;
    }
  }
}