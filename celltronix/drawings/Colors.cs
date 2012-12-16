using System;
using System.Collections.Generic;
using Cairo;


namespace celltronix {
	public class Colors {

		private Dictionary<string, Color> color;

		public Colors() {
			this.color = new Dictionary<string, Color>();
			this.color.Add("background", 		new Color(0.2d, 0.2d, 0.2d));
			this.color.Add("grid", 				new Color(1.0d, 1.0d, 1.0d, 0.15d));

			this.color.Add("link_chk",			new Color(0d, 0.5d, 1d, 0.4d));

			this.color.Add("via",				new Color(0d, 0d, 0d));

			this.color.Add("metal_fill",		new Color(0.7d, 0.7d, 0.7d, 0.6d));

			this.color.Add("snN_fill",			new Color(0.2d, 0.2d, 1.0d));

			this.color.Add("snP_fill",			new Color(0.2d, 1.0d, 0.2d));

			this.color.Add("gate_NPN",			new Color(0.0d, 0.7d, 0.7d));
			this.color.Add("gate_PNP",			new Color(0.0d, 0.7d, 0.7d));
			this.color.Add("powered",			new Color(1.0d, 1.0d, 0.0d, 0.5d));
		}

		public Color find(string c) {
			return color[c];
		}
	}
}

