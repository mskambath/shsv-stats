using System;
using System.Timers;
using Xwt;

namespace Records.Common
{
	public class ProgressBarSample : VBox
	{
		Timer timer = new Timer (100);
		ProgressBar determinateProgressBar;
		ProgressBar indeterminateProgressBar;

		public ProgressBarSample ()
		{
			PackStart (new ProgressBar { Fraction = 0 });
			PackStart (new ProgressBar { Fraction = 0.5d, MinHeight = 20 });
			PackStart (new ProgressBar { Fraction = 1 });
			indeterminateProgressBar = new ProgressBar ();
			PackStart (indeterminateProgressBar, true);
			indeterminateProgressBar.Indeterminate = true;

			timer.Elapsed += Increase;
			determinateProgressBar = new ProgressBar ();
			determinateProgressBar.Fraction = 0.0;
			PackStart(determinateProgressBar, true);
			timer.Start ();

			var spinner = new Spinner ();
			spinner.Animate = true;
			PackStart (spinner);
		}

		public void Increase (object sender, ElapsedEventArgs args)
		{
			double nextFraction;
			double? currentFraction = determinateProgressBar.Fraction;
			if (currentFraction != null && currentFraction.Value >= 0.0 && currentFraction.Value <= 0.9) {
				nextFraction = currentFraction.Value + 0.1;
			} else {
				nextFraction = 0.0;
			}
			Application.Invoke ( () => determinateProgressBar.Fraction = nextFraction );
		}
	}
}

