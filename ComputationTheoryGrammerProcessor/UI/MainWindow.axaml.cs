using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ComputationTheoryGrammerProcessor.Core;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;

namespace UI
{
    public partial class MainWindow : Window
    {
        private readonly Button btnParse;
        private readonly TextBox txtGrammar, txtAlphabet;
        private readonly CheckBox chkEliminateExtra;
        private readonly DataGrid grdRules;
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            btnParse = this.FindControl<Button>(nameof(btnParse));
            txtAlphabet = this.FindControl<TextBox>(nameof(txtAlphabet));
            txtGrammar = this.FindControl<TextBox>(nameof(txtGrammar));
            chkEliminateExtra = this.FindControl<CheckBox>(nameof(chkEliminateExtra));
            grdRules = this.FindControl<DataGrid>(nameof(grdRules));
            
            btnParse.Click += BtnParseOnClick;
        }

        private async void BtnParseOnClick(object? sender, RoutedEventArgs e)
        {
            var alphabet = txtAlphabet.Text?
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(c => c.Length == 1)
                .Select(c => c[0])
                .ToArray() ?? new char[]{};
            var grammarText = txtGrammar.Text?.Trim() ?? "";
            if (alphabet.Length == 0 || grammarText.Length == 0)
            { 
                await MessageBox.Avalonia.MessageBoxManager.GetMessageBoxStandardWindow(new MessageBoxStandardParams()
                {
                    Topmost = true,
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Error",
                    ContentHeader = "Invalid Input",
                    ContentMessage = "Alphabet and Grammar text can't be empty.",
                    Icon = MessageBox.Avalonia.Enums.Icon.Error,
                })
                    .ShowDialog(this);
                return;
            }
            txtAlphabet.Text = string.Join(" ", alphabet);
            var grammar = Grammar.Create(alphabet, grammarText, chkEliminateExtra.IsChecked ?? false);
            grdRules.Items = grammar.Rules.Select(r => new RulesGridItemModel(r)).ToArray();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}