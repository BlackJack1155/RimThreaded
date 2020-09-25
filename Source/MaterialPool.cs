﻿using System.Threading;
using UnityEngine;
using Verse;

namespace RimThreaded
{    
    public class MaterialPool_Patch
    {
		public static bool MatFrom(ref Material __result, MaterialRequest req)
		{
			Material material;
			//if (RimThreaded.materialResults.TryGetValue(req, out material))
			//{
			//__result = material;
			//return false;
			//}
			int tID = Thread.CurrentThread.ManagedThreadId;
			if (RimThreaded.materialWaits.TryGetValue(tID, out EventWaitHandle eventWaitStart)) {
				RimThreaded.materialRequests.TryAdd(tID, req);
				RimThreaded.mainThreadWaitHandle.Set();
				eventWaitStart.WaitOne();
				RimThreaded.materialResults.TryGetValue(req, out material);
				__result = material;
				return false;
			}
			return true;
		}

	}

}
