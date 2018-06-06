using System;
using System.Runtime.CompilerServices;
using UnityEditor.Importers;
using UnityEngine;
using Utf8Json;
using Utf8Json.Resolvers;
using Utf8Json.Unity;

namespace UnityEditor
{

    [JsonVersioned(typeof(Person1))]
    public class Person
    {
        public int age;
        public string name;

        public Person() {}

        public override string ToString()
        {
            return string.Format("Person(Name={0}, Age={1})", name, age);
        }
    }

    [JsonVersioned(typeof(Person0))]
    public class Person1 : IUpgradableTo<Person>
    {
        public int age;
        public string fullName;

        public Person1() {}

        public Person Upgrade()
        {
            return new Person
            {
                age = age,
                name = fullName
            };
        }
    }

    public class Person0 : IUpgradableTo<Person1>
    {
        public int age;
        public string totallyFullName;

        public Person0() {}

        public Person1 Upgrade()
        {
            return new Person1
            {
                age = age,
                fullName = totallyFullName
            };
        }
    }

    [InitializeOnLoad]
    public static class Class1
    {

        static Class1()
        {
            Person1 p0 = new Person1 { age = 99, fullName = "foobar" };
            byte[] json0 = JsonSerializer.Serialize(p0, new UpgradeResolver(CompositeResolver.Create(UnityResolver.Instance, StandardResolver.Default)));
            Debug.Log(JsonSerializer.PrettyPrint(json0));
            Person p = JsonSerializer.Deserialize<Person>(json0, new UpgradeResolver(CompositeResolver.Create(UnityResolver.Instance, StandardResolver.Default)));
            Debug.Log(p);
        }
    }
}
