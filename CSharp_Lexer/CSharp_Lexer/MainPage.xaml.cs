using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.FilePicker;
using System.Diagnostics;


namespace CSharp_Lexer
{
	public partial class MainPage : ContentPage
	{
        public string Input { get; set; }
		public MainPage()
		{
			InitializeComponent();
            btn_OpenFile.Clicked += Btn_OpenFile_Clicked;
            btn_Analyze.Clicked += Btn_Analyze_Clicked;
		}
        
    
        private void Btn_OpenFile_Clicked(object sender, EventArgs e)
        {
            Picker();
        }
        private void Btn_Analyze_Clicked(object sender, EventArgs e)
        {
            txt_LexedData.Text = "";

            Lexer l = new Lexer(Input);
            var Tokens= l.Tokenize();

            foreach(Token t in Tokens)
            {
                Debug.WriteLine(t.ToString());
                txt_LexedData.Text += t.ToString() + "\n";
            }
        }

        public async void Picker()
        {
            try
            {
                Plugin.FilePicker.Abstractions.FileData F = await CrossFilePicker.Current.PickFile();
                var x = Encoding.ASCII.GetChars(F.DataArray, 0, F.DataArray.Count());
                string InputText = "";
                foreach (char C in x)
                {
                    InputText += C.ToString();
                }
                Input = InputText;
                txt_Status.Text = "Input Is Ready!";
            }
            catch { }
        }
	}
}
