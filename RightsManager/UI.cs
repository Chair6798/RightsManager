using MenuLib;
using MenuLib.MonoBehaviors;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
namespace RightsManager
{
    internal class UI : MonoBehaviour
    {
        REPOPopupPage playerslist;
        REPOPopupPage rightslist;

        Player selectedPlayer;

        void Awake()
        {
            MenuAPI.AddElementToLobbyMenu(parent => {
                var repoButton = MenuAPI.CreateREPOButton("Mods", , parent, new Vector2(200f, 60f));
                repoButton.labelTMP.fontSize = 28;
            });
        }
        void ReloadRights()
        {
            if (rightslist!=null&&rightslist.isActiveAndEnabled)
            {
                rightslist.ClosePage(false);
            }
            rightslist = null;
        }
        void OpenRights()
        {
            if (rightslist == null)
            {
                rightslist = MenuAPI.CreateREPOPopupPage("Rights", REPOPopupPage.PresetSide.Right, shouldCachePage: false);
                foreach (string right in RM.GetBaseRights())
                {

                }
                rightslist.OpenPage(true);
                return;
            }
            if (!rightslist.isActiveAndEnabled)
            {
                rightslist.OpenPage(false);
                return;
            }

            
        }
        void OpenList()
        {
            if (!Lib.IsAvaiable()) { return; }
            playerslist = MenuAPI.CreateREPOPopupPage("Players", REPOPopupPage.PresetSide.Left, shouldCachePage: false);
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                playerslist.AddElementToScrollView(parent =>
                {
                    var b = MenuAPI.CreateREPOButton(p.NickName, () =>
                    {
                        selectedPlayer = p;
                    }, parent);
                    return b.rectTransform;
                });
            }
        }
        void Update()
        {
            if (!Lib.IsAvaiable()) { return; }
            if (Lib.GetKeyDown(Loader.menuKey.Value))
            {

            }
        }
    }
}
