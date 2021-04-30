using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EventCallbacks
{
    public static class EventSystem<EventType> where EventType : EventInfo {
        
        private static Dictionary<Type, Action<EventType>> typeEventListeners;

        static EventSystem() {
            typeEventListeners = new Dictionary<Type, Action<EventType>>();
        }
        
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
            //om dictionairy är tom, lägg till ett keyset med typeof(T), List<EventListener>>()
            if (typeEventListeners == null)
            {
                typeEventListeners = new Dictionary<Type, Action<EventType>>();
            }
            //om vi inte har en kategori för listeners av denna typ, eller om kategorin inte har något V, skapa en ny lista för kategorin
            if (typeEventListeners.ContainsKey(typeof(EventType)) == false)
            {
                typeEventListeners.Add(typeof(EventType), null);
            }
            //Tog bort checken nedan för den försökte lägga till en nyckel som redan existerade, om nyckeln finns 
            //så kommer null instansieras till en ny Action
            // || typeEventListeners[typeof(EventType)] == null
            
            /* RegisterType() kollar om dictionaryn är null och i så fall initializerar den, 
             * kollar om det finns en nyckel med typ TEvent och om den inte finns ligger till en tom entry. 
             * FireEvent () tar in en TEvent och anropar .Invoke() för den. 
             * Sedan behöver man hantera hur man  registerar och unregistrerar events. */
        }

        
        public static void FireEvent(EventType eventInfo)
        {
            if (typeEventListeners == null || !typeEventListeners.ContainsKey(typeof(EventInfo)))
            {
                return;
            }
            typeEventListeners[typeof(EventType)]?.Invoke(eventInfo);
        }

        public static void UnregisterAllListeners() {
            Debug.Log(typeEventListeners.GetHashCode());
        }
    }
}
