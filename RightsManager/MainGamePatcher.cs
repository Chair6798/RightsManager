using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;


namespace RightsManager
{
    [HarmonyPatch(typeof(PlayerHealth))]
    internal static class PlayerHealthPatch
    {
        [HarmonyPatch("UpdateHealthRPC")]
        [HarmonyPrefix]
        private static bool UpdateHealthRPC_Prefix(PlayerHealth __instance, int healthNew, int healthMax, bool effect, bool hurtByHeal, PhotonMessageInfo _info = default(PhotonMessageInfo))
        {
            Player p = Lib.GetPlayer(__instance);
            if (p==null)
            {
                return true;
            }
            if (!RM.HasRight(p, "health_lose")&&((int)Lib.GetValue(__instance, "health") > healthNew))
            {
                __instance.GetComponent<PhotonView>().RPC("UpdateHealthRPC", RpcTarget.Others, (int)Lib.GetValue(__instance, "health"), healthMax, false, false);
                return false;
            }
            if (!RM.HasRight(p, "health_get") && ((int)Lib.GetValue(__instance, "health") < healthNew))
            {
                __instance.GetComponent<PhotonView>().RPC("UpdateHealthRPC", RpcTarget.Others, (int)Lib.GetValue(__instance, "health"), healthMax, false, false);
                return false;
            }
            return true;
        }
        [HarmonyPatch("HurtOther")]
        [HarmonyPrefix]
        private static bool HurtOther_Prefix(PlayerHealth __instance, int damage, Vector3 hurtPosition, bool savingGrace, int enemyIndex = -1, bool hurtByHeal = false)
        {
            Player p = Lib.GetPlayer(__instance);
            if (p == null)
            {
                return true;
            }
            if (!RM.HasRight(p, "health_lose"))
            {
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(PlayerAvatar))]
    internal static class PlayerAvatarPatch
    {
        [HarmonyPatch("PlayerDeath")]
        [HarmonyPrefix]
        private static bool PlayerDeath_Prefix(PlayerAvatar __instance)
        {
            Player p = Lib.GetPlayer(__instance);
            if (p == null)
            {
                return true;
            }
            if (!RM.HasRight(p, "health_death"))
            {
                return false;
            }
            return true;
        }
        [HarmonyPatch("PlayerDeathRPC")]
        [HarmonyPrefix]
        private static bool PlayerDeathRPC_Prefix(PlayerAvatar __instance)
        {
            Player p = Lib.GetPlayer(__instance);
            if (p == null)
            {
                return true;
            }
            if (!RM.HasRight(p, "health_death"))
            {

                return true;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(ItemEquippable))]
    internal static class ItemEquippablePatch
    {
        [HarmonyPatch("RPC_RequestEquip")]
        [HarmonyPrefix]
        private static bool RPC_RequestEquip_Prefix(ItemEquippable __instance, int physGrabberPhotonViewID)
        {
            var p = PhotonView.Find(physGrabberPhotonViewID).Owner;
            try 
            { 
                return RM.HasRight(p, "item_equip");
            }
            catch
            {
                return true;
            }
            
        }
    }
    [HarmonyPatch(typeof(PhysGrabObject))]
    internal static class PhysGrabObjectPatch
    {
        [HarmonyPatch("GrabStartedRPC")]
        [HarmonyPrefix]
        private static bool GrabStartedRPC_Prefix(PhysGrabObject __instance, int playerPhotonID)
        {
            if (__instance.GetComponent<ValuableObject>()!=null && !RM.HasRight(PhotonView.Find(playerPhotonID).Owner, "valuable_grab"))
            {
                return false;
            }
            if (__instance.GetComponent<ItemAttributes>() != null && !RM.HasRight(PhotonView.Find(playerPhotonID).Owner, "item_grab"))
            {
                return false;
            }
            if (__instance.GetComponent<PlayerTumble>() != null && !RM.HasRight(PhotonView.Find(playerPhotonID).Owner, "tumble_grab"))
            {
                return false;
            }
            if (__instance.GetComponent<PlayerDeathHead>() != null && !RM.HasRight(PhotonView.Find(playerPhotonID).Owner, "head_grab"))
            {
                return false;
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(GameplayManager))]
    internal static class GameplayManagerPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void Start_Postfix(GameplayManager __instance)
        {
            __instance.gameObject.AddComponent<UI>();
        }
    }
}
//[HarmonyPatch("")]
//[HarmonyPrefix]
//private static bool _Prefix
//PlayerDeathRPC