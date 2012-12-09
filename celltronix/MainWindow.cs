using System;
using celltronix;
using Cairo;
using Gtk;



public partial class MainWindow: Gtk.Window {	





	private Label cellCount;
	private Circuit circuit;
	private Sim sim;
	private DrawingArea circuitAera;
	private enum ToolType {
		SILICON_P,
		SILICON_N,
		METAL,
		VIA,
		DEL_M,
		DEL_S,
		LINK_CHK,
		IO
	}

	private ToolType selectedTool;
	private bool linkingCells = false;
	private uint mouseButton;

	public MainWindow(): base (Gtk.WindowType.Toplevel) {
		Build();
	}
	


	protected void OnDeleteEvent(object sender, DeleteEventArgs a) {
		Application.Quit();
		a.RetVal = true;
	}



	protected void onNew(object sender, EventArgs e) {
		newCircuit();
	}



	protected void onDAExposed(object o, ExposeEventArgs args) {
		circuitAera = (DrawingArea)o;
	}



	protected void onRedraw(object sender, EventArgs e) {
		if (circuit != null) {
			circuit.draw();
		}
	}



	protected void onSP(object sender, EventArgs e) {
		selectedTool = ToolType.SILICON_P;
	}



	protected void onSN(object sender, EventArgs e) {
		selectedTool = ToolType.SILICON_N;
	}



	protected void onMetal(object sender, EventArgs e) {
		selectedTool = ToolType.METAL;
	}



	protected void onDelM(object sender, EventArgs e) {
		selectedTool = ToolType.DEL_M;
	}



	protected void onDelS(object sender, EventArgs e) {
		selectedTool = ToolType.DEL_S;
	}



	protected void onSize(object o, SizeAllocatedArgs args) {
		if (circuit != null)
			circuit.draw();
	}



	protected void onDAButtonRelease(object o, ButtonReleaseEventArgs args) {
		if (circuit == null)
			return;
		linkingCells = false;
		circuit.stopLinking();
	}



	protected void onDAButtonPress(object o, ButtonPressEventArgs args) {

		if (circuit == null)
			return;

		mouseButton = args.Event.Button;

		switch (mouseButton) {

			case 1: // bouton 1
				clickCell(args.Event.X, args.Event.Y);


				break;

		}
	}



	protected void onDAMotionNotify(object o, MotionNotifyEventArgs args) {

		if (circuit == null)
			return;

		if (selectedTool == ToolType.LINK_CHK) {
			circuit.linkCheck(args.Event.X, args.Event.Y);
		}

		if (selectedTool == ToolType.DEL_M || selectedTool == ToolType.DEL_S) {
			clickCell(args.Event.X, args.Event.Y);
		}


		if (!linkingCells)
			return;

		switch (mouseButton) {

			case 1: // bouton 1

				switch (selectedTool) {
					case ToolType.SILICON_N:
						if (linkingCells)
							clickCell(args.Event.X, args.Event.Y);
						break;
					case ToolType.SILICON_P:
						if (linkingCells)
							clickCell(args.Event.X, args.Event.Y);
						break;
					case ToolType.METAL:
						if (linkingCells)
							clickCell(args.Event.X, args.Event.Y);
						break;
				}
				break;

		}
	}



	private void clickCell(double x, double y) {
		switch (selectedTool) {
			case ToolType.SILICON_N:
				linkingCells = true;
				circuit.createCellLinks(x, y, Cell.LayerType.SILICON_N);
				break;

			case ToolType.SILICON_P:
				linkingCells = true;
				circuit.createCellLinks(x, y, Cell.LayerType.SILICON_P);
				break;

			case ToolType.METAL:
				linkingCells = true;
				circuit.createCellLinks(x, y, Cell.LayerType.METAL);
				break;

			case ToolType.VIA:
				circuit.createVia(x, y);
				break;

			case ToolType.DEL_M:
				circuit.deleteLayer(x, y, Cell.LayerType.METAL);
				break;

			case ToolType.DEL_S:
				circuit.deleteLayer(x, y, Cell.LayerType.SILICON_N);
				circuit.deleteLayer(x, y, Cell.LayerType.SILICON_P);
				break;

			case ToolType.IO:
				circuit.createIO(x, y);
				break;
		}
	}

	private void newCircuit() {
		circuit = new Circuit(circuitAera, cellCount);
		sim = new Sim(circuit);
	}

	protected void onLinkChk(object sender, EventArgs e) {
		selectedTool = ToolType.LINK_CHK;
	}



	protected void onVia(object sender, EventArgs e) {
		selectedTool = ToolType.VIA;
	}



	protected void OnLblCellCountExposeEvent(object o, ExposeEventArgs args) {
		cellCount = (Label)o;
	}



	protected void onInput(object sender, EventArgs e) {
		selectedTool = ToolType.IO;
	}


	protected void showHideS(object sender, EventArgs e) {
		if (circuit != null)
			circuit.layerVisible(Circuit.showLayerType.S, ((ToggleAction)sender).Active);
	}

	protected void showHideM(object sender, EventArgs e) {
		if (circuit != null)
			circuit.layerVisible(Circuit.showLayerType.M, ((ToggleAction)sender).Active);
	}

	protected void OnSimStepActionActivated(object sender, EventArgs e) {
		sim.tick();
	}
}