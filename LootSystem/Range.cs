using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootSystem {
	public class Range {
		public double Min;
		public double Max;

		public Range(double min, double max) {
			Min = min;
			Max = max;
		}

		public double RandomDouble(Random r) {
			return r.NextDouble() * (Max - Min) + Min;
		}

		public int RandomInt(Random r) {
			return r.Next((int)Min, (int)Max);
		}
	}
}
