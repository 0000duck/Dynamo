﻿using System.Windows;
using DiffPlex.Wpf.Controls;

namespace Dynamo.PythonMigration.Controls
{
    /// <summary>
    /// Interaction logic for VisualDifferenceViewer.xaml
    /// </summary>
    public partial class VisualDifferenceViewer : Window
    {
        private PythonMigrationAssistantViewModel ViewModel { get; set; }
        
        internal VisualDifferenceViewer(PythonMigrationAssistantViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            LoadData();
            DiffView.ViewModeChanged += OnViewModeChanged;
            DiffView.Loaded += OnDiffViewLoaded;
            Closed += OnVisualDifferenceViewerClosed;
        }

        private void OnDiffViewLoaded(object sender, RoutedEventArgs e)
        {
            SizeWindowToContent();
        }

        private void OnViewModeChanged(object sender, DiffViewer.ViewModeChangedEventArgs e)
        {
            SizeWindowToContent();
        }

        private void LoadData()
        {
            DiffView.OldText = ViewModel.OldCode;
            DiffView.NewText = ViewModel.NewCode;
        }

        private void DiffButton_Click(object sender, RoutedEventArgs e)
        {
            if (DiffView.IsInlineViewMode)
            {
                DiffView.ShowSideBySide();
                return;
            }

            DiffView.ShowInline();
        }

        private void SizeWindowToContent()
        {
            SizeToContent = SizeToContent.WidthAndHeight;
            // We need to set the SizeToContent back to manual otherwise it will freeze
            // the GridSplitter on the DiffView
            SizeToContent = SizeToContent.Manual;
        }

        private void OnAcceptButtonClicked(object sender, RoutedEventArgs e)
        {
            ViewModel.ChangeCode();
            this.Close();
        }

        private void OnRejectButtonClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void OnVisualDifferenceViewerClosed(object sender, System.EventArgs e)
        {
            DiffView.ViewModeChanged -= OnViewModeChanged;
            DiffView.Loaded -= OnDiffViewLoaded;
            Closed -= OnVisualDifferenceViewerClosed;
        }
    }
}
