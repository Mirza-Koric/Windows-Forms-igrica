using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace crossy_road
{
    public partial class Form1 : Form
    {
        //bug: cars won't spawn if you are standing on the edge that the cars are supposed to spawn from

        Color temp_color; // if temp_color = loosing color!!!
        int current_position = 4;
        int street_counter = 0;
        bool left_right = false;
        int score = 0;

        int[] cars_left = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        int[] cars_right = new int[7] { 0, 0, 0, 0, 0, 0, 0 };
        //0=random, 1=red, 2=gray

        Random ra = new Random();

        public Form1()
        {
            this.KeyPreview = true;

            InitializeComponent();
        }

        public void shift_down()
        {
            Button current_up = (Button)this.Controls["btn" + 4 + current_position.ToString()];
            Button current_down = (Button)this.Controls["btn" + 6 + current_position.ToString()];
            current_down.BackColor = temp_color;
            temp_color = current_up.BackColor;

            for (int i = 6; i > 0; i--)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button b = (Button)this.Controls["btn"+i + j.ToString()];
                    Button b2 = (Button)this.Controls["btn"+(i-1) + j.ToString()];

                    if (!(b2.BackColor==Color.Purple))
                    {
                        b.BackColor = b2.BackColor;
                        b2.BackColor = default(Color);
                        //b2.BackColor = Color.FromArgb(225, 225, 225);
                    }

                }
            }
            Button current = (Button)this.Controls["btn" + 5 + current_position.ToString()];
            current.BackColor = Color.Purple;

            if (left_right == false)
                left_right = true;
            else left_right = false;
        }

        public void move_left()
        {
            Button b = (Button)this.Controls["btn" + 5 + current_position.ToString()];
            Button bu = (Button)this.Controls["btn" + 5 + (current_position-1).ToString()];
            Color temp_c;

            if (current_position > 0)
            {
                temp_c = bu.BackColor;
                bu.BackColor = Color.Purple;
                b.BackColor = temp_color;
                temp_color = temp_c;

                current_position--;
            }
            else if(current_position==0)
                b.BackColor = Color.Purple;
        }

        public void move_right()
        {
            Button b = (Button)this.Controls["btn" + 5 + current_position.ToString()];
            Button bu = (Button)this.Controls["btn" + 5 + (current_position + 1).ToString()];
            Color temp_c;

                if (current_position < 8)
                {
                    temp_c = bu.BackColor;
                    bu.BackColor = Color.Purple;
                    b.BackColor = temp_color;
                    temp_color = temp_c;

                    current_position++;
                }
                else if (current_position == 8)
                    b.BackColor = Color.Purple;
        }

        public void generate_river()
        {
            for (int i = 0; i < 9; i++)
            {
                Button b = (Button)this.Controls["btn" + 0 + i.ToString()];
                b.BackColor = Color.Blue;
            }

            int number1,number2;
            Random r = new Random();
            number1 = r.Next() % 9;
            int log_or_pads = r.Next() % 2;

            if(log_or_pads==0)
            {
                Button bu = (Button)this.Controls["btn" + 0 + number1.ToString()];
                bu.BackColor = Color.Lime;

                do
                {
                    number2 = r.Next() % 9;
                } while (number1 == number2);

                bu = (Button)this.Controls["btn" + 0 + number2.ToString()];
                bu.BackColor = Color.Lime;
            }

            else if(log_or_pads==1)
            {
                if (number1 > 5)
                    number1 = 5;

                for (int i = number1; i < (number1+4); i++)
                {
                    if (i > 8)
                        break;
                    Button but = (Button)this.Controls["btn" + 0 + i.ToString()];
                    but.BackColor = Color.FromArgb(123,63,0);
                }
            }
        }

        public void generate_street()
        {
            for (int i = 0; i < 9; i++)
            {
                Button b = (Button)this.Controls["btn" + 0 + i.ToString()];
                b.BackColor = Color.FromArgb(85,85,85);
            }

            int number1;
            Random r = new Random();

            for (int i = 2; i < 8; i += 3)
            {
                number1 = r.Next() % 4;
                if (number1 != 0)
                    continue;

                Button b = (Button)this.Controls["btn" + 0 + i.ToString()];
                b.BackColor = Color.Red;
                Button bu = (Button)this.Controls["btn" + 0 + (i+1).ToString()];
                bu.BackColor = Color.Red;
            }
        }

        public void move_car(bool flag, int row)
        {
            if (row % 2 == 0 && flag==false || row % 2 != 0 && flag == true)
            {
                for (int i = 8; i > 0; i--)
                {
                    Button b = (Button)this.Controls["btn" + row + i.ToString()];
                    Button bu = (Button)this.Controls["btn" + row + (i-1).ToString()];

                    if (!(b.BackColor == Color.Purple || bu.BackColor == Color.Purple))
                        b.BackColor = bu.BackColor;
                    else
                        b.BackColor = Color.FromArgb(85, 85, 85);
                }
                Generate_car_on_left(row);
            }

            else if (row % 2 != 0 && flag==false || row % 2 == 0 && flag == true)
            {
                for (int i = 0; i < 8; i++)
                {
                    Button b = (Button)this.Controls["btn" + row + i.ToString()];
                    Button bu = (Button)this.Controls["btn" + row + (i + 1).ToString()];

                    if (!(b.BackColor == Color.Purple || bu.BackColor == Color.Purple))
                        b.BackColor = bu.BackColor;
                    else
                        b.BackColor = Color.FromArgb(85, 85, 85);
                }
                generate_car_on_right(row);
            }
        }

        public void Generate_car_on_left(int row)
        {
            Button b = (Button)this.Controls["btn" + row + 0.ToString()];

            int a = ra.Next(0,10);

            if (cars_left[row] == 0)
            {
                if (a < 2)
                {
                    b.BackColor = Color.Red;
                    cars_left[row]++;
                }
                else
                    b.BackColor = Color.FromArgb(85, 85, 85);
            }
            else if (cars_left[row] == 1)
            {
                b.BackColor = Color.Red;
                cars_left[row]++;
            }
            else if (cars_left[row] == 2)
            {
                b.BackColor = Color.FromArgb(85, 85, 85);
                cars_left[row] = 0;
            }

        }
        public void generate_car_on_right(int row)
        {
            Button b = (Button)this.Controls["btn" + row + 8.ToString()];

            int a = ra.Next(0,10);

            if (cars_right[row] == 0)
            {
                if(a<2)
                {
                    b.BackColor = Color.Red;
                    cars_right[row]++;
                }
                else
                    b.BackColor = Color.FromArgb(85, 85, 85);
            }
            else if(cars_right[row] == 1)
            {
                b.BackColor = Color.Red;
                cars_right[row]++;
            }
            else if(cars_right[row] == 2)
            {
                b.BackColor = Color.FromArgb(85, 85, 85);
                cars_right[row] = 0;
            }

        }

        public void shift_left_right()
        {
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Button b = (Button)this.Controls["btn" + i + j.ToString()];
                    if (b.BackColor == Color.Red || b.BackColor == Color.FromArgb(85, 85, 85))
                    {
                        move_car(left_right, i);
                        break;
                    }
                }
            }
        }

        public void end_of_game()
        {
            if(temp_color==Color.Red || temp_color == Color.Blue)
            {
                MessageBox.Show("Game Over");
                reset();
            }
        }

        private void reset()
        {
            score = 0;
            update_score();
            current_position = 4;
            temp_color = default(Color);
            street_counter = 0;

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Button b = (Button)this.Controls["btn" + i + j.ToString()];
                    if (!(i == 5 && j == 4))
                        b.BackColor = default(Color);
                    else
                        b.BackColor = Color.Purple;
                }
            }
        }

        public void update_score()
        {
            lblScore.Text = $"Score: {score}";
            lblScore.Refresh();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == 'w' || e.KeyChar == 'a' || e.KeyChar == 'd'))
                return;

            if (e.KeyChar == 'a')
            {
                Button b = (Button)this.Controls["btn" + 5 + (current_position + 1).ToString()];
                if(current_position<8 && b.BackColor==Color.Red)
                {
                    shift_left_right();
                    move_left();
                    b = (Button)this.Controls["btn" + 5 + (current_position + 1).ToString()];
                    b.BackColor = Color.Red;
                }
                else
                {
                    shift_left_right();
                    move_left();
                }
            }
            else if (e.KeyChar == 'd')
            {
                Button b = (Button)this.Controls["btn" + 5 + (current_position - 1).ToString()];
                if (current_position>0 && b.BackColor==Color.Red)
                {
                    shift_left_right();
                    move_right();
                    b = (Button)this.Controls["btn" + 5 + (current_position - 1).ToString()];
                    b.BackColor = Color.Red;
                }
                else
                {
                    shift_left_right();
                    move_right();
                }
            }
            else if (e.KeyChar == 'w')
            {
                shift_left_right();
                shift_down();

                if (street_counter == 0)
                {
                    generate_river();
                    street_counter++;
                }

                else if (street_counter > 0)
                {
                    generate_street();
                    street_counter++;
                    if (street_counter > 4)
                    {
                        street_counter = 0;
                    }
                }

                for (int i = 6; i > 0; i--)
                {
                    cars_left[i] = cars_left[i - 1];
                    cars_right[i] = cars_right[i - 1];
                }
                score++;
                cars_left[0] = 0;
                cars_right[0] = 0;
            }
            update_score();
            end_of_game();
        }
    }
}
