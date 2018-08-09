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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace Stock_Libros
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditarLibro : Window
    {
        //SQLiteConnection conn = new SQLiteConnection();
        SQLiteCommand command;
        private string[] dataLibro;

        public EditarLibro()
        {
            InitializeComponent();
        }

        //E1 - LLENA LOS CAMPOS DEL FORM CON LOS DATOS ACTUALES DEL LIBRO
        public void setData(string[] data)
        {
            //0 - id
            //1 - Titulo
            //2 - Autor
            //3 - Editorial
            //4 - Ingreso
            //5 - Vendidos
            //6 - Stock
            //7 - Ultima Venta
            //8 - Ultimo Ingreso 

            try
            {
                dataLibro = data;
                tituloEditar.Text = data[1];
                autorEditar.Text = data[2];
                editorialEditar.SelectedValue = data[3];
                ingresoEditar.Text = data[4];
                vendidosEditar.Text = data[5];
                stockEditar.Text = data[6];
                if (data[7] == "0") ultimaVentaEditar.Text = "";
                else ultimaVentaEditar.Text = data[7];
                ultimoIngresoEditar.Text = data[8];
            }
            catch(Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error E1");
            }

        }

        //E2 - SE GUARDAN LOS DATOS EDITADOS DEL LIBRO
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "", q6 = "", q7 = "", q8 = "", q9 = "", q10 = "";
                try
                {
                    q1 = "UPDATE Libros SET ";
                    q2 = "Titulo=\'" + tituloEditar.Text + "\'";
                    q3 = ", Autor=\'" + autorEditar.Text + "\'";
                }
                catch(Exception f)
                {
                    MessageBox.Show(f.Message.ToString(),"Error E2.1");
                }
                q4 = ", Editorial=" + editorialEditar.SelectedValue.ToString();
                q5 = ", Ingreso=" + ingresoEditar.Text;
                if (vendidosEditar.Text == null)
                    q6 = "0";
                else q6 = ", Vendidos=" + vendidosEditar.Text;
                q7 = ", Stock=" + stockEditar.Text;
                q8 = ", UltimaVenta=\'";
                if (ultimaVentaEditar.SelectedDate != null)
                    q8 += ultimaVentaEditar.SelectedDate.Value.Date.ToShortDateString() + "\'";
                else q8 += "0\'";

                q9 = ", UltimoIngreso=\'" + ultimoIngresoEditar.SelectedDate.Value.Date.ToShortDateString() + "\'";
                q10 = " WHERE id=" + dataLibro[0];

                string query = q1 + q2 + q3 + q4 + q5 + q6 + q7 + q8 + q9 + q10;
                SQLiteConnection connE = ((MainWindow)this.Owner).conn;
                connE.Open();
                command = new SQLiteCommand(query, connE);
                command.ExecuteNonQuery();
                connE.Close();
                ((MainWindow)this.Owner).refreshStockView();
                this.Close();
            }
            catch(Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error E2");
            }
        }
    }
}
