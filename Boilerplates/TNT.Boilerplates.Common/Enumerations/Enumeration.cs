using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TNT.Boilerplates.Common.Enumerations
{
    public class Enumeration<ValueType>
    {
        public ValueType Value { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public static IEnumerable<Enumeration<ValueType>> GetListFrom<EnumType>() where EnumType : struct, Enum
        {
            return Enums.GetMembers(typeof(EnumType)).Select(member =>
            {
                var enumeration = new Enumeration<ValueType>
                {
                    Value = (ValueType)member.Value,
                    Name = member.Name,
                    DisplayName = member.Attributes.GetDisplayName(),
                    Description = member.Attributes.GetDescription()
                };

                return enumeration;
            });
        }
    }

    public static class Enumeration
    {
        public static IEnumerable<EnumType> GetListFrom<EnumType>() where EnumType : struct, Enum
            => Enums.GetMembers(typeof(EnumType)).Select(e => (EnumType)e.Value);
    }
}
