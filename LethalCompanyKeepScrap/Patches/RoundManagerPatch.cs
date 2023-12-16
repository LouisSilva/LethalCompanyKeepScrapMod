using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace LethalCompanyKeepScrap.Patches
{
    [HarmonyPatch(typeof(RoundManager))]
    [HarmonyPatch("DespawnPropsAtEndOfRound")]
    internal class DespawnPropsAtEndOfRoundPatch
    {
        static bool Prefix(ref RoundManager __instance, bool despawnAllItems = false)
        {
            if (!__instance.IsServer)
            {
                return false;
            }

            GrabbableObject[] objectsOfType = UnityEngine.Object.FindObjectsOfType<GrabbableObject>();
            for (int index = 0; index < objectsOfType.Length; ++index)
            {
                if (despawnAllItems || !objectsOfType[index].isHeld && !objectsOfType[index].isInShipRoom || objectsOfType[index].deactivated) // In this if statement the extra OR condition: `StartOfRound.Instance.allPlayersDead && objectsOfType[index].itemProperties.isScrap` was removed
                {
                    if (objectsOfType[index].isHeld && (UnityEngine.Object) objectsOfType[index].playerHeldBy != (UnityEngine.Object) null)
                        objectsOfType[index].playerHeldBy.DropAllHeldItems();
                    objectsOfType[index].gameObject.GetComponent<NetworkObject>().Despawn();
                }
                else
                    objectsOfType[index].scrapPersistedThroughRounds = true;
                if (__instance.spawnedSyncedObjects.Contains(objectsOfType[index].gameObject))
                    __instance.spawnedSyncedObjects.Remove(objectsOfType[index].gameObject);
            }

            foreach (UnityEngine.Object @object in GameObject.FindGameObjectsWithTag("TemporaryEffect"))
            {
                UnityEngine.Object.Destroy(@object);
            }

            return false;
        }
    }   
}