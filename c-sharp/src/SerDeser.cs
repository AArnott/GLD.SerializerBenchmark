using System;
using System.Collections.Generic;
using System.IO;
using PolyType;

namespace GLD.SerializerBenchmark
{
    public abstract class SerDeser : ISerDeser
    {
        protected Type _primaryType;
        protected List<Type> _secondaryTypes;
        public bool JustInitialized;

        public abstract string Name { get; }

        /// <summary>Installed package version for CSV (resolved from the library assembly).</summary>
        public virtual string Version => SerializerVersionRegistry.Resolve(Name);

        /// <summary>
        /// Stream-path honesty for CSV StreamMode (B-6). Default native (real Stream API).
        /// Override or use <see cref="StreamModeHelper"/> for adapted / text_on_stream codecs.
        /// </summary>
        public virtual string StreamMode => StreamModeHelper.Resolve(Name);

        public virtual bool Supports(string testDataName) => true;

        /// <summary>Run-config type id for this fixture. table and table_project share a CLR type.</summary>
        public string FixtureName { get; private set; } = "";

        public virtual void BindFixture(string testDataName)
        {
            FixtureName = testDataName ?? "";
        }

        protected bool IsTableProject =>
            string.Equals(FixtureName, "table_project", StringComparison.Ordinal);

        public virtual void Initialize(Type serializablePrimaryType, List<Type> serializableSecondaryTypes = null)
        {
            _primaryType = serializablePrimaryType;
            _secondaryTypes = serializableSecondaryTypes;
            JustInitialized = true;
        }

        /// <summary>
        /// Untimed: convert suite fixtures to library-native forms / cache formatters.
        /// Called once per fixture after <see cref="Initialize"/>.
        /// </summary>
        public virtual void PrepareData(object data)
        {
        }

        public virtual object ToDomain(object decoded) => decoded;

        public virtual string Serialize<T>(T serializable)
            where T : IShapeable<T>
            => Serialize((object)serializable);

        public virtual object Deserialize<T>(string serialized)
            where T : IShapeable<T>
            => Deserialize(serialized);

        public virtual void Serialize<T>(T serializable, Stream outputStream)
            where T : IShapeable<T>
            => Serialize((object)serializable, outputStream);

        public virtual object Deserialize<T>(Stream inputStream)
            where T : IShapeable<T>
            => Deserialize(inputStream);

        public virtual string Serialize(object serializable)
            => throw new NotSupportedException($"{Name} does not implement string serialization.");

        public virtual object Deserialize(string serialized)
            => throw new NotSupportedException($"{Name} does not implement string deserialization.");

        public virtual void Serialize(object serializable, Stream outputStream)
            => throw new NotSupportedException($"{Name} does not implement stream serialization.");

        public virtual object Deserialize(Stream inputStream)
            => throw new NotSupportedException($"{Name} does not implement stream deserialization.");
    }
}
