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
	public partial class EscapeInformer : Form
	{
		/// <summary>
		/// The extrapolated critter ID.
		/// </summary>
		public int CritterID {
			get
			{
				return (int)critterID.Value;
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public EscapeInformer()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Exit with an OK dialog result.
		/// </summary>
		private void OK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			Close();
		}

		/// <summary>
		/// Exit with a Cancel dialog result.
		/// </summary>
		private void cancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}
