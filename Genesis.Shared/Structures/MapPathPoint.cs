﻿using System;
using System.IO;

namespace Genesis.Shared.Structures
{
    public struct MapPathPoint
    {
        public Single AcceptDistance;
        public Vector3 Position;
        public UInt64 ReactionCOID;
        public UInt32 WaitTime;

        public static MapPathPoint Read(BinaryReader br)
        {
            var mp = new MapPathPoint
            {
                Position = Vector3.Read(br),
                AcceptDistance = br.ReadSingle(),
                ReactionCOID = br.ReadUInt64(),
                WaitTime = br.ReadUInt32(),
            };

            br.ReadBytes(4);

            return mp;
        }
    }
}
