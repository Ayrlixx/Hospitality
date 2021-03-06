using RimWorld;
using Verse;
using Verse.AI;

namespace Hospitality
{
    public class ThinkNode_FilterGuestRooms : ThinkNode_Priority
    {
        private static RoomRoleDef kitchen;
        private static RoomRoleDef guestroom;
        private static RoomRoleDef room;

        public override void ResolveReferences()
        {
            kitchen = DefDatabase<RoomRoleDef>.GetNamed("Kitchen");
            guestroom = DefDatabase<RoomRoleDef>.GetNamed("GuestRoom");
            room = DefDatabase<RoomRoleDef>.GetNamed("Room");
            base.ResolveReferences();
        }

        public override ThinkResult TryIssueJobPackage(Pawn pawn)
        {
            ThinkResult result = base.TryIssueJobPackage(pawn);
            if (result.IsValid)
            {
                if(IsForbidden(result)) return ThinkResult.NoJob;

                Map map = pawn.MapHeld;

                // Order doesn't matter here
                if (result.Job.targetA.Cell.GetRoom(map) == null) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == RoomRoleDefOf.DiningRoom) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == RoomRoleDefOf.Hospital) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == RoomRoleDefOf.RecRoom) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == RoomRoleDefOf.None) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == RoomRoleDefOf.Laboratory) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == RoomRoleDefOf.Barracks) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == guestroom) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == kitchen) return result;
                if (result.Job.targetA.Cell.GetRoom(map).Role == room) return result;
            }
            return ThinkResult.NoJob;
        }

        private static bool IsForbidden(ThinkResult result)
        {
            if (!result.Job.targetA.HasThing) return false;
            return result.Job.targetA.Thing.IsForbidden(Faction.OfPlayer);
        }
    }
}