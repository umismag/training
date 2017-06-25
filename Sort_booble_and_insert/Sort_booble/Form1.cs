using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sort_booble
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
			

        }
        
        
        private void button1_Click(object sender, EventArgs e)
        {
            //Бульбашковий метод сортування - порівняння пар елементів 
            //та заміна, у разі необхідності, їх місцями

            //int[] spysok1 = { 6, 2, 4, 7, 1, 3, 8, 5 };
            int[] spysok1 = { 15, 4, 10, 8, 6, 9, 16, 1, 7, 3, 11, 14, 2, 5, 12, 13 };
            bool swapped; //чи була здійснена заміна місцями хоч однієї пари елементів?
            int unSortedCount=spysok1.Length; //довжина невідсортованого списку
            int tmp; //тимчасова змінна для зберігання елемента масиву при заміні місцями

            do
            {
                swapped = false; // заміну елементів масиву ще не виконували
                for (int i = 0; i < unSortedCount-1; i++) //переглядаємо всі пари не відсортованого списку
                {
                    if (spysok1[i] > spysok1[i + 1]) //якщо пара елементів не відсортована,
                    {
                        tmp = spysok1[i + 1];
                        spysok1[i + 1] = spysok1[i];  //то міняємо їх місцями
                        spysok1[i] = tmp;
                        swapped = true;
                    }
                }                   //елемент з найбільшим значенням знаходиться в самому кінці списку
                unSortedCount--;    //не відсортованих елементів стає на 1 менше
            }
            while (swapped);        //завершуємо лише тоді, коли у всьому списку пари елементів не мінялися місцями
            //
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //int[] spysok1 = { 6, 2, 4, 7, 1, 3, 8, 5 };
            int[] spysok1 = { 15, 4, 10, 8, 6, 9, 16, 1, 7, 3, 11, 14, 2, 5, 12, 13 };
            int insertedIndex;  //індекс елемента масиву, на місце якого вставляється новий елемент
            int tmp; //тимчасова змінна для зберігання елемента масиву, що буде вставлятися

            for (int i= 1; i < spysok1.Length; i++) //вважаємо перший елемент відсортованим масивом довжиною 1
                                                    //тому починаємо з другого елемента
            {
                                        //всі попередні елементи - відсортована частина списку
                insertedIndex = i;      //позиція елемента, що вставляється
                tmp = spysok1[i];       //значення елемента масиву, що вставляється
                while (                             //поки
                        (insertedIndex > 0)         //не досягли першого елемента відсортованої частини списку 
                        &&                          // і
                        (tmp < spysok1[insertedIndex-1]) //елемент, що вставляється, менший, ніж 
                                                                //черговий елемент відсортованої частини масиву
                      )
                {
                    spysok1[insertedIndex] = spysok1[insertedIndex-1];  //цей черговий елемент зсуваємо праворуч
                    insertedIndex--;                                    //а позиція вставки нового елемента 
                                                                        //зсувається ліворуч
                }
                spysok1[insertedIndex] = tmp;                    //новий елемент вставляється на свою позицію
            }
        }

        
        public sortingArray spysok1;
        private void button3_Click(object sender, EventArgs e)
        {
            int[] M = { 15, 4, 10, 8, 6, 9, 16, 1, 7, 3, 11, 14, 2, 5, 12, 13 };
            spysok1=new sortingArray(M, groupBox1);
			button4.Enabled = true;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (spysok1.stepSortingNext())
                button4.Enabled = true;
            else button4.Enabled = false;
        }
    }

    public class sortingArray
    {
        struct massTxt //структура
        {
            public int item;
            public TextBox TxtBox;
            public int position;
        }
		

        //int[] spysok1 = { 15, 4, 10, 8, 6, 9, 16, 1, 7, 3, 11, 14, 2, 5, 12, 13 };
        massTxt[] massyv; //масив структур
        int txtBoxWidth = 28,
            txtBoxHeight = 10,
            txtBoxLeft = 4,
            txtBoxTop = 20;
		enum state {initial, preWhile, inWhile,postWhile};
		state sortingState;

        int insertedIndex;  //індекс елемента масиву, на місце якого вставляється новий елемент 
		massTxt tmp;//тимчасова змінна для зберігання елемента масиву, що буде вставлятися
		int insertingIndex = 0;

        public sortingArray(int[] m, System.Windows.Forms.Control parentObj)   //конструктор
        {
			sortingState = state.initial;
			massyv = new massTxt[m.Length];
            for (int i = 0; i < m.Length; i++)
            {
                massyv[i].item = m[i];
                massyv[i].position = i;
                massyv[i].TxtBox = new TextBox();
                massyv[i].TxtBox.Parent = parentObj;
                massyv[i].TxtBox.Width = txtBoxWidth;
                massyv[i].TxtBox.Height = txtBoxHeight;
                massyv[i].TxtBox.Left = txtBoxLeft * (i + 1) + txtBoxWidth * i;
                massyv[i].TxtBox.Top = txtBoxTop;
                massyv[i].TxtBox.Text = m[i].ToString();
				massyv[i].TxtBox.BackColor = Color.LightYellow;
				massyv[i].TxtBox.TextAlign = HorizontalAlignment.Center;
			}
			tmp.TxtBox = new TextBox();
			tmp.TxtBox.Parent = parentObj;
			tmp.TxtBox.Width = txtBoxWidth;
			tmp.TxtBox.Height = txtBoxHeight;
			tmp.TxtBox.Top = txtBoxTop*2+txtBoxHeight;
			tmp.TxtBox.BackColor = Color.Red;
			tmp.TxtBox.Visible = false;
			tmp.TxtBox.TextAlign = HorizontalAlignment.Center;
		}

        public bool stepSortingNext()
        {
			switch (sortingState){
				case state.initial:
					{
						massyv[insertedIndex].TxtBox.BackColor = Color.LawnGreen;
						if (insertingIndex < massyv.Length - 1)
						{
							insertingIndex++;//вважаємо перший елемент відсортованим масивом довжиною 1
											 //тому починаємо з другого елемента
						}
						else return false;
						//всі попередні елементи - відсортована частина списку
						insertedIndex = insertingIndex;      //позиція елемента, що вставляється
						massyv[insertingIndex].TxtBox.BackColor = Color.Red;
						massyv[insertedIndex - 1].TxtBox.BackColor = Color.LawnGreen;
						sortingState = state.preWhile;
						break;
					}
				case state.preWhile:
					{ 
						tmp.item = massyv[insertingIndex].item;  //значення елемента масиву, що вставляється
						tmp.TxtBox.Left= txtBoxLeft * (insertingIndex+1) + txtBoxWidth * insertingIndex;
						tmp.TxtBox.Text = tmp.item.ToString();
						tmp.TxtBox.Visible = true;massyv[insertingIndex].TxtBox.Text = "?";
						massyv[insertingIndex].TxtBox.BackColor = Color.LightGray;
						sortingState = state.inWhile;
						break;
					}
				case state.inWhile:
					{
						if ((sortingState == state.inWhile) &&                             //поки
								(insertedIndex > 0)         //не досягли першого елемента відсортованої частини списку 
								&&                          // і
								(tmp.item < massyv[insertedIndex - 1].item) //елемент, що вставляється, менший, ніж черговий елемент відсортованої частини масиву
							  )
						{
							MooveRight(insertedIndex - 1);//цей черговий елемент зсуваємо праворуч
							insertedIndex--;//а позиція вставки нового елемента зсувається ліворуч                     
						}
						else
						{
							sortingState = state.postWhile;
							massyv[insertedIndex].TxtBox.Text ="ok";
							massyv[insertedIndex].TxtBox.BackColor = Color.LightSkyBlue;
							tmp.TxtBox.Left = txtBoxLeft * (insertedIndex + 1) + txtBoxWidth * insertedIndex;
						}
						break;
					}
				case state.postWhile:
					{
						massyv[insertedIndex].item = tmp.item;//новий елемент вставляється на свою позицію
						massyv[insertedIndex].TxtBox.Text = tmp.item.ToString();
						tmp.TxtBox.Visible = false;
						//massyv[insertedIndex].TxtBox.BackColor = Color.Red;
						sortingState = state.initial;
						break;
					}
			}
            return true;
        }

        bool MooveRight(int currentPosition)
        {
            massyv[currentPosition + 1].item = massyv[currentPosition].item;
            massyv[currentPosition + 1].TxtBox.Text = massyv[currentPosition].TxtBox.Text;
            massyv[currentPosition + 1].TxtBox.BackColor = massyv[currentPosition].TxtBox.BackColor;
            massyv[currentPosition].TxtBox.Text = "?";
            massyv[currentPosition].TxtBox.BackColor = Color.LightGray;
			


            return true;
        }
    }

}
