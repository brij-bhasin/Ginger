﻿using Amdocs.Ginger.Common;
using Amdocs.Ginger.UserControls;
using Ginger.TwoLevelMenuLib;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ginger.GeneralWindows
{
    /// <summary>
    /// Interaction logic for MenusPage.xaml
    /// </summary>
    public partial class TwoLevelMenuPage : Page
    {
        readonly TwoLevelMenu mTwoLevelMenu;
        // static for reuse
        static LoadingPage loadingPage = new LoadingPage();

        TopMenuItem SelectedMainListItem
        {
            get
            {
                if (xMainNavigationListView != null && xMainNavigationListView.SelectedItem != null)
                {
                    return (TopMenuItem)xMainNavigationListView.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }
          
        SubMenuItem SelectedSubListItem
        {
            get
            {
                if (xSubNavigationListView != null && xSubNavigationListView.SelectedItem != null)
                {
                    return (SubMenuItem)xSubNavigationListView.SelectedItem;
                }
                else
                {
                    return null;
                }
            }
        }

        public TwoLevelMenuPage(TwoLevelMenu twoLevelMenu)
        {
            InitializeComponent();
            mTwoLevelMenu = twoLevelMenu;
            LoadMenus();            
        }        

        public void Reset()
        {
            mTwoLevelMenu.Reset();
            xMainNavigationListView.SelectedItem = null;
            xSubNavigationListView.SelectedItem = null;            
            xSelectedItemFrame.SetContent(null);
            if (App.UserProfile.Solution != null)
            {
                SelectFirstTopMenu();
            }
        }

        private void LoadMenus()
        {            
            foreach(TopMenuItem menu in mTwoLevelMenu.MenuList)
            {
                xMainNavigationListView.Items.Add(menu);
            }
            SelectFirstTopMenu();            
        }

        private void SelectFirstTopMenu()
        {
            xMainNavigationListView.SelectedItem = xMainNavigationListView.Items[0];            
        }

        private void xMainNavigationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedMainListItem == null)
            {
                xSelectedItemFrame.SetContent(null);
                return;
            }

            SetSelectedListItemStyle(xMainNavigationListView, Brushes.DarkGray);

            ObservableList<SubMenuItem> subItems;
            subItems = SelectedMainListItem.SubItems;

            xSubNavigationListView.Items.Clear();
            foreach (SubMenuItem subItem in subItems)
            {
                xSubNavigationListView.Items.Add(subItem);
            }

            // if we have only one sub item no need to show sub menu
            if (xSubNavigationListView.Items.Count > 1)
            {
                xSubNavigationListView.Visibility = Visibility.Visible;
            }
            else
            {
                xSubNavigationListView.Visibility = Visibility.Collapsed;
            }

            // get the user back to the same sub item he had before or select the first item
            if (SelectedMainListItem.LastSubMenuItem == null)
            {
                // first time click auto select first sub menu item
                SelectedMainListItem.LastSubMenuItem = subItems[0];
            }
            // Get the user back to the last sub item he had
            if (xSubNavigationListView.SelectedItem != SelectedMainListItem.LastSubMenuItem)
            {
                xSubNavigationListView.SelectedItem = SelectedMainListItem.LastSubMenuItem;
            }
            else
            {
                if (subItems.Count > 1)
                { 
                    SetSelectedListItemStyle(xSubNavigationListView, Brushes.Gray);
                }
            }

        }
        
        private void xSubNavigationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSubListItem != null)
            {
                SelectedMainListItem.LastSubMenuItem = SelectedSubListItem;
            }

            if (SelectedSubListItem == null)
            {
                xSelectedItemFrame.SetContent(null);
                return;
            }

            if (!SelectedSubListItem.IsPageLoaded)
            {
                // since the page might take time to load we show Loading, will happen with source control connected                
                xSelectedItemFrame.SetContent(loadingPage);                
                GingerCore.General.DoEvents();
            }

            if (SelectedSubListItem != null && SelectedSubListItem.ItemPage != null)
            { 
                xSelectedItemFrame.SetContent(SelectedSubListItem.ItemPage);
            }

            if (xSubNavigationListView.Items.Count > 1)
            { 
                SetSelectedListItemStyle(xSubNavigationListView, Brushes.Gray);
            }
        }

        private void SetSelectedListItemStyle(ListView listView, Brush defualtForeground)
        {
            try
            {
                GingerCore.General.DoEvents();

                for (int i = 0; i < listView.Items.Count; i++)
                {
                    ListViewItem listViewItem = (ListViewItem)listView.ItemContainerGenerator.ContainerFromIndex(i);
                    if (listViewItem != null)
                    {
                        StackPanel stack = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(listViewItem, 0), 0), 0) as StackPanel;
                        if (i == listView.SelectedIndex)
                        {
                            ((ImageMakerControl)stack.Children[0]).Foreground = (Brush)Application.Current.Resources["$SelectionColor_Pink"];
                            ((Label)stack.Children[1]).Foreground = (Brush)Application.Current.Resources["$SelectionColor_Pink"];
                        }
                        else
                        {
                            ((ImageMakerControl)stack.Children[0]).Foreground = defualtForeground;
                            ((Label)stack.Children[1]).Foreground = defualtForeground;
                        }                       
                    }                    
                }

            }
            catch(Exception ex)
            {
                GingerCore.Reporter.ToLog(eAppReporterLogLevel.ERROR, "Failed to Set Selected ListItem Style", ex);
            }
        }

        private void xMainNavigationListView_Loaded(object sender, RoutedEventArgs e)
        {
            SetSelectedListItemStyle(xMainNavigationListView, Brushes.DarkGray);
        }

        private void xSubNavigationListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (xSubNavigationListView.Items.Count > 1 && xSubNavigationListView.IsVisible)
            { 
                SetSelectedListItemStyle(xSubNavigationListView, Brushes.Gray);
            }
        }
    }
}
