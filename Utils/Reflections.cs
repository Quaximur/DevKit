using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public static class Reflections
{
    public static void SetValueRecursive(object target, string memberName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        var targetType = target.GetType();

        var property = GetPropertyRecursive(targetType, memberName, bindingFlags);
        if (property != null)
        {
            if (property.CanWrite)
            {
                property.SetValue(target, value);
                return;
            }

            var backingFieldName = $"<{memberName}>k__BackingField";
            var backingField = GetFieldRecursive(targetType, backingFieldName, bindingFlags);
            if (backingField != null && backingField.FieldType == property.PropertyType)
            {
                backingField.SetValue(target, value);
                return;
            }

            throw new InvalidOperationException($"Property '{memberName}' is read-only and no backing field found");
        }

        var field = GetFieldRecursive(targetType, memberName, bindingFlags);
        if (field != null)
        {
            field.SetValue(target, value);
            return;
        }

        throw new ArgumentException($"Member '{memberName}' not found in type '{targetType.Name}' or its base classes");
    }

    public static List<MemberInfo> GetMembersRecursive(Type type, BindingFlags bindingFlags)
    {
        if (type == null)
            return null;

        var members = type.GetMembers(bindingFlags)
            .ToList();

        if (type.BaseType != null && type.BaseType != typeof(object))
            members.InsertRange(0, GetMembersRecursive(type.BaseType, bindingFlags));

        return members;
    }

    public static PropertyInfo GetPropertyRecursive(Type type, string propertyName, BindingFlags bindingFlags)
    {
        if (type == null)
            return null;

        var property = type.GetProperty(propertyName, bindingFlags);
        if (property != null)
            return property;

        if (type.BaseType != null && type.BaseType != typeof(object))
            return GetPropertyRecursive(type.BaseType, propertyName, bindingFlags);

        return null;
    }

    public static FieldInfo GetFieldRecursive(Type type, string fieldName, BindingFlags bindingFlags)
    {
        if (type == null)
            return null;

        var field = type.GetField(fieldName, bindingFlags);
        if (field != null)
            return field;

        if (type.BaseType != null && type.BaseType != typeof(object))
            return GetFieldRecursive(type.BaseType, fieldName, bindingFlags);

        return null;
    }

    public static void SetMemberValue(MemberInfo memberInfo, object target, object value)
    {
        if (memberInfo is FieldInfo fieldInfo)
            fieldInfo.SetValue(target, value);
        else
            ((PropertyInfo)memberInfo).SetValue(target, value);
    }
}