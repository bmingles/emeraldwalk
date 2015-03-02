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
            UpdateTextBoxes();
        }

        private void UpdateTextBoxes()
        {
            foreach (TextBox textBox in this.GetChildren<TextBox>(this))
            {
                BindingExpression bindingExpression = textBox.GetBindingExpression(TextBox.TextProperty);
                if (bindingExpression != null)
                {
                    bindingExpression.UpdateSource();
                }
            }
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

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.UpdateTextBoxes();
            this.SaveCommandsGrid.CommitEdit();
        }
    }
}
