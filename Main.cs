using System;
using HarmonyLib;
using Il2Cpp;
using Il2CppTLD.Gear;
using System.Collections.Generic;
using Il2CppTLD.ModularElectrolizer;
using MelonLoader;
using UnityEngine;
using static Il2Cpp.GridUI;

[assembly: MelonInfo(typeof(TLDTestMod.Main), "TLD Electric Mod", "0.0.0", "LEI")]
[assembly: MelonGame("Hinterland", "TheLongDark")]

namespace TLDTestMod
{
    public class Main : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("[TLD Electric Mod] is loaded!");
        }
    }

    [HarmonyPatch(typeof(GearItem), nameof(GearItem.DecayOverTODHours))]
    internal class GearItem_DecayOverTODHours_CustomLogic
    {
        private static void Postfix(GearItem __instance, float deltaTODHours)
        {
            if (__instance == null || __instance.m_CurrentHP <= 0f) return;

            if (__instance.name.StartsWith("GEAR_CarBattery", StringComparison.OrdinalIgnoreCase))
            {
                bool isAuroraActive = false;
                if (GameManager.GetAuroraManager() != null)
                {
                    isAuroraActive = GameManager.GetAuroraManager().AuroraIsActive();
                }


                float customDailyDecay  = 1f;
                float customHourLoadDecay   = 1f;
                float customHourCharge = 3f;

                if (isAuroraActive){
                    float chargeAmount = customHourCharge * deltaTODHours;

                    __instance.m_CurrentHP = Mathf.Clamp(__instance.m_CurrentHP + chargeAmount, 0f, 100f);
                }
                else {



                    if (GetSignBitAsBool(__instance.m_CurrentHP)) 
                    {
                        float decayAmount = customHourLoadDecay * deltaTODHours;
                        __instance.m_CurrentHP = Mathf.Clamp(__instance.m_CurrentHP - decayAmount, 0.1f, 100f);
                    }
                    else
                    { 
                        float decayAmount = (customDailyDecay / 24f) * deltaTODHours;
                        __instance.m_CurrentHP = Mathf.Clamp(__instance.m_CurrentHP - decayAmount, 0.1f, 100f);
                    }
                }
            }
        }


        [HarmonyPatch(typeof(AuroraModularElectrolizer), "Initialize")]
        internal class AuroraElectrolizer_Initialize
        {
            private static void Postfix(AuroraModularElectrolizer __instance)
            {
                if (Light.IsMenu())
                {
                    return;
                }
                if (InterfaceManager.IsMainMenuEnabled() || GameManager.IsOutDoorsScene(GameManager.m_ActiveScene)) //indoor only
                {
                    return;
                }


                AuroraActivatedToggle[] radios = __instance.gameObject.GetComponentsInParent<AuroraActivatedToggle>();
                AuroraScreenDisplay[] screens = __instance.gameObject.GetComponentsInChildren<AuroraScreenDisplay>();

                if (radios.Length == 0 && screens.Length == 0)
                {
                    Light.electroSources.Add(__instance); 
                }

            }
        }


        [HarmonyPatch(typeof(AuroraManager), "UpdateForceAurora")]
        internal class AuroraManager_UpdateForceAurora
        {
            private static void Postfix(AuroraManager __instance)
            {
                if (Light.IsMenu())
                {
                    return;
                }
                if (InterfaceManager.IsMainMenuEnabled() || (GameManager.IsOutDoorsScene(GameManager.m_ActiveScene)))
                {
                    return;
                }

                if (Light.electroSources.Count > 0)
                {
                    Light.UpdateElectroLights(__instance);
                }
            }
        }

        public static bool GetSignBitAsBool(float value)
        {
            uint bits = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
            return ((bits & 0x01) != 0);
        }
        
    }
}
