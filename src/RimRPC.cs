using HarmonyLib;
using RimWorld;
using System;
using System.Reflection;
using System.Security.Cryptography;
using Verse;

namespace RimRPC
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            var femboyfoxes = new Harmony("weilbyte.rimworld.rimrpc");

            MethodInfo targetmethod = AccessTools.Method(typeof(MainMenuDrawer), nameof(MainMenuDrawer.MainMenuOnGUI));
            HarmonyMethod postfixmethod = new HarmonyMethod(typeof(RimRPC), "GoToMainMenu_Postfix");
            MethodInfo eventtarget = AccessTools.Method(typeof(IncidentWorker), "TryExecute");
            HarmonyMethod eventwatch = new HarmonyMethod(typeof(RimRPC), "Event_overwatch");

            
            femboyfoxes.Patch(targetmethod, null, postfixmethod);
            femboyfoxes.Patch(eventtarget, null, eventwatch);

            RimRPC.BootMeUp();
        }
    }

    public class RimRPC
    {
        internal static DiscordRPC.RichPresence Presence;
        internal static string Colony;
        internal static int onDay;
        internal static long Started = (DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1).Ticks) / TimeSpan.TicksPerSecond;


        public static void BootMeUp()
        {
            DiscordRPC.EventHandlers eventHandlers = default;
            
            eventHandlers.ReadyCallback = (DiscordRPC.ReadyCallback)Delegate.Combine(eventHandlers.ReadyCallback, new DiscordRPC.ReadyCallback(ReadyCallback));
            eventHandlers.DisconnectedCallback = (DiscordRPC.DisconnectedCallback)Delegate.Combine(eventHandlers.DisconnectedCallback, new DiscordRPC.DisconnectedCallback(DisconnectedCallback));
            eventHandlers.ErrorCallback = (DiscordRPC.ErrorCallback)Delegate.Combine(eventHandlers.ErrorCallback, new DiscordRPC.ErrorCallback(ErrorCallback));
            eventHandlers.JoinCallback = (DiscordRPC.JoinCallback)Delegate.Combine(eventHandlers.JoinCallback, new DiscordRPC.JoinCallback(JoinCallback));
            eventHandlers.SpectateCallback = (DiscordRPC.SpectateCallback)Delegate.Combine(eventHandlers.SpectateCallback, new DiscordRPC.SpectateCallback(SpectateCallback));
            eventHandlers.RequestCallback = (DiscordRPC.RequestCallback)Delegate.Combine(eventHandlers.RequestCallback, new DiscordRPC.RequestCallback(RequestCallback));

            DiscordRPC.Initialize("1483917459240128724", ref eventHandlers, true, "0612");

            Presence = default;
            Presence.LargeImageKey = "logo";
            Presence.State = "Loading";
            
            DiscordRPC.UpdatePresence(ref Presence);
            ReadyCallback();
        }

        private static void RequestCallback(DiscordRPC.JoinRequest request)
        {
        }

        private static void SpectateCallback(string secret)
        {
        }

        private static void JoinCallback(string secret)
        {
        }

        private static void ErrorCallback(int errorCode, string message)
        {
            Log.Message("RichPresence :: Oopsie woopsie. We made a wittle fucky wucky!");
            Log.Message("RichPresence :: ErrorCallback: " + errorCode + " " + message);
        }

        private static void DisconnectedCallback(int errorCode, string message)
        {
            Log.Message("RichPresence :: Oopsie woopsie. We made a wittle fucky wucky!");
            Log.Message("RichPresence :: DisconnectedCallback: " + errorCode + " " + message);
        }

        public static class GlobalRpc
        {
            public static string lastEvent;
        }

        private static void ReadyCallback()
        {
            Log.Message("RichPresence :: Running");
        }

        public static void GoToMainMenu_Postfix()
        {
            //Log.Message("RichPresence :: Attempted MainMenu_Postfix");
            StateHandler.MenuState();
        }

        public static void Event_overwatch(IncidentWorker __instance, IncidentParms parms, bool __result)
        {
            if(__result)
            {
                //Log.Message($" > {__instance.def.label}");
                GlobalRpc.lastEvent = __instance.def.label;
            }
        }
    }
}