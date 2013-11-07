using UnityEngine;
using System.Collections;

public class Profiler
{

		private System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch ();
		private string name;
		private long wtime, wmem;
		private long mem;

		public Profiler ()
		{
				this.wtime = 5l; // 5ms
				this.wmem = 64l; // 64kb
		}

		public Profiler (long wtime, long wmem)
		{
				this.wtime = wtime;
				this.wmem = wmem;
		}

		public void Start (string n)
		{
				this.name = n;
				this.mem = System.GC.GetTotalMemory (false);
				watch.Reset ();
				watch.Start ();
		}

		public void Stop ()
		{
				watch.Stop ();
				if (watch.ElapsedMilliseconds >= wtime) {
						Debug.Log (this.name + " took : " + watch.ElapsedMilliseconds + "ms");
				}
				long kb = (System.GC.GetTotalMemory (false) - this.mem) / 1024;
				if (kb >= wmem) {
						Debug.Log (this.name + " consumed : " + kb + "kb");
				}
				
		}
}
