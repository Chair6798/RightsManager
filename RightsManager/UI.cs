using MenuLib;
using MenuLib.MonoBehaviors;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.ObjectModel;
using System.Collections.Generic;
// ✓ ✗
namespace RightsManager
{
    internal class UI : MonoBehaviour
    {
        internal static UI instance;

        internal static REPOPopupPage playerslist;
        internal static REPOPopupPage rightslist;

        internal static Player selectedPlayer;

        internal static Dictionary<string, REPOButton> rb;


        void Awake()
        {
            instance = this;
            playerslist = null;
            rightslist = null;
            selectedPlayer = null;
            rb = new Dictionary<string, REPOButton>();
            MenuAPI.AddElementToEscapeMenu(parent => MenuAPI.CreateREPOButton("Rights", OpenList, parent, new Vector2(200f, 65f)));
        }
        internal void ReloadRights()
        {
            if (rightslist!=null&&rightslist.isActiveAndEnabled)
            {
                rightslist.ClosePage(false);
                Destroy(rightslist);
            }
            rightslist = null;
            OpenRights();
        }
        void OpenRights()
        {
            if (rightslist == null)
            {
                rb= new Dictionary<string, REPOButton>();
                rightslist = MenuAPI.CreateREPOPopupPage("Rights", REPOPopupPage.PresetSide.Right, shouldCachePage: false);
                foreach (string right in RM.GetBaseRights())
                {
                    rightslist.AddElementToScrollView(parent =>
                    {
                        REPOButton b;
                        b = MenuAPI.CreateREPOButton(right + " " + (RM.HasRight(selectedPlayer, right)? "V" : "X"), () =>
                        {
                            if (RM.HasRight(selectedPlayer, right))
                            {
                                RM.RemoveRight(selectedPlayer, right);
                                rb[right].labelTMP.text = right + " " + (RM.HasRight(selectedPlayer, right) ? "V" : "X");
                            }
                            else
                            {
                                RM.AddRight(selectedPlayer, right);
                                rb[right].labelTMP.text = right + " " + (RM.HasRight(selectedPlayer, right) ? "V" : "X");
                            }
                        }, parent);
                        rb.Add(right, b);
                        return b.rectTransform;
                    });
                }
                rightslist.OpenPage(true);
                return;
            }
            if (!rightslist.isActiveAndEnabled)
            {
                rightslist.OpenPage(true);
                return;
            }

            
        }
        internal void OpenList()
        {
            if (!Lib.IsAvaiable()) { return; }
            if (playerslist!=null && playerslist.isActiveAndEnabled)
            {
                playerslist.ClosePage(false);
                Destroy(playerslist);
            }
            playerslist = MenuAPI.CreateREPOPopupPage("Players", REPOPopupPage.PresetSide.Left, shouldCachePage: false);
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                playerslist.AddElementToScrollView(parent =>
                {
                    var b = MenuAPI.CreateREPOButton(p.NickName, () =>
                    {
                        selectedPlayer = p;
                        ReloadRights();
                    }, parent);
                    return b.rectTransform;
                });
            }
            playerslist.OpenPage(true);
        }
        void Update()
        {
            if (!Lib.IsAvaiable()) { return; }
            if (Lib.GetKeyDown(Loader.menuKey.Value))
            {
                OpenList();
            }
        }
    }
}
