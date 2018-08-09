using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Data.SQLite;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace Stock_Libros
{
    /// <summary>
    /// Programa de control de stock de Libros para venta.
    /// Por LAUTARO ROMEO (lau337.r@gmail.com) - Paraná - ER, Argentina
    /// </summary>
    public partial class MainWindow : Window
    {
        public SQLiteConnection conn = new SQLiteConnection();
        public SQLiteCommand command;
        SQLiteDataAdapter da;
        DataTable dt;
        Window editar = new EditarLibro();

        public MainWindow()
        {
            conn = new SQLiteConnection("Data Source=data.sqlite;Version=3;");
            checkFile();

            InitializeComponent();
            refreshStockView();
            refreshEdit();

        }

        // ---->>>>>
        //1 - DEVUELVE DATATABLE CON LIBROS
        private DataTable listarLibros(int editorialID)
        {
            DataTable libros = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "SELECT id,Titulo FROM Libros WHERE Editorial=" + editorialID.ToString();
                da = new SQLiteDataAdapter(query, conn);
                da.Fill(libros);

                conn.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 1");
            }
            return libros;
        }

        //2- RECARGA LOS COMBOBOX DE LIBRO PARA INGRESO Y VENTA
        private void refreshLibro(string accion)
        {
            try
            {
                if (accion == "v")
                {
                    libroVenta.Items.Clear();
                    libroVenta.SelectedValuePath = "Key";
                    libroVenta.DisplayMemberPath = "Value";
                    DataTable librosVen = new DataTable();
                    librosVen = listarLibros(Convert.ToInt32(editorialVenta.SelectedValue));
                    foreach (DataRow row in librosVen.Rows)
                    {
                        int k = Convert.ToInt32(row.ItemArray[0]);
                        String v = row.ItemArray[1].ToString();
                        KeyValuePair<int, String> e = new KeyValuePair<int, string>(k, v);
                        libroVenta.Items.Add(e);
                    }
                }
                else if (accion == "i")
                {
                    libroIngreso.Items.Clear();
                    libroIngreso.SelectedValuePath = "Key";
                    libroIngreso.DisplayMemberPath = "Value";
                    DataTable librosIng = new DataTable();
                    librosIng = listarLibros(Convert.ToInt32(editorialIngreso.SelectedValue));
                    foreach (DataRow row in librosIng.Rows)
                    {
                        int k = Convert.ToInt32(row.ItemArray[0]);
                        String v = row.ItemArray[1].ToString();
                        KeyValuePair<int, String> e = new KeyValuePair<int, String>(k, v);
                        libroIngreso.Items.Add(e);
                    }
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 2");
            }
        }

        //3 - LLAMA A refreshLibro() AL CAMBIAR LA EDITORIAL EN VENTA
        private void editorialVenta_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                refreshLibro("v");
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 3");
            }
        }

        //4 - LLAMA A refreshLibro() AL CAMBIAR LA EDITORIAL EN INGRESO
        private void editorialIngreso_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                refreshLibro("i");
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 4");
            }
        }

        //5 - DEVUELVE TABLA PARA INFORME
        private DataTable dataInforme(int editorialID)
        {
            DataTable libros = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "SELECT Titulo,Autor,Vendidos,Stock,UltimoIngreso,Ingreso FROM Libros WHERE Editorial=" + editorialID.ToString();
                da = new SQLiteDataAdapter(query, conn);
                da.Fill(libros);
                conn.Close();

            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 5");
            }
            return libros;
        }

        //6 - DEVUELVE NOMBRE EDITORIAL (SEGUN ID)
        private string nombreEditorial(int editorialID)
        {
            string edit = "";
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "SELECT Editorial FROM Editorial WHERE id=" + editorialID.ToString();
                SQLiteCommand comm = new SQLiteCommand(query, conn);
                SQLiteDataReader dr = comm.ExecuteReader();
                while (dr.Read())
                {
                    edit = dr.GetString(0);
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 6");
            }
            return edit;
        }

        //7 - GENERA INFORME PDF
        private void informe(int editorial)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Pdf File |*.pdf";
                if (sfd.ShowDialog() == true)
                {
                    string nombreEdit = nombreEditorial(editorial);
                    DataTable libInf = dataInforme(editorial);
                    Document doc = new Document(PageSize.A4);
                    iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 10);
                    iTextSharp.text.Font font20 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 20);
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(sfd.FileName, FileMode.Create));
                    doc.SetMargins(10f, 10f, 20f, 20f);
                    doc.Open();


                    doc.Add(new iTextSharp.text.Paragraph("Informe - Editorial \"" + nombreEdit + "\"", font20));
                    doc.Add(Chunk.NEWLINE);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(libInf.Columns.Count);
                    float[] widths = new float[] { 6f, 6f, 3f, 3f, 3f, 3f };
                    table.SetWidths(widths);

                    table.WidthPercentage = 100;
                    PdfPCell cell = new PdfPCell(new Phrase("Libros"));
                    cell.Colspan = libInf.Columns.Count;

                    foreach (DataColumn dc in libInf.Columns)
                    {
                        table.AddCell(new Phrase(dc.ColumnName, font5));
                    }

                    //0 - Titulo
                    //1 - Autor
                    //2 - Vendidos
                    //3 - Stock
                    //4 - UltimoIngreso
                    //5 - Ingreso
                    foreach (DataRow r in libInf.Rows)
                    {
                        if (libInf.Rows.Count > 0)
                        {
                            table.AddCell(new Phrase(r[0].ToString(), font5));
                            table.AddCell(new Phrase(r[1].ToString(), font5));
                            table.AddCell(new Phrase(r[2].ToString(), font5));
                            table.AddCell(new Phrase(r[3].ToString(), font5));
                            table.AddCell(new Phrase(r[4].ToString(), font5));
                            table.AddCell(new Phrase(r[5].ToString(), font5));
                        }
                    }

                    doc.Add(table);
                    doc.Close();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 7");
            }


        }

        //8 - REGISTRA VENTA DE LIBROS (CANTIDAD Y FECHA DE VENTA)
        private void registrarVenta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(editorialVenta.Text)) MessageBox.Show("Seleccione la editorial");
                else if (String.IsNullOrEmpty(libroVenta.Text)) MessageBox.Show("Seleccione el libro");
                else if (String.IsNullOrEmpty(cantidadVenta.Text)) MessageBox.Show("Ingrese la cantidad");
                else if (String.IsNullOrEmpty(fechaVenta.Text)) MessageBox.Show("Ingrese la fecha");
                else
                {
                    string edit = editorialVenta.SelectedValue.ToString();
                    string libro = libroVenta.SelectedValue.ToString();
                    string q = cantidadVenta.Text;
                    string fecha = fechaVenta.SelectedDate.Value.Date.ToShortDateString();

                    SQLiteCommand ing = new SQLiteCommand();

                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "UPDATE Libros SET Stock = Stock-" + q + ",Vendidos = Vendidos+" + q + ", UltimaVenta=\'" + fecha + "\' WHERE id=" + libro;
                    ing = new SQLiteCommand(query, conn);
                    int affected = ing.ExecuteNonQuery();

                    if (affected == 1) MessageBox.Show("Venta registrada con exito");
                    else MessageBox.Show("Hubo un error al registrar la venta");

                    refreshStockView();
                    emptyBoxesVenta();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 8");
            }

        }

        //9 - REGISTRA INGRESO DE LIBROS (CANTIDAD Y FECHA DE INGRESO)
        private void registrarIngreso_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(editorialIngreso.Text)) MessageBox.Show("Seleccione la editorial");
                else if (String.IsNullOrEmpty(libroIngreso.Text)) MessageBox.Show("Seleccione el libro");
                else if (String.IsNullOrEmpty(cantidadIngreso.Text)) MessageBox.Show("Ingrese la cantidad");
                else if (String.IsNullOrEmpty(fechaIngreso.Text)) MessageBox.Show("Ingrese la fecha");
                else
                {
                    string edit = editorialIngreso.SelectedValue.ToString();
                    string libro = libroIngreso.SelectedValue.ToString();
                    string q = cantidadIngreso.Text;
                    string fecha = fechaIngreso.SelectedDate.Value.Date.ToShortDateString();

                    SQLiteCommand ing = new SQLiteCommand();

                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string query = "UPDATE Libros SET Ingreso=" + q + ", Stock = Stock+" + q + ", UltimoIngreso=\'" + fecha + "\' WHERE id=" + libro;
                    ing = new SQLiteCommand(query, conn);
                    int affected = ing.ExecuteNonQuery();

                    if (affected == 1) MessageBox.Show("Ingreso registrado con exito");
                    else MessageBox.Show("Hubo un error al registrar el ingreso");

                    refreshStockView();
                    emptyBoxesIngreso();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 9");
            }
        }

        //10 - REVISA QUE EL ARCHIVO DE LA DB EXISTA, DE NO SER ASI, SE CREA LA DB
        private void checkFile()
        {
            try
            {
                if (!File.Exists("data.sqlite"))
                {
                    if (!File.Exists("data.sqlite.bckp"))
                    {
                        conn.Open();
                        //TABLA EDITORIAL
                        string query = "CREATE TABLE `Editorial` ( `id` INTEGER PRIMARY KEY AUTOINCREMENT, `Editorial` TEXT NOT NULL )";
                        command = new SQLiteCommand(query, conn);
                        command.ExecuteNonQuery();
                        //INDEX EDITORIAL
                        query = "CREATE INDEX `EditorialID` ON `Editorial` (`id` )";
                        command = new SQLiteCommand(query, conn);
                        command.ExecuteNonQuery();
                        //TABLA LIBROS
                        query = "CREATE TABLE `Libros` (`id`	INTEGER PRIMARY KEY AUTOINCREMENT,`Titulo`	TEXT NOT NULL,`Autor`	TEXT,`Editorial`	INTEGER NOT NULL,`Ingreso`	INTEGER DEFAULT 0,`Vendidos`	INTEGER DEFAULT 0,`Stock`	INTEGER DEFAULT 0,`UltimaVenta`	TEXT,`UltimoIngreso`	TEXT,FOREIGN KEY(`Editorial`) REFERENCES `EditorialID`); ";
                        command = new SQLiteCommand(query, conn);
                        command.ExecuteNonQuery();

                        conn.Close();
                    }
                    else
                    {
                        File.Copy("data.sqlite.bckp", "data.sqlite");
                        checkFile();
                        return;
                    }
                }
                else
                {
                    File.Delete("data.sqlite.bckp");
                    File.Copy("data.sqlite", "data.sqlite.bckp");
                    conn = new SQLiteConnection("Data Source=data.sqlite;Version=3;");


                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 10");
            }
        }

        //11 - REVISA QUE EL LIBRO NO SE ENCUENTRE YA REGISTRADO EN LA DB
        private bool alreadyReg(string titulo, string autor, string editorial)
        {
            bool aR = false;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "SELECT count(*) FROM Libros WHERE Titulo=\'" + titulo + "\' AND Autor=\'" + autor + "\' AND Editorial=\'" + editorial + "\'";
                command = new SQLiteCommand(query, conn);
                int count = Convert.ToInt32(command.ExecuteScalar());
                if (count != 0)
                {
                    aR = true;
                    conn.Close();
                }
                aR = false;
                conn.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 11");
            }
            return aR;
        }

        //12 - REGISTRA LIBRO EN DB
        private void registrarLibro_Click(object sender, RoutedEventArgs e)
        {
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                SQLiteCommand registro = new SQLiteCommand();
                string[] data = dataFormReg();
                //0 - titulo
                //1 - autor
                //2 - editorial
                //3 - ingreso
                //4 - vendidos
                //5 - stock
                //6 - ultima venta 
                //7 - ultimo ingreso
                string query1 = "INSERT INTO Libros (id, Titulo, Autor, Editorial, Ingreso, Vendidos, Stock, UltimaVenta, UltimoIngreso) ";
                string query2 = "VALUES (null,\'" + data[0] + "\',\'" + data[1] + "\'," + data[2] + "," + data[3] + "," + data[4] + "," + data[5] + " ,\'" + data[6] + "\',\'" + data[7] + "\')";

                string query = query1 + query2;
                registro = new SQLiteCommand(query, conn);
                if (!alreadyReg(data[0], data[1], data[2]))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    registro.ExecuteNonQuery();
                    MessageBox.Show("Libro registrado con exito", "Mensaje");
                }
                else { MessageBox.Show("Libro ya registrado", "ERROR"); }
                conn.Close();
                refreshStockView();
                emptyBoxesReg();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 12");
            }
        }

        //13 - RECARGA LOS COMBOBOX DE EDITORIALES (REGISTRO E INFORMES)
        private void refreshEdit()
        {
            try
            {
                editorialBoxInf.Items.Clear();
                editorialBoxRegL.Items.Clear();
                editorialIngreso.Items.Clear();
                editorialVenta.Items.Clear();
                ((EditarLibro)editar).editorialEditar.Items.Clear();

                editorialBoxRegL.SelectedValuePath = "Key";
                editorialBoxRegL.DisplayMemberPath = "Value";
                editorialBoxInf.SelectedValuePath = "Key";
                editorialBoxInf.DisplayMemberPath = "Value";
                editorialIngreso.SelectedValuePath = "Key";
                editorialIngreso.DisplayMemberPath = "Value";
                editorialVenta.SelectedValuePath = "Key";
                editorialVenta.DisplayMemberPath = "Value";
                ((EditarLibro)editar).editorialEditar.SelectedValuePath = "Key";
                ((EditarLibro)editar).editorialEditar.DisplayMemberPath = "Value";

                DataTable editoriales = listarEdit();

                foreach (DataRow row in editoriales.Rows)
                {
                    int k = Convert.ToInt32(row.ItemArray[0]);
                    String v = row.ItemArray[1].ToString();
                    KeyValuePair<int, String> e = new KeyValuePair<int, String>(k, v);
                    editorialBoxRegL.Items.Add(e);
                    editorialBoxInf.Items.Add(e);
                    editorialIngreso.Items.Add(e);
                    editorialVenta.Items.Add(e);
                    ((EditarLibro)editar).editorialEditar.Items.Add(e);
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 13");
            }

        }

        //14 - SACA LOS DATOS DEL FORM DE REGISTRO DE LIBRO Y LOS DEVUELVE EN UN STRING[]
        private string[] dataFormReg()
        {
            string[] data = new string[8];
            try
            {
                if (String.IsNullOrEmpty(titulo.Text)) MessageBox.Show("Ingrese el titulo del libro");
                else if (String.IsNullOrEmpty(autor.Text)) MessageBox.Show("Ingrese el autor del libro");
                else if (String.IsNullOrEmpty(editorialBoxRegL.Text)) MessageBox.Show("Seleccione la editorial del libro");
                else if (String.IsNullOrEmpty(ingreso.Text)) MessageBox.Show("Ingrese la cantidad de ingreso");
                else if (String.IsNullOrEmpty(ultimoIngreso.Text)) MessageBox.Show("Ingrese la fecha de ingreso");
                else
                {
                    data[0] = titulo.Text.ToUpper().Replace("\'", "\'\'");
                    data[1] = autor.Text.ToUpper().Replace("\'", "\'\'");
                    data[2] = editorialBoxRegL.SelectedValue.ToString();
                    data[3] = ingreso.Text.ToString();
                    if (String.IsNullOrEmpty(vendidos.Text)) data[4] = "0";
                    else data[4] = vendidos.Text.ToString();
                    data[5] = (Convert.ToInt32(data[3]) - Convert.ToInt32(data[4])).ToString();
                    if (String.IsNullOrEmpty(ultimaVenta.Text)) data[6] = "0";
                    else data[6] = ultimaVenta.SelectedDate.Value.Date.ToShortDateString();
                    data[7] = ultimoIngreso.SelectedDate.Value.Date.ToShortDateString();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 14");
            }
            return data;
        }

        //15 - RECARGA EL DATAGRID StockView SEGUN titulo, autor y editorial
        private void refreshStockView(string titulo, string autor, string editorial)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                da = new SQLiteDataAdapter("SELECT L.id, L.Titulo, L.Autor, E.Editorial, L.Editorial, L.Stock, L.Vendidos, L.UltimaVenta, L.UltimoIngreso, L.Ingreso FROM libros AS L, Editorial AS E WHERE E.id = L.Editorial", conn);
                dt = new DataTable();
                da.Fill(dt);

                int quant = dt.Rows.Count;


                filterStockView();

                stockView.ItemsSource = dt.DefaultView;
                conn.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 15");
            }
        }

        //16 - LLAMA A refreshStockView con filtros vacios
        public void refreshStockView()
        {
            try
            {
                refreshStockView("", "", "");
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 16");
            }
        }

        //17 - DEVUELVE DATATABLE CON NOMBRES DE EDITORIALES
        private DataTable listarEdit()
        {
            DataTable editoriales = new DataTable();
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                da = new SQLiteDataAdapter("SELECT * FROM Editorial", conn);
                da.Fill(editoriales);
                conn.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 17");
            }
            return editoriales;
        }

        //18 - APLICA LOS FILTROS DE titulo, autor y editorial A LA DATATABLE
        private void filterStockView()
        {
            try
            {
                StringBuilder filter = new StringBuilder();
                if (!(string.IsNullOrEmpty(tituloBusqueda.Text)))
                    filter.Append("titulo Like '%" + tituloBusqueda.Text + "%'");

                if (!(string.IsNullOrEmpty(autorBusqueda.Text)))
                {
                    if (filter.Length > 0) filter.Append(" OR ");
                    filter.Append("autor Like '%" + autorBusqueda.Text + "%'");
                }

                if (!(string.IsNullOrEmpty(editorialBusqueda.Text)))
                {
                    if (filter.Length > 0) filter.Append(" OR ");
                    filter.Append("editorial Like '%" + editorialBusqueda.Text + "%'");
                }

                DataView dv = dt.DefaultView;
                dv.RowFilter = filter.ToString();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 18");
            }

        }

        //19 - REGISTRA EDITORIAL
        private void regEditorialButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(editorialReg.Text)) MessageBox.Show("Ingrese el nombre de la editorial a registrar");
                else
                {
                    string editorial = editorialReg.Text.ToUpper();
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    command = new SQLiteCommand("SELECT count(*) FROM Editorial WHERE Editorial = '" + editorial + "'", conn);

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 0)
                    {
                        string query = "INSERT INTO Editorial (id, Editorial) VALUES (null, '" + editorial + "')";
                        command = new SQLiteCommand(query, conn);
                        command.ExecuteNonQuery();
                        MessageBox.Show("Editorial registrada con exito");
                        refreshEdit();
                        editorialReg.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Error al registrar Editorial");
                    }
                    conn.Close();
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 19");
            }
        }

        //20 - LIMPIA FILTROS Y RECARGA STOCKVIEW
        private void noFilter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tituloBusqueda.Clear();
                autorBusqueda.Clear();
                editorialBusqueda.Clear();
                refreshStockView();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 20");
            }
        }

        //21 - VACIA EL STOCKVIEW
        private void vaciarStockView()
        {
            try
            {
                dt.Clear();
                stockView.ItemsSource = dt.DefaultView;
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 21");
            }
        }

        //22 - EVENTO DEL BOTON DE FILTRAR
        private void filterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vaciarStockView();
                refreshStockView(tituloBusqueda.Text, autorBusqueda.Text, editorialBusqueda.Text);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 22");
            }
        }

        //23 - MUESTRA MENSAJE AL HACER CLICK EN "ACERCA DE"
        private void info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Programa de control de stock de libros.\n\nPor Lautaro Romeo (lau337.r@gmail.com)");
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 23");
            }
        }

        //24 - VERIFICA QUE NO SE INGRESEN CARACTERES NO NUMERICOS EN LOS TEXTBOX
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                e.Handled = regex.IsMatch(e.Text);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 24");
            }
        }

        //25 - LLAMA A LA FUNCION informe(ID DE EDITORIAL)
        private void generarInforme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(editorialBoxInf.Text)) MessageBox.Show("Seleccione una editorial \npara generar el informe");
                informe(Convert.ToInt32(editorialBoxInf.SelectedValue));
                editorialBoxInf.SelectedIndex = 0;
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 25");
            }
        }

        //26 - MENU CONTEXTUAL STOCKVIEW (BOTON ELIMINAR)
        private void Context_Eliminar(object sender, System.EventArgs e)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                var contextMenu = (ContextMenu)menuItem.Parent;
                var item = (DataGrid)contextMenu.PlacementTarget;

                DataRowView drv = (DataRowView)item.SelectedCells[0].Item;

                string idLibro = drv[0].ToString();

                if (MessageBox.Show("Estas seguro?", "", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (eliminarLibro(idLibro))
                    {
                        MessageBox.Show("Libro eliminado con exito");
                        refreshStockView();
                    }
                    else
                    {
                        MessageBox.Show("Hubo un error al eliminar el libro");
                        refreshStockView();
                    }
                }
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 26");
            }




        }

        //27 - MENU CONTEXTUAL STOCKVIEW (BOTON REGISTRAR INGRESO)
        private void Context_Ingreso(object sender, System.EventArgs e)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                var contextMenu = (ContextMenu)menuItem.Parent;
                var item = (DataGrid)contextMenu.PlacementTarget;

                DataRowView drv = (DataRowView)item.SelectedCells[0].Item;

                string libroID = drv[0].ToString();
                string editorialID = drv[4].ToString();

                tabControl.SelectedIndex = 4;
                editorialIngreso.SelectedValue = Convert.ToInt32(editorialID);
                refreshLibro("i");
                libroIngreso.SelectedValue = Convert.ToInt32(libroID);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 27");
            }
        }

        //28 - MENU CONTEXTUAL STOCKVIEW (BOTON REGISTRAR VENTA)
        private void Context_Venta(object sender, System.EventArgs e)
        {
            try
            {
                var menuItem = (MenuItem)sender;
                var contextMenu = (ContextMenu)menuItem.Parent;
                var item = (DataGrid)contextMenu.PlacementTarget;

                DataRowView drv = (DataRowView)item.SelectedCells[0].Item;

                string libroID = drv[0].ToString();
                string editorialID = drv[4].ToString();

                //0 - "Registrar Libro"
                //1 - "Ver stock disponible"
                //2 - "Informes"
                //3 - "Registrar Editorial"
                //4 - "Registrar Ingreso"
                //5 - "Registrar Venta"

                tabControl.SelectedIndex = 5;
                editorialVenta.SelectedValue = Convert.ToInt32(editorialID);
                refreshLibro("v");
                libroVenta.SelectedValue = Convert.ToInt32(libroID);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 28");
            }
        }

        //29 - ELIMINA EL REGISTRO DEL LIBRO DE LA DB
        private bool eliminarLibro(string id)
        {
            int x = 2;
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                string query = "DELETE FROM Libros WHERE id=" + id;
                command = new SQLiteCommand(query, conn);
                command.ExecuteNonQuery();
                query = "SELECT count(*)  FROM Libros WHERE id=" + id;
                command = new SQLiteCommand(query, conn);
                x = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 29");
            }
            if (x == 0)
                return true;
            else return false;
        }

        //30 - LIMPIAR CAMPOS DE REGISTRAR LIBRO
        private void emptyBoxesReg()
        {
            try
            {
                titulo.Text = "";
                autor.Text = "";
                editorialBoxRegL.SelectedIndex = -1;
                ingreso.Text = "";
                ultimoIngreso.Text = "";
                vendidos.Text = "";
                ultimaVenta.Text = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 30");
            }
        }

        //31 - LIMPIAR CAMPOS DE REGISTRAR INGRESO
        private void emptyBoxesIngreso()
        {
            try
            {
                editorialIngreso.SelectedIndex = 0;
                libroIngreso.SelectedIndex = 0;
                cantidadIngreso.Text = "";
                fechaIngreso.Text = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 31");
            }
        }

        //32 - LIMPIAR CAMPOS DE REGISTRAR VENTA
        private void emptyBoxesVenta()
        {
            try
            {
                editorialVenta.SelectedIndex = 0;
                libroVenta.SelectedIndex = 0;
                cantidadVenta.Text = "";
                fechaVenta.Text = "";
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 32");
            }
        }

        //33 - MENU CONTEXTUAL STOCKVIEW (BOTON EDITAR)
        private void Context_Editar(object sender, System.EventArgs e)
        {
            try
            {
                editar = new EditarLibro();
                editar.Owner = this;
                var menuItem = (MenuItem)sender;
                var contextMenu = (ContextMenu)menuItem.Parent;
                var item = (DataGrid)contextMenu.PlacementTarget;
                DataRowView drv = (DataRowView)item.SelectedCells[0].Item;

                string libroID = drv[0].ToString();
                string[] data = getDataLibro(libroID);

                refreshEdit();
                try
                {
                    ((EditarLibro)editar).setData(data);
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.Message.ToString(), "Error 33.2");
                }
                try
                {
                    editar.Show();
                    conn.Close();
                }
                catch (Exception f)
                {
                    MessageBox.Show(f.Message.ToString(), "Error 33.3");
                }

            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 33");
            }
        }

        //34 - OBTENER DATA DE UN SOLO LIBRO  (RECIBE ID)
        private string[] getDataLibro(string id)
        {
            DataTable libro = new DataTable();
            string[] data = new string[9];
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
                string query = "";
                if (conn.State == ConnectionState.Closed) conn.Open();
                try { query = "SELECT * FROM Libros WHERE id=" + id; }
                catch (Exception f) { MessageBox.Show(f.Message.ToString(), "Error 34.1"); }
                try { da = new SQLiteDataAdapter(query, conn); }
                catch (Exception f) { MessageBox.Show(f.Message.ToString(), "Error 34.2"); }
                try { da.Fill(libro); }
                catch (Exception f) { MessageBox.Show(f.Message.ToString(), "Error 34.3"); }
                data[0] = libro.Rows[0].ItemArray[0].ToString();
                data[1] = libro.Rows[0].ItemArray[1].ToString();
                data[2] = libro.Rows[0].ItemArray[2].ToString();
                data[3] = libro.Rows[0].ItemArray[3].ToString();
                data[4] = libro.Rows[0].ItemArray[4].ToString();
                data[5] = libro.Rows[0].ItemArray[5].ToString();
                data[6] = libro.Rows[0].ItemArray[6].ToString();
                data[7] = libro.Rows[0].ItemArray[7].ToString();
                data[8] = libro.Rows[0].ItemArray[8].ToString();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message.ToString(), "Error 34");
            }
            return data;
        }

        //LISTAR INFO DE EDITORIALES


        //---->>>>

        //SIN USO

        private void precio_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void stockView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void tituloBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void autorBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void editorialBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void titulo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void autor_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void editorial_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void precio_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void editorialBoxInf_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cantidadIngreso_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        //---->>>>

    }

}


