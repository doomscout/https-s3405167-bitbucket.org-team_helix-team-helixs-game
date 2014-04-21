using UnityEngine;
using System.Collections;
using System.Diagnostics;

public delegate void TimeMethodInit();
public delegate void TimeMethodStart();

//This class is used for timing methods
//Example of use: To test the BuildMesh() function, do TestTiming.timeMethod(new TimeMethodStart(BuildMesh));
public static class TestTiming {
	
	public static void doNothingMethod() {
		
	}

	public static void timeMethod(TimeMethodStart start) {
		timeMethod(new TimeMethodInit(doNothingMethod), start);
	}
	
	public static void timeMethod(TimeMethodInit init, TimeMethodStart start) {
		Stopwatch s =  new Stopwatch();
		init();
		s.Start();
		start();
		s.Stop();
		UnityEngine.Debug.LogError (s.ElapsedMilliseconds);
	}

}
