using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using GWord;

namespace GuessEngine
{
    /// <summary>
    /// Реализация IOPort для WindowsForms
    /// </summary>
    public class IOPortWinForms :  IOPort
    {
        public IOPortWinForms(System.Windows.Forms.Form IOForm):base(IOForm)
        {

        }

        public override void printFindedData(IEnumerable<string> ToPrint)
        {
            ((Form1)this._interfaceLink).richTextBox1.Text = "";
            foreach (string element in ToPrint)
            {
                ((Form1)this._interfaceLink).richTextBox1.Text += (element + "\n");
            }
        }

    }


}