﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;

namespace Client
{
    public class Main : BaseScript
    {
        private bool _reticleAllowed = false;

        private List<WeaponHash> _overrideDisable = new List<WeaponHash>()
        {
            WeaponHash.Unarmed, WeaponHash.StunGun, WeaponHash.SniperRifle, WeaponHash.HeavySniper,
            WeaponHash.HeavySniperMk2, WeaponHash.MarksmanRifle,
        };

        public Main()
        {
            Log("Resource loaded.");
            TriggerServerEvent("NoReticle:Server:GetPlayerReticleAceAllowed", Game.Player.Handle);
        }

        [EventHandler("NoReticle:Client:SetPlayerReticleAceAllowed")]
        private void SetPlayerReticleAceAllowed()
        {
            Screen.ShowNotification("Allowing reticle.", true);
            _reticleAllowed = true;
        }

        [Tick]
        private async Task ProcessTask()
        {
            // gets client's current weapon.
            Weapon w = Game.PlayerPed?.Weapons?.Current;

            if (w != null)
            {
                WeaponHash wHash = w.Hash;

                if (!_overrideDisable.Contains(wHash) && API.GetHashKey(wHash.ToString()) != -1783943904) // add MarksmanRifle MKII
                {
                    // if ace perm "Reticle" is not allowed (cannot have reticle) then..
                    if (!_reticleAllowed)
                    {
                        // hides reticle (white dot [HUD] when aiming in).
                        API.HideHudComponentThisFrame(14);
                    }
                }
            }
        }

        /// <summary>
        /// Writes debug message to client's console.
        /// </summary>
        /// <param name="msg">Message to display.</param>
        private void Log(string msg)
        {
            Debug.Write($"[NoReticle] - {msg}");
        }
    }
}
