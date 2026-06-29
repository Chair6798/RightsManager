using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightsManager
{
    [HarmonyPatch(typeof(ChatManager))]
    internal class ChatManagerPatch
    {
        [HarmonyPatch("MessageSend")]
        [HarmonyPrefix]
        private static bool MessageSend_Prefix(ChatManager __instance, ref string ___chatMessage)
        {
            var args = ___chatMessage.Replace("<b>|</b>", string.Empty).ToLower().Split(' ');
            switch (args[0])
            {
                case "!rm":
                    UI.instance.OpenList();
                    return false;
            }
            return true;
        }
    }
}
