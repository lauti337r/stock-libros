   M S B A M L   `   `   `  ����G  CStock Libros, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Stock_LibrosStock_Libros  5   	       Stock_Libros.EditarLibro   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Stock_Libros     5   	     � Title$  Editar Libro��5   	   $��346.667��6   $	��300��6/   .��5	      � .+�5
   
   �� -   5         ��Name$ label��5
      $��Titulo��6   $
��Left=�60   $	��Top=�6K   $��	10,10,0,0q�6c   �� 5   
   -   5        $ label1��5      $��Autor��6    $
��Left=�60   $	��Top=�6K   $��	10,41,0,0q�6c   �� 5   
   -   5        $ label2��5      $��	Editorial��6    $
��Left=�64   $	��Top=�6O   $��	10,72,0,0q�6g   �� 5   
   -   5        $ label3��5      $��Cantidad Vendidos��6    $
��Left=�6<   $	��Top=�6W   $��
10,103,0,0q�6o   �� 5   
   -   5         ��Name$ tituloEditar��5      $
��Left=�6(   $��23��6C    �� TextWrapping$
 Wrap=�6O   $	��Top=�6c   $	��120��6{   $��
125,14,0,0q�6�   �� 5   
   -   5        $ autorEditar��5      $
��Left=�6'   $��23��6B   $��
125,45,0,0q�6N   $
 Wrap=�6b   $	��Top=�6v   $	��120��6�   �� 5   
   -   5         ��Name$ editorialEditar��5      $
��Left=�6,   $��
125,75,0,0q�6G   $	��Top=�6[   $	��120��6s   $��23��6   �� 5   
   -   5        $ vendidosEditar��5      $
��Left=�6*   $��23��6E   $
 Wrap=�6Q   $	��Top=�6e   $	��120��6}   $��125,107,0,0q�6�   �� 5   
   -	   5        $ label4��5      $��Ultima Venta��6    $
��Left=�67   $	��Top=�6R   $��
10,134,0,0q�6j   (  "System.Windows.Controls.DatePicker  5   
   -
   5          Name$ ultimaVentaEditar��5      $
��Left=�60   $	��Top=�6K   $��125,135,0,0q�6c   $��24��6x   $	��120��6�   �� 5   
   -   5        $ label5��5      $��Cantidad Ingreso��6    $
��Left=�6;   $��
10,165,0,0q�6V   $	��Top=�6j   �� 5   
   -   5        $ ingresoEditar��5      $
��Left=�6)   $��23��6D   $��125,169,0,0q�6P   $
 Wrap=�6e   $	��Top=�6y   $	��120��6�   �� 5   
   -   5        $ label6��5      $��Ultimo Ingreso��6    $
��Left=�69   $��
10,195,0,0q�6T   $	��Top=�6h     5   
   -   5        $ ultimoIngresoEditar��5      $
��Left=�62   $��125,196,0,0q�6M   $	��Top=�6b   $��24��6z   $	��120��6�   �� 5   
   -   5        $ label7��5      $��Stock��6    $
��Left=�60   $��
10,226,0,0q�6K   $	��Top=�6_   �� 5   
   -   5        $ stockEditar��5      $
��Left=�6'   $��23��6B   $��125,230,0,0q�6N   $
 Wrap=�6c   $	��Top=�6w   $	��120��6�   �� 5   
   -   5         ��Name$ button��5      $��Guardar��6!   $
��Left=�63   $��125,276,0,0q�6N   $	��Top=�6c   $��75��6{   6�   5      5      