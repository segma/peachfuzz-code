﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Be.Windows.Forms;
using PeachFuzzFactory.Models;
using Peach.Core.Dom;
using Peach.Core;
using Peach.Core.Cracker;
using Peach.Core.IO;
using Peach.Core.Analyzers;
using System.Reflection;
using ActiproSoftware.Windows.Controls.PropertyGrid;
using ActiproSoftware.Windows.Controls.SyntaxEditor.EditActions;
using ActiproSoftware.Text.Languages.Xml;
using ActiproSoftware.Text.Languages.Xml.Implementation;

namespace PeachFuzzFactory
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Dictionary<DataElement, CrackModel> crackMap = new Dictionary<DataElement, CrackModel>();
		protected string binaryFileName = null;
		protected string tempPitFileName = null;
		protected DataModel crackerSelectedDataModel = null;

		public MainWindow()
		{
			InitializeComponent();

			this.Title = "Peach FuzzFactory v3 DEV - test.xml";

			binaryFileName = @"4-Key.png";
			tempPitFileName = @"test.xml";

			byte[] buff;
			using (Stream sin = File.OpenRead(binaryFileName))
			{
				buff = new byte[sin.Length];
				sin.Read(buff, 0, buff.Length);
			}

			DynamicFileByteProvider dynamicFileByteProvider;
			dynamicFileByteProvider = new DynamicFileByteProvider(new MemoryStream(buff));
			TheHexBox.ByteProvider = dynamicFileByteProvider;

			PitParser parser = new PitParser();
			Dom dom = parser.asParser(new Dictionary<string, string>(), File.OpenRead(tempPitFileName));

			XmlSchemaResolver schemaResolver = new XmlSchemaResolver();
			using (Stream stream = typeof(Engine).Assembly.GetManifestResourceStream("Peach.Core.peach.xsd"))
			{
				schemaResolver.LoadSchemaFromStream(stream);
			}
			xmlEditor.Document.Language.RegisterXmlSchemaResolver(schemaResolver);

			xmlEditor.Document.LoadFile(File.OpenRead(tempPitFileName), Encoding.UTF8);

			var models = new List<DesignModel>();
			models.Add(new DesignPeachModel(dom));
			DesignerTreeView.ItemsSource = models;

			DesignHexDataModelsCombo.ItemsSource = dom.dataModels.Values;
			if (dom.dataModels.Count > 0)
				DesignHexDataModelsCombo.SelectedIndex = dom.dataModels.Count - 1;
		}

		protected void HandleCracking()
		{
			byte[] buff;
			using (Stream sin = File.OpenRead(binaryFileName))
			{
				buff = new byte[sin.Length];
				sin.Read(buff, 0, buff.Length);
			}

			PitParser parser = new PitParser();
			Dom dom = parser.asParser(new Dictionary<string, string>(), File.OpenRead(tempPitFileName));

			string dataModelName = crackerSelectedDataModel.name;
			DesignHexDataModelsCombo.ItemsSource = dom.dataModels.Values;

			for (int cnt = 0; cnt < dom.dataModels.Count; cnt++)
			{
				if (dom.dataModels[cnt].name == dataModelName)
				{
					DesignHexDataModelsCombo.SelectedIndex = cnt;
					break;
				}
			}

			try
			{
				BitStream data = new BitStream(buff);
				DataCracker cracker = new DataCracker();
				cracker.EnterHandleNodeEvent += new EnterHandleNodeEventHandler(cracker_EnterHandleNodeEvent);
				cracker.ExitHandleNodeEvent += new ExitHandleNodeEventHandler(cracker_ExitHandleNodeEvent);
				cracker.CrackData(crackerSelectedDataModel, data);
			}
			catch
			{
				crackMap[crackerSelectedDataModel].Error = true;
			}

			CrackModel.Root = crackMap[crackerSelectedDataModel];
			CrackTree.Model = CrackModel.Root;

			// No longer needed
			crackMap.Clear();
		}

		void cracker_ExitHandleNodeEvent(DataElement element, BitStream data)
		{
			var currentModel = crackMap[element];
			currentModel.Length = ((BitStream)currentModel.DataElement.Value).LengthBytes;

			if (element.parent != null)
				crackMap[element.parent].Children.Add(currentModel);
		}

		void cracker_EnterHandleNodeEvent(DataElement element, BitStream data)
		{
			crackMap[element] = new CrackModel(element, data.TellBytes(), 0);
		}

		private void DesignerTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			DesignPropertyGrid.Items.Clear();

			if (!(DesignerTreeView.SelectedItem is DesignDataElementModel))
				return;

			DesignDataElementModel model = (DesignDataElementModel)DesignerTreeView.SelectedItem;
			foreach (Attribute attribute in model.DataElement.GetType().GetCustomAttributes(true))
			{
				if (attribute is ParameterAttribute)
				{
					var item = new PropertyGridPropertyItem();
					item.Name = ((ParameterAttribute)attribute).name;
					item.ValueName = ((ParameterAttribute)attribute).name;
					item.Value = GetValue(model.DataElement, ((ParameterAttribute)attribute).name);
					item.Description = ((ParameterAttribute)attribute).description;
					item.ValueType = ((ParameterAttribute)attribute).type;

					DesignPropertyGrid.Items.Add(item);
				}
			}
		}

		private string GetValue(DataElement elem, string property)
		{
			var pinfo = elem.GetType().GetProperty(property);
			if (pinfo == null)
				return "";

			return pinfo.GetValue(elem, new object[0]).ToString();
		}

		private void CrackTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;

			CrackModel model = (CrackModel) ((PeachFuzzFactory.Controls.TreeNode)e.AddedItems[0]).Tag;
			TheHexBox.Select(model.Position, model.Length);
		}

		private void ButtonNewPit_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonPitOpen_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonSavePit_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonSavePitAs_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonShowCracking_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonShowXml_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonCrackBinOpen_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonXmlSave_Click(object sender, RoutedEventArgs e)
		{
			// Save and update designer view
		}

		private void ButtonXmlCopy_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.CopyToClipboard();
		}

		private void ButtonXmlCut_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.CutToClipboard();
		}

		private void ButtonXmlPaste_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.PasteFromClipboard();
		}

		private void ButtonXmlDelete_Click(object sender, RoutedEventArgs e)
		{
			// TODO
		}

		private void ButtonXmlFind_Click(object sender, RoutedEventArgs e)
		{
			//xmlEditor
		}

		private void ButtonXmlFindAndReplace_Click(object sender, RoutedEventArgs e)
		{

		}

		private void ButtonXmlRedo_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.ExecuteEditAction(new RedoAction());
		}

		private void ButtonXmlUndo_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.ExecuteEditAction(new ActiproSoftware.Windows.Controls.SyntaxEditor.EditActions.UndoAction());
		}

		private void ButtonXmlSelectAll_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.ExecuteEditAction(new SelectAllAction());
		}

		private void ButtonXmlIndent_Click(object sender, RoutedEventArgs e)
		{
			xmlEditor.ActiveView.ExecuteEditAction(new IndentAction());
		}

		private void ButtonXmlIndentLess_Click(object sender, RoutedEventArgs e)
		{
			// TODO
		}

		private void ButtonCrackBinRefresh_Click(object sender, RoutedEventArgs e)
		{
			HandleCracking();
		}

		private void DesignHexDataModelsCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
				crackerSelectedDataModel = e.AddedItems[0] as DataModel;
			else
				crackerSelectedDataModel = null;
		}
	}
}