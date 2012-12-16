using System;
using System.Collections.Generic;
using Cairo;
using Gtk;



namespace celltronix {
	public class Circuit {


		public List<Cell> cells;			// cellules du circuit
		public List<InOut> ios;				// entrées / sorties

		private Label cellCount;
		private Cell firstLinkingCell;		// premiere cellule à lier à une deuxième

		public DrawingArea drawingAera;	// zone de tracage
		private Vector2D drawingAeraSize;	// taille de la zone
		public Colors color;				// couleurs

		public double cellNormalWidth = 20;// taille d'une cellule à zoom = 1

		public enum showLayerType {
			S,
			M
		}

		public bool[] showLayer;


		// Constructeur
		public Circuit(DrawingArea drawingAera, Label cellCount) {
			cells = new List<Cell>();
			ios = new List<InOut>();
			color = new Colors();
			this.drawingAera = drawingAera;
			this.cellCount = cellCount;
			showLayer = new bool[] {true, true};
			updateStatusBarCellCount();
			draw();
		}


		// Trace le circuit
		public void draw() {

			drawingAeraSize = aeraSize();
			Context grCxt = Gdk.CairoHelper.Create(drawingAera.GdkWindow);
			grCxt.Antialias = Antialias.None;
			grCxt.Scale(1d, 1d);
			grCxt.LineWidth = 1d;

			drawGrid(grCxt);
			drawCells(grCxt);

			((IDisposable)grCxt.Target).Dispose();                                      
			((IDisposable)grCxt).Dispose();
		}



		public Cell clickCell(double x, double y, Cell.LayerType layerType) {
			int coordX = (int)Math.Ceiling(x / cellNormalWidth) - 1;
			int coordY = (int)Math.Ceiling(y / cellNormalWidth) - 1;

			Cell c = findCellByCoords(coordX, coordY);

			if (c == null)
				c = createCell(coordX, coordY);

			c.setLayer(layerType);

			draw();

			return c;
		}



		public void createCellLinks(double x, double y, Cell.LayerType layerType) {
			int coordX = (int)Math.Ceiling(x / cellNormalWidth) - 1;
			int coordY = (int)Math.Ceiling(y / cellNormalWidth) - 1;

			Cell c = findCellByCoords(coordX, coordY);

			if (c == null)
				c = createCell(coordX, coordY);

			c.setLayer(layerType);

			if (firstLinkingCell == null) {
				// premiere cellule : pas de lien
				firstLinkingCell = c;
				return;
			} else {
				if (c.x == firstLinkingCell.x && c.y == firstLinkingCell.y) {
					// toujours la même cellule : pas de lien
					return;
				} else {
					// cellule suivante : lien avec la cellule précédente
					bool lnkd = c.link(firstLinkingCell, layerType);
					if (lnkd)
						firstLinkingCell = c;
					else
						firstLinkingCell = null;
				}
			}
		}



		public void stopLinking() {
			firstLinkingCell = null;
		}



		public void deleteLayer(double x, double y, Cell.LayerType layerType) {

			int coordX = (int)Math.Ceiling(x / cellNormalWidth) - 1;
			int coordY = (int)Math.Ceiling(y / cellNormalWidth) - 1;

			Cell c = findCellByCoords(coordX, coordY);

			if (c != null) 
				c.deleteLayer(layerType);

//			bool killCell = true;
//			foreach (CellLayer cl in c.layers) {
//				if (cl.isSet) {
//					killCell = false;
//					continue;
//				}
//			}
//
//			if (killCell) {
//				cells.Remove(c);
//				updateStatusBarCellCount();
//			}
		}


		// dessine les lien entre les cellules
		public void linkCheck(double x, double y) {
			int coordX = (int)Math.Ceiling(x / cellNormalWidth) - 1;
			int coordY = (int)Math.Ceiling(y / cellNormalWidth) - 1;
			Cell c = findCellByCoords(coordX, coordY);
			if (c != null)
				c.drawLinkChk();
			else
				Console.WriteLine("No Cell here!");
		}


		// liasion S/M
		public void createVia(double x, double y) {
			int coordX = (int)Math.Ceiling(x / cellNormalWidth) - 1;
			int coordY = (int)Math.Ceiling(y / cellNormalWidth) - 1;
			Cell c = findCellByCoords(coordX, coordY);
			if (c != null) {
				if (c.layers[(int)Cell.LayerType.SILICON_N].isSet ^ c.layers[(int)Cell.LayerType.SILICON_P].isSet) {
					c.via = true;
					c.draw();
				}
			}
		}


		// Input/Output
		public void createIO(double x, double y) {
			int coordX = (int)Math.Ceiling(x / cellNormalWidth) - 1;
			int coordY = (int)Math.Ceiling(y / cellNormalWidth) - 1;

			// verif place libre
			bool place = true;
			for (int r = -1; r <= 1; r++) {
				for (int s = -1; s <= 1; s++) {
					if (findCellByCoords(coordX + r, coordY + s) != null) {
						place = false;
						break;
					}
				}
				if (!place)
					break;
			}

			if (place) {

				// creation des cellules
				List<Cell> iocells = new List<Cell>();
				for (int r = -1; r <= 1; r++) {
					for (int s = -1; s <= 1; s++) {
						Cell c = createCell(coordX + r, coordY + s);
						c.setLayer(Cell.LayerType.IO);
						c.setLayer(Cell.LayerType.METAL);
						iocells.Add(c);
					}
				}

				// liens
				foreach (Cell c in iocells) {
					switch (coordX - c.x) {
						case 1:
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.E] = findCellByCoords(c.x + 1, c.y);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.E] = findCellByCoords(c.x + 1, c.y);
							break;
						case 0:
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.W] = findCellByCoords(c.x - 1, c.y);
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.E] = findCellByCoords(c.x + 1, c.y);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.W] = findCellByCoords(c.x - 1, c.y);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.E] = findCellByCoords(c.x + 1, c.y);
							break;
						case -1:
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.W] = findCellByCoords(c.x - 1, c.y);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.W] = findCellByCoords(c.x - 1, c.y);
							break;
					}
					switch (coordY - c.y) {
						case 1:
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.S] = findCellByCoords(c.x, c.y + 1);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.S] = findCellByCoords(c.x, c.y + 1);
							break;
						case 0:
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.S] = findCellByCoords(c.x, c.y + 1);
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.N] = findCellByCoords(c.x, c.y - 1);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.S] = findCellByCoords(c.x, c.y + 1);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.N] = findCellByCoords(c.x, c.y - 1);
							break;
						case -1:
							c.layers[(int)Cell.LayerType.IO].links[(int)CellLayer.Cardinals.N] = findCellByCoords(c.x, c.y - 1);
							c.layers[(int)Cell.LayerType.METAL].links[(int)CellLayer.Cardinals.N] = findCellByCoords(c.x, c.y - 1);
							break;
					}
				}

				InOut io = new InOut();
				ios.Add(io);
				io.iocells = iocells;

				draw();
			}
		}



		public void layerVisible(showLayerType type, bool visible) {
			showLayer[(int)type] = visible;
			draw();
		}

		
		// trouve une cellule d'après ses coordonées
		public Cell findCellByCoords(int x, int y) {
			foreach (Cell c in cells) {
				if (c.x == x && c.y == y)
					return c;
			}
			return null;
		}

		// recupère la taille de la zone de tracage
		private Vector2D aeraSize() {
			return new Vector2D(drawingAera.Allocation.Width, drawingAera.Allocation.Height);
		}


		// trace la grille de cellules
		private void drawGrid(Context grCxt) {
			PointD p0, p1, p2, p3;
			grCxt.Save();
			grCxt.Color = color.find("background");
			p0 = new PointD(0, 0);
			p1 = new PointD(drawingAeraSize.x, 0);
			p2 = new PointD(drawingAeraSize.x, drawingAeraSize.y);
			p3 = new PointD(0, drawingAeraSize.y);
			grCxt.MoveTo(p0);
			grCxt.LineTo(p1);
			grCxt.LineTo(p2);
			grCxt.LineTo(p3);
			grCxt.Fill();

			grCxt.Color = color.find("grid");
			for (double r = 0.5d; r < drawingAeraSize.x; r += cellNormalWidth) {
				p0 = new PointD(r, 0.5);
				p1 = new PointD(r, drawingAeraSize.y);
				grCxt.MoveTo(p0);
				grCxt.LineTo(p1);
			}
			for (double r = 0.5d; r < drawingAeraSize.y; r += cellNormalWidth) {
				p0 = new PointD(0.5, r);
				p1 = new PointD(drawingAeraSize.x, r);
				grCxt.MoveTo(p0);
				grCxt.LineTo(p1);
			}

			grCxt.Stroke();
			grCxt.Restore();
		}


		// dessine les cellules
		private void drawCells(Context grCxt) {

			grCxt.Save();
			foreach (Cell c in cells)
				c.draw(grCxt, cellNormalWidth, color);
			grCxt.Restore();
		}





		// cree une nouvelle cellule
		private Cell createCell(int x, int y) {
			Cell c = new Cell(x, y, this);
			cells.Add(c);
			updateStatusBarCellCount();
			return c;
		}


		// mise a jour nombre de cellule dans la barre de status
		private void updateStatusBarCellCount() {
			cellCount.Text = cells.Count.ToString() + " cells";
		}
	}
}

