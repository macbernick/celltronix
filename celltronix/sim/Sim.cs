using System;



namespace celltronix {
	public class Sim {

		private Circuit circuit;

		public Sim(Circuit circuit) {
			this.circuit = circuit;
		}


		public void run() {

		}


		public void tick() {

			foreach (InOut io in circuit.ios) {
				io.tick();
			}

		}

	}
}

