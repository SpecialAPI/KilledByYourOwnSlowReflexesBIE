using BepInEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using BepInEx.Configuration;

namespace KilledByYourOwnSlowReflexesBIE
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "spapi.etg.killedbyyourownslowreflexes";
        public const string NAME = "Killed By Your Own Slow Reflexes";
        public const string VERSION = "1.0.0";
        public static ConfigEntry<bool> IsSkillIssueMode;
        public static ConfigEntry<bool> IsRatDrawingMode;

        public void Awake()
        {
            IsSkillIssueMode = Config.Bind("KilledByYourOwnSlowReflexes", "SkillIssueMode", false, "If true, the death label will say \"A skill issue\" instead of \"Your own slow reflexes\"");
            IsRatDrawingMode = Config.Bind("KilledByYourOwnSlowReflexes", "RatDrawingMode", false, "If true, the rat drawings that are displayed when killed by the Resourceful Rat will always be displayed.");
            new Harmony(GUID).PatchAll();
        }

        [HarmonyPatch(typeof(AmmonomiconDeathPageController), nameof(AmmonomiconDeathPageController.InitializeRightPage))]
        [HarmonyPostfix]
        public static void DoTheChange(AmmonomiconDeathPageController __instance)
        {
            if (!__instance.isVictoryPage)
            {
                __instance.killedByLabel.Text = IsSkillIssueMode.Value ? "A skill issue" : StringTableManager.GetEnemiesString("#KILLEDBYDEFAULT", -1);
                __instance.killedByLabel.PerformLayout();
                __instance.UpdateTapeLabel(__instance.killedByLabel, __instance.killedByLabel.GetAutosizeWidth());
                if (__instance.RatDeathDrawings && IsRatDrawingMode.Value)
                {
                    __instance.RatDeathDrawings.IsVisible = true;
                }
            }
        }
    }
}
