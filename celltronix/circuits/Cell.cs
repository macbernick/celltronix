using System.Collections.Generic;
using System;
using Gtk;
using Cairo;



namespace celltronix {
	public class Cell {



		public int x, y;					// coordonnées grille de la cellule

		// marges pour tracage cellules
		double mgN = 3;
		double mgE = 2;
		double mgS = 2;
		double mgW = 3;

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
		private InOut io;



		public Cell(int x, int y, Circuit circuit) {

			this.circuit = circuit;
			this.x = x;
			this.y = y;

			layers = new CellLayer[Enum.GetValues(typeof(LayerType)).Length];
			for (int clid = 0; clid < layers.Length; clid++) {
				layers[clid] = new CellLayer((Cell.LayerType)Enum.GetValues(typeof(Cell.LayerType)).GetValue(clid));
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



		public bool link(Cell c, LayerType layerType) {

			if (layers[(int)layerType].isSet && c.layers[(int)layerType].isSet) {

				// lien Nord
				if (c.x == x && c.y - y == -1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.N] = c;
					c.layers[(int)layerType].links[(int)CellLayer.Cardinals.S] = this;
					c.draw();
					return true;

					// lien Sud
				} else if (c.x == x && c.y - y == 1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.S] = c;
					c.layers[(int)layerType].links[(int)CellLayer.Cardinals.N] = this;
					c.draw();
					return true;

					// lien Ouest
				} else if (c.y == y && c.x - x == -1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.W] = c;
					c.layers[(int)layerType].links[(int)CellLayer.Cardinals.E] = this;
					c.draw();
					return true;

					// lien Est
				} else if (c.y == y && c.x - x == 1) {
					layers[(int)layerType].links[(int)CellLayer.Cardinals.E] = c;
					c.layers[(int)layerType].links[(int)CellLayer.Cardinals.W] = this;
					c.draw();
					return true;
				}
			}

			// creation portes PNP NPN
			switch (layerType) {

			// PNP
				case LayerType.SILICON_N:
					if (layers[(int)LayerType.SILICON_P].isSet) {

						// nord
						if (c.x == x && c.y - y == -1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] = c;
								c.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] = this;
								draw();
								c.draw();
							}
						}

						// sud
						if (c.x == x && c.y - y == 1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.S] = c;
								c.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.N] = this;
								draw();
								c.draw();
							}
						}

						// ouest
						if (c.y == y && c.x - x == -1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.E] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] = c;
								c.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] = this;
								draw();
								c.draw();
							}
						}

						// est
						if (c.y == y && c.x - x == 1) { 
							if (layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_P].links[(int)CellLayer.Cardinals.W] == null) {

								layers[(int)LayerType.PNP].isSet = true;
								layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.E] = c;
								c.layers[(int)LayerType.PNP].links[(int)CellLayer.Cardinals.W] = this;
								draw();
								c.draw();
							}
						}
					}
					break;

			// NPN
				case LayerType.SILICON_P:
					if (layers[(int)LayerType.SILICON_N].isSet) {

						// nord
						if (c.x == x && c.y - y == -1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] = c;
								c.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] = this;
								draw();
								c.draw();
							}
						}

						// sud
						if (c.x == x && c.y - y == 1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.S] = c;
								c.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.N] = this;
								draw();
								c.draw();
							}
						}

						// ouest
						if (c.y == y && c.x - x == -1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.E] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] = c;
								c.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] = this;
								draw();
								c.draw();
							}
						}

						// est
						if (c.y == y && c.x - x == 1) { 
							if (layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.N] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.S] != null &&
								layers[(int)LayerType.SILICON_N].links[(int)CellLayer.Cardinals.W] == null) {

								layers[(int)LayerType.NPN].isSet = true;
								layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.E] = c;
								c.layers[(int)LayerType.NPN].links[(int)CellLayer.Cardinals.W] = this;
								draw();
								c.draw();
							}
						}
					}
					break;
			}


			return false;
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
			// TODO : tracer les liens avec couche Metal
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
				foreach(Cell c in l.links)
					if (c != null) lnkCnt ++;

				Console.WriteLine("layer " + l.layerType + " : " + l.isSet + " | pwr : " + l.isPowered + " | links : " + lnkCnt);
			}
			Console.WriteLine("::::::::::::::::::::::");

			((IDisposable)grCxt.Target).Dispose();                                      
			((IDisposable)grCxt).Dispose();

		}


		// propage le courant
		public void setPower(LayerType layerType, bool power) {
			if (!layers[(int)layerType].isPowered) {

				Console.WriteLine("cell powered " + layerType + " : x = " + x + " y = " + y);


				// cellule testée
				layers[(int)layerType].isPowered = power;

				// liaisons sur la même couche
				foreach (Cell c in layers[(int)layerType].links) {
					if (c != null && !c.layers[(int)layerType].isPowered) {
						c.setPower(layerType, power);
					}
				}

				// liaison IO / Metal
				if (layerType == LayerType.IO
				    && layers[(int)LayerType.METAL].isSet
				    && !layers[(int)LayerType.METAL].isPowered) {
					setPower(LayerType.METAL, power);
				}

				// liaison Metal / Silicon N
				if (layerType == LayerType.METAL
				    && layers[(int)LayerType.SILICON_N].isSet
				    && !layers[(int)LayerType.SILICON_N].isPowered
				    && via) {
					setPower(LayerType.SILICON_N, power);
				}

				// liaison Silicon N / Metal
				if (layerType == LayerType.SILICON_N
				    && layers[(int)LayerType.METAL].isSet
				    && !layers[(int)LayerType.METAL].isPowered
				    && via) {
					setPower(LayerType.METAL, power);
				}

				// liaison Metal / Silicon P
				if (layerType == LayerType.METAL
				    && layers[(int)LayerType.SILICON_P].isSet
				    && !layers[(int)LayerType.SILICON_P].isPowered
				    && via) {
					setPower(LayerType.SILICON_P, power);
				}

				// liaison Silicon P / Metal
				if (layerType == LayerType.SILICON_P
				    && layers[(int)LayerType.METAL].isSet
				    && !layers[(int)LayerType.METAL].isPowered
				    && via) {
					setPower(LayerType.METAL, power);
				}
				draw();
			}
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

