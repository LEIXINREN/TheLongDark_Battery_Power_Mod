using Il2Cpp;
using Il2CppTLD.ModularElectrolizer;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Il2Cpp.DarkLightingManager;

namespace TLDTestMod
{

    class Light
    {
        public static List<AuroraModularElectrolizer> electroSources = new List<AuroraModularElectrolizer>();
        private static HashSet<GearItem> activeBatteriesList = new HashSet<GearItem>();

        internal static bool IsMenu()
        {
            if (GameManager.m_ActiveScene != null)
            {
                if (GameManager.m_ActiveScene.Contains("MainMenu") || InterfaceManager.IsMainMenuEnabled())
                {
                    return true;
                }
            }

            return false;

        }




        internal static void UpdateElectroLights(AuroraManager mngr)
        {

            if (mngr.AuroraIsActive())
            {
                for (int e = 0; e < electroSources.Count; e++)
                {
                    electroSources[e].UpdateIntensity(Time.deltaTime, mngr.m_NormalizedActive);
                }
            }
            else
            {

                List<GearItem> carBatteries = new List<GearItem>();
                HashSet<GearItem> ActiveCarBatteriesTemp = new HashSet<GearItem>();
                foreach (var gear in GearManager.m_Gear)
                {
                    if (gear != null && gear.name == "GEAR_CarBattery")
                    {
                        carBatteries.Add(gear);
                    }
                }

                for (int e = 0; e < electroSources.Count; e++)
                {
                    GearItem bat = null;

                    if (electroSources[e] != null && electroSources[e].m_LocalLights != null)
                    {
                        float distance = 0f;
                        foreach (var battery in carBatteries)
                        {
                            distance = Mathf.Abs(Vector3.Distance(electroSources[e].gameObject.transform.position, battery.transform.position));
                            if (distance < 3 && distance > 0)
                            {
                                bat = battery;
                                break;
                            }
                        }

                        if (bat == null)
                        {
                            electroSources[e].UpdateIntensity(1f, 0f);
                            electroSources[e].UpdateLight(true);
                            electroSources[e].UpdateEmissiveObjects(true);
                            electroSources[e].UpdateAudio();

                            continue;
                        }
                        else if (bat.m_CurrentHP < 1 || distance > 3)
                        {
                            bat.CurrentHP = ModifyFloatBits(bat.m_CurrentHP, false);//加入這個

                            electroSources[e].UpdateIntensity(1f, 0f);
                            electroSources[e].UpdateLight(true);
                            electroSources[e].UpdateEmissiveObjects(true);
                            electroSources[e].UpdateAudio();
                            continue;
                        }
                        else
                        {
                            if (!electroSources[e].gameObject.name.Contains("Alarm") &&
                                !electroSources[e].gameObject.name.Contains("Headlight") &&
                                !electroSources[e].gameObject.name.Contains("Taillight") &&
                                !electroSources[e].gameObject.name.Contains("Television") &&
                                !electroSources[e].gameObject.name.Contains("Computer") &&
                                !electroSources[e].gameObject.name.Contains("Machine") &&
                                !electroSources[e].gameObject.name.Contains("ControlBox") &&
                                !electroSources[e].gameObject.name.Contains("Interiorlight"))
                            {
                                electroSources[e].UpdateIntensity(1f, 3f);
                                electroSources[e].UpdateLight(false);
                                electroSources[e].UpdateEmissiveObjects(false);

                                if (true)
                                    electroSources[e].UpdateAudio();
                                else
                                    electroSources[e].StopAudio();

                                bat.CurrentHP = ModifyFloatBits(bat.m_CurrentHP, true);//加入這個
                                ActiveCarBatteriesTemp.Add(bat);
                            }


                        }

                    }

                }

                foreach (var bat in activeBatteriesList)
                {
                    if (bat != null && !ActiveCarBatteriesTemp.Contains(bat))
                    {
                        bat.CurrentHP = ModifyFloatBits(bat.m_CurrentHP, false);
                    }
                }
                activeBatteriesList = ActiveCarBatteriesTemp;

            }

        }

        public static float ModifyFloatBits(float originalValue, bool load)//加入這個
        {
            uint bits = BitConverter.ToUInt32(BitConverter.GetBytes(originalValue), 0);

            if (load)
                bits |= 0x00000001;
            else
                bits &= 0xFFFFFFFE;

            return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0);
        }
        
    }
}
