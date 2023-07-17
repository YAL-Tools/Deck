using CropperDeck.Properties;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace CropperDeck {
	public class HomePanel : Panel {
		public DeckPicker DeckPicker;
		public Button BtLoad;
		public Button BtMenu;
		public MainForm MainForm = null;
		public ContextMenuStrip CmActions;
		public string DeckName {
			get => BtLoad.Text;
			set => BtLoad.Text = value;
		}
		public HomePanel(DeckPicker deckPicker, string name) : base() {
			DeckPicker = deckPicker;
			Margin = new Padding(12, 12, 12, 12);
			Dock = DockStyle.Top;
			Height = 40;
			Action<Control> vcenter = (ctl) => {
				ctl.Height = 30;
				ctl.Top = (Height - ctl.Height) / 2;
			};

			CmActions = new ContextMenuStrip();

			#region Change Name
			var tfName = new ToolStripTextBox();
			tfName.Text = name;
			tfName.KeyDown += (sender, args) => {
				if (args.KeyCode == Keys.Enter) {
					var newName = tfName.Text;
					if (!DeckPicker.IsValidDeckName(newName)) {
						MessageBox.Show(
							$"Can't change deck name to \"{newName}\" - not a valid filename",
							DeckPicker.Text, MessageBoxButtons.OK, MessageBoxIcon.Error
						);
						return;
					}
					try {
						File.Move(DeckState.GetPath(DeckName), DeckState.GetPath(newName));
					} catch (Exception ex) {
						MessageBox.Show(
							$"Can't change deck name to \"{newName}\":\n" + ex,
							DeckPicker.Text, MessageBoxButtons.OK, MessageBoxIcon.Error
						);
						return;
					}
					if (MainForm != null) {
						MainForm.DeckName = newName;
					}
					DeckName = newName;
					args.Handled = true;
					args.SuppressKeyPress = true;
					CmActions.Close();
				}
			};
			CmActions.Items.Add(tfName);
			#endregion

			#region Copy colors
			var tbCopyColors = new ToolStripButton("Copy custom colors");
			tbCopyColors.Click += (sender, args) => {
				DeckColors cc;
				if (MainForm != null) {
					cc = MainForm.CustomColors;
				} else cc = DeckState.Load(DeckName).CustomColors;
				cc.Enabled = true;
				DeckPicker.CustomColors = cc;
				DeckPicker.SyncCustomColors();
				DeckPicker.CbCustomColors.Checked = true;
			};
			CmActions.Items.Add(tbCopyColors);
			#endregion

			#region Delete
			var tbDelete = new ToolStripButton("Delete");
			tbDelete.Image = Resources.delete;
			tbDelete.Click += (sender, args) => {
				if (MessageBox.Show(
					$"Are you sure you want to delete deck \"{DeckName}\"? This cannot be undone!",
					DeckPicker.Text,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning
				) != DialogResult.Yes) return;

				MainForm?.Close();
				try {
					File.Delete(DeckState.GetPath(DeckName));
				} catch (Exception ex) {
					MessageBox.Show(
						"Couldn't delete the deck file:\n" + ex,
						DeckPicker.Text, MessageBoxButtons.OK, MessageBoxIcon.Error
					);
					return;
				}
			};
			CmActions.Items.Add(tbDelete);
			#endregion

			CmActions.Opening += (sender, args) => {
				tfName.Text = DeckName;
			};
			ContextMenuStrip = CmActions;

			BtMenu = new Button();
			BtMenu.Image = Resources.cog;
			BtMenu.Width = 30;
			BtMenu.Anchor = AnchorStyles.Right;
			BtMenu.Left = Width - BtMenu.Width - 6;
			BtMenu.Click += (sender, args) => {
				CmActions.Show(this, PointToClient(MousePosition));
			};
			var w = BtMenu.Width;
			vcenter(BtMenu);
			BtMenu.AutoSize = false;
			BtMenu.Width = w;
			Controls.Add(BtMenu);

			BtLoad = new Button();
			var loadPad = BtLoad.Padding;
			loadPad.Left += 12;
			BtLoad.Padding = loadPad;
			BtLoad.TextAlign = ContentAlignment.MiddleLeft;
			BtLoad.Text = name;
			BtLoad.Anchor = AnchorStyles.Left | AnchorStyles.Right;
			vcenter(BtLoad);
			BtLoad.Left = 6;
			BtLoad.Width = BtMenu.Left - 12;
			BtLoad.Click += (sender, args) => { Open(); };
			Controls.Add(BtLoad);

			ApplyColors(DeckPicker.CustomColors);
		}
		public void Open() {
			if (MainForm != null) {
				MainForm.Focus();
				if (DeckPicker.CbAutoHide.Checked) { DeckPicker.Hide(); }
				return;
			}
			if (DeckPicker.OverlayServer == null) DeckPicker.StartOverlayServer();
			MainForm = new MainForm(this);
			MainForm.Show();
			if (DeckPicker.CbAutoHide.Checked) { DeckPicker.Hide(); }
		}
		public void AddTo(Panel ctr) {
			var ctrCount = ctr.Controls.Count;
			var top = (ctrCount > 0 ? ctr.Controls[ctrCount - 1].Bottom : 0) + Margin.Top;
			Top = top;
			Left = Margin.Left;
			Width = ctr.Width - Left - Margin.Right;
			ctr.Controls.Add(this);
			ctr.Controls.SetChildIndex(this, 0);
		}
		public void ApplyColors(DeckColors cc) {
			cc.ApplyToButton(BtLoad);
			cc.ApplyToButton(BtMenu);
			cc.ApplyToToolStrip(CmActions);
		}
	}
}
