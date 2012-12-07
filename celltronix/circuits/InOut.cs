using System;
using System.Collections.Generic;



namespace celltronix {
	public class InOut {

		public List<bool> sequence;
		public string name;
		public int x, y;

		public InOut() {

			sequence = new List<bool>{true};
			name = "+VCC";

		}
	}
}

