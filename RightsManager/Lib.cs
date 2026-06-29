using Photon.Pun;
using Photon.Realtime;
using System.Reflection;
using UnityEngine;
namespace RightsManager
{
    internal static class Lib
    {
        internal static Player GetPlayer(PlayerHealth health)
        {
            try
            {
                return GetPlayer(health.GetComponent<PlayerAvatar>());
            }
            catch
            {
                return null;
            }
        }
        internal static Player GetPlayer(PhysGrabber grabber)
        {
            try
            {
                return GetPlayer(grabber.GetComponent<PlayerAvatar>());
            }
            catch
            {
                return null;
            }
        }
        internal static Player GetPlayer(PlayerTumble tumble)
        {
            try
            {
                return GetPlayer(tumble.transform.parent.GetComponentInChildren<PlayerAvatar>());
            }
            catch
            {
                return null;
            }
        }
        internal static Player GetPlayer(PlayerAvatar avatar)
        {
            try
            {
                return avatar.GetComponent<PhotonView>().Owner;
            }
            catch
            {
                return null;
            }
        }

        internal static PlayerAvatar GetAvatar(Player p)
        {
            if (p == null)
            {
                return null;
            }
            try
            {
                return SemiFunc.PlayerAvatarGetFromPhotonPlayer(p);
            }
            catch
            {
                return null;
            }
        }
        internal static PlayerDeathHead GetHead(Player p)
        {
            try
            {
                var a = GetAvatar(p);
                if (a == null)
                {
                    return null;
                }
                return (PlayerDeathHead)GetValue(a, "playerDeathHead");
            }
            catch
            {
                return null;
            }
        }
        internal static PlayerTumble GetTumble(Player p)
        {
            try
            {
                var a = GetAvatar(p);
                if (a == null)
                {
                    return null;
                }
                return (PlayerTumble)GetValue(a, "tumble");
            }
            catch
            {
                return null;
            }
        }
        internal static void Revive(Player p)
        {
            try
            {
                GetAvatar(p).Revive();
            }
            catch
            {
                return;
            }
        }
        internal static void Death(Player p)
        {
            try
            {
                GetAvatar(p).PlayerDeath(-1);
            }
            catch
            {
                return;
            }
        }
        
        internal static void Teleport(Player p, Vector3 pos)
        {
            try
            {
                var a = GetAvatar(p);
                if (a == null)
                {
                    return;
                }
                if ((bool)GetValue(a, "deadSet"))
                {
                    Transform(GetHead(p).gameObject, pos);
                    return;
                }
                if ((bool)GetValue(a, "isTumbling"))
                {
                    Transform(GetTumble(p).gameObject, pos);
                    return;
                }
                a.Spawn(pos, Quaternion.identity);
            }
            catch
            {
                return;
            }
        }
        internal static void Transform(GameObject go,  Vector3 position)
        {
            Transform(go, position, go.transform.rotation);
        }
        internal static void Transform(GameObject go, Vector3 position, Quaternion rotation)
        {
            if(!SemiFunc.IsMasterClientOrSingleplayer())
            {
                return;
            }
            var ph = go.GetComponent<Rigidbody>();
            if (ph == null)
            {
                ph = go.GetComponentInChildren<Rigidbody>();
                if (ph == null)
                {
                    if (SemiFunc.IsMultiplayer())
                        return;
                    go.transform.position = position;
                    go.transform.rotation = rotation;
                }
            }
            ph.Move(position, rotation);
        }

        internal static bool IsChatOpened()
        {
            return ChatManager.instance != null && (bool)GetValue(ChatManager.instance, "chatActive");
        }
        internal static bool IsAvaiable()
        {
            return SemiFunc.IsMasterClient() && SemiFunc.IsMultiplayer() && !SemiFunc.IsMainMenu() && !SemiFunc.IsSplashScreen();
        }
        internal static bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key)&&!IsChatOpened();
        }

        internal static FieldInfo GetField(object inst, string field)
        {
            return inst.GetType().GetField(field, BindingFlags.NonPublic | BindingFlags.Instance);
        }
        internal static object GetValue(object inst, string field)
        {
            return GetField(inst, field).GetValue(inst);
        }
    }
    
}
