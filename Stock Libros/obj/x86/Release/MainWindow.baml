   M S B A M L   `   `   `  ���� G  CStock Libros, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Stock_LibrosStock_Libros      Stock_Libros.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     ,localclr-namespace:Stock_Libros       � Title$  Stock Libros��$��413.529��$��722.294��.��� .+��� -    ��Name$ 
tabControl��$
��Left=�$	��383��$	��Top=�$	��714��.@��� $��Registrar libro��.��� ���	#FFE5E5E5 � RowDefinitions �� $��3* ��� $	��68* � � ColumnDefinitions �� $	��77* ��� $
��631* �.+��� -    ��Name$ tituloLabel��$��Titulo��$
��Left=�$	��Top=�$��	10,11,0,0q�$��1��$��26��$	��115��$��2���� -   $ 
autorLabel��$��Autor��$
��Left=�$	��Top=�$��	10,25,0,0q�$��1��$��26��$	��115��$��1���� -   $ editorialLabel��$��	Editorial��$
��Left=�$	��Top=�$��	10,52,0,0q�$��1��$��26��$	��115��$��1���� -    ��Name$ titulo��$
��Left=�$��23�� �� TextWrapping$
 Wrap=�$	��Top=�$	��300��$��	125,0,0,0q�$��1��$��1���� -   $ autor��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��300��$��
125,28,0,0q�$��1��$��1���� -    ��Name$ registrarLibro��$��Registrar libro��$
��Left=�$	��Top=�$	��150��$��125,230,0,0q�$��1��$��20��$��1���� -    ��Name$ editorialBoxRegL��$
��Left=�$	��Top=�$	��300��$��
125,56,0,0q�$��1��$��1��$	 �� IsSynchronizedWithCurrentItem$
	 TrueY�
 �� 
IsReadOnly
 . �� -	   $ vendidosLabel��$��Cantidad Vendidos��$
��Left=�$	��Top=�$��26��$��1��$��
10,111,0,0q�$��1��$	��115���� -
   $ vendidos��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��1��$��125,111,0,0q�$��1���� -   $ label1��$��Cantidad Ingreso��$
��Left=�$	��Top=�$��26��$��1��$��	10,83,0,0q�$��1��$	��115���� -   $ ingreso��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��1��$��
125,83,0,0q�$��1���� -   $ label2��$��Última Venta��$
��Left=�$	��Top=�$��26��$��1��$��266,111,0,0q�$��1��(  "System.Windows.Controls.DatePicker  -     Name$ ultimaVenta��$
��Left=�$	��Top=�$��23��$��1��$��361,111,0,0q�$��1��   SelectedDateFormat$ Short=��� -   $ label9��$��Último Ingreso��$
��Left=�$	��Top=�$��26��$	��115��$��1��$��
266,83,0,0q�$��1��  -   $ ultimoIngreso��$
��Left=�$	��Top=�$��1��$��
361,83,0,0q�$��1��$��23���� $��Ver stock disponible��$��3,0,-7,0q�.��� ���	#FFE5E5E5 �� $
��118* ��� $
��235* �.+�&   System.Windows.Controls.DataGrid  -     Name$ 	stockView��$
��Left=�$	��Top=�$	��285��$	��688��$��	10,41,0,0q�$��2��$��1q� �� IsTextSearchEnabled .  J� 	AllowDrop .    
IsReadOnly .    AutoGenerateColumns .     Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ Titulo��1  +System.Windows.Controls.DataGridBoundColumn   Binding �� *Titulo+  $ Autor�� �� *Autor+  $ 	Editorial�� �� *	Editorial+  $ Stock�� �� *Stock+  $ Vendidos�� �� *
Vendidos+  $ UltimaVenta�� �� *UltimaVenta+  $ Ultimo Ingreso�� �� *UltimoIngreso+  $ Ingreso�� �� *	Ingreso+ � ContextMenu �� .@�m� -   $��Registrar Ingreso��m� -   $��Registrar Venta��m� -   $��Editar Libro��m� -   $��Eliminar Libro���� -   $ tituloBusqueda��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��50,7,0,0q��� -   $ autorBusqueda��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��	220,7,0,0q��� -   $ editorialBusqueda��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��	398,7,0,0q��� -   $ filterButton��$��Filtrar��$
��Left=�$	��Top=�$��51��$��
523,10,0,0q�$��20���� -   $ noFilter��$��Limpiar filtro��$
��Left=�$	��Top=�$��75��$��
579,10,0,0q�$��20���� -   $ editorialLabel1��$��	Editorial��$
��Left=�$	��Top=�$��	345,4,0,0q��� -   $ autorLabel1��$��Autor��$
��Left=�$	��Top=�$��	175,4,0,0q��� -   $ tituloLabel1��$��Titulo��$
��Left=�$	��Top=�$��10,4,0,0q��� -    ��Name$ informes��$��Informes��$
��Left=�$��61��$��427,0,-427,0q�.��� ���	#FFE5E5E5.+��� -   $ editorialBoxInf��$
��Left=�$	��Top=�$	��120��$��
128,10,0,0q��� -    $ label��$��Seleccione editorial��$
��Left=�$	��Top=�$��10,6,0,0q��� -!   $ generarInforme��$��Generar informe��$
��Left=�$	��Top=�$	��120��$��	253,9,0,0q��� $��Registrar Editorial��$
��Left=�$��20��$	��Top=�$	��112��$��251,0,-251,0q�.��� ���	#FFE5E5E5 �� $��3* ��� $	��68* � �� $	��77* ��� $
��631* �.+��� -"   $ editorialLabelReg��$��Nombre editorial��$
��Left=�$	��Top=�$��2��$��	10,10,0,0q�$��2���� -#   $ editorialReg��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��39,0,0,0q�$��1��$��1���� -$   $ regEditorialButton��$��Registrar Editorial��$��1��$
��Left=�$��	164,1,0,0q�$��1��$	��Top=�$	��120���� $��Registrar Ingreso��$��Right=�$��20��$��Bottom=�$	��115��$��-163,0,163,0q�.��� ���	#FFE5E5E5.+��� -%   $ editorialIngreso��$
��Left=�$	��Top=�$	��120��$��
115,14,0,0q��� -&   $ label3��$��	Editorial��$
��Left=�$	��Top=�$��	10,10,0,0q��� -'   $ label4��$��Libro��$
��Left=�$	��Top=�$��	10,41,0,0q��� -(   $ label5��$��Cantidad Ingreso��$
��Left=�$	��Top=�$��	10,72,0,0q��� -)   $ libroIngreso��$
��Left=�$	��Top=�$	��120��$��
115,45,0,0q��� -*   $ cantidadIngreso��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��
115,75,0,0q��� -+   $ registrarIngreso��$��Registrar ingreso��$
��Left=�$	��Top=�$	��120��$��115,154,0,0q��� -,   $ fechaIngresoLabel��$��Fecha Ingreso��$
��Left=�$	��Top=�$��
10,103,0,0q�  --   $ fechaIngreso��$
��Left=�$	��Top=�$��115,104,0,0q��� $��Registrar venta��$
��Left=�$��97��$��
-160,0,0,0q�.��� ���	#FFE5E5E5.+��� -.   $ editorialVenta��$
��Left=�$	��Top=�$	��120��$��
107,14,0,0q��� -/   $ 
libroVenta��$
��Left=�$	��Top=�$	��120��$��
107,45,0,0q��� -0   $ label6��$��	Editorial��$
��Left=�$��	10,10,0,0q�$	��Top=��� -1   $ label7��$��Libro��$
��Left=�$	��Top=�$��	10,41,0,0q��� -2   $ label8��$��Cantidad Venta��$
��Left=�$	��Top=�$��	10,72,0,0q��� -3   $ cantidadVenta��$
��Left=�$��23��$
 Wrap=�$	��Top=�$	��120��$��
107,75,0,0q��� -4   $ registrarVenta��$��Registrar Venta��$
��Left=�$	��Top=�$	��120��$��107,144,0,0q��� -5   $ fechaVentaLabel��$��Fecha Venta��$
��Left=�$	��Top=�$��
10,104,0,0q�  -6   $ 
fechaVenta��$
��Left=�$	��Top=�$��107,104,0,0q��� -7   $
 info��$��	Acerca de��$
��Left=�$��629,353,0,0q�$	��Top=�$��75��