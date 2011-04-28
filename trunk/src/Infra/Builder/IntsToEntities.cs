﻿using System;
using System.Collections.Generic;
using Omu.ValueInjecter;
using Core.Model;
using Core.Repository;

namespace Infra.Builder
{
    public class IntsToEntities : ConventionInjection
    {
        protected override bool Match(ConventionInfo c)
        {
            if (c.SourceProp.Name != c.TargetProp.Name) return false;
            var s = c.SourceProp.Type;
            var t = c.TargetProp.Type;

            if (!s.IsGenericType || !t.IsGenericType
                || s.GetGenericTypeDefinition() != typeof(IEnumerable<>)
                || t.GetGenericTypeDefinition() != typeof(ICollection<>)) return false;

            return s.GetGenericArguments()[0] == (typeof(int))
                   && (t.GetGenericArguments()[0].IsSubclassOf(typeof(Entity)));
        }

        protected override object SetValue(ConventionInfo c)
        {
            if (c.SourceProp.Value == null) return null;
            dynamic repo = IoC.Resolve(typeof(IReadRepo<>).MakeGenericType(c.TargetProp.Type.GetGenericArguments()[0]));
            dynamic list = Activator.CreateInstance(typeof(List<>).MakeGenericType(c.TargetProp.Type.GetGenericArguments()[0]));

            foreach (var i in ((IEnumerable<int>)c.SourceProp.Value))
                list.Add(repo.Get(i));
            return list;
        }
    }
}