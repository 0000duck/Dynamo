using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Dynamo.Controls;
using Dynamo.Nodes;
using Dynamo.ViewModels;
using Dynamo.Wpf;

namespace Dynamo.Wpf
{
    public abstract class BasicInteractiveViewCustomization<T> : INodeViewCustomization<BasicInteractive<T>>
    {
        protected DynamoViewModel dynamoViewModel;

        public void CustomizeView(BasicInteractive<T> nodeModel, dynNodeView nodeView)
        {
            this.dynamoViewModel = nodeView.ViewModel.DynamoViewModel;

            //add an edit window option to the 
            //main context window
            var editWindowItem = new MenuItem { Header = "Edit...", IsCheckable = false };
            nodeView.MainContextMenu.Items.Add(editWindowItem);
            editWindowItem.Click += editWindowItem_Click;
        }

        public virtual void editWindowItem_Click(object sender, RoutedEventArgs e)
        {
            //override in child classes
        }

        public void Dispose()
        {

        }
    }
}