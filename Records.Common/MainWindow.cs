using System;
using Xwt;
using Xwt.Drawing;
using System.Xml;

namespace Records.Common
{
	public class MainWindow: Window
	{

		ListStore storeData;
		ListStore storeViews;
		ListView lvViews;
		ListView lvData;

		Image icon;
		VBox sampleBox;
		Label title;

		/*Widget currentSample;*/
		
/*		DataField<string> nameCol = new DataField<string> ();
		DataField<Sample> widgetCol = new DataField<Sample> ();
		DataField<Image> iconCol = new DataField<Image> ();*/
		

		public MainWindow ()
		{
			Title = "SHSV Rekordverwaltung";
			Width = 500;
			Height = 400;
	

			Menu menu = new Menu ();
			var file = new MenuItem ("Datei");
			file.SubMenu = new Menu ();
			MenuItem mi_newView = new MenuItem("Neue Tabelle");
			file.SubMenu.Items.Add(mi_newView);
			mi_newView.Clicked += HandleCreateNewView;

			MenuItem mi_newRecords = new MenuItem("Finde neue Rekorde");
			file.SubMenu.Items.Add(mi_newRecords);
			mi_newRecords.Clicked += HandleFindNewRecords;

			MenuItem mi_exit = new MenuItem ("Schließen");
			file.SubMenu.Items.Add(mi_exit);
			mi_exit.Clicked += delegate {
				Application.Exit();
			};
			
			MainMenu = menu;
			menu.Items.Add(file);

			HPaned box = new HPaned ();
			VBox vbox = new VBox();

			icon = Image.FromResource (typeof(App), "document-generic.png");




			lvViews= new ListView();
			lvData = new ListView();

			var tfLabel = new DataField<string>();
			var tfCourse = new DataField<string>();
			var tfType = new DataField<string>();
			var tfSex = new DataField<string>();
			var tfAK = new DataField<string>();

			var tfDisc = new DataField<string>();
			var tfSwimmerName = new DataField<string>();
			var tfSwimmerId = new DataField<string>();
			var tfClub = new DataField<string>();
			var tfClubId = new DataField<string>();
			var tfAge = new DataField<string>();
			var tfTime = new DataField<string>();
			var tfDate = new DataField<DateTime>();

			storeViews = new ListStore(tfLabel,tfCourse,tfType,tfSex,tfAK);
			storeData = new ListStore(tfDisc, tfSwimmerName, tfSwimmerId, tfClub, tfClubId, tfAge, tfDate, tfTime);

			lvViews.DataSource = storeViews;
			lvViews.GridLinesVisible = GridLines.Horizontal;
			lvViews.Columns.Add(new ListViewColumn("Bezeichnung", new TextCellView { Editable = false, TextField = tfLabel }));
			lvViews.Columns.Add(new ListViewColumn("Bahn", new TextCellView { Editable = false, TextField = tfCourse }));
			lvViews.Columns.Add(new ListViewColumn("Geschlecht", new TextCellView { Editable = false, TextField = tfSex }));
			lvViews.Columns.Add(new ListViewColumn("AK", new TextCellView { Editable = false, TextField = tfAK }));


			var ageCol = new ItemCollection();
			ageCol.Add(0);
			ageCol.Add(1);
			ageCol.Add(2);
			ageCol.Add(1991);

			lvData.DataSource = storeData;
			lvData.Columns.Add(new ListViewColumn("Strecke",   new TextCellView { Editable = false, TextField = tfDisc }));
			lvData.Columns.Add(new ListViewColumn("Schwimmer", new TextCellView { Editable = true, TextField = tfSwimmerName }));
			lvData.Columns.Add(new ListViewColumn("ID", new TextCellView { Editable = true, TextField = tfSwimmerId }));
			lvData.Columns.Add(new ListViewColumn("Club", new TextCellView { Editable = true, TextField = tfClub }));
			lvData.Columns.Add(new ListViewColumn("ClubId", new TextCellView { Editable = true, TextField = tfClubId }));
			lvData.Columns.Add(new ListViewColumn("Alter", new ComboBoxCellView { Editable = true, SelectedTextField = tfAge, Items = ageCol }));
			lvData.Columns.Add(new ListViewColumn("Datum", new TextCellView { Editable = true, TextField = tfDate }));
			lvData.Columns.Add(new ListViewColumn("Zeit", new TextCellView { Editable = true, TextField = tfTime }));



			for (int n = 0; n < 10; n++)
			{
				var r = storeViews.AddRow();
				storeViews.SetValue(r, tfLabel, "Jahrgangsrekorde" );
				storeViews.SetValue(r, tfCourse, "50m" );
				storeViews.SetValue(r, tfSex, "männlich");
				storeViews.SetValue(r, tfAK, "AK "+n);

				 r = storeData.AddRow();
				storeData.SetValue(r, tfDisc, "50R");
				storeData.SetValue(r, tfSwimmerName, "Skambath, Malte");
				storeData.SetValue(r, tfSwimmerId, "127006");
				storeData.SetValue(r, tfClub, "50R");
				storeData.SetValue(r, tfClubId, "0000");
				storeData.SetValue(r, tfAge, "1991");
				storeData.SetValue<DateTime>(r, tfDate, DateTime.Now.Date);
				storeData.SetValue(r, tfTime, "00:00,00");
			}
			lvViews.SelectionChanged += OnSelectView;
			lvViews.ButtonPressed += OnSelectView;
			lvViews.KeyPressed += OnSelectView;
			lvViews.RowActivated += OnSelectView;
			lvViews.GotFocus += OnSelectView;

			lvViews.SelectionMode = SelectionMode.Single;
			

			box.Panel1.Content = vbox;
			vbox.PackStart(lvViews, true);
			var btn = new Button("Add Row");
			btn.Clicked += delegate {
				var row = storeViews.AddRow();
				storeViews.SetValues(row, tfLabel, "New editable text", tfLabel, "New non-editable text");
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
			title = new Label ("");
			sampleBox.PackStart (title);
			sampleBox.PackStart(lvData,true);
			
			box.Panel2.Content = sampleBox;
			box.Panel2.Resize = true;
			box.Position = 300;

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

		void OnSelectView(object sender, EventArgs e)
		{
			storeData.Clear();
			title.Text = lvViews.DataSource.GetValue(lvViews.SelectedRow, 0).ToString() + " " + lvViews.DataSource.GetValue(lvViews.SelectedRow, 1).ToString() +
				" " +
			lvViews.DataSource.GetValue(lvViews.SelectedRow, 2).ToString() + " " + lvViews.DataSource.GetValue(lvViews.SelectedRow, 3).ToString();
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

