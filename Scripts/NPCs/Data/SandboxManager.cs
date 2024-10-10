using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class SandboxManager : MonoBehaviour
{
    List<IdleMarker> IdleMarkers = new List<IdleMarker>();
    List<NPC> Characters = new List<NPC>();

    public static SandboxManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void RegisterNPC(NPC npc)
    {
        if (Characters.Contains(npc))
        {
            return;
        }
        else
        {
            Characters.Add(npc);
        }
    }

    public void RegisterIdleMarker(IdleMarker marker)
    {
        if (IdleMarkers.Contains(marker))
        {
            return;
        }
        else
        {
            IdleMarkers.Add(marker);
        }
    }

    //Returns an Idle Marker for the character to interact with.
    public IdleMarker GetAvailableIdleMarker()
    {
        IdleMarker[] availableMarkers = new IdleMarker[IdleMarkers.Count];
        int markersNum = 0;
        
        // Iterate over all markers and grab available ones.
        foreach(IdleMarker data in IdleMarkers) 
        {
            if (data.IsAvailable && data.IsEverUsable)
            {
                availableMarkers[markersNum] = data;
                ++markersNum;
            }
        }

        //If there are any available, return a random one.
        if (availableMarkers.Length > 0)
        {
            int index = Random.Range(0, availableMarkers.Length);
            return availableMarkers[index];
        }
        
        return null;
    }

    public IdleMarker GetAvailableIdleMarkerInRange(float range, Vector3 origin)
    {
        foreach (IdleMarker marker in IdleMarkers)
        {
            if ((marker.transform.position - origin).magnitude < range)
            {
                if (marker.IsAvailable && marker.IsEverUsable) 
                {
                    marker.IsAvailable = false;
                    return marker;
                }
            }
        }
        
        return null;
    }
    
    //Overload of GetAvailableIdleMarker which will return only markers that match the character's Identity.
    public IdleMarker GetAvailableIdleMarker(CharacterIdentity CharacterMask, bool FallBackToAny = false) 
    {
        //TODO: randomise
        foreach (IdleMarker data in IdleMarkers)
        {
            if (data.IsAvailable && data.IsEverUsable)
            {
                if (data.IdentityMask.HasFlag(CharacterMask))
                {
                    data.IsAvailable = false;
                    return data;
                }
            }
        }

        if (FallBackToAny) 
        {
            return GetAvailableIdleMarker();
        }

        return null;
    }


    public void MakeAllIdlesAvailable()
    {
        foreach (IdleMarker data in IdleMarkers)
        {
            data.IsAvailable = true;
        }
    }
}

