using System;
using System.Collections.Generic;



namespace celltronix {
	public class InOut {

		private bool[] sequence;
		public string name;
		public List<Cell> iocells;
		public CellLayerGroup layerGroup;

		private int currentStateId;


		public InOut() {
			sequence = new bool[] { true, false };
			currentStateId = 0;
			name = "VCC";
		}


		public void tick() {

			layerGroup.setPower(sequence[currentStateId]);

			currentStateId ++;
			if (currentStateId >= sequence.Length) currentStateId = 0;
		}
	}
}

