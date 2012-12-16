using System.Collections.Generic;
using System;



namespace celltronix {
	public class CellLayer {


		public enum Cardinals {
			N,
			E,
			S,
			W
		}

		public enum IOType {
			IN,
			OUT
		
		}

		public bool isSet;
		public bool groupedIn;
		public Cell[] links;
		public Cell.LayerType layerType;
		public IOType ioType;
		public Cell parentCell;
		public bool isPowered;

		public CellLayer(Cell.LayerType layerType, Cell parentCell) {
			this.parentCell = parentCell;
			links = new Cell[Enum.GetValues(typeof(Cardinals)).Length];
			this.layerType = layerType;
		}



		public void breakLinks() {
			if (links[(int)Cardinals.N] != null) {
				links[(int)Cardinals.N].layers[(int)layerType].links[(int)Cardinals.S] = null;
				links[(int)Cardinals.N].draw();
				links[(int)Cardinals.N] = null;
			}
			if (links[(int)Cardinals.E] != null) {
				links[(int)Cardinals.E].layers[(int)layerType].links[(int)Cardinals.W] = null;
				links[(int)Cardinals.E].draw();
				links[(int)Cardinals.E] = null;
			}
			if (links[(int)Cardinals.S] != null) {
				links[(int)Cardinals.S].layers[(int)layerType].links[(int)Cardinals.N] = null;
				links[(int)Cardinals.S].draw();
				links[(int)Cardinals.S] = null;
			}
			if (links[(int)Cardinals.W] != null) {
				links[(int)Cardinals.W].layers[(int)layerType].links[(int)Cardinals.E] = null;
				links[(int)Cardinals.W].draw();
				links[(int)Cardinals.W] = null;
			}
		}
	}
}

