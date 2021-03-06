﻿using System.Collections.Generic;
using System.IO;

namespace Genesis.Shared.Entities
{
    using Structures;
    using Utils.Extensions;

    public class OutPost : SimpleObject
    {
        public float CaptureRadius;
        public bool IsOutpost;
        public Vector4 Location;
        public string OutPostName;
        public List<OutpostInformation> OutpostInformations = new List<OutpostInformation>();
        public float OutpostXPScalar;
        public uint VarTotalBeacons;
        public uint Unkvalue;

        public override void Unserialize(BinaryReader br, uint mapVersion)
        {
            Location = Vector4.Read(br);
            Unkvalue = br.ReadUInt32();
            OutPostName = br.ReadUtf8StringOn(65);
            OutpostXPScalar = br.ReadSingle();

            if (mapVersion >= 56)
            {
                CaptureRadius = br.ReadSingle();
                VarTotalBeacons = br.ReadUInt32();
            }

            if (mapVersion >= 59)
                IsOutpost = br.ReadBoolean();

            for (var j = 0; ; )
            {
                var oinfo = new OutpostInformation
                {
                    BeaconVar = br.ReadUInt32(),
                    Spawns = new List<long>(),
                    Objects = new List<long>(),
                    OutpostSkills = new List<OutpostSkill>(),
                    Reactions = new List<long>()
                };

                var spawnCount = br.ReadInt32();
                for (var i = 0; i < spawnCount; ++i)
                    oinfo.Spawns.Add(br.ReadInt64());

                var objectCount = br.ReadInt32();
                for (var i = 0; i < objectCount; ++i)
                    oinfo.Objects.Add(br.ReadInt64());

                var skillCount = br.ReadInt32();
                for (var i = 0; i < skillCount; ++i)
                    oinfo.OutpostSkills.Add(OutpostSkill.Read(br));

                if (mapVersion >= 58)
                {
                    var reactionCount = br.ReadUInt32();
                    for (var i = 0; i < reactionCount; ++i)
                        oinfo.Reactions.Add(br.ReadInt64());
                }

                OutpostInformations.Add(oinfo);

                if (mapVersion >= 57 || j < 2)
                    if (++j < 4)
                        continue;

                break;
            }
        }
    }
}
