using Verse;
using static RimRPC.RimRPC;

namespace RimRPC
{
    internal class Details
    {
        public static string GetDetails(string[] args)
        {
            var _colonyname = args[0];
            var _colonistnumber = args[1];
            var _lastEvent = args[2];

            if (RWRPCMod.Settings.RpcCustomTop)
                return RWRPCMod.Settings.RpcCustomTopText;

            if (RWRPCMod.Settings.RpcColony)
            {
                if (RWRPCMod.Settings.RpcColonistCount)
                {
                    return _colonyname + " (" + _colonistnumber + ")";
                }
                return _colonyname;
            }

            if (RWRPCMod.Settings.RpcColonistCount)
            {
                return _colonistnumber + " RPC_Colonists".Translate();
            }

            if (RWRPCMod.Settings.RpcEvents)
                return $" {_lastEvent}";

            return "";
        }
    }
}