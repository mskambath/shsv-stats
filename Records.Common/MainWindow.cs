using System;
using Xwt;
using Xwt.Drawing;
using System.Xml;

namespace Records.Common
{
	public class MainWindow: Window
	{

		ListStore store;
		Image icon;
		VBox sampleBox;
		Label title;
		/*Widget currentSample;*/
		
/*		DataField<string> nameCol = new DataField<string> ();
		DataField<Sample> widgetCol = new DataField<Sample> ();
		DataField<Image> iconCol = new DataField<Image> ();*/
		
		StatusIcon statusIcon;
		
		public MainWindow ()
		{
			Title = "SHSV Rekordverwaltung";
			Width = 500;
			Height = 400;
			/*
			try {
				statusIcon = Application.CreateStatusIcon ();
				statusIcon.Menu = new Menu ();
				statusIcon.Menu.Items.Add (new MenuItem ("Test"));
				statusIcon.Image = Image.FromResource (GetType (), "package.png");
			} catch {
				Console.WriteLine ("Status icon could not be shown");
			}
			*/

			Menu menu = new Menu ();
			var file = new MenuItem ("File");
			file.SubMenu = new Menu ();
			MenuItem mi_newView = new MenuItem("New Record Table");
			file.SubMenu.Items.Add(mi_newView);
			mi_newView.Clicked += HandleCreateNewView;

			MenuItem mi_newRecords = new MenuItem("Find New Records");
			file.SubMenu.Items.Add(mi_newRecords);
			mi_newRecords.Clicked += HandleFindNewRecords;

			MenuItem mi_exit = new MenuItem ("Exit");
			file.SubMenu.Items.Add(mi_exit);
			mi_exit.Clicked += delegate {
				Application.Exit();
			};
			
			MainMenu = menu;
			menu.Items.Add(file);

			HPaned box = new HPaned ();
			VBox vbox = new VBox();

			icon = Image.FromResource (typeof(App), "document-generic.png");




			ListView lvViews = new ListView();
			var tfLabel = new DataField<string>();
			var tfCourse = new DataField<string>();
			var tfType = new DataField<string>();
			var tfSex = new DataField<string>();
			var tfAK = new DataField<string>();

			store = new ListStore(tfLabel,tfCourse,tfType,tfSex,tfAK);
			
			lvViews.DataSource = store;
			lvViews.GridLinesVisible = GridLines.Horizontal;


			lvViews.Columns.Add(new ListViewColumn("Bezeichnung", new TextCellView { Editable = false, TextField = tfLabel }));
			lvViews.Columns.Add(new ListViewColumn("Bahn", new TextCellView { Editable = false, TextField = tfCourse }));
			lvViews.Columns.Add(new ListViewColumn("Geschlecht", new TextCellView { Editable = false, TextField = tfSex }));
			lvViews.Columns.Add(new ListViewColumn("AK", new TextCellView { Editable = false, TextField = tfAK }));



			for (int n = 0; n < 10; n++)
			{
				var r = store.AddRow();
				store.SetValue(r, tfLabel, "Jahrgangsrekorde" );
				store.SetValue(r, tfCourse, "50m" );
				store.SetValue(r, tfSex, "männlich");
				store.SetValue(r, tfAK, "AK "+n);
			}


			box.Panel1.Content = vbox;
			vbox.PackStart(lvViews, true);
			var btn = new Button("Add Row");
			btn.Clicked += delegate {
				var row = store.AddRow();
				store.SetValues(row, tfLabel, "New editable text", tfLabel, "New non-editable text");
				lvViews.SelectRow(row);//.StartEditingCell(row, textCellView);
			};
			vbox.PackStart(btn, false, hpos: WidgetPlacement.Start);

			//var btnbox = new VBox();
			//vbox.Panel2.Content = btnbox;

			var b1 = new  Button();
			b1.Label = "Test";
			//btnbox.PackStart(b1);
			var b2 = new Button();
			b2.Label = "Test2";
			//btnbox.PackStart(b2);

			vbox.PackStart(b1, false, hpos: WidgetPlacement.Start);
			vbox.PackStart(b2, false, hpos: WidgetPlacement.Start);

			sampleBox = new VBox ();
			title = new Label ("Sample:");
			sampleBox.PackStart (title);
			
			box.Panel2.Content = sampleBox;
			box.Panel2.Resize = true;
			box.Position = 160;

			Content = box;
			
			/*samplesTree.SelectionChanged += HandleSamplesTreeSelectionChanged;*/

			CloseRequested += HandleCloseRequested;
		}

		void HandleCloseRequested (object sender, CloseRequestedEventArgs args)
		{
			/*args.AllowClose = MessageDialog.Confirm ("Samples will be closed", Command.Ok);
			if (args.AllowClose)*/
				Application.Exit ();
		}
		
		protected override void Dispose (bool disposing)
		{
			base.Dispose (disposing);
			
			if (statusIcon != null) {
				statusIcon.Dispose ();
			}
		}

		void HandleCreateNewView(object sender, EventArgs e)
		{
			Dialog d = new Dialog();
			d.Title = "Erstelle eine neue Rekordliste";
			var cbCourse = new ComboBox();
			cbCourse.Items.Add("25m");
			cbCourse.Items.Add("50m");
			cbCourse.Items.Add("Freiwasser");
			var cbSex = new ComboBox();
			cbSex.Items.Add("männlich");
			cbSex.Items.Add("weiblich");
			cbSex.Items.Add("mixed");
			var cbType = new ComboBox();
			cbType.Items.Add("Einzel");
			cbType.Items.Add("Staffel");
			var teAK = new TextEntry();
			var teLabel = new TextEntry();
			teLabel.MinWidth = 400;

			Table t = new Table();
			t.Add(new Label("Bezeichnung:"), 0, 0);
			t.Add(teLabel, 1, 0);
			t.Add(new Label("Bahnlänge:"), 0, 1);
			t.Add(cbCourse, 1, 1);
			t.Add(new Label("Geschlecht:"), 0, 2);
			t.Add(cbSex, 1, 2);
			t.Add(new Label("Typ:"), 0, 3);
			t.Add(cbType, 1, 3);
			t.Add(new Label("AK:"), 0, 4);
			t.Add(teAK, 1, 4);
			t.MinWidth = 200;
			d.Content = t;
		


			d.Buttons.Add(new DialogButton(Command.Cancel));
			d.Buttons.Add(new DialogButton(Command.Ok));

			d.DefaultCommand = Command.Ok;

			/*d.CommandActivated += (sender, e) => {
				if (e.Command == custom)
				{
							
					e.Handled = !MessageDialog.Confirm("Really close?", Command.Close);
				}
			};*/

			var r = d.Run(this);
			/*var r = d.Run(this.ParentWindow);
			db.Label = "Result: " + (r != null ? r.Label : "(Closed)");*/
			d.Dispose();
		}

		void HandleFindNewRecords(object sender, EventArgs e)
		{

		}

		void HandleSamplesTreeSelectionChanged (object sender, EventArgs e)
		{
			/*if (samplesTree.SelectedRow != null) {
				if (currentSample != null)
					sampleBox.Remove (currentSample);
				Sample s = store.GetNavigatorAt (samplesTree.SelectedRow).GetValue (widgetCol);
				if (s.Type != null) {
					if (s.Widget == null)
						s.Widget = (Widget)Activator.CreateInstance (s.Type);
					sampleBox.PackStart (s.Widget, true);
				}

			//	Console.WriteLine (System.Xaml.XamlServices.Save (s.Widget));
				currentSample = s.Widget;
				Dump (currentSample, 0);
			}*/
		}
		
		/*void Dump (IWidgetSurface w, int ind)
		{
			if (w == null)
				return;
			var s = w.GetPreferredSize ();
			Console.WriteLine (new string (' ', ind * 2) + " " + w.GetType ().Name + " " + s.Width + " " + s.Height);
			foreach (var c in w.Children)
				Dump (c, ind + 1);
		}
		
		TreePosition AddSample (TreePosition pos, string name, Type sampleType)
		{
			//if (page != null)
			//	page.Margin.SetAll (5);
			return store.AddNode (pos).SetValue (nameCol, name).SetValue (iconCol, icon).SetValue (widgetCol, new Sample (sampleType)).CurrentPosition;
		}*/
	}
	
	/*class Sample
	{
		public Sample (Type type)
		{
			Type = type;
		}

		public Type Type;
		public Widget Widget;
	}*/

	class View
	{
		public string label;
	}
}

