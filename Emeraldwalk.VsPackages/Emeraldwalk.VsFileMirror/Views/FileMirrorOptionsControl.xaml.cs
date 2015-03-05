using Emeraldwalk.Emeraldwalk_VsFileMirror.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emeraldwalk.Emeraldwalk_VsFileMirror.Views
{
    public class DelegateCommand: ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<object> ExecuteAction { get; set; }

        public DelegateCommand(
            Action<object> executeAction)
        {
            this.ExecuteAction = executeAction;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.ExecuteAction(parameter);
        }
    }

    /// <summary>
    /// Interaction logic for FileMirrorOptionsControl.xaml
    /// </summary>
    public partial class FileMirrorOptionsControl : UserControl
    {
        private IFileMirrorOptions FileMirrorOptions { get; set; }

        public FileMirrorOptionsControl(IFileMirrorOptions fileMirrorOptions)
        {
            InitializeComponent();
            this.FileMirrorOptions = fileMirrorOptions;
            this.DataContext = fileMirrorOptions;

            this.LostKeyboardFocus += FileMirrorOptionsControl_LostKeyboardFocus;
        }

        /// <summary>
        /// TextBoxes won't necessarily lose focus and commit the value,
        /// so forcing it on user control lost focus.
        /// </summary>
        private void FileMirrorOptionsControl_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            UpdateSources();
        }

        /// <summary>
        /// Handle the fact that last edited control may not have updated its source when save button is clicked
        /// due to still having focus.
        /// </summary>
        private void UpdateSources()
        {
            foreach (Control control in this.GetChildren<Control>(this))
            {
                BindingExpression bindingExpression = null;
                if(control is TextBox)
                {
                    bindingExpression = control.GetBindingExpression(TextBox.TextProperty);
                }
                else if(control is CheckBox)
                {
                    bindingExpression = control.GetBindingExpression(CheckBox.IsCheckedProperty);
                }

                if (bindingExpression != null)
                {
                    bindingExpression.UpdateSource();
                }
            }
        }

        private T GetParent<T>(DependencyObject dependencyObject)
            where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(dependencyObject) as DependencyObject;
            while(parent != null)
            {
                if(parent is T)
                {
                    return (T)parent;
                }
                parent = VisualTreeHelper.GetParent(parent) as DependencyObject;
            }

            return null;
        }

        private IEnumerable<T> GetChildren<T>(DependencyObject dependencyObject)
            where T: DependencyObject
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
                if(child is T)
                {
                    yield return (T)child;
                }

                foreach(T grandChild in this.GetChildren<T>(child))
                {
                    yield return grandChild;
                }
            }
        }

        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            this.FileMirrorOptions.OnPropertyChanged("SaveCommandsOutput");
        }

        private void DataGridCell_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if(!cell.IsEditing && !cell.IsReadOnly)
            {
                cell.Focus();
                if(this.SaveCommandsGrid.SelectionUnit == DataGridSelectionUnit.FullRow)
                {
                    DataGridRow row = this.GetParent<DataGridRow>(cell);
                    if(!row.IsSelected)
                    {
                        row.IsSelected = true;
                    }
                }
                else
                {
                    if(!cell.IsSelected)
                    {
                        cell.IsSelected = true;
                    }
                }
            }
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.UpdateSources();
            this.SaveCommandsGrid.CommitEdit();
        }
    }
}
