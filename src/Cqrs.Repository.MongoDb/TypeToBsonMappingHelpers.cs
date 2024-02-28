namespace Platform.Infrastructure.Repository.MongoDb
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using MongoDB.Bson.Serialization.Serializers;

    public static class TypeToBsonMappingHelpers
    {
        public static void CheckImmutability(IEnumerable<Type> typesToBeChecked)
        {
            ImmutableTypeClassMapConvention immutableTypeClassMapConvention = new ImmutableTypeClassMapConvention();

            foreach (Type type in typesToBeChecked)
            {
                BsonClassMap bsonClassMap = new BsonClassMap(type);

                immutableTypeClassMapConvention.Apply(bsonClassMap);

                if (bsonClassMap.HasCreatorMaps == false)
                {
                    throw new Exception($"Type: {type.FullName} is not an immutable type");
                }

                bsonClassMap.SetIgnoreExtraElements(true);

                BsonClassMap.LookupClassMap(type);
            }
        }

        public static void MapAggregateRoot<T>()
        {
            FieldInfo[] privateFieldInfos = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            BsonClassMap<T> map = BsonClassMap.RegisterClassMap<T>();

            map.AutoMap();

            map.SetIgnoreExtraElements(true);

            foreach (FieldInfo privateFieldInfo in privateFieldInfos)
            {
                map.MapField(privateFieldInfo.Name);
            }
        }

        public static void SetEnumStringConvention<T>()
            where T : struct, Enum
        {
            BsonSerializer.RegisterSerializer(new EnumSerializer<T>(BsonType.String));
        }
    }
}
