#region License
/*
Copyright © 2014-2018 European Support Limited

Licensed under the Apache License, Version 2.0 (the "License")
you may not use this file except in compliance with the License.
You may obtain a copy of the License at 

http://www.apache.org/licenses/LICENSE-2.0 

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS, 
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
See the License for the specific language governing permissions and 
limitations under the License. 
*/
#endregion


using Amdocs.Ginger.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Ginger.UserControls;
using GingerCore;
using GingerCore.Actions;
using GingerCore.FlowControlLib;
using Ginger.Actions.UserControls;
using Ginger.BusinessFlowFolder;

namespace Ginger.Actions
{
    /// <summary>
    /// Interaction logic for ActionFlowControlPage.xaml
    /// </summary>
    public partial class ActionFlowControlPage : Page
    {
        private Act mAct;
        BusinessFlow mActParentBusinessFlow = null;
        Activity mActParentActivity = null;       
        
        public ActionFlowControlPage(Act act, BusinessFlow actParentBusinessFlow, Activity actParentActivity, General.RepositoryItemPageViewMode editMode = General.RepositoryItemPageViewMode.Automation)
        {
            InitializeComponent();

            mAct = act;
            mActParentBusinessFlow = actParentBusinessFlow;
            mActParentActivity = actParentActivity;  
            
            SetGridView();
            FlowControlGrid.DataSourceList = mAct.FlowControls;

            FlowControlGrid.btnAdd.AddHandler(Button.ClickEvent, new RoutedEventHandler(AddFlowControl));

            // TODO:  open new edit page -  FlowControlGrid.btnEdit.AddHandler(Button.ClickEvent, new RoutedEventHandler(EditAction));                      
            if (editMode == General.RepositoryItemPageViewMode.View)
            {
                SetViewMode();
            }
        }

        private void AddFlowControl(object sender, RoutedEventArgs e)
        {
            FlowControl FC = new FlowControl();
            FC.Active = true;
            mAct.FlowControls.Add(FC);
        }
        
        public void SetViewMode()
        {           
                FlowControlGrid.ShowToolsBar = Visibility.Collapsed;                
                FlowControlGrid.ToolsTray.Visibility = Visibility.Collapsed;
                FlowControlGrid.DisableGridColoumns();
        }

        private void SetGridView()
        {
            GridViewDef view = new GridViewDef(GridViewDef.DefaultViewName);
            ObservableList<GridColView> viewCols = new ObservableList<GridColView>();
            view.GridColsView = viewCols;

            viewCols.Add(new GridColView() { Field = FlowControl.Fields.Active, WidthWeight = 50, StyleType = GridColView.eGridColStyleType.CheckBox });
            view.GridColsView.Add(new GridColView() { Field = FlowControl.Fields.Condition, Header = "Condition", WidthWeight = 200, StyleType = GridColView.eGridColStyleType.Template, CellTemplate = ucGrid.getDataColValueExpressionTemplate("ConditionVE") });
            viewCols.Add(new GridColView() { Field = FlowControl.Fields.ConditionCalculated, Header = "Condition Calculated", WidthWeight = 150, BindingMode = BindingMode.OneWay });            
            view.GridColsView.Add(new GridColView() { Field = FlowControl.Fields.FlowControlAction, Header = "Action", WidthWeight = 200, StyleType = GridColView.eGridColStyleType.Template, CellTemplate = GetDataColActionFlowControlTemplate("ActionForEdit") });
            viewCols.Add(new GridColView() { Field = FlowControl.Fields.Status, WidthWeight = 150 });      
            FlowControlGrid.SetAllColumnsDefaultView(view);
            FlowControlGrid.InitViewItems();
        }
        
        private DataTemplate GetDataColActionFlowControlTemplate(string Path)
        {
            DataTemplate template = new DataTemplate();
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(UCFlowControlAction));
            factory.SetBinding(UCDataColGrid.DataContextProperty, new Binding(Path));
            factory.SetValue(UCFlowControlAction.ActParentBusinessFlowProperty, mActParentBusinessFlow);
            factory.SetValue(UCFlowControlAction.ActParentActivityProperty, mActParentActivity);
            template.VisualTree = factory;
            return template;
        }
    }
}
