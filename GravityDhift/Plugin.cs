using BepInEx;
using System;
using UnityEngine;
using Utilla;
using Utilla.Models;

namespace GravityShift
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode("GRAVITYSHIFT", "GRAVITY SHIFT", BaseGamemode.Infection)]
    [ModdedGamemode("GRAVITYSHIFTEXTREME", "GRAVITY SHIFT X", BaseGamemode.Infection)]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void OnGameInitialized(object sender, EventArgs e)
        {
            new GameObject("BannedModsChecker").AddComponent<BannedModsChecker>();
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            if (gamemode.Contains("GRAVITYSHIFTEXTREME"))
            {
                if (GravityManager.instance == null) GravityManager.instance = new GameObject("GravityManager").AddComponent<GravityManager>();
                else GravityManager.instance.enabled = true;
                GravityManager.instance.maxY = 0f;
                GravityManager.instance.minY = -2f;
                GravityManager.instance.lerpSpeed = 0.5f;
                Debug.Log("Joined Extreme Gravity Shift");
            }
            else if (gamemode.Contains("GRAVITYSHIFT"))
            {
                if (GravityManager.instance == null) GravityManager.instance = new GameObject("GravityManager").AddComponent<GravityManager>();
                else GravityManager.instance.enabled = true;
                GravityManager.instance.maxY = -2f;
                GravityManager.instance.minY = -3.5f;
                GravityManager.instance.lerpSpeed = 0.1f;
                Debug.Log("Joined Gravity Shift");
            }
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            GravityManager.instance.enabled = false;
            Debug.Log("Left Gravity Shift");
        }
    }
}
