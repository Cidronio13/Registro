using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;

namespace Registro
{
    public partial class Datos : Form
    {
        string connectionString = "Data Source=RegistroUsuarios.db;Version=3;";
        private string rutaImagen = "";
        public string Encargado = "";

        public Datos(string DatosEncargado)
        {
            InitializeComponent();
            Encargado = DatosEncargado;
            CargarDatos();
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GuardarDatos();
            Imprimir();
            Limpiar();
        }
        private void CargarDatos()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Nombre, ApellidoPaterno, ApellidoMaterno,CURP FROM Usuarios";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    DgvMostrarUsu.DataSource = dataTable; 
                }
            }
        }
        bool EstaImagenBloqueada(Image imagen)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                return false;
            }
            catch (System.Runtime.InteropServices.ExternalException)
            {
                return true;
            }
        }
        public static byte[] ConvertirPictureBoxABytes(PictureBox pictureBox)
        {
            if (pictureBox.Image == null)
                throw new ArgumentNullException("El PictureBox no tiene una imagen.");

            using (MemoryStream ms = new MemoryStream())
            {
                pictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Puedes cambiar el formato si lo necesitas
                return ms.ToArray();
            }
        }

        public void GuardarDatos()
        {
            //try
            //{
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query;
                    if(PcbFoto.Image != null)
                    {
                        if (EstaImagenBloqueada(PcbFoto.Image) == false)
                        {
                        byte[] imagenBytes = ConvertirPictureBoxABytes(PcbFoto);
                        if (string.IsNullOrEmpty(TxtID.Text))
                            {
                                query = @"INSERT INTO Usuarios (Nombre, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, PaisOrigen, Nacionalidad, CURP, RFC, Ocupacion, TipoIdentificacion, NumeroIdentificacion, FechaExpedicion, LugarExpedicion, Calle, NumeroExterior, NumeroInterior, Colonia, AlcaldiaMunicipio, Estado, Pais, CodigoPostal, Telefono, Email, DocumentoEstanciaLegal, IdentificacionOficial, ComprobanteDomicilio, CURPAdjunto, RFCVigente, imagen) 
                        VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @FechaNacimiento, @PaisOrigen, @Nacionalidad, @CURP, @RFC, @Ocupacion, @TipoIdentificacion, @NumeroIdentificacion, @FechaExpedicion, @LugarExpedicion, @Calle, @NumeroExterior, @NumeroInterior, @Colonia, @AlcaldiaMunicipio, @Estado, @Pais, @CodigoPostal, @Telefono, @Email, @DocumentoEstanciaLegal, @IdentificacionOficial, @ComprobanteDomicilio, @CURPAdjunto, @RFCVigente, @imagen);";
                            }
                            else
                            {
                                query = $@"UPDATE Usuarios 
                        SET Nombre = @Nombre, 
                        ApellidoPaterno = @ApellidoPaterno, 
                        ApellidoMaterno = @ApellidoMaterno, 
                        FechaNacimiento = @FechaNacimiento, 
                        PaisOrigen = @PaisOrigen, 
                        Nacionalidad = @Nacionalidad, 
                        CURP = @CURP, 
                        RFC = @RFC, 
                        Ocupacion = @Ocupacion, 
                        TipoIdentificacion = @TipoIdentificacion, 
                        NumeroIdentificacion = @NumeroIdentificacion, 
                        FechaExpedicion = @FechaExpedicion, 
                        LugarExpedicion = @LugarExpedicion, 
                        Calle = @Calle, 
                        NumeroExterior = @NumeroExterior, 
                        NumeroInterior = @NumeroInterior, 
                        Colonia = @Colonia, 
                        AlcaldiaMunicipio = @AlcaldiaMunicipio, 
                        Estado = @Estado, 
                        Pais = @Pais, 
                        CodigoPostal = @CodigoPostal, 
                        Telefono = @Telefono, 
                        Email = @Email, 
                        DocumentoEstanciaLegal = @DocumentoEstanciaLegal, 
                        IdentificacionOficial = @IdentificacionOficial, 
                        ComprobanteDomicilio = @ComprobanteDomicilio, 
                        CURPAdjunto = @CURPAdjunto, 
                        RFCVigente = @RFCVigente, 
                        imagen = @imagen 
                        WHERE ID = {TxtID.Text};";
                            }
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Nombre", TxtNombre.Text);
                                command.Parameters.AddWithValue("@ApellidoPaterno", TxtApePat.Text);
                                command.Parameters.AddWithValue("@ApellidoMaterno", TxtApeMat.Text);
                                command.Parameters.AddWithValue("@FechaNacimiento", DtpFechaNaci.Value.ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@PaisOrigen", TxtPaisOri.Text);
                                command.Parameters.AddWithValue("@Nacionalidad", TxtNacionalidad.Text);
                                command.Parameters.AddWithValue("@CURP", TxtCurp.Text);
                                command.Parameters.AddWithValue("@RFC", TxtRfc.Text);
                                command.Parameters.AddWithValue("@Ocupacion", TxtActividad.Text);
                                command.Parameters.AddWithValue("@TipoIdentificacion", TxtTipoIde.Text);
                                command.Parameters.AddWithValue("@NumeroIdentificacion", TxtNumIde.Text);
                                command.Parameters.AddWithValue("@FechaExpedicion", DtpFechaExpedicion.Value.ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@LugarExpedicion", TxtLugarExpedicion.Text);
                                command.Parameters.AddWithValue("@Calle", TxtCalle.Text);
                                command.Parameters.AddWithValue("@NumeroExterior", TxtNumExterior.Text);
                                command.Parameters.AddWithValue("@NumeroInterior", TxtNumInterior.Text);
                                command.Parameters.AddWithValue("@Colonia", TxtColonia.Text);
                                command.Parameters.AddWithValue("@AlcaldiaMunicipio", TxtAlcaldia.Text);
                                command.Parameters.AddWithValue("@Estado", TxtEstado.Text);
                                command.Parameters.AddWithValue("@Pais", TxtPais.Text);
                                command.Parameters.AddWithValue("@CodigoPostal", TxtCp.Text);
                                command.Parameters.AddWithValue("@Telefono", TxtTelefono.Text);
                                command.Parameters.AddWithValue("@Email", TxtEmail.Text);
                                command.Parameters.AddWithValue("@DocumentoEstanciaLegal", TxtDocumentoExtran.Text);
                                command.Parameters.AddWithValue("@IdentificacionOficial", ChbIdentificacion.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@ComprobanteDomicilio", ChbComprobante.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@CURPAdjunto", ChbCurp.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@RFCVigente", ChbRfcActu.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@imagen", imagenBytes);

                                command.ExecuteNonQuery();
                                MessageBox.Show("Usuario insertado correctamente.");
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(TxtID.Text))
                            {
                                query = @"INSERT INTO Usuarios (Nombre, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, PaisOrigen, Nacionalidad, CURP, RFC, Ocupacion, TipoIdentificacion, NumeroIdentificacion, FechaExpedicion, LugarExpedicion, Calle, NumeroExterior, NumeroInterior, Colonia, AlcaldiaMunicipio, Estado, Pais, CodigoPostal, Telefono, Email, DocumentoEstanciaLegal, IdentificacionOficial, ComprobanteDomicilio, CURPAdjunto, RFCVigente) 
                        VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @FechaNacimiento, @PaisOrigen, @Nacionalidad, @CURP, @RFC, @Ocupacion, @TipoIdentificacion, @NumeroIdentificacion, @FechaExpedicion, @LugarExpedicion, @Calle, @NumeroExterior, @NumeroInterior, @Colonia, @AlcaldiaMunicipio, @Estado, @Pais, @CodigoPostal, @Telefono, @Email, @DocumentoEstanciaLegal, @IdentificacionOficial, @ComprobanteDomicilio, @CURPAdjunto, @RFCVigente);";
                            }
                            else
                            {
                                query = $@"UPDATE Usuarios 
                        SET Nombre = @Nombre, 
                        ApellidoPaterno = @ApellidoPaterno, 
                        ApellidoMaterno = @ApellidoMaterno, 
                        FechaNacimiento = @FechaNacimiento, 
                        PaisOrigen = @PaisOrigen, 
                        Nacionalidad = @Nacionalidad, 
                        CURP = @CURP, 
                        RFC = @RFC, 
                        Ocupacion = @Ocupacion, 
                        TipoIdentificacion = @TipoIdentificacion, 
                        NumeroIdentificacion = @NumeroIdentificacion, 
                        FechaExpedicion = @FechaExpedicion, 
                        LugarExpedicion = @LugarExpedicion, 
                        Calle = @Calle, 
                        NumeroExterior = @NumeroExterior, 
                        NumeroInterior = @NumeroInterior, 
                        Colonia = @Colonia, 
                        AlcaldiaMunicipio = @AlcaldiaMunicipio, 
                        Estado = @Estado, 
                        Pais = @Pais, 
                        CodigoPostal = @CodigoPostal, 
                        Telefono = @Telefono, 
                        Email = @Email, 
                        DocumentoEstanciaLegal = @DocumentoEstanciaLegal, 
                        IdentificacionOficial = @IdentificacionOficial, 
                        ComprobanteDomicilio = @ComprobanteDomicilio, 
                        CURPAdjunto = @CURPAdjunto, 
                        RFCVigente = @RFCVigente
                        WHERE ID = {TxtID.Text};";
                            }
                            using (var command = new SQLiteCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@Nombre", TxtNombre.Text);
                                command.Parameters.AddWithValue("@ApellidoPaterno", TxtApePat.Text);
                                command.Parameters.AddWithValue("@ApellidoMaterno", TxtApeMat.Text);
                                command.Parameters.AddWithValue("@FechaNacimiento", DtpFechaNaci.Value.ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@PaisOrigen", TxtPaisOri.Text);
                                command.Parameters.AddWithValue("@Nacionalidad", TxtNacionalidad.Text);
                                command.Parameters.AddWithValue("@CURP", TxtCurp.Text);
                                command.Parameters.AddWithValue("@RFC", TxtRfc.Text);
                                command.Parameters.AddWithValue("@Ocupacion", TxtActividad.Text);
                                command.Parameters.AddWithValue("@TipoIdentificacion", TxtTipoIde.Text);
                                command.Parameters.AddWithValue("@NumeroIdentificacion", TxtNumIde.Text);
                                command.Parameters.AddWithValue("@FechaExpedicion", DtpFechaExpedicion.Value.ToString("yyyy-MM-dd"));
                                command.Parameters.AddWithValue("@LugarExpedicion", TxtLugarExpedicion.Text);
                                command.Parameters.AddWithValue("@Calle", TxtCalle.Text);
                                command.Parameters.AddWithValue("@NumeroExterior", TxtNumExterior.Text);
                                command.Parameters.AddWithValue("@NumeroInterior", TxtNumInterior.Text);
                                command.Parameters.AddWithValue("@Colonia", TxtColonia.Text);
                                command.Parameters.AddWithValue("@AlcaldiaMunicipio", TxtAlcaldia.Text);
                                command.Parameters.AddWithValue("@Estado", TxtEstado.Text);
                                command.Parameters.AddWithValue("@Pais", TxtPais.Text);
                                command.Parameters.AddWithValue("@CodigoPostal", TxtCp.Text);
                                command.Parameters.AddWithValue("@Telefono", TxtTelefono.Text);
                                command.Parameters.AddWithValue("@Email", TxtEmail.Text);
                                command.Parameters.AddWithValue("@DocumentoEstanciaLegal", TxtDocumentoExtran.Text);
                                command.Parameters.AddWithValue("@IdentificacionOficial", ChbIdentificacion.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@ComprobanteDomicilio", ChbComprobante.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@CURPAdjunto", ChbCurp.Checked ? 1 : 0);
                                command.Parameters.AddWithValue("@RFCVigente", ChbRfcActu.Checked ? 1 : 0);

                                command.ExecuteNonQuery();
                                button1.Text = "Guardar";
                                button2.Text = "Guardar e imprimir";
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(TxtID.Text))
                        {
                            query = @"INSERT INTO Usuarios (Nombre, ApellidoPaterno, ApellidoMaterno, FechaNacimiento, PaisOrigen, Nacionalidad, CURP, RFC, Ocupacion, TipoIdentificacion, NumeroIdentificacion, FechaExpedicion, LugarExpedicion, Calle, NumeroExterior, NumeroInterior, Colonia, AlcaldiaMunicipio, Estado, Pais, CodigoPostal, Telefono, Email, DocumentoEstanciaLegal, IdentificacionOficial, ComprobanteDomicilio, CURPAdjunto, RFCVigente) 
                        VALUES (@Nombre, @ApellidoPaterno, @ApellidoMaterno, @FechaNacimiento, @PaisOrigen, @Nacionalidad, @CURP, @RFC, @Ocupacion, @TipoIdentificacion, @NumeroIdentificacion, @FechaExpedicion, @LugarExpedicion, @Calle, @NumeroExterior, @NumeroInterior, @Colonia, @AlcaldiaMunicipio, @Estado, @Pais, @CodigoPostal, @Telefono, @Email, @DocumentoEstanciaLegal, @IdentificacionOficial, @ComprobanteDomicilio, @CURPAdjunto, @RFCVigente);";
                        }
                        else
                        {
                            query = $@"UPDATE Usuarios 
                        SET Nombre = @Nombre, 
                        ApellidoPaterno = @ApellidoPaterno, 
                        ApellidoMaterno = @ApellidoMaterno, 
                        FechaNacimiento = @FechaNacimiento, 
                        PaisOrigen = @PaisOrigen, 
                        Nacionalidad = @Nacionalidad, 
                        CURP = @CURP, 
                        RFC = @RFC, 
                        Ocupacion = @Ocupacion, 
                        TipoIdentificacion = @TipoIdentificacion, 
                        NumeroIdentificacion = @NumeroIdentificacion, 
                        FechaExpedicion = @FechaExpedicion, 
                        LugarExpedicion = @LugarExpedicion, 
                        Calle = @Calle, 
                        NumeroExterior = @NumeroExterior, 
                        NumeroInterior = @NumeroInterior, 
                        Colonia = @Colonia, 
                        AlcaldiaMunicipio = @AlcaldiaMunicipio, 
                        Estado = @Estado, 
                        Pais = @Pais, 
                        CodigoPostal = @CodigoPostal, 
                        Telefono = @Telefono, 
                        Email = @Email, 
                        DocumentoEstanciaLegal = @DocumentoEstanciaLegal, 
                        IdentificacionOficial = @IdentificacionOficial, 
                        ComprobanteDomicilio = @ComprobanteDomicilio, 
                        CURPAdjunto = @CURPAdjunto, 
                        RFCVigente = @RFCVigente
                        WHERE ID = {TxtID.Text};";
                        }
                        using (var command = new SQLiteCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Nombre", TxtNombre.Text);
                            command.Parameters.AddWithValue("@ApellidoPaterno", TxtApePat.Text);
                            command.Parameters.AddWithValue("@ApellidoMaterno", TxtApeMat.Text);
                            command.Parameters.AddWithValue("@FechaNacimiento", DtpFechaNaci.Value.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@PaisOrigen", TxtPaisOri.Text);
                            command.Parameters.AddWithValue("@Nacionalidad", TxtNacionalidad.Text);
                            command.Parameters.AddWithValue("@CURP", TxtCurp.Text);
                            command.Parameters.AddWithValue("@RFC", TxtRfc.Text);
                            command.Parameters.AddWithValue("@Ocupacion", TxtActividad.Text);
                            command.Parameters.AddWithValue("@TipoIdentificacion", TxtTipoIde.Text);
                            command.Parameters.AddWithValue("@NumeroIdentificacion", TxtNumIde.Text);
                            command.Parameters.AddWithValue("@FechaExpedicion", DtpFechaExpedicion.Value.ToString("yyyy-MM-dd"));
                            command.Parameters.AddWithValue("@LugarExpedicion", TxtLugarExpedicion.Text);
                            command.Parameters.AddWithValue("@Calle", TxtCalle.Text);
                            command.Parameters.AddWithValue("@NumeroExterior", TxtNumExterior.Text);
                            command.Parameters.AddWithValue("@NumeroInterior", TxtNumInterior.Text);
                            command.Parameters.AddWithValue("@Colonia", TxtColonia.Text);
                            command.Parameters.AddWithValue("@AlcaldiaMunicipio", TxtAlcaldia.Text);
                            command.Parameters.AddWithValue("@Estado", TxtEstado.Text);
                            command.Parameters.AddWithValue("@Pais", TxtPais.Text);
                            command.Parameters.AddWithValue("@CodigoPostal", TxtCp.Text);
                            command.Parameters.AddWithValue("@Telefono", TxtTelefono.Text);
                            command.Parameters.AddWithValue("@Email", TxtEmail.Text);
                            command.Parameters.AddWithValue("@DocumentoEstanciaLegal", TxtDocumentoExtran.Text);
                            command.Parameters.AddWithValue("@IdentificacionOficial", ChbIdentificacion.Checked ? 1 : 0);
                            command.Parameters.AddWithValue("@ComprobanteDomicilio", ChbComprobante.Checked ? 1 : 0);
                            command.Parameters.AddWithValue("@CURPAdjunto", ChbCurp.Checked ? 1 : 0);
                            command.Parameters.AddWithValue("@RFCVigente", ChbRfcActu.Checked ? 1 : 0);

                            command.ExecuteNonQuery();
                            button1.Text = "Guardar";
                            button2.Text = "Guardar e imprimir";
                        }
                    }
                    
                }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("Ocurrió un error al guardar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

        }

        private void TxtNombre_TextChanged(object sender, EventArgs e)
        {
            string filtro = TxtNombre.Text.Trim();

            if (string.IsNullOrEmpty(filtro))
            {
                CargarDatos();  
            }
            else
            {
                FiltrarDatos(filtro);
            }
        }
        private void LlenarCampos(int idUsuario)
        {
            string connectionString = "Data Source=RegistroUsuarios.db;Version=3;";
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Usuarios WHERE ID = @ID";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", idUsuario);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtID.Text = reader["ID"].ToString();
                                TxtNombre.Text = reader["Nombre"].ToString();
                                TxtApePat.Text = reader["ApellidoPaterno"].ToString();
                                TxtApeMat.Text = reader["ApellidoMaterno"].ToString();
                                DtpFechaNaci.Value = reader["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(reader["FechaNacimiento"]) : DateTime.Now;
                                TxtPaisOri.Text = reader["PaisOrigen"].ToString();
                                TxtNacionalidad.Text = reader["Nacionalidad"].ToString();
                                TxtCurp.Text = reader["CURP"].ToString();
                                TxtRfc.Text = reader["RFC"].ToString();
                                TxtActividad.Text = reader["Ocupacion"].ToString();
                                TxtTipoIde.Text = reader["TipoIdentificacion"].ToString();
                                TxtNumIde.Text = reader["NumeroIdentificacion"].ToString();
                                DtpFechaExpedicion.Value = reader["FechaExpedicion"] != DBNull.Value ? Convert.ToDateTime(reader["FechaExpedicion"]) : DateTime.Now;
                                TxtLugarExpedicion.Text = reader["LugarExpedicion"].ToString();
                                TxtCalle.Text = reader["Calle"].ToString();
                                TxtNumExterior.Text = reader["NumeroExterior"].ToString();
                                TxtNumInterior.Text = reader["NumeroInterior"].ToString();
                                TxtColonia.Text = reader["Colonia"].ToString();
                                TxtAlcaldia.Text = reader["AlcaldiaMunicipio"].ToString();
                                TxtEstado.Text = reader["Estado"].ToString();
                                TxtPais.Text = reader["Pais"].ToString();
                                TxtCp.Text = reader["CodigoPostal"].ToString();
                                TxtTelefono.Text = reader["Telefono"].ToString();
                                TxtEmail.Text = reader["Email"].ToString();
                                TxtDocumentoExtran.Text = reader["DocumentoEstanciaLegal"].ToString();

                                ChbIdentificacion.Checked = Convert.ToInt32(reader["IdentificacionOficial"]) == 1;
                                ChbComprobante.Checked = Convert.ToInt32(reader["ComprobanteDomicilio"]) == 1;
                                ChbCurp.Checked = Convert.ToInt32(reader["CURPAdjunto"]) == 1;
                                ChbRfcActu.Checked = Convert.ToInt32(reader["RFCVigente"]) == 1;
                                button2.Text = "Actualizar e imprimir";
                                button1.Text = "Actualizar";
                                button3.Visible = true;
                                LblSeleccionado.Text = "Cliente seleccionado:";

                                TxtMostrarNombre.Text = reader["Nombre"].ToString();
                                TxtID.Visible = true;
                                TxtMostrarNombre.Visible = true;
                                LblID.Visible = true;
                                LblNombre.Visible = true;

                                if (reader["Imagen"] != DBNull.Value)
                                {
                                    byte[] imagenBytes = (byte[])reader["Imagen"];
                                    using (MemoryStream ms = new MemoryStream(imagenBytes))
                                    {
                                        PcbFoto.Image = System.Drawing.Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    PcbFoto.Image = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMostrarUsu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < DgvMostrarUsu.Rows.Count - 1)
            {
                DataGridViewRow row = DgvMostrarUsu.Rows[e.RowIndex];
                int idUsuario = Convert.ToInt32(row.Cells["ID"].Value);

                LlenarCampos(idUsuario);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            button1.Text = "Guardar";
            button2.Text = "Imprimir y guardar";
            Limpiar();
        }
        private void Limpiar()
        {
            TxtNombre.Text = string.Empty;
            TxtApePat.Text = string.Empty;
            TxtApeMat.Text = string.Empty;
            TxtPaisOri.Text = string.Empty;
            TxtNacionalidad.Text = string.Empty;
            TxtCurp.Text = string.Empty;
            TxtRfc.Text = string.Empty;
            TxtActividad.Text = string.Empty;
            TxtTipoIde.Text = string.Empty;
            TxtNumIde.Text = string.Empty;
            TxtLugarExpedicion.Text = string.Empty;
            TxtCalle.Text = string.Empty;
            TxtNumExterior.Text = string.Empty;
            TxtNumInterior.Text = string.Empty;
            TxtColonia.Text = string.Empty;
            TxtAlcaldia.Text = string.Empty;
            TxtEstado.Text = string.Empty;
            TxtPais.Text = string.Empty;
            TxtCp.Text = string.Empty;
            TxtTelefono.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtDocumentoExtran.Text = string.Empty;
            TxtID.Text = string.Empty;

            DtpFechaNaci.Value = DateTime.Now;
            DtpFechaExpedicion.Value = DateTime.Now;

            ChbIdentificacion.Checked = false;
            ChbComprobante.Checked = false;
            ChbCurp.Checked = false;
            ChbRfcActu.Checked = false;
            button3.Visible = false;
            PcbFoto.Image = null;
            LblID.Visible = false;
            LblNombre.Visible = false;
            TxtMostrarNombre.Visible = false;
            TxtID.Visible = false;
            LblSeleccionado.Text = "Cliente sin seleccionar";
        }
        private void FiltrarDatos(string filtro)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ID, Nombre, ApellidoPaterno, ApellidoMaterno, CURP FROM Usuarios WHERE Nombre LIKE @Filtro";

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Filtro", "%" + filtro + "%");

                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        DgvMostrarUsu.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al filtrar datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtNombre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string connectionString = "Data Source=RegistroUsuarios.db;Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ID, Nombre FROM Usuarios WHERE Nombre LIKE @Nombre";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", TxtNombre.Text);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TxtID.Text = reader["ID"].ToString();
                                
                            }
                        }
                    }
                }
                if (TxtID.Text != "")
                    LlenarCampos(int.Parse(TxtID.Text));
            }
        }

        private void TxtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TxtCp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el carácter si no es un número
            }
        }

        public void Imprimir()
        {
            string filePath = Path.GetFullPath("RegistroListo.pdf");
            string outputPath = Path.GetFullPath("ModificadoImprimir.pdf");

            ModificarCampoTexto(filePath, outputPath);

            Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });
        }
        public void ModificarCampoTexto(string filePath, string outputPath)
        {
            try
            {
                PdfReader pdfReader = new PdfReader(filePath);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, new FileStream(outputPath, FileMode.Create));

                AcroFields acroFields = pdfStamper.AcroFields;

                acroFields.SetField("Text1", DateTime.Now.ToString("dd-MM-yyyy"));
                acroFields.SetField("Text2", TxtNombre.Text);
                acroFields.SetField("Text3", TxtApePat.Text);
                acroFields.SetField("Text4", TxtApeMat.Text);
                acroFields.SetField("Text5", DtpFechaNaci.Value.ToString("dd-MM-yyyy"));
                acroFields.SetField("Text6", $"{TxtNombre.Text} {TxtApePat.Text} {TxtApeMat.Text}");
                acroFields.SetField("Text7", TxtPaisOri.Text);
                acroFields.SetField("Text8", TxtNacionalidad.Text);
                acroFields.SetField("Text9", TxtCurp.Text);
                acroFields.SetField("Text11", TxtRfc.Text);
                acroFields.SetField("Text12", TxtActividad.Text);
                acroFields.SetField("Text13", TxtTipoIde.Text);
                acroFields.SetField("Text14", TxtNumIde.Text);
                acroFields.SetField("Text15", DtpFechaExpedicion.Value.ToString("dd-MM-yyyy"));
                acroFields.SetField("Text16", TxtLugarExpedicion.Text);
                acroFields.SetField("Text17", TxtCalle.Text);
                acroFields.SetField("Text18", TxtNumExterior.Text);
                acroFields.SetField("Text19", TxtNumInterior.Text);
                acroFields.SetField("Text20", TxtColonia.Text);
                acroFields.SetField("Text21", TxtAlcaldia.Text);
                acroFields.SetField("Text22", TxtCp.Text);
                acroFields.SetField("Text23", TxtEstado.Text);
                acroFields.SetField("Text24", TxtPais.Text);
                acroFields.SetField("Text25", TxtTelefono.Text);
                acroFields.SetField("Text26", TxtEmail.Text);
                acroFields.SetField("Text27", TxtDocumentoExtran.Text);
                acroFields.SetField("Text32", Encargado);
                acroFields.SetField("Button28", "No");
                acroFields.SetField("Button29", "No");
                acroFields.SetField("Button30", "No");
                acroFields.SetField("Button31", "No");
                acroFields.SetField("Button28", ChbIdentificacion.Checked ? "No" : "Off");
                acroFields.SetField("Button29", ChbComprobante.Checked ? "No" : "Off"); 
                acroFields.SetField("Button30", ChbCurp.Checked ? "No" : "Off"); 
                acroFields.SetField("Button31", ChbRfcActu.Checked ? "No" : "Off"); 


                pdfStamper.Close();
                pdfReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar el PDF: " + ex.Message);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            Imprimir();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GuardarDatos();
            Limpiar();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TxtID.Visible = false;
            TxtID.Text = string.Empty;
            TxtMostrarNombre.Visible = false;
            LblNombre.Visible = false;
            LblID.Visible = false;
            button3.Visible = false;
            button2.Text = "Guardar e imprimir";
            button1.Text = "Guardar";
            LblSeleccionado.Text = "Cliente sin seleccionar";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                rutaImagen = openFileDialog.FileName;
                PcbFoto.Image = System.Drawing.Image.FromFile(rutaImagen);
            }
        }
    }
}
