using System;
using System.Collections.Generic;
using System.IO;
using Nerdbank.MessagePack;

namespace GLD.SerializerBenchmark.Serializers
{
    /// <summary>
    /// Nerdbank.MessagePack with reflection-based shapes and stable numeric
    /// member keys on the suite POCOs.
    /// https://github.com/AArnott/Nerdbank.MessagePack
    /// </summary>
    internal class NerdbankMessagePackSerializerSer : SerDeser
    {
        private static readonly MessagePackSerializer Serializer = new();

        public override string Name => "Nerdbank.MessagePack";

        public override string Serialize<T>(T serializable)
            => Convert.ToBase64String(Serializer.Serialize(serializable));

        public override object Deserialize<T>(string serialized)
            => Serializer.Deserialize<T>(Convert.FromBase64String(serialized));

        public override void Serialize<T>(T serializable, Stream outputStream)
            => Serializer.Serialize(outputStream, serializable);

        public override object Deserialize<T>(Stream inputStream)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
            return Serializer.Deserialize<T>(inputStream);
        }
    }
}
