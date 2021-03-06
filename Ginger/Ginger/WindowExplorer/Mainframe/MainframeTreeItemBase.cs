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

using Ginger.SolutionWindows.TreeViewItems;
using GingerCore.Drivers.MainFrame;
using Open3270.TN3270;
using System;
using System.Collections.Generic;
using Amdocs.Ginger.Common;
using GingerWPF.UserControlsLib.UCTreeView;
using Amdocs.Ginger.Common.UIElement;

namespace Ginger.WindowExplorer.Mainframe
{
    class MainframeTreeItemBase : TreeViewItemBase, ITreeViewItem, IWindowExplorerTreeItem
    {
        public MainFrameDriver MFDriver { get; set; }

        public string Name { get; set; }
        public string Path { get; set; }

        public ObservableList<GingerCore.Actions.Act> GetElementActions()
        {   
            ObservableList<GingerCore.Actions.Act> ACL = new ObservableList<GingerCore.Actions.Act>();
            return ACL;
        }

        public ObservableList<ControlProperty> GetElementProperties()
        {
            throw new NotImplementedException ();
        }

        public List<ITreeViewItem> Childrens()
        {    List<ITreeViewItem> Childrens = new List<ITreeViewItem>();
            MFDriver = (MainFrameDriver)App.AutomateTabGingerRunner.ApplicationAgents[0].Agent.Driver;
          XMLScreen XMLS=MFDriver.GetRenderedScreen ();
            foreach(XMLScreenField xf in XMLS.Fields)
            {
                MainframeControlTreeItem MFTI = new MainframeControlTreeItem ();
                MFTI.Name = xf.Text;
                MFTI.XSF = xf;
                MFTI.Path = xf.Location.left + "/" + xf.Location.top;
                Childrens.Add (MFTI);
            }
            return Childrens;
        }

        public System.Windows.Controls.Page EditPage()
        {
            return null;
        }

        public System.Windows.Controls.StackPanel Header()
        {
            string ImageFileName = "@Window_16x16.png";
            return TreeViewUtils.CreateItemHeader (Name, ImageFileName);
        }

        public bool IsExpandable()
        {
            return true;
        }

        public System.Windows.Controls.ContextMenu Menu()
        {
            throw new NotImplementedException ();
        }

        public object NodeObject()
        {
            return null;
        }

        public void SetTools(ITreeView TV)
        {
            return;
        }
    }
}