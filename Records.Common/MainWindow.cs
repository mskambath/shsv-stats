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

		DataField<string> tfDisc = new DataField<string>();
		DataField<string> tfSwimmerName = new DataField<string>();
		DataField<string> tfSwimmerId = new DataField<string>();
		DataField<string> tfClub = new DataField<string>();
		DataField<string> tfClubId = new DataField<string>();
		DataField<string> tfAge = new DataField<string>();
		DataField<string> tfTime = new DataField<string>();
		DataField<string> tfDate = new DataField<string>();

		Records.Common.Model.RecordDB db;

		Image icon;
		VBox sampleBox;


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

			db = new Records.Common.Model.RecordDB();


			lvViews= new ListView();
			lvData = new ListView();

			var tfLabel = new DataField<string>();
			var tfCourse = new DataField<string>();
			var tfType = new DataField<string>();
			var tfSex = new DataField<string>();
			var tfAK = new DataField<string>();

		

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
			lvData.Columns.Add(new ListViewColumn("Alter", new TextCellView { Editable = true, TextField = tfAge }));
			lvData.Columns.Add(new ListViewColumn("Datum", new TextCellView { Editable = true, TextField = tfDate }));
			lvData.Columns.Add(new ListViewColumn("Zeit", new TextCellView { Editable = true, TextField = tfTime }));


			foreach (var table in db.Views)
			{
				var r = storeViews.AddRow();
				storeViews.SetValue(r, tfLabel, table.Name);
				storeViews.SetValue(r, tfCourse, table.CourseType.ToString());
				storeViews.SetValue(r, tfSex, table.Sex.ToString());
				storeViews.SetValue(r, tfAK, table.AgeGroup.ToString());
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
			//sampleBox.PackStart (title);
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

		}

		void OnSelectView(object sender, EventArgs e)
		{
			Console.WriteLine("XYV");
			storeData.Clear();
			foreach(var entry in db.GetTable(db.Views.ToArray()[0])) {
				var r = storeData.AddRow();
				storeData.SetValue(r, tfDisc , entry.Discipline.ToString());
				storeData.SetValue(r, tfAge, entry.Agegroup.ToString());
				storeData.SetValue(r, tfClub, entry.Club);
				storeData.SetValue(r, tfSwimmerName, entry.Name + ", " + entry.Surname);
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

			var run = d.Run(this);
			/*var r = d.Run(this.ParentWindow);
			db.Label = "Result: " + (r != null ? r.Label : "(Closed)");*/
			d.Dispose();
		}

		void HandleFindNewRecords(object sender, EventArgs e)
		{
			Console.WriteLine(1);
			var r = storeData.AddRow();
			Console.WriteLine(2);
			storeData.SetValue(r, tfDisc, "D");
			Console.WriteLine(db.Views.Count);

			storeViews.SetValue(r, tfAge, "");
			storeViews.SetValue(r, tfClub, "");
			storeViews.SetValue(r, tfSwimmerName, ",");

			Console.WriteLine(4);
		}

		void HandleSamplesTreeSelectionChanged (object sender, EventArgs e)
		{
	
		}
	
	}
}

