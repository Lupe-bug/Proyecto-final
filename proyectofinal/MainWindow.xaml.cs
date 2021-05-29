using proyectofinal.clases.Archivos;
using proyectofinal.clases.BaseDatos;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace proyectofinal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void cargarArchivoextrerno()
        {
            string fuente = @"C:\Users\barah\Downloads\archivo cvs\dat.csv";
            ClsArchivo ar = new ClsArchivo();
            ClsConexion cn = new ClsConexion();

            //obtener todo el archivo en un arreglo 
            string[] Arreglo = ar.LeerArchivo(fuente);
            string sentencia_sql = "";
            int numerolinea = 0;
            //iteramos sobre el arreglo linea por linea para luego convertirlo en datos individuales
            foreach (string linea in Arreglo)
            {
                string[] datos = linea.Split(';');
                if (numerolinea > 0)
                {
                    sentencia_sql += $"insert into venta values({datos[0]},'{datos[1]}','{datos[2]}',{datos[3]},{datos[4]});\n";

                }//end foreach
                numerolinea++;
                cn.EjecutaSQLDirecto(sentencia_sql);

            }
            numerolinea = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cargarArchivoextrerno();
        }
        

        public DataTable CargarDAtosDB(string condicion = "1=!")
        {
            ClsConexion cn = new ClsConexion();
            DataTable dt = new DataTable();
            String sentencia = $"select *from venta where {condicion}";
            dt = cn.consultaTablaDirecta(sentencia);
            return dt;
        }



        private void ButtonBuscarID_Click(object sender, RoutedEventArgs e)
        {
            string id = TexboxID.Text.Trim();
            string condicion = $"codlibro = {id}";
            DataTable dt = CargarDAtosDB(condicion);

            if (dt.Rows.Count > 0)
            {
                string nombre = dt.Rows[0].Field<string>("nomlibro");
                TexBoxNombre.Text = nombre;
            }
            else
            {
                TexBoxNombre.Text = "Ningun libro registrado por ese ID";
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Buttonbuscarpornombre_Click(object sender, RoutedEventArgs e)
        {
            string nombre = TexBoxNombre.Text.Trim();
            ClsConexion cn = new ClsConexion();
            DataTable dt = new DataTable();
            string sentencia = $" select * from venta where nomlibro like ('%{nombre}%')";
            dt = cn.consultaTablaDirecta(sentencia);
            if (dt.Rows.Count > 0)
            {
                int id = dt.Rows[0].Field<Int32>("codlibro");
                TexboxID.Text = id + "";
            }
            else
            {
                TexBoxNombre.Text = "Ningun libro registrado por ese ID";
            }
        }

        private void Buscar_por_fecha_Click(object sender, RoutedEventArgs e)
        {
            string fecha = TexBoxfecha.Text.Trim();
            ClsConexion cn = new ClsConexion();
            DataTable dt = new DataTable();
            string sentencia = $" select * from venta where fechcompra like ('%{fecha}%')";
            dt = cn.consultaTablaDirecta(sentencia);
            if (dt.Rows.Count > 0)
            {
                string nombre = dt.Rows[0].Field<string>("nomlibro");
                TexBoxNombre.Text = nombre;
            }
            else
            {
                TexBoxNombre.Text = "Ningun libro registrado por ese ID";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
