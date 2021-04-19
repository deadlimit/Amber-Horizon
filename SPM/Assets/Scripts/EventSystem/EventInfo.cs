using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public abstract class EventInfo
    {
        //grundinfo

        public string EventDescription; 
    }
    public class DebugEventInfo : EventInfo
    {
        public int VerbosityLevel;
        new public string EventDescription = "DebugEventInfo:: ";
    }
    public class UnitDeathEventInfo : EventInfo
    {
        public GameObject UnitGO; 
    }

    public class UnitClickedEventInfo : EventInfo
    {

    }
    public class KeyPickUpEvent : EventInfo { }
    public class UnlockEvent : EventInfo { }
}
