using System;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class EtherealSpecial : EtherealMount
    {
        public readonly int NorthZ = 27;
        public readonly int RightZ = 22;
        public readonly int EastZ = 22;
        public readonly int DownZ = 24;
        public readonly int SouthZ = 22;
        public readonly int LeftZ = 22;
        public readonly int WestZ = 27;
        public readonly int UpZ = 32;
        //public override int LabelNumber { get { return 1049749; } } // Ethereal Swamp Dragon Statuette
        private Dragon drag;

        public int Move(PlayerMobile from)
        {
            if (drag == null || drag.Deleted)
            {
                drag = new Dragon();
                drag.Frozen = true;
                drag.Blessed = true;
                drag.Direction = from.Direction;
                drag.MoveToWorld(from.Location, from.Map);
            }
            drag.Direction = from.Direction;
            switch (from.Direction)
            {
                case Direction.North:
                    drag.X = from.X - 1;
                    drag.Y = from.Y - 1;
                    return drag.Z + NorthZ;
                    break;
                case Direction.Right:
                    drag.X = from.X;
                    drag.Y = from.Y - 1;
                    return drag.Z + RightZ;
                    break;
                case Direction.East:
                    drag.X = from.X;
                    drag.Y = from.Y;
                    return drag.Z + EastZ;
                    break;
                case Direction.Down:
                    drag.X = from.X;
                    drag.Y = from.Y;
                    return drag.Z + DownZ;
                    break;
                case Direction.South:
                    drag.X = from.X;
                    drag.Y = from.Y;
                    return drag.Z + SouthZ;
                    break;
                case Direction.Left:
                    drag.X = from.X - 1;
                    drag.Y = from.Y;
                    return drag.Z + LeftZ;
                    break;
                case Direction.West:
                    drag.X = from.X - 1;
                    drag.Y = from.Y - 1;
                    return drag.Z + WestZ;
                    break;
                case Direction.Up:
                    drag.X = from.X - 1;
                    drag.Y = from.Y - 1;
                    return drag.Z + EastZ;
                    break;
                default:
                    return 0;
                    break;
            }

        }

        [Constructable]
        public EtherealSpecial() : base( 0x2619, 0x3E00 )
        {
            Name = "a special ethereal";
        }

        public EtherealSpecial(Serial serial) : base( serial )
        {
        }
        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            drag = new Dragon();
            drag.Frozen = true;
            drag.Blessed = true;
            drag.Direction = from.Direction;
            drag.MoveToWorld(from.Location, from.Map);
        }

        public override void OnRemoved(object parent)
        {
            if (parent != null && parent is PlayerMobile && drag != null)
            {
                drag.Delete();
                drag = null;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

        }
    }
}