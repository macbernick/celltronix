using System;
using System.Collections.Generic;



namespace celltronix {
	public class Sim {

		private Circuit circuit;
		private bool isInit;
		private bool firstTick;
		public List<CellLayerGroup> layerGroups;
		private List<CellLayerGroup> LayerSubGroups;
		private List<Cell> gates;
		private List<Cell> subGates;



		public Sim(Circuit circuit) {
			this.circuit = circuit;
			layerGroups = new List<CellLayerGroup>();
			LayerSubGroups = new List<CellLayerGroup>();
			gates = new List<Cell>();
			subGates = new List<Cell>();
		}



		private void init() {

			// nettoyage des portes
			// FIXME : trouver pourquoi il reste un silicon sous une porte ?!
			foreach (Cell c in circuit.cells) {

				if (c.layers[(int)Cell.LayerType.NPN].isSet) {

					int r = 0;
					for (int s = 0; s < c.layers[(int)Cell.LayerType.NPN].links.Length; s++)
						if (c.layers[(int)Cell.LayerType.NPN].links[s] != null)
							r++;

					if (r == 3) {
						c.layers[(int)Cell.LayerType.SILICON_N].isSet = false;
						c.layers[(int)Cell.LayerType.SILICON_P].isSet = false;
					}

				} else if (c.layers[(int)Cell.LayerType.PNP].isSet) {

					int r = 0;
					for (int s = 0; s < c.layers[(int)Cell.LayerType.PNP].links.Length; s++)
						if (c.layers[(int)Cell.LayerType.PNP].links[s] != null)
							r++;

					if (r == 3) {
						c.layers[(int)Cell.LayerType.SILICON_N].isSet = false;
						c.layers[(int)Cell.LayerType.SILICON_P].isSet = false;
					}
				}
			}

			// creation des groupes entrées/sorties
			foreach (InOut io in circuit.ios) {
				io.layerGroup = createGroup(io.iocells[0].layers[(int)Cell.LayerType.METAL]);
				layerGroups.Add(io.layerGroup);
			}

			// creation de tout les sous-groupes
			foreach (CellLayerGroup clg in layerGroups) {
				groupAll(clg);
			}

			foreach (CellLayerGroup clg in LayerSubGroups) {
				layerGroups.Add(clg);
			}

		}



		public void stop() {
			isInit = false;
		}



		public void tick() {

			// initialisation du premier tick
			if (firstTick)
				firstTick = false;

			// premier tick -> initialisation de la simulation
			if (!isInit) {
				init();
				isInit = true;
				firstTick = true;
			}

			// reinitialisation des cellules pour le tick suivant
			foreach (CellLayerGroup clg in layerGroups) {
				clg.initTick();
			}

			// gestion des portes PNP/NPN
			if (!firstTick) {
				foreach (Cell c in gates) {

					// NPN
//					if (c.layers[(int)Cell.LayerType.NPN].isSet) {
//						List<Cell> NCells = new List<Cell>();
//						Cell PCell = null;
//						foreach (Cell lnkC in c.layers[(int)Cell.LayerType.NPN].links) {
//							if (lnkC.layers[(int)Cell.LayerType.SILICON_N].isSet) {
//								NCells.Add(lnkC);
//							} else if (lnkC.layers[(int)Cell.LayerType.SILICON_P].isSet) {
//								PCell = lnkC;
//							}
//
////							Console.WriteLine(PCell);
//
////							if (PCell.layers[(int)Cell.LayerType.SILICON_P].isPowered) {
////
////							}
//						}
//
//					// PNP
//					} else if (c.layers[(int)Cell.LayerType.PNP].isSet) {
//						List<Cell> PCells = new List<Cell>();
//						Cell NCell;
//						foreach (Cell lnkC in c.layers[(int)Cell.LayerType.PNP].links) {
//							if (lnkC.layers[(int)Cell.LayerType.SILICON_P].isSet) {
//								PCells.Add(lnkC);
//							} else if (lnkC.layers[(int)Cell.LayerType.SILICON_N].isSet) {
//								NCell = lnkC;
//							}
//						}
//					}
				}
			}

			// gestion des entrées/sorties
			foreach (InOut io in circuit.ios) {
				io.tick();
			}
		}



		// cree récursivement les groupes de cellules en suivant les liens
		// FIXME : creer des liens dans l'éditeur pour trouver les portes
		// FIXME : ne pas grouper les layers silicon sous les portes !!
		private void groupAll(CellLayerGroup clg) {

			foreach (Cell c in clg.viaCells) {
				switch (clg.layers[0].layerType) {

					case Cell.LayerType.SILICON_N:

						if (c.layers[(int)Cell.LayerType.METAL].isSet 
							&& c.layers[(int)Cell.LayerType.METAL].groupedIn == false) {

							CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.METAL]);
							c.layerGroup = lg;
							LayerSubGroups.Add(lg);
							groupAll(lg);
						}
						break;

					case Cell.LayerType.SILICON_P:

						if (c.layers[(int)Cell.LayerType.METAL].isSet 
							&& c.layers[(int)Cell.LayerType.METAL].groupedIn == false) {

							CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.METAL]);
							c.layerGroup = lg;
							LayerSubGroups.Add(lg);
							groupAll(lg);
						}
						break;

					case Cell.LayerType.METAL:

						if (c.layers[(int)Cell.LayerType.SILICON_N].isSet 
							&& c.layers[(int)Cell.LayerType.SILICON_N].groupedIn == false) {

							CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.SILICON_N]);
							c.layerGroup = lg;
							LayerSubGroups.Add(lg);
							groupAll(lg);
						}

						if (c.layers[(int)Cell.LayerType.SILICON_P].isSet 
							&& c.layers[(int)Cell.LayerType.SILICON_P].groupedIn == false) {

							CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.SILICON_P]);
							c.layerGroup = lg;
							LayerSubGroups.Add(lg);
							groupAll(lg);
						}
						break;
				}
			}


			foreach (Cell c in subGates) {
				gates.Add(c);
			}

			Cell[] subGatesCopy = new Cell[subGates.Count];
			subGates.CopyTo(subGatesCopy);

			subGates = new List<Cell>();

			foreach (Cell c in subGatesCopy) {

				if (c.layers[(int)Cell.LayerType.NPN].isSet) {
					foreach (Cell lnkC in c.layers[(int)Cell.LayerType.NPN].links) {

						if (lnkC != null) {

							if (lnkC.layers[(int)Cell.LayerType.SILICON_N].isSet
								&& lnkC.layers[(int)Cell.LayerType.SILICON_N].groupedIn == false
							    ) {

								CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.SILICON_N]);
								LayerSubGroups.Add(lg);
								groupAll(lg);

							} else if (lnkC.layers[(int)Cell.LayerType.SILICON_P].isSet
								&& lnkC.layers[(int)Cell.LayerType.SILICON_P].groupedIn == false
							    ) {

								CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.SILICON_P]);
								LayerSubGroups.Add(lg);
								groupAll(lg);

							}
						}
					}

				} else if (c.layers[(int)Cell.LayerType.PNP].isSet) {
					foreach (Cell lnkC in c.layers[(int)Cell.LayerType.PNP].links) {
//						if (lnkC.layers[(int)Cell.LayerType.SILICON_N].isSet
//						    && lnkC.layers[(int)Cell.LayerType.SILICON_N].groupedIn == false
//						    ) {
//
//							CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.SILICON_N]);
//							LayerSubGroups.Add(lg);
//							groupAll(lg);
//
//						} else if (lnkC.layers[(int)Cell.LayerType.SILICON_P].isSet
//						           && lnkC.layers[(int)Cell.LayerType.SILICON_P].groupedIn == false
//						    ) {
//
//							CellLayerGroup lg = createGroup(c.layers[(int)Cell.LayerType.SILICON_P]);
//							LayerSubGroups.Add(lg);
//							groupAll(lg);
//
//						}
					}
				}
			}
		}



		private CellLayerGroup createGroup(CellLayer cl) {

			CellLayerGroup layerGroup = new CellLayerGroup();

			if (cl.groupedIn == false) {
				layerGroup.layers.Add(cl);
				cl.groupedIn = true;
			}

			connectLayers(cl, layerGroup);

			return layerGroup;
		}


		// ajoute récursivement les cellules dans un groupe
		private void connectLayers(CellLayer cl, CellLayerGroup layerGroup) {

			foreach (Cell c in cl.links) {

				// cellule existante encore non groupée. arrêt à la première porte
				if (c != null 
					&& c.layers[(int)cl.layerType].groupedIn == false
					&& c.layers[(int)Cell.LayerType.NPN].isSet == false
					&& c.layers[(int)Cell.LayerType.PNP].isSet == false
				    ) {

					layerGroup.layers.Add(c.layers[(int)cl.layerType]);
					c.layers[(int)cl.layerType].groupedIn = true;
					if (c.layers[(int)cl.layerType].parentCell.via)
						layerGroup.viaCells.Add(c);
					connectLayers(c.layers[(int)cl.layerType], layerGroup);
				}

				// ajout d'une porte à la liste de portes
				if (c != null) {

					if (c.layers[(int)Cell.LayerType.NPN].isSet
						&& c.layers[(int)Cell.LayerType.NPN].groupedIn == false
					  	) {
						c.layers[(int)Cell.LayerType.NPN].groupedIn = true;
						subGates.Add(c);

					} else if (c.layers[(int)Cell.LayerType.PNP].isSet
								&& c.layers[(int)Cell.LayerType.PNP].groupedIn == false
					           ) {
						c.layers[(int)Cell.LayerType.PNP].groupedIn = true;
						subGates.Add(c);
					}
				}
			}
		}
	}
}

