﻿using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace HANDMod.Content.HANDSurvivor.Components.Body
{
    public class HANDNetworkComponent : NetworkBehaviour
    {
        private CharacterBody characterBody;
        public void Awake()
        {
            characterBody = base.GetComponent<CharacterBody>();
        }

        [Server]
        public void ResetSpecialStock()
        {
            if (NetworkServer.active)
            {
                RpcResetSpecialStock();
            }
        }

        [ClientRpc]
        private void RpcResetSpecialStock()
        {
            characterBody.skillLocator.special.stock = 0;
        }

        /*[Client]
        public void StopMomentum(uint networkID)
        {
            if (this.hasAuthority)
            {
                CmdStopMomentum(networkID);
            }
        }

        [Command]
        private void CmdStopMomentum(uint networkID)
        {
            GameObject go = ClientScene.FindLocalObject(new NetworkInstanceId(networkID));
            if (go)
            {
                CharacterMaster cm = go.GetComponent<CharacterMaster>();
                if (cm)
                {
                    GameObject bodyObject = cm.GetBodyObject();
                    if (bodyObject)
                    {
                        CharacterBody cb = bodyObject.GetComponent<CharacterBody>();
                        if (cb)
                        {
                            if (cb.rigidbody)
                            {
                                cb.rigidbody.velocity = new Vector3(0f, cb.rigidbody.velocity.y, 0f);
                                cb.rigidbody.angularVelocity = new Vector3(0f, cb.rigidbody.angularVelocity.y, 0f);
                            }
                            if (cb.characterMotor != null)
                            {
                                cb.characterMotor.velocity.x = 0f;
                                cb.characterMotor.velocity.z = 0f;
                                cb.characterMotor.rootMotion.x = 0f;
                                cb.characterMotor.rootMotion.z = 0f;
                            }
                        }
                    }
                }
            }
        }*/

        [Client]
        public void SquashEnemy(uint networkID)
        {
            if (this.hasAuthority)
            {
                CmdAddSquash(networkID);
            }
        }

        [Command]
        private void CmdAddSquash(uint networkID)
        {
            RpcAddSquash(networkID);
        }

        [ClientRpc]
        private void RpcAddSquash(uint networkID)
        {
            GameObject go = ClientScene.FindLocalObject(new NetworkInstanceId(networkID));
            if (go)
            {
                CharacterMaster cm = go.GetComponent<CharacterMaster>();
                if (cm)
                {
                    GameObject bodyObject = cm.GetBodyObject();
                    if (bodyObject)
                    {
                        SquashedComponent sq = bodyObject.GetComponent<SquashedComponent>();
                        if (sq)
                        {
                            sq.ResetGraceTimer();
                        }
                        else
                        {
                            bodyObject.AddComponent<SquashedComponent>();
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            Util.PlaySound("Play_MULT_shift_end", base.gameObject);
        }
    }
}