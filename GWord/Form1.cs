using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GuessEngine;
using System.Text.RegularExpressions;

namespace GWord
{
    public partial class Form1 : Form
    {
        private GuessDriver driver = null;

        public Form1()
        {
            InitializeComponent();
            driver = new GuessDriver(new IOPortWinForms(this));
        }

        /// <summary>
        /// Изменилось значение количества цифр в шаблоне - нужно удалить звоздочки в поле шаблона (или их добавить)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > textBox2.Text.Length)
            {
                for (int i = textBox2.Text.Length; i < numericUpDown1.Value; i++ )
                {
                    textBox2.Text += "*";
                }
            }
            else
            {
                for (int i = textBox2.Text.Length; i > numericUpDown1.Value; i--)
                {
                    textBox2.Text = textBox2.Text.Remove(textBox2.Text.Length-1);
                }
            }
        }

        private void textBox2_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.Select(textBox2.SelectionStart, 0);
        }

        private bool IsCyr(char Letter)
        {
            return (!(new Regex(@"\P{IsCyrillic}")).IsMatch(new string(Letter, 1)));
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            TextBox currentBox = ((TextBox)sender);
            if ((Keys)e.KeyChar == Keys.Back)
            {
                // Если нажат бекспейс, нужно добавить звёздочку (в том случае, если мы не стоим в начале шаблона)
                if (currentBox.SelectionStart==0)
                {
                    e.Handled = true;
                    return;
                }
                int tempSelStart1 = currentBox.SelectionStart;
                currentBox.Text = currentBox.Text.Insert(currentBox.SelectionStart,"*");
                currentBox.Select(tempSelStart1, 0);
                return;
            }

            // Запрещаем вводить символы в конце шаблона и некириллические символы
            if (
                (currentBox.SelectionStart==currentBox.Text.Length) || 
                //(!Char.IsLetter(e.KeyChar))||
                (!IsCyr(e.KeyChar))
                )
            {
                e.Handled = true;
                return;
            }
            // Нужно удалить следующий символ по SelectionStart и вместо него вставится символ, который мы нажали
            int tempSelStart3 = currentBox.SelectionStart;
            currentBox.Text = currentBox.Text.Remove(currentBox.SelectionStart, 1);
            currentBox.Select(tempSelStart3, 0);
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        /// <summary>
        /// Обрабатываем нажатие Delete в тексбоксе с шаблоном
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox currentBox = ((TextBox)sender);
            if (e.KeyCode == Keys.Delete)
            {
                if (currentBox.SelectionStart < currentBox.Text.Length)
                {
                    // В случае нажатия Delete - добавляем звёздочку через одну позицию от SelectionStart (чтобы был эффект, что вместо стираемого символа подставилась звёздочка)
                    int tempSelStart2 = currentBox.SelectionStart;
                    currentBox.Text = currentBox.Text.Insert(currentBox.SelectionStart + 1, "*");
                    currentBox.Select(tempSelStart2, 0);
                }
                return;
            }
        }

        /// <summary>
        /// В текстбоксе буков - запрещаем любые знаки кроме кириллицы и бекспейса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(IsCyr(e.KeyChar) || ( e.KeyChar == (char)Keys.Back )))
            {
                e.Handled = true;
                return;
            }
            e.KeyChar = char.ToUpper(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            driver.findWords( textBox2.Text, textBox1.Text);
        }
    }
}
