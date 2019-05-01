using CritterRobots.Critters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CritterRobots.Forms
{
	public partial class CoachWindow : Form
	{
		/// <summary>
		/// The reference coach critter on which to call for the generation end.
		/// </summary>
		private CritterCoach ReferenceCoach { get; }

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CoachWindow()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Creates a new coach configuration window while
		/// keeping a reference to the original coach object.
		/// </summary>
		public CoachWindow(CritterCoach reference) : this()
		{
			ReferenceCoach = reference;
		}

		/// <summary>
		/// Ends this generation.
		/// </summary>
		private void resolutionButton_Click(object sender, EventArgs e)
		{
			ReferenceCoach?.FinishGeneration();
			Close();
		}

		/// <summary>
		/// Allows to pick a potentially escaped critter that was not
		/// picked up by the CritterWorld messaging system.
		/// </summary>
		private void checkEscapedCritter_Click(object sender, EventArgs e)
		{
			MessageBox.Show("I don't care.", "Coach!");
			EscapeInformer informer = new EscapeInformer();

			informer.ShowDialog();
			if (informer.DialogResult == DialogResult.OK)
			{
				if (informer.CritterID >= 0 && informer.CritterID < ReferenceCoach.StudentsCount)
				{
					ReferenceCoach.SetStudentEscaped(informer.CritterID);
				}
				else
				{
					MessageBox.Show("This is bogus! Are you trying to kill me?", "Coach!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				}
			}
		}
	}
}
