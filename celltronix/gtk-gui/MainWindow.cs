
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	private global::Gtk.RadioAction executeAction;
	private global::Gtk.RadioAction executeAction4;
	private global::Gtk.RadioAction executeAction5;
	private global::Gtk.RadioAction executeAction6;
	private global::Gtk.RadioAction executeAction7;
	private global::Gtk.Action newAction;
	private global::Gtk.Action openAction;
	private global::Gtk.Action saveAction;
	private global::Gtk.Action refreshAction;
	private global::Gtk.RadioAction gotoBottomAction;
	private global::Gtk.RadioAction gotoTopAction;
	private global::Gtk.RadioAction executeAction8;
	private global::Gtk.RadioAction findAction;
	private global::Gtk.Action FileAction;
	private global::Gtk.ToggleAction executeAction9;
	private global::Gtk.ToggleAction executeAction10;
	private global::Gtk.ToggleAction executeAction11;
	private global::Gtk.VBox vbox1;
	private global::Gtk.MenuBar menubar1;
	private global::Gtk.HBox hbox1;
	private global::Gtk.Toolbar toolbar1;
	private global::Gtk.DrawingArea drawingarea1;
	private global::Gtk.VBox vbox2;
	private global::Gtk.Toolbar toolbar2;
	private global::Gtk.HButtonBox hbuttonbox2;
	private global::Gtk.Toolbar toolbar3;
	private global::Gtk.Statusbar statusbar1;
	private global::Gtk.Label lblCellCount;
	
	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.executeAction = new global::Gtk.RadioAction ("executeAction", global::Mono.Unix.Catalog.GetString ("SP"), null, "gtk-execute", 0);
		this.executeAction.Group = new global::GLib.SList (global::System.IntPtr.Zero);
		this.executeAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("SP");
		w1.Add (this.executeAction, null);
		this.executeAction4 = new global::Gtk.RadioAction ("executeAction4", global::Mono.Unix.Catalog.GetString ("SN"), null, "gtk-execute", 0);
		this.executeAction4.Group = this.executeAction.Group;
		this.executeAction4.ShortLabel = global::Mono.Unix.Catalog.GetString ("SN");
		w1.Add (this.executeAction4, null);
		this.executeAction5 = new global::Gtk.RadioAction ("executeAction5", global::Mono.Unix.Catalog.GetString ("Metal"), null, "gtk-execute", 0);
		this.executeAction5.Group = this.executeAction4.Group;
		this.executeAction5.ShortLabel = global::Mono.Unix.Catalog.GetString ("Metal");
		w1.Add (this.executeAction5, null);
		this.executeAction6 = new global::Gtk.RadioAction ("executeAction6", global::Mono.Unix.Catalog.GetString ("Del S"), null, "gtk-execute", 0);
		this.executeAction6.Group = this.executeAction4.Group;
		this.executeAction6.ShortLabel = global::Mono.Unix.Catalog.GetString ("Del S");
		w1.Add (this.executeAction6, null);
		this.executeAction7 = new global::Gtk.RadioAction ("executeAction7", global::Mono.Unix.Catalog.GetString ("Del M"), null, "gtk-execute", 0);
		this.executeAction7.Group = this.executeAction4.Group;
		this.executeAction7.ShortLabel = global::Mono.Unix.Catalog.GetString ("Del M");
		w1.Add (this.executeAction7, null);
		this.newAction = new global::Gtk.Action ("newAction", null, null, "gtk-new");
		w1.Add (this.newAction, null);
		this.openAction = new global::Gtk.Action ("openAction", null, null, "gtk-open");
		w1.Add (this.openAction, null);
		this.saveAction = new global::Gtk.Action ("saveAction", null, null, "gtk-save");
		w1.Add (this.saveAction, null);
		this.refreshAction = new global::Gtk.Action ("refreshAction", global::Mono.Unix.Catalog.GetString ("Redraw"), null, "gtk-refresh");
		this.refreshAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Redraw");
		w1.Add (this.refreshAction, null);
		this.gotoBottomAction = new global::Gtk.RadioAction ("gotoBottomAction", global::Mono.Unix.Catalog.GetString ("Input"), null, "gtk-goto-bottom", 0);
		this.gotoBottomAction.Group = this.executeAction4.Group;
		this.gotoBottomAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Input");
		w1.Add (this.gotoBottomAction, null);
		this.gotoTopAction = new global::Gtk.RadioAction ("gotoTopAction", global::Mono.Unix.Catalog.GetString ("Output"), null, "gtk-goto-top", 0);
		this.gotoTopAction.Group = this.executeAction4.Group;
		this.gotoTopAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Output");
		w1.Add (this.gotoTopAction, null);
		this.executeAction8 = new global::Gtk.RadioAction ("executeAction8", global::Mono.Unix.Catalog.GetString ("Via"), null, "gtk-execute", 0);
		this.executeAction8.Group = this.executeAction4.Group;
		this.executeAction8.ShortLabel = global::Mono.Unix.Catalog.GetString ("Via");
		w1.Add (this.executeAction8, null);
		this.findAction = new global::Gtk.RadioAction ("findAction", global::Mono.Unix.Catalog.GetString ("LinkChk"), null, "gtk-find", 0);
		this.findAction.Group = this.executeAction4.Group;
		this.findAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("LinkChk");
		w1.Add (this.findAction, null);
		this.FileAction = new global::Gtk.Action ("FileAction", global::Mono.Unix.Catalog.GetString ("File"), null, null);
		this.FileAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("File");
		w1.Add (this.FileAction, null);
		this.executeAction9 = new global::Gtk.ToggleAction ("executeAction9", null, null, "gtk-execute");
		this.executeAction9.Active = true;
		w1.Add (this.executeAction9, null);
		this.executeAction10 = new global::Gtk.ToggleAction ("executeAction10", null, null, "gtk-execute");
		this.executeAction10.Active = true;
		w1.Add (this.executeAction10, null);
		this.executeAction11 = new global::Gtk.ToggleAction ("executeAction11", null, null, "gtk-execute");
		this.executeAction11.Active = true;
		w1.Add (this.executeAction11, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Events = ((global::Gdk.EventMask)(256));
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Celltronix");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='FileAction' action='FileAction'/></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox1.Add (this.menubar1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><toolbar name='toolbar1'><toolitem name='newAction' action='newAction'/><toolitem name='openAction' action='openAction'/><toolitem name='saveAction' action='saveAction'/><toolitem name='refreshAction' action='refreshAction'/></toolbar></ui>");
		this.toolbar1 = ((global::Gtk.Toolbar)(this.UIManager.GetWidget ("/toolbar1")));
		this.toolbar1.Name = "toolbar1";
		this.toolbar1.Orientation = ((global::Gtk.Orientation)(1));
		this.toolbar1.ShowArrow = false;
		this.hbox1.Add (this.toolbar1);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.toolbar1]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.drawingarea1 = new global::Gtk.DrawingArea ();
		this.drawingarea1.Events = ((global::Gdk.EventMask)(774));
		this.drawingarea1.Name = "drawingarea1";
		this.hbox1.Add (this.drawingarea1);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.drawingarea1]));
		w4.Position = 1;
		// Container child hbox1.Gtk.Box+BoxChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><toolbar name='toolbar2'><toolitem name='executeAction4' action='executeAction4'/><toolitem name='executeAction' action='executeAction'/><toolitem name='executeAction5' action='executeAction5'/><toolitem name='executeAction8' action='executeAction8'/><toolitem name='executeAction6' action='executeAction6'/><toolitem name='executeAction7' action='executeAction7'/><toolitem name='gotoBottomAction' action='gotoBottomAction'/><toolitem name='gotoTopAction' action='gotoTopAction'/><toolitem name='findAction' action='findAction'/></toolbar></ui>");
		this.toolbar2 = ((global::Gtk.Toolbar)(this.UIManager.GetWidget ("/toolbar2")));
		this.toolbar2.Name = "toolbar2";
		this.toolbar2.Orientation = ((global::Gtk.Orientation)(1));
		this.toolbar2.ShowArrow = false;
		this.vbox2.Add (this.toolbar2);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.toolbar2]));
		w5.Position = 0;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hbuttonbox2 = new global::Gtk.HButtonBox ();
		this.hbuttonbox2.Name = "hbuttonbox2";
		this.vbox2.Add (this.hbuttonbox2);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hbuttonbox2]));
		w6.Position = 1;
		w6.Expand = false;
		w6.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><toolbar name='toolbar3'><toolitem name='executeAction9' action='executeAction9'/><toolitem name='executeAction10' action='executeAction10'/></toolbar></ui>");
		this.toolbar3 = ((global::Gtk.Toolbar)(this.UIManager.GetWidget ("/toolbar3")));
		this.toolbar3.Name = "toolbar3";
		this.toolbar3.ShowArrow = false;
		this.toolbar3.ToolbarStyle = ((global::Gtk.ToolbarStyle)(0));
		this.toolbar3.IconSize = ((global::Gtk.IconSize)(2));
		this.vbox2.Add (this.toolbar3);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.toolbar3]));
		w7.Position = 2;
		w7.Expand = false;
		w7.Fill = false;
		this.hbox1.Add (this.vbox2);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.vbox2]));
		w8.Position = 2;
		w8.Expand = false;
		w8.Fill = false;
		this.vbox1.Add (this.hbox1);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox1]));
		w9.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar1 = new global::Gtk.Statusbar ();
		this.statusbar1.Name = "statusbar1";
		this.statusbar1.Spacing = 6;
		// Container child statusbar1.Gtk.Box+BoxChild
		this.lblCellCount = new global::Gtk.Label ();
		this.lblCellCount.Name = "lblCellCount";
		this.statusbar1.Add (this.lblCellCount);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.statusbar1 [this.lblCellCount]));
		w10.Position = 1;
		w10.Expand = false;
		w10.Fill = false;
		this.vbox1.Add (this.statusbar1);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar1]));
		w11.Position = 2;
		w11.Expand = false;
		w11.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 962;
		this.DefaultHeight = 600;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.SizeAllocated += new global::Gtk.SizeAllocatedHandler (this.onSize);
		this.executeAction.Activated += new global::System.EventHandler (this.onSP);
		this.executeAction4.Activated += new global::System.EventHandler (this.onSN);
		this.executeAction5.Activated += new global::System.EventHandler (this.onMetal);
		this.executeAction6.Activated += new global::System.EventHandler (this.onDelS);
		this.executeAction7.Activated += new global::System.EventHandler (this.onDelM);
		this.newAction.Activated += new global::System.EventHandler (this.onNew);
		this.refreshAction.Activated += new global::System.EventHandler (this.onRedraw);
		this.gotoBottomAction.Activated += new global::System.EventHandler (this.onInput);
		this.executeAction8.Activated += new global::System.EventHandler (this.onVia);
		this.findAction.Activated += new global::System.EventHandler (this.onLinkChk);
		this.executeAction9.Activated += new global::System.EventHandler (this.showHideM);
		this.executeAction10.Activated += new global::System.EventHandler (this.showHideS);
		this.executeAction11.Activated += new global::System.EventHandler (this.showHideM);
		this.drawingarea1.ExposeEvent += new global::Gtk.ExposeEventHandler (this.onDAExposed);
		this.drawingarea1.ButtonPressEvent += new global::Gtk.ButtonPressEventHandler (this.onDAButtonPress);
		this.drawingarea1.ButtonReleaseEvent += new global::Gtk.ButtonReleaseEventHandler (this.onDAButtonRelease);
		this.drawingarea1.MotionNotifyEvent += new global::Gtk.MotionNotifyEventHandler (this.onDAMotionNotify);
		this.lblCellCount.ExposeEvent += new global::Gtk.ExposeEventHandler (this.OnLblCellCountExposeEvent);
	}
}
