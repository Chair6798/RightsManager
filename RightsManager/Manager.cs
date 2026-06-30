using System.Collections.Generic;
using System.Collections.ObjectModel;
using Photon.Realtime;

namespace RightsManager
{
    internal static class RM
    {
        static Dictionary<Player, Collection<string>> Rights = new Dictionary<Player, Collection<string>>();

        internal static Collection<string> GetBaseRights()
        {
            var r = new Collection<string>();
            r.Add("valuable_grab");
            r.Add("item_grab");
            r.Add("item_equip");
            r.Add("tumble_start");
            r.Add("tumble_end");
            r.Add("tumble_jump");
            r.Add("tumble_grab");
            r.Add("head_grab");
            r.Add("hinge_grab");
            r.Add("health_lose");
            r.Add("health_get");
            r.Add("health_death");
            return r;
        }
        static Collection<string> Validate(Player p)
        {
            if (!Rights.ContainsKey(p))
            {
                Rights.Add(p, GetBaseRights());
            }
            return Rights[p];
        }
        internal static bool HasRight(Player p, string right)
        {
            return Validate(p).Contains(right);
        }
        internal static void AddRight(Player p, string right)
        {
            if (!Lib.IsAvaiable()) { return; }
            if (HasRight(p, right))
            {
                return;
            }
            Validate(p).Add(right);
        }
        internal static void RemoveRight(Player p, string right)
        {
            if (!Lib.IsAvaiable()) { return; }
            if (!HasRight(p, right))
            {
                return;
            }
            Validate(p).Remove(right);
        }
    }
}
