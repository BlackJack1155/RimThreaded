﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimThreaded
{
    public class GenTypes_Patch
    {
        private static readonly Type Original = typeof(GenTypes);
        private static readonly Type Patched = typeof(GenTypes_Patch);

        public static void RunDestructivePatches()
        {
            RimThreadedHarmony.Prefix(Original, Patched, "AllSubclassesNonAbstract");
        }
        public static bool AllSubclassesNonAbstract(Type baseType, ref List<Type> __result)
		{
            if (GenTypes.cachedSubclassesNonAbstract.TryGetValue(baseType, out List<Type> typeList))
            {
                __result = typeList;
                return false;
            }
            lock (GenTypes.cachedSubclassesNonAbstract)
            {
                if (!GenTypes.cachedSubclassesNonAbstract.TryGetValue(baseType, out List<Type> typeList2))
                {
                    typeList = GenTypes.AllTypes.Where((Type x) => x.IsSubclassOf(baseType) && !x.IsAbstract).ToList();
                    GenTypes.cachedSubclassesNonAbstract.Add(baseType, typeList);
                } else
                {
                    typeList = typeList2;
                }
                __result = typeList;
                return false;
            }
		}
	}
}
