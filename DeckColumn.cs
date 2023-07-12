using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CropperDeck.Properties;

namespace CropperDeck {
	public class DeckColumn : Panel {
		public string ColumnName = "Column";
		public string IconName = "";
		public string IconPath { get => $"images/{IconName}.png";  }
		public MainForm MainForm;

		public ToolStrip ToolStrip;
		public ToolStripButton TbInsert;
		public ToolStripButton TbExpand;
		public ToolStripTextBox TfWidth;
		public ToolStripDropDownButton TbPadding;
		public ToolStripDropDownButton TbConfig;
		public ToolStripTextBox TfName;
		public DeckWindow Window = null;
		public CropMargins CropMargins = CropMargins.Zero;
		public Panel WindowCtr;
		public ToolStripSpringLabel TlName;
		public Point OverlaySize = new Point(0, 0);
		public DeckColumn(MainForm mainForm) {
			MainForm = mainForm;

			BackColor = Color.Transparent;
			//BorderStyle = BorderStyle.FixedSingle;
			Dock = DockStyle.Left;
			Width = 200;
			Margin = new Padding(0, 0, 1, 0);

			TbInsert = new ToolStripButton("Insert/remove window", Resources.application);
			TbInsert.DisplayStyle = ToolStripItemDisplayStyle.Image;
			TbInsert.Click += new EventHandler(TbInsert_Click);

			TmPadding_Init();
			FdWidth_Init();
			TbConfig_Init();
			TlName = new ToolStripSpringLabel();
			TlName.TextAlign = ContentAlignment.MiddleLeft;
			TlName.Text = ColumnName;

			TbExpand = new ToolStripButton("Expand", Resources.arrow_out);
			TbExpand.DisplayStyle = ToolStripItemDisplayStyle.Image;
			TbExpand.Click += (object sender, EventArgs e) => {
				ShowOverlay();
			};

			ToolStrip = new ToolStrip(new ToolStripItem[] { TbInsert, TbPadding, TfWidth, TbExpand, TbConfig, TlName });

			WindowCtr = new Panel();
			WindowCtr.Dock = DockStyle.Fill;

			Controls.Add(WindowCtr);
			Controls.Add(ToolStrip);
		}
		public void Destroy() {
			Window?.Eject();
			MainForm.PanCtr.Controls.Remove(this);
			MainForm.RebuildQuickAccess();
			MainForm.FlushConfig();
			TbConfig.Image = null;
			if (IconName != "") {
				if (File.Exists(IconPath)) File.Delete(IconPath);
			}
		}
		public void ChangeName(string newName) {
			if (newName == ColumnName) return;
			ColumnName = newName;
			TlName.Text = newName;
			MainForm.RebuildQuickAccess();
			MainForm.FlushConfig();
		}
		public static Image LoadImage(string path) {
			var bytes = File.ReadAllBytes(path);
			var stream = new MemoryStream(bytes);
			return Image.FromStream(stream);
		}

		public void ShowOverlay() {
			if (Window == null) return;
			MainForm.ShowOverlay(true);
			var panOverlay = MainForm.PanOverlay;
			MainForm.PanOverlayColumn = this;
			WinAPI_Rect rect;
			WinAPI.GetWindowRect(Window.Handle, out rect);
			WinAPI.SetParent(Window.Handle, panOverlay.Handle);
			if (!OverlaySize.IsEmpty && OverlaySize.X < panOverlay.Width && OverlaySize.Y < panOverlay.Height) {
				WinAPI.SetWindowRect(Window.Handle,
					(panOverlay.Width - OverlaySize.X) / 2,
					(panOverlay.Height - OverlaySize.Y) / 2,
					OverlaySize.X,
					OverlaySize.Y);
			} else {
				WinAPI.SetWindowRect(Window.Handle, 40, 10, panOverlay.Width - 80, panOverlay.Height - 20);
			}
		}
		public void HideOverlay() {
			if (Window != null) {
				WinAPI_Rect rect;
				WinAPI.GetWindowRect(Window.Handle, out rect);
				OverlaySize.X = rect.Width;
				OverlaySize.Y = rect.Height;
				Window.Insert();
				Window.Update();
			}
			MainForm.ShowOverlay(false);
			MainForm.PanOverlayColumn = null;
		}

		private void TmPadding_Init() {
			TbPadding = new ToolStripDropDownButton("Crop...", Resources.shape_handles);
			TbPadding.DisplayStyle = ToolStripItemDisplayStyle.Image;
			TmPadding_Build();
		}
		public void TmPadding_Build() {
			TbPadding.DropDownItems.Clear();
			foreach (var m in MainForm.CropMargins) {
				var tb = new ToolStripButton(m.Name);
				tb.TextAlign = ContentAlignment.MiddleLeft;
				//tb.AutoSize = true;
				tb.Width = 80;
				tb.Click += (object sender, EventArgs e) => {
					CropMargins = m;
					Window?.Update();
					MainForm.FlushConfig();
				};
				TbPadding.DropDownItems.Add(tb);
			}
		}
		private void TbConfig_Init() {
			TbConfig = new ToolStripDropDownButton("Edit...", Resources.pencil);
			TbConfig.DisplayStyle = ToolStripItemDisplayStyle.Image;

			TfName = new ToolStripTextBox();
			TfName.Text = ColumnName;
			TfName.KeyDown += (object sender, KeyEventArgs e) => {
				if (e.KeyCode == Keys.Enter) ChangeName(TfName.Text);
			};
			TfName.LostFocus += (object sender, EventArgs e) => {
				ChangeName(TfName.Text);
			};
			TbConfig.DropDownItems.Add(TfName);

			var tbIcon = new ToolStripButton("Change icon", Resources.pencil);
			tbIcon.Click += (object sender, EventArgs e) => {
				MainForm.OnIconPick = (string path) => {
					if (IconName != "") {
						if (File.Exists(IconPath)) File.Delete(IconPath);
					}
					if (!Directory.Exists("images")) {
						Directory.CreateDirectory("images");
					}
					IconName = Guid.NewGuid().ToString();
					File.Copy(path, IconPath);
					TbConfig.Image = LoadImage(IconPath);
					MainForm.RebuildQuickAccess();
					MainForm.FlushConfig();
				};
				MainForm.DlgIconPicker.ShowDialog();
			};
			TbConfig.DropDownItems.Add(tbIcon);

			var tbLeft = new ToolStripButton("Move left", Resources.arrow_left);
			tbLeft.Click += (object sender, EventArgs e) => {
				var ind = MainForm.PanCtr.Controls.GetChildIndex(this);
				if (ind < MainForm.PanCtr.Controls.Count - 1) {
					MainForm.PanCtr.Controls.SetChildIndex(this, ind + 1);
					MainForm.RebuildQuickAccess();
					MainForm.FlushConfig();
				}
			};
			TbConfig.DropDownItems.Add(tbLeft);

			var tbRight = new ToolStripButton("Move Right", Resources.arrow_right);
			tbRight.Click += (object sender, EventArgs e) => {
				var ind = MainForm.PanCtr.Controls.GetChildIndex(this);
				if (ind > 0) {
					MainForm.PanCtr.Controls.SetChildIndex(this, ind - 1);
					MainForm.RebuildQuickAccess();
					MainForm.FlushConfig();
				}
			};
			TbConfig.DropDownItems.Add(tbRight);

			var tbDelete = new ToolStripButton("Remove", Resources.delete);
			tbDelete.Click += (object sender, EventArgs e) => {
				if (MessageBox.Show(
					"Are you sure you want to delete this column? This cannot be undone!",
					MainForm.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning
				) != DialogResult.Yes) return;
				Destroy();
			};
			TbConfig.DropDownItems.Add(tbDelete);
		}

		private void FdWidth_Init() {
			TfWidth = new ToolStripTextBox();
			TfWidth.ToolTipText = "Column width (in pixels)";
			TfWidth.Width = 30;
			TfWidth.Font = new Font(TfWidth.Font.FontFamily, 7f);
			var margin = TfWidth.Margin;
			margin.Top = 1;
			margin.Bottom = 1;
			TfWidth.Margin = margin;
			TfWidth.BorderStyle = BorderStyle.FixedSingle;
			TfWidth.Text = "";
			TfWidth.LostFocus += FdWidth_TextChanged;
			TfWidth.KeyDown += FdWidth_KeyDown;
		}

		private void FdWidth_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) {
				FdWidth_TextChanged(sender, e);
				e.Handled = true;
			}
		}

		private void FdWidth_TextChanged(object sender, EventArgs e) {
			if (!int.TryParse(TfWidth.Text, out int newWidth)) {
				TfWidth.Text = "" + Width;
				return;
			}
			if (Width != newWidth) {
				Width = newWidth;
				MainForm.FlushConfig();
			}
			Window?.Update();
		}

		public void TbInsert_Click(object sender, EventArgs e) {
			if (Window != null) {
				Window.Eject();
				Window = null;
				return;
			}
			WindowPicker.Open((IntPtr hwnd) => {
				if (hwnd == IntPtr.Zero) return;
				Window?.Eject();
				Window = new DeckWindow(hwnd, this);
				if (TfWidth.Text == "") {
					Width = Window.GetWidth();
					TfWidth.Text = "" + Width;
				}
				Window.Insert();
				Window.Update();
			}, this.FindForm());
		}
		public void UpdateCrop() {
			Window?.Update();
		}
	}
}
