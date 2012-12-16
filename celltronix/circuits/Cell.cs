using System.Collections.Generic;
using System;
using Gtk;
using Cairo;



namespace celltronix {
	public class Cell {



		public int x, y;					// coordonnées grille de la cellule

		// marges pour tracage cellules
		double mgN = 4;
		double mgE = 3;
		double mgS = 3;
		double mgW = 4;

		// type de couches contenues dans la cellule
		public enum LayerType {
			SILICON_P,
			SILICON_N,
			METAL,
			PNP,
			NPN,
			IO
		}


		private Circuit circuit;
		public CellLayer[] layers;
		public bool via;
		public CellLayerGroup layerGroup;



		public Cell(int x, int y, Circuit circuit) {

			this.circuit = circuit;
			this.x = x;
			this.y = y;

			layers = new CellLayer[Enum.GetValues(typeof(LayerType)).Length];
			for (int clid = 0; clid < layers.Length; clid++) {
				layers[clid] = new CellLayer((Cell.LayerType)Enum.GetValues(typeof(Cell.LayerType)).GetValue(clid), this);
			}

		}



		public void setLayer(LayerType layerType) {

			if ((layerType == LayerType.SILICON_N && layers[(int)LayerType.SILICON_P].isSet) ||
				(layerType == LayerType.SILICON_P && layers[(int)LayerType.SILICON_N].isSet)) {
				return;
			} else {
				layers[(int)layerType].isSet = true;
				draw();
			}
		}



		public bool getLayer(LayerType layerType) {
			return layers[(int)layerType].isSet;
		}



		public bool link(Cell originCell, LayerType layerType) {

			if (layers[(int)layerType].isSet && originCell.layers[(int)layerType].isSet) {

				// lien Nord
				if (originCell.x == x && originCell.y - y == -1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.N] = originCell;
					originCell.layers[(int)layerType].links[(int)CellLayer.Cardinals.S] = this;
					originCell.draw();
					return true;

					// lien Sud
				} else if (originCell.x == x && originCell.y - y == 1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.S] = originCell;
					originCell.layers[(int)layerType].links[(int)CellLayer.Cardinals.N] = this;
					originCell.draw();
					return true;

					// lien Ouest
				} else if (originCell.y == y && originCell.x - x == -1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.W] = originCell;
					originCell.layers[(int)layerType].links[(int)CellLayer.Cardinals.E] = this;
					originCell.draw();
					return true;

					// lien Est
				} else if (originCell.y == y && originCell.x - x == 1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.E] = originCell;
					originCell.layers[(int)layerType].links[(int)CellLayer.Cardinals.W] = this;
					originCell.draw();
					return true;
				}
			}

			// creation portes PNP NPN
			switch (layerType) {

				// PNP
				case LayerType.SILICON_N:
					if (layers[(int)LayerType.SILICON_P].isSet) {

						// nord
						if (originCell.x == x && originCell.y - y == -1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] = originCell;

								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] 
								= circuit.findCellByCoords(x - 1, y);
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] 
								= circuit.findCellByCoords(x + 1, y);

								originCell.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;
								
								draw();
								originCell.draw();
							}
						} else

						// sud
						if (originCell.x == x && originCell.y - y == 1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] = originCell;

								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] 
								= circuit.findCellByCoords(x - 1, y);
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] 
								= circuit.findCellByCoords(x + 1, y);


								originCell.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						} else

						// ouest
						if (originCell.y == y && originCell.x - x == -1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] = originCell;

								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] 
								= circuit.findCellByCoords(x, y + 1);
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] 
								= circuit.findCellByCoords(x, y - 1);


								originCell.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						} else

						// est
						if (originCell.y == y && originCell.x - x == 1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] = originCell;

								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] 
								= circuit.findCellByCoords(x, y + 1);
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] 
								= circuit.findCellByCoords(x, y - 1);

								originCell.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						}
					}
					break;

				// NPN
				case LayerType.SILICON_P:
					if (layers[(int)LayerType.SILICON_N].isSet) {

						// nord
						if (originCell.x == x && originCell.y - y == -1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] = originCell;

								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] 
								= circuit.findCellByCoords(x - 1, y);
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] 
								= circuit.findCellByCoords(x + 1, y);

								originCell.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						} else

						// sud
						if (originCell.x == x && originCell.y - y == 1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] = originCell;

								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] 
								= circuit.findCellByCoords(x - 1, y);
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] 
								= circuit.findCellByCoords(x + 1, y);

								originCell.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						} else

						// ouest
						if (originCell.y == y && originCell.x - x == -1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] = originCell;

								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] 
								= circuit.findCellByCoords(x, y + 1);
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] 
								= circuit.findCellByCoords(x, y - 1);

								originCell.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						} else

						// est
						if (originCell.y == y && originCell.x - x == 1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] = originCell;

								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] 
								= circuit.findCellByCoords(x, y + 1);
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] 
								= circuit.findCellByCoords(x, y - 1);

								originCell.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] = this;

								layers[(int)LayerType.SILICON_N].isSet = false;
								layers[(int)LayerType.SILICON_P].isSet = false;

								draw();
								originCell.draw();
							}
						}
					}
					break;
			}
			return false;
		}



		public void setLayerGroup(CellLayerGroup layerGroup) {
			this.layerGroup = layerGroup;
		}



		public void draw(Context grCxt, double cellWidth, Colors color) {

			PointD p0, p1, p2, p3;

			// efface la case
			p0 = new PointD(this.x * cellWidth, this.y * cellWidth);
			p1 = new PointD((this.x + 1) * cellWidth, this.y * cellWidth);
			p2 = new PointD((this.x + 1) * cellWidth, (this.y + 1) * cellWidth);
			p3 = new PointD(this.x * cellWidth, (this.y + 1) * cellWidth);

			grCxt.MoveTo(p0);
			grCxt.LineTo(p1);
			grCxt.LineTo(p2);
			grCxt.LineTo(p3);
			grCxt.ClosePath();
			grCxt.Color = color.find("background");
			grCxt.Fill();

			p0 = new PointD(this.x * cellWidth + 1, (this.y + 1) * cellWidth);
			p1 = new PointD(this.x * cellWidth + 1, this.y * cellWidth + 1);
			p2 = new PointD((this.x + 1) * cellWidth, this.y * cellWidth + 1);
			grCxt.MoveTo(p0);
			grCxt.LineTo(p1);
			grCxt.LineTo(p2);
			grCxt.Color = color.find("grid");
			grCxt.Stroke();

			// couche SILICON_N
			if (circuit.showLayer[(int)Circuit.showLayerType.S]) {
				if (layers[(int)LayerType.SILICON_N].isSet) {

					grCxt.MoveTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] != null ||
						layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] != null) {

						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] != null ||
						layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] != null) {

						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, this.y * cellWidth + mgN));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, (this.y + 1) * cellWidth - mgS));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] != null ||
						layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] != null) {

						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth));
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth));
					}

					grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] != null ||
						layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] != null) {

						grCxt.LineTo(new PointD(this.x * cellWidth, (this.y + 1) * cellWidth - mgS));
						grCxt.LineTo(new PointD(this.x * cellWidth, this.y * cellWidth + mgN));
					}

					grCxt.ClosePath();
					grCxt.Color = color.find("snN_fill");
					grCxt.Fill();
				}
			}

			// couche SILICON_P
			if (circuit.showLayer[(int)Circuit.showLayerType.S]) {
				if (layers[(int)LayerType.SILICON_P].isSet) {

					grCxt.MoveTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] != null ||
						layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] != null ||
						layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, this.y * cellWidth + mgN));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, (this.y + 1) * cellWidth - mgS));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] != null ||
						layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth));
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth));
					}

					grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] != null ||
						layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth, (this.y + 1) * cellWidth - mgS));
						grCxt.LineTo(new PointD(this.x * cellWidth, this.y * cellWidth + mgN));
					}

					grCxt.ClosePath();
					grCxt.Color = color.find("snP_fill");
					grCxt.Fill();
				}
			}


			// PNP / NPN
			if (circuit.showLayer[(int)Circuit.showLayerType.S]) {
				if (layers[(int)LayerType.PNP].isSet) {
					grCxt.MoveTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, this.y * cellWidth + mgN));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, (this.y + 1) * cellWidth - mgS));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth));
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth));
					}

					grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth, (this.y + 1) * cellWidth - mgS));
						grCxt.LineTo(new PointD(this.x * cellWidth, this.y * cellWidth + mgN));
					}

					grCxt.ClosePath();
					grCxt.Color = color.find("gate_PNP");
					grCxt.Fill();
				}

				if (layers[(int)LayerType.NPN].isSet) {
					grCxt.MoveTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, this.y * cellWidth + mgN));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, (this.y + 1) * cellWidth - mgS));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth));
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth));
					}

					grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth, (this.y + 1) * cellWidth - mgS));
						grCxt.LineTo(new PointD(this.x * cellWidth, this.y * cellWidth + mgN));
					}

					grCxt.ClosePath();
					grCxt.Color = color.find("gate_NPN");
					grCxt.Fill();
				}
			}

			// couche METAL
			if (circuit.showLayer[(int)Circuit.showLayerType.M]) {
				if (layers[(int)LayerType.METAL].isSet && !layers[(int)LayerType.IO].isSet) {

					grCxt.MoveTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.METAL].links[(int)CellLayer.Cardinals.N] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, this.y * cellWidth));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, this.y * cellWidth + mgN));

					if (layers[(int)LayerType.METAL].links[(int)CellLayer.Cardinals.E] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, this.y * cellWidth + mgN));
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth, (this.y + 1) * cellWidth - mgS));
					}

					grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.METAL].links[(int)CellLayer.Cardinals.S] != null) {
						grCxt.LineTo(new PointD((this.x + 1) * cellWidth - mgE, (this.y + 1) * cellWidth));
						grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth));
					}

					grCxt.LineTo(new PointD(this.x * cellWidth + mgW, (this.y + 1) * cellWidth - mgS));

					if (layers[(int)LayerType.METAL].links[(int)CellLayer.Cardinals.W] != null) {
						grCxt.LineTo(new PointD(this.x * cellWidth, (this.y + 1) * cellWidth - mgS));
						grCxt.LineTo(new PointD(this.x * cellWidth, this.y * cellWidth + mgN));
					}

					grCxt.ClosePath();
					grCxt.Color = color.find("metal_fill");
					grCxt.Fill();
				}
			}

			// couche I/O
			if (layers[(int)LayerType.IO].isSet) {
				traceLinkedFullSquare(grCxt, cellWidth, color.find("metal_fill"), LayerType.IO);
			}


			// via
			if (circuit.showLayer[(int)Circuit.showLayerType.S]) {
				if (via) {
					grCxt.MoveTo(new PointD(x * cellWidth + 7d, y * cellWidth + 7d));
					grCxt.LineTo(new PointD((x + 1) * cellWidth - 6d, y * cellWidth + 7d));
					grCxt.LineTo(new PointD((x + 1) * cellWidth - 6d, (y + 1) * cellWidth - 6d));
					grCxt.LineTo(new PointD(x * cellWidth + 7d, (y + 1) * cellWidth - 6d));
					grCxt.ClosePath();
					grCxt.Color = color.find("via");
					grCxt.Stroke();
				}
			}


			// powered
			bool pwr = false;
			foreach (CellLayer l in layers)
				if (l.isPowered)
					pwr = true;

			if (pwr) {
				grCxt.MoveTo(new PointD(x * cellWidth + mgW, y * cellWidth + mgN));
				grCxt.LineTo(new PointD((x + 1) * cellWidth - mgE, y * cellWidth + mgN));
				grCxt.LineTo(new PointD((x + 1) * cellWidth - mgE, (y + 1) * cellWidth - mgS));
				grCxt.LineTo(new PointD(x * cellWidth + mgW, (y + 1) * cellWidth - mgS));
				grCxt.ClosePath();
				grCxt.Color = color.find("powered");
				grCxt.Fill();
			}

		}



		public void draw() {
			Context grCxt = Gdk.CairoHelper.Create(circuit.drawingAera.GdkWindow);
			grCxt.Antialias = Antialias.None;
			grCxt.Scale(1d, 1d);
			grCxt.LineWidth = 1d;

			draw(grCxt, circuit.cellNormalWidth, circuit.color);

			((IDisposable)grCxt.Target).Dispose();                                      
			((IDisposable)grCxt).Dispose();
		}



		public void deleteLayer(LayerType layerType) {

			layers[(int)layerType].isSet = false;

			layers[(int)layerType].breakLinks();

			if (layerType == LayerType.SILICON_N ^ layerType == LayerType.SILICON_P)
				via = false;

			draw();
		}



		public void drawLinkChk() {

			circuit.draw();

			Context grCxt = Gdk.CairoHelper.Create(circuit.drawingAera.GdkWindow);
			grCxt.Antialias = Antialias.None;
			grCxt.Scale(1d, 1d);
			grCxt.LineWidth = 1d;

			foreach (CellLayer l in layers) {
				foreach (Cell c in l.links) {
					if (c != null) {
						grCxt.MoveTo(new PointD(c.x * circuit.cellNormalWidth, c.y * circuit.cellNormalWidth));
						grCxt.LineTo(new PointD((c.x + 1) * circuit.cellNormalWidth, c.y * circuit.cellNormalWidth));
						grCxt.LineTo(new PointD((c.x + 1) * circuit.cellNormalWidth, (c.y + 1) * circuit.cellNormalWidth));
						grCxt.LineTo(new PointD(c.x * circuit.cellNormalWidth, (c.y + 1) * circuit.cellNormalWidth));
						grCxt.ClosePath();
						grCxt.Color = circuit.color.find("link_chk");
						grCxt.Fill();
					}
				}

				int lnkCnt = 0;
				foreach (Cell c in l.links)
					if (c != null)
						lnkCnt ++;

				Console.WriteLine("layer " + l.layerType + " : " + l.isSet 
				                  + " \t| pwr : " + l.isPowered 
				                  + " \t| links : " + lnkCnt
				                  + " \t| grouped : " + l.groupedIn);
			}
			Console.WriteLine(" : : : : : : : : : : : : : : : : : : : : : :");

			((IDisposable)grCxt.Target).Dispose();                                      
			((IDisposable)grCxt).Dispose();

		}





		// trace un carré plein à liaisons
		private void traceLinkedFullSquare(Context grCxt, double cellWidth, Color fillColor, LayerType layerType) {
			double marginN, marginE, marginS, marginW;
			if (layers[(int)layerType].links[(int)CellLayer.Cardinals.N] != null)
				marginN = 0;
			else
				marginN = mgN;
			if (layers[(int)layerType].links[(int)CellLayer.Cardinals.E] != null)
				marginE = 0;
			else
				marginE = mgE;
			if (layers[(int)layerType].links[(int)CellLayer.Cardinals.S] != null)
				marginS = 0;
			else
				marginS = mgS;
			if (layers[(int)layerType].links[(int)CellLayer.Cardinals.W] != null)
				marginW = 0;
			else
				marginW = mgW;

			grCxt.MoveTo(x * cellWidth + marginW, y * cellWidth + marginN);
			grCxt.LineTo((x + 1) * cellWidth - marginE, y * cellWidth + marginN);
			grCxt.LineTo((x + 1) * cellWidth - marginE, (y + 1) * cellWidth - marginS);
			grCxt.LineTo(x * cellWidth + marginW, (y + 1) * cellWidth - marginS);
			grCxt.ClosePath();
			grCxt.Color = fillColor;
			grCxt.Fill();
		}
	}
}

