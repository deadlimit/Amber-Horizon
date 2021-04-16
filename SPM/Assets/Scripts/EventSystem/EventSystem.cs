using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventCallbacks
{
    public static class EventSystem<EventType> where EventType : EventInfo
    {
        private static Dictionary<Type, Action<EventType>> typeEventListeners;

        public static void RegisterListener(Action<EventType> listener)
        {
            RegisterType();
            typeEventListeners[typeof(EventType)] += listener;
        }
        public static void UnregisterListener(Action<EventType> listener)
        {
            typeEventListeners[typeof(EventType)] -= listener;
        }

        private static void RegisterType() 
        {
            //om dictionairy �r tom, l�gg till ett keyset med typeof(T), List<EventListener>>()
            if (typeEventListeners == null)
            {
                typeEventListeners = new Dictionary<Type, Action<EventType>>();
            }
            //om vi inte har en kategori f�r listeners av denna typ, eller om kategorin inte har n�got V, skapa en ny lista f�r kategorin
            if (typeEventListeners.ContainsKey(typeof(EventType)) == false || typeEventListeners[typeof(EventType)] == null)
            {
                typeEventListeners.Add(typeof(EventType), null);
            }
            /* RegisterType() kollar om dictionaryn �r null och i s� fall initializerar den, 
             * kollar om det finns en nyckel med typ TEvent och om den inte finns ligger till en tom entry. 
             * FireEvent () tar in en TEvent och anropar .Invoke() f�r den. 
             * Sedan beh�ver man hantera hur man  registerar och unregistrerar events. */
        }

        
        public static void FireEvent(EventType eventInfo)
        {
            if (typeEventListeners == null)
            {
                return;
            }
            typeEventListeners[typeof(EventType)]?.Invoke(eventInfo);
        }
    }
}
