using System;
using System.Collections.Generic;



namespace celltronix {
	public class CellLayerGroup {

		public List<CellLayer> layers;
		public List<Cell> viaCells;
		private bool powerWait = true;


		public CellLayerGroup() {
			layers = new List<CellLayer>();
			viaCells = new List<Cell>();
		}


		public void initTick() {
			powerWait = true;
		}


		public void setPower(bool p) {

			if (powerWait) {

				powerWait = false;

				foreach (CellLayer cl in layers) {
					cl.isPowered = p;
					if (cl.parentCell.layerGroup != null) {
						cl.parentCell.layerGroup.setPower(p);
					}
				}
			}
		}
	}
}

