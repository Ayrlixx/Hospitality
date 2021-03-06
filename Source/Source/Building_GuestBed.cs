using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace Hospitality
{
    public class Building_GuestBed : Building_Bed
    {
        //public new Pawn owner;

        private static readonly Color guestFieldColor = new Color(170/255f, 79/255f, 255/255f);

        private static readonly Color sheetColorForGuests = new Color(89/255f, 55/255f, 121/255f);

        private static readonly List<IntVec3> guestField = new List<IntVec3>();

        public Pawn CurOccupant
        {
            get
            {
                var list = Map.thingGrid.ThingsListAt(Position);
                return list.OfType<Pawn>()
                    .Where(pawn => pawn.jobs.curJob != null)
                    .FirstOrDefault(pawn => pawn.jobs.curJob.def == JobDefOf.LayDown && pawn.jobs.curJob.targetA.Thing == this);
            }
        }

        public override Color DrawColor
        {
            get
            {
                if (def.MadeFromStuff)
                {
                    return base.DrawColor;
                }
                return DrawColorTwo;
            }
        }

        public override void Draw()
        {
            base.Draw();
            if (ForPrisoners) ForPrisoners = false;
        }

        public override Color DrawColorTwo { get { return sheetColorForGuests; } }

        public override void DeSpawn()
        {
            foreach (var owner in owners.ToArray())
            {
                owner.ownership.UnclaimBed();
            }
            var room = Position.GetRoom(Map);
            base.DeSpawn();
            if (room != null)
            {
                room.RoomChanged();
            }
        }

        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
            var room = this.GetRoom();
            if (room == null) return;
            if (room.isPrisonCell) return;

            if (room.RegionCount < 20 && !room.TouchesMapEdge)
            {
                foreach (var current in room.Cells)
                {
                    guestField.Add(current);
                }
                var color = guestFieldColor;
                color.a = Pulser.PulseBrightness(1f, 0.6f);
                GenDraw.DrawFieldEdges(guestField, color);
                guestField.Clear();
            }
        }

        public override string GetInspectString()
        {
            var stringBuilder = new StringBuilder();
            //stringBuilder.Append(base.GetInspectString());
            stringBuilder.Append(InspectStringPartsFromComps());
            //stringBuilder.AppendLine();
            stringBuilder.Append("ForGuestUse".Translate());
            {
                stringBuilder.AppendLine();
                if (owners.Count == 0)
                {
                    stringBuilder.AppendLine("Owner".Translate() + ": " + "Nobody".Translate());
                }
                else if (owners.Count == 1)
                {
                    stringBuilder.AppendLine("Owner".Translate() + ": " + owners[0].LabelCap);
                }
                else
                {
                    stringBuilder.Append("Owners".Translate() + ": ");
                    bool notFirst = false;
                    foreach (Pawn owner in owners) {
                        if (notFirst)
                        {
                            stringBuilder.Append(", ");
                        }
                        notFirst = true;
                        stringBuilder.Append(owner.Label);
                    }
                    stringBuilder.AppendLine();
                }
            }
            return stringBuilder.ToString();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            // Dummy for replacement
            yield break;
        }

        public override void PostMake()
        {
            base.PostMake();
            PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDef.Named("GuestBeds"), KnowledgeAmount.Total);
        }

        public override void DrawGUIOverlay()
        {
            //if (Find.CameraMap.CurrentZoom == CameraZoomRange.Closest)
            //{
            //    if (owner != null && owner.InBed() && owner.CurrentBed().owner == owner)
            //    {
            //        return;
            //    }
            //    string text;
            //    if (owner != null)
            //    {
            //        text = owner.NameStringShort;
            //    }
            //    else
            //    {
            //        text = "Unowned".Translate();
            //    }
            //    GenWorldUI.DrawThingLabel(this, text, new Color(1f, 1f, 1f, 0.75f));
            //}
        }
    }
}
