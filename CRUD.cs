using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security;
using System.IO;

namespace HairSystem
{
    public partial class FrmClientes : Form
    {

        string strCodigoUF = "";
     
        public FrmClientes()
        {
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            pnlConsulta.Enabled = false;
            pnlCadastro.Enabled = true;
            pnlFotos.Enabled = true;
            btnExcluir.Enabled = false;
            btnAlterar.Enabled = false;
            btnGravar.Enabled = true;
            btnCancelar.Enabled = true;
            pnlCadastro.Enabled = true;
            LimpaCampos();
        }

        private void LimpaCampos()
        {
            txbCodigo.Enabled = false;

            txbCodigo.Text = "";
            txbNome.Text = "";
            txbApelido.Text = "";
            txbCelular.Text = "";
            txbLogradouro.Text = "";
            txbPesquisa.Text = "";
            txbNum.Text = "";
            txbBairro.Text = "";
            txbEmail.Text = "";           
            txbOBS.Text = "";
            
            mtbDtAniv.Text = "";
            mtbCEP.Text = "";            
            mtbCEP.Text = "";
            mtbCPF.Text = "";

            mtbDiaCorte1.Text = "";
            mtbDiaCorte2.Text = "";

            dgvFotos.Dispose();            
            pnlFotos.Visible = false;

            picImagem2.Image = null;

            mtbCPF.Focus();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Excluir o Cliente?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ExcluirRegistro();
            }
        }
        
        private void ExcluirRegistro()
        {
            string scon = ClassSerrano.GetConnectionStrings();

            SqlConnection con = new SqlConnection(scon);
                                               
            if (dgvFotos.Rows.Count <= 0)
            {
                try
                { 
                    string sSQL = " DELETE FROM Clientes ";                   
                    sSQL = sSQL + " WHERE Cli_Codigo = @Cli_Codigo ";
                    
                    SqlCommand cmd = new SqlCommand(sSQL, con);
                    cmd.Parameters.AddWithValue("@Cli_Codigo", txbCodigo.Text);

                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Cliente excluido com sucesso!");

                    pnlCadastro.Enabled = false;
                    btnNovo.Enabled = true;
                    btnGravar.Enabled = false;
                    btnExcluir.Enabled = false;
                    btnAlterar.Enabled = false;
                    LimpaCampos();
                    Pesquisar();

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + ex);
                }

                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Existe uma ou mais fotos gravadas para este cliente. Por favor, excluir as fotos antes de excluir o Cliente.");
            }
        }

        private void btnGravar_Click(object sender, EventArgs e)
        {
            // validar os campos...

            if (txbNome.Text.Trim() != "")
            {
                if (btnNovo.Enabled)  
                {
                    if (MessageBox.Show("Deseja gravar o novo Cliente?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        InserirRegistro();
                    }
                }
                else
                {
                    if (MessageBox.Show("Deseja alterar os dados do Cliente?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AlterarRegistro();
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, informar o nome do cliente...");
                txbNome.Focus();
            }
        }

        private void InserirRegistro()
        {
            string scon = ClassSerrano.GetConnectionStrings();

            SqlConnection con = new SqlConnection(scon);

            try
            { 
           
                String sSQL = " INSERT INTO Clientes (Cli_Nome, Cli_Apelido, Cli_Celular, Cli_DtAniversario, Cli_CPF, Cli_Logradouro,";
                sSQL = sSQL + " Cli_Numero, Cli_CEP, Cli_Bairro, Cid_CodIBGE, UF_CodIBGE, Cli_OBS, Cli_eMail, Cli_DiaCorte, Cli_DiaCorte2) ";
                sSQL = sSQL + " VALUES (@Nome, @Apelido, @Celular, @DtAniv, @CPF, @Logradouro, @Num, @CEP, @Bairro, @Cod_Cidade, ";
                sSQL = sSQL + " @Cod_UF, @OBS, @eMail, @DiaCorte1, @DiaCorte2)  ";
           
                SqlCommand cmd = new SqlCommand(sSQL, con);
                
                cmd.Parameters.AddWithValue("@Nome", txbNome.Text);
                cmd.Parameters.AddWithValue("@Apelido", txbApelido.Text);
                cmd.Parameters.AddWithValue("@Celular", txbCelular.Text);
                cmd.Parameters.AddWithValue("@DtAniv", mtbDtAniv.Text);
                cmd.Parameters.AddWithValue("@CPF", mtbCPF.Text);
                cmd.Parameters.AddWithValue("@Logradouro", txbLogradouro.Text);
                cmd.Parameters.AddWithValue("@Num", txbNum.Text);
                cmd.Parameters.AddWithValue("@CEP", mtbCEP.Text);
                cmd.Parameters.AddWithValue("@Bairro", txbBairro.Text);
                cmd.Parameters.AddWithValue("@Cod_Cidade", Convert.ToString(cbCidade.SelectedValue));
                cmd.Parameters.AddWithValue("@Cod_UF", Convert.ToString(cbUF.SelectedValue));
                cmd.Parameters.AddWithValue("@OBS", txbOBS.Text);
                cmd.Parameters.AddWithValue("@eMail", txbEmail.Text);
                cmd.Parameters.AddWithValue("@DiaCorte1", mtbDiaCorte1.Text);
                cmd.Parameters.AddWithValue("@DiaCorte2", mtbDiaCorte2.Text);
               
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente incluido com sucesso!");

                pnlCadastro.Enabled = false;
                pnlFotos.Enabled = false;
                btnNovo.Enabled = true;
                btnGravar.Enabled = false;
                btnExcluir.Enabled = false;
                btnAlterar.Enabled = false;
                LimpaCampos();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + ex);
            }
            finally
            {
                con.Close();
            }
        }

        private void InserirRegistroPaiAguardaFoto()
        {
            string scon = ClassSerrano.GetConnectionStrings();
            SqlConnection con = new SqlConnection(scon);

            try
            {
                String sSQL = " INSERT INTO Clientes (Cli_Nome, Cli_Apelido, Cli_Celular, Cli_DtAniversario, Cli_CPF, Cli_Logradouro,";
                sSQL = sSQL + " Cli_Numero, Cli_CEP, Cli_Bairro, Cid_CodIBGE, UF_CodIBGE, Cli_OBS, Cli_eMail, Cli_DiaCorte, Cli_DiaCorte2) ";
                sSQL = sSQL + " VALUES (@Nome, @Apelido, @Celular, @DtAniv, @CPF, @Logradouro, @Num, @CEP, @Bairro, @Cod_Cidade, ";
                sSQL = sSQL + " @Cod_UF, @OBS, @eMail, @DiaCorte1, @DiaCorte2)  ";

                SqlCommand cmd = new SqlCommand(sSQL, con);

                cmd.Parameters.AddWithValue("@Nome", txbNome.Text);
                cmd.Parameters.AddWithValue("@Apelido", txbApelido.Text);
                cmd.Parameters.AddWithValue("@Celular", txbCelular.Text);
                cmd.Parameters.AddWithValue("@DtAniv", mtbDtAniv.Text);
                cmd.Parameters.AddWithValue("@CPF", mtbCPF.Text);
                cmd.Parameters.AddWithValue("@Logradouro", txbLogradouro.Text);
                cmd.Parameters.AddWithValue("@Num", txbNum.Text);
                cmd.Parameters.AddWithValue("@CEP", mtbCEP.Text);
                cmd.Parameters.AddWithValue("@Bairro", txbBairro.Text);
                cmd.Parameters.AddWithValue("@Cod_Cidade", Convert.ToString(cbCidade.SelectedValue));
                cmd.Parameters.AddWithValue("@Cod_UF", Convert.ToString(cbUF.SelectedValue));
                cmd.Parameters.AddWithValue("@OBS", txbOBS.Text);
                cmd.Parameters.AddWithValue("@eMail", txbEmail.Text);
                cmd.Parameters.AddWithValue("@DiaCorte1", mtbDiaCorte1.Text);
                cmd.Parameters.AddWithValue("@DiaCorte2", mtbDiaCorte2.Text);
            
                cmd.Connection = con;                
                con.Open();
                cmd.ExecuteNonQuery();                              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + ex);
            }
            finally
            {
                con.Close();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            btnExcluir.Enabled = false;
            btnAlterar.Enabled = false;
            btnGravar.Enabled = false;
            btnNovo.Enabled = true;
            pnlCadastro.Enabled = false;
            pnlConsulta.Enabled = true;
            pnlFotos.Enabled = false;           
            LimpaCampos();
            txbPesquisa.Focus();
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (txbCodigo.Text.Trim() != "")
            {
                pnlCadastro.Enabled = true;
                btnGravar.Enabled = true;
                mtbCPF.Focus();
            }
        }
        
        private void PopulaComboUF()
        {
            string scon = ClassSerrano.GetConnectionStrings();

            SqlConnection con = new SqlConnection(scon);
            try
            {
                con.Open();

                String sSQL = " SELECT UF_CodIBGE, UF ";
                sSQL = sSQL + " FROM UFs ";
                sSQL = sSQL + " ORDER BY UF ";

                SqlCommand cmd = new SqlCommand(sSQL, con);
                cmd.Connection = con;
                cmd.CommandText = sSQL;

                SqlDataAdapter daUFs = new SqlDataAdapter(sSQL, con);
                DataTable dtResultado = new DataTable();

                dtResultado.Clear();

                cbUF.DataSource = null;
                daUFs.Fill(dtResultado);
                cbUF.DataSource = dtResultado;

                cbUF.ValueMember = "UF_CodIBGE";
                cbUF.DisplayMember = "UF";
                cbUF.SelectedItem = "";

                cbUF.Refresh();
            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);                
            }
            finally
            {
                con.Close();
            }
        }

        private void PopulaComboCidades()
        {
            String sSQL = " SELECT uf.UF, cid.Cid_Descricao, uf.UF_CodIBGE, cid.Cid_CodIBGE ";
            sSQL = sSQL + " FROM Cidades cid inner join UFs uf ON cid.UF_CodIBGE = uf.UF_CodIBGE ";
            sSQL = sSQL + " WHERE cid.UF_CodIBGE = @CodigoUF ";
            sSQL = sSQL + " ORDER BY uf.UF ";

            SqlConnection con = new SqlConnection(ClassSerrano.GetConnectionStrings());
            try
            {
                strCodigoUF = Convert.ToString(cbUF.SelectedValue);
                if ((strCodigoUF.Length > 3) || (strCodigoUF == "")) { strCodigoUF = "12"; }

                SqlCommand cmd = new SqlCommand(sSQL, con);
                con.Open();
                cmd.Parameters.AddWithValue("@CodigoUF", strCodigoUF);                
                cmd.ExecuteReader();               

                SqlDataAdapter daCidades = new SqlDataAdapter();
                daCidades.SelectCommand = cmd;

                DataTable dtResultadoCidades = new DataTable();
                dtResultadoCidades.Clear();
                con.Close();

                cbCidade.DataSource = null;
                daCidades.Fill(dtResultadoCidades);
                cbCidade.DataSource = dtResultadoCidades;

                cbCidade.ValueMember = "Cid_CodIBGE";
                cbCidade.DisplayMember = "Cid_Descricao";
                cbCidade.SelectedItem = "";

                cbCidade.Refresh();
            }
            catch (SqlException sqle)
            {                
                MessageBox.Show("Falha ao efetuar a conexão. Erro: " + sqle);                
            }
            finally
            {
                con.Close();
            }
        }

        private void AlterarRegistro()
        {
            string scon = ClassSerrano.GetConnectionStrings();

            SqlConnection con = new SqlConnection(scon);
            try 
            {   
                String sSQL = " UPDATE Clientes SET ";
                sSQL = sSQL + " Cli_Nome = @Nome, ";
                sSQL = sSQL + " Cli_Apelido = @Apelido, ";
                sSQL = sSQL + " Cli_Celular = @Celular, " ;
                sSQL = sSQL + " Cli_DtAniversario = @DtAniv, ";
                sSQL = sSQL + " Cli_CPF = @CPF, ";
                sSQL = sSQL + " Cli_Logradouro = @Logradouro, ";
                sSQL = sSQL + " Cli_Numero = @Num, ";               
                sSQL = sSQL + " Cli_CEP = @CEP, ";
                sSQL = sSQL + " Cli_Bairro = @Bairro, ";
                if (cbCidade.SelectedValue != null)
                {
                    sSQL = sSQL + " Cid_CodIBGE = @Cid_CodIBGE, ";
                }
                if (cbUF.SelectedValue != null)
                {
                    sSQL = sSQL + " UF_CodIBGE = @UF_CodIBGE, ";
                }
                sSQL = sSQL + " Cli_OBS = @OBS, ";
                sSQL = sSQL + " Cli_eMail =  @eMail, ";
                sSQL = sSQL + " Cli_DiaCorte = @DiaCorte1, ";
                sSQL = sSQL + " Cli_DiaCorte2 = @DiaCorte2 ";
                sSQL = sSQL + " WHERE Cli_Codigo = @CliCodigo ";

                SqlCommand cmd = new SqlCommand(sSQL, con);

                cmd.Parameters.AddWithValue("@Nome", txbNome.Text);
                cmd.Parameters.AddWithValue("@Apelido", txbApelido.Text);
                cmd.Parameters.AddWithValue("@Celular", txbCelular.Text);
                cmd.Parameters.AddWithValue("@DtAniv", mtbDtAniv.Text);
                cmd.Parameters.AddWithValue("@CPF", mtbCPF.Text);
                cmd.Parameters.AddWithValue("@Logradouro", txbLogradouro.Text);
                cmd.Parameters.AddWithValue("@Num", txbNum.Text);
                cmd.Parameters.AddWithValue("@CEP", mtbCEP.Text);
                cmd.Parameters.AddWithValue("@Bairro", txbBairro.Text);
                if (cbCidade.SelectedValue != null)
                {
                    cmd.Parameters.AddWithValue("@Cid_CodIBGE", Convert.ToString(cbCidade.SelectedValue));
                }
                if (cbUF.SelectedValue != null)
                {
                    cmd.Parameters.AddWithValue("@UF_CodIBGE", Convert.ToString(cbUF.SelectedValue));
                }
                cmd.Parameters.AddWithValue("@OBS", txbOBS.Text);
                cmd.Parameters.AddWithValue("@eMail", txbEmail.Text);
                cmd.Parameters.AddWithValue("@DiaCorte1", mtbDiaCorte1.Text);
                cmd.Parameters.AddWithValue("@DiaCorte2", mtbDiaCorte2.Text);
                cmd.Parameters.AddWithValue("@CliCodigo", txbCodigo.Text.Trim());

                cmd.Connection = con;               
                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Cliente alterado com sucesso!");

                pnlCadastro.Enabled = false;
                pnlFotos.Enabled = false;
                btnNovo.Enabled = true;
                btnGravar.Enabled = false;
                btnExcluir.Enabled = false;
                btnAlterar.Enabled = false;

                LimpaCampos();
                Pesquisar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + ex);
            }
            finally
            {
                con.Close();
            }                   
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            txbCodigo.Enabled = false;
            pnlCadastro.Enabled = false;
            pnlFotos.Enabled = false;
            pnlFotos.Visible = false;
            pnlConsulta.Enabled = true;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            btnGravar.Enabled = false;
            PopulaComboUF();
            PopulaComboCidades();
            Pesquisar();
            txbPesquisa.Focus();
        }

        private void cbUF_TextChanged(object sender, EventArgs e)
        {
            PopulaComboCidades();
        }

        private void Pesquisar()
        {            
            string scon = ClassSerrano.GetConnectionStrings();

            SqlConnection con = new SqlConnection(scon);
            try
            {
                con.Open();

                String sSQL = " SELECT Cli.Cli_Codigo, Cli.Cli_Nome, Cli.Cli_Apelido, Cli.Cli_Celular, Convert(varchar(10),Cli.Cli_DtAniversario,103), ";
                sSQL = sSQL + " Cli.Cli_CPF, Cli.Cli_Logradouro, Cli.Cli_Numero, Cli.Cli_CEP, Cli.Cli_Bairro, ";
                sSQL = sSQL + " Cli.Cli_OBS, Cli.Cli_eMail, Cid.UF_CodIBGE, Cid.Cid_CodIBGE, Cid.Cid_Descricao, Cli.Cli_DiaCorte, Cli.Cli_DiaCorte2 ";
                sSQL = sSQL + " FROM Clientes Cli ";
                sSQL = sSQL + "      LEFT JOIN Cidades Cid ON Cli.Cid_CodIBGE = Cid.Cid_CodIBGE AND Cli.UF_CodIBGE = Cid.UF_CodIBGE ";
                sSQL = sSQL + " WHERE Cli.Cli_Nome like @Pesquisa ";

                sSQL = sSQL + " UNION ";

                sSQL = sSQL + " SELECT Cli.Cli_Codigo, Cli.Cli_Nome, Cli.Cli_Apelido, Cli.Cli_Celular, Convert(varchar(10),Cli.Cli_DtAniversario,103), ";
                sSQL = sSQL + " Cli.Cli_CPF, Cli.Cli_Logradouro, Cli.Cli_Numero, Cli.Cli_CEP, Cli.Cli_Bairro, ";
                sSQL = sSQL + " Cli.Cli_OBS, Cli.Cli_eMail, Cid.UF_CodIBGE, Cid.Cid_CodIBGE, Cid.Cid_Descricao, Cli.Cli_DiaCorte, Cli.Cli_DiaCorte2 ";
                sSQL = sSQL + " FROM Clientes Cli ";
                sSQL = sSQL + "      LEFT JOIN Cidades Cid ON Cli.Cid_CodIBGE = Cid.Cid_CodIBGE AND Cli.UF_CodIBGE = Cid.UF_CodIBGE ";
                sSQL = sSQL + " WHERE Cli.Cli_Apelido like @Pesquisa ";

                sSQL = sSQL + " UNION ";

                sSQL = sSQL + " SELECT Cli.Cli_Codigo, Cli.Cli_Nome, Cli.Cli_Apelido, Cli.Cli_Celular, Convert(varchar(10),Cli.Cli_DtAniversario,103), ";
                sSQL = sSQL + " Cli.Cli_CPF, Cli.Cli_Logradouro, Cli.Cli_Numero, Cli.Cli_CEP, Cli.Cli_Bairro, ";
                sSQL = sSQL + " Cli.Cli_OBS, Cli.Cli_eMail, Cid.UF_CodIBGE, Cid.Cid_CodIBGE, Cid.Cid_Descricao, Cli.Cli_DiaCorte, Cli.Cli_DiaCorte2 ";
                sSQL = sSQL + " FROM Clientes Cli ";
                sSQL = sSQL + "      LEFT JOIN Cidades Cid ON Cli.Cid_CodIBGE = Cid.Cid_CodIBGE AND Cli.UF_CodIBGE = Cid.UF_CodIBGE ";
                sSQL = sSQL + " WHERE Cli.Cli_CPF like @Pesquisa ";

                sSQL = sSQL + " UNION ";

                sSQL = sSQL + " SELECT Cli.Cli_Codigo, Cli.Cli_Nome, Cli.Cli_Apelido, Cli.Cli_Celular, Convert(varchar(10),Cli.Cli_DtAniversario,103), ";
                sSQL = sSQL + " Cli.Cli_CPF, Cli.Cli_Logradouro, Cli.Cli_Numero, Cli.Cli_CEP, Cli.Cli_Bairro, ";
                sSQL = sSQL + " Cli.Cli_OBS, Cli.Cli_eMail, Cid.UF_CodIBGE, Cid.Cid_CodIBGE, Cid.Cid_Descricao, Cli.Cli_DiaCorte, Cli.Cli_DiaCorte2 ";
                sSQL = sSQL + " FROM Clientes Cli ";
                sSQL = sSQL + "      LEFT JOIN Cidades Cid ON Cli.Cid_CodIBGE = Cid.Cid_CodIBGE AND Cli.UF_CodIBGE = Cid.UF_CodIBGE ";
                sSQL = sSQL + " WHERE Cli.Cli_Celular like @Pesquisa ";

                SqlCommand cmd = new SqlCommand(sSQL, con);
                cmd.Parameters.AddWithValue("@Pesquisa", "%" + txbPesquisa.Text.Trim() + "%");
                cmd.ExecuteReader();
                con.Close();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;

                DataTable dtClientes = new DataTable();
                adapter.Fill(dtClientes);

                dgvConsulta.DataSource = dtClientes;

            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + sqle);                
            }
            finally
            {
                con.Close();
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar();
        }

        private void dgvConsulta_DoubleClick(object sender, EventArgs e)
        {
            PopulaCampos();
        }

        private void PopulaCampos()
        {            
            btnNovo.Enabled = false;
            btnAlterar.Enabled = true;
            btnExcluir.Enabled = true;
            pnlFotos.Enabled = true;
                      
            DataGridViewRow rowData = dgvConsulta.Rows[dgvConsulta.CurrentRow.Index];
                       
            txbCodigo.Text = rowData.Cells[0].Value.ToString();
            mtbCPF.Text = rowData.Cells[5].Value.ToString();
            txbNome.Text = rowData.Cells[1].Value.ToString();
            txbApelido.Text = rowData.Cells[2].Value.ToString();
            txbCelular.Text = rowData.Cells[3].Value.ToString();
            mtbDtAniv.Text = rowData.Cells[4].Value.ToString();
            mtbCEP.Text = rowData.Cells[8].Value.ToString();
            txbLogradouro.Text = rowData.Cells[6].Value.ToString();
            txbNum.Text = rowData.Cells[7].Value.ToString();
            txbBairro.Text = rowData.Cells[9].Value.ToString();

            cbUF.SelectedValue = rowData.Cells[12].Value.ToString();
            cbCidade.SelectedValue = rowData.Cells[13].Value.ToString();

            txbOBS.Text = rowData.Cells[10].Value.ToString();
            txbEmail.Text = rowData.Cells[11].Value.ToString();

            mtbDiaCorte1.Text = rowData.Cells[15].Value.ToString();
            mtbDiaCorte2.Text = rowData.Cells[16].Value.ToString();

            PopulaGridFotos(rowData.Cells[0].Value.ToString());

            pnlFotos.Visible = true;

            btnNovo.Enabled = false;
            btnExcluir.Enabled = true;
            btnAlterar.Enabled = true;
            btnCancelar.Enabled = true;
        }

        private void PopulaGridFotos(string sCodCliente)
        {
            string scon = ClassSerrano.GetConnectionStrings();
            SqlConnection con = new SqlConnection(scon);
            try
            {
                con.Open();

                String sSQL = " SELECT Img_DataFoto, Img_Foto, Img_Codigo ";
                sSQL = sSQL + " FROM Imagens ";
                sSQL = sSQL + " WHERE Cli_Codigo = @CodCliente ";

                SqlCommand cmd = new SqlCommand(sSQL, con);
                cmd.Parameters.AddWithValue("@CodCliente", sCodCliente);
                cmd.ExecuteReader();
                con.Close();

                SqlDataAdapter daFotos = new SqlDataAdapter();
                daFotos.SelectCommand = cmd;

                try
                {
                    picImagem2.Image = null;

                    DataTable dtFotos = new DataTable();
                    daFotos.Fill(dtFotos);

                    dgvFotos.DataSource = dtFotos;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex);
                }

            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + sqle);               
            }
            finally
            {
                con.Close();
            }             
        }

        private void dgvConsulta_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            PopulaCampos();
        }

        private void btnFecharPanelImg_Click(object sender, EventArgs e)
        {
            pnlImagen.Visible = false;
            pnlCadastro.Enabled = true;
            pnlFotos.Enabled = true;
        }

        private void btnAddImg_Click(object sender, EventArgs e)
        {          
            pnlImagen.Visible = true;
            pnlCadastro.Enabled = false;
            pnlFotos.Enabled = false;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnDiretorio_Click(object sender, EventArgs e)
        {           
            openFileDialog1.Filter = "Images(*.BMP; *.JPG; *.GIF,*.PNG,*.TIFF)| *.BMP; *.JPG; *.GIF; *.PNG; *.TIFF | " + "All files(*.*)| *.*";
              
            DialogResult dr = this.openFileDialog1.ShowDialog();                
           
            if (dr == System.Windows.Forms.DialogResult.OK)                
            {                
                txbDiretorio.Text = "" + openFileDialog1.FileName + "";                
                try                        
                {                    
                    Image Imagem = Image.FromFile(txbDiretorio.Text.Trim());
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBox1.Image = Imagem;                          
                }
                catch (SecurityException ex)
                {                             
                    MessageBox.Show("Erro de segurança Contate o administrador de segurança da rede.\n\n" +
                                    "Mensagem : " + ex.Message + "\n\n" +
                                    "Detalhes (enviar ao suporte):\n\n" + ex.StackTrace);                        
                }
                        
                catch (Exception ex)                       
                {                   
                    MessageBox.Show("Não é possível exibir a imagem : " + txbDiretorio.Text.Trim() +
                                    ". Você pode não ter permissão para ler o arquivo , ou " +
                                    " ele pode estar corrompido.\n\nErro reportado : " + ex.Message);
                }                    
            }     
            
        }

        private void btnGravaFoto_Click(object sender, EventArgs e)
        {            
            string scon = ClassSerrano.GetConnectionStrings();
            SqlConnection con = new SqlConnection(scon);
                       
            try
            {
                //Alterar utilizando passagem de parametro no SQL.
                String sSQL = " INSERT INTO Imagens(Cli_Codigo, Img_DataFoto, Img_Foto) ";
                sSQL = sSQL + " SELECT " + txbCodigo.Text.Trim() + ", ";
                sSQL = sSQL + " GETDATE(), BulkColumn ";
                sSQL = sSQL + " FROM Openrowset(Bulk '" + txbDiretorio.Text.Trim() + "', Single_Blob) as Img_Foto ";
                
                SqlCommand cmd = new SqlCommand(sSQL, con);
                cmd.Connection = con;

                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Imagem incluida com sucesso!");
                                
                txbDiretorio.Text = "";
                pictureBox1.Image = null;
                pnlImagen.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + ex);
                MessageBox.Show("Usuário sem permissão para acessar o diretório...");
            }

            finally
            {
                con.Close();
            }
        }

        private void btnDelImg_Click(object sender, EventArgs e)
        {
            ExcluiFoto();
        }

        private void ExcluiFoto()
        {
            if (dgvFotos.Rows.Count > 0)
            {
                DataGridViewRow rowData = dgvFotos.Rows[dgvConsulta.CurrentRow.Index];                
                
                string scon = ClassSerrano.GetConnectionStrings();
                SqlConnection con = new SqlConnection(scon);

                try
                {
                    con.Open();

                    string sSQL = " DELETE FROM Imagens ";
                    sSQL = sSQL + " WHERE Cli_Codigo = @CliCodigo ";
                    sSQL = sSQL + " AND Img_Codigo = @ImgCodigo ";

                    SqlCommand cmd = new SqlCommand(sSQL, con);
                    cmd.Parameters.AddWithValue("@CliCodigo", txbCodigo.Text.Trim());
                    cmd.Parameters.AddWithValue("@ImgCodigo", rowData.Cells[2].Value.ToString());                                       
                    cmd.ExecuteNonQuery();

                    PopulaGridFotos(txbCodigo.Text.Trim());  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Falha ao efetuar a conexão com o Banco de Dados. Erro: " + ex);
                }

                finally
                {
                    con.Close();
                }
            }
        }

        private void dgvFotos_DoubleClick(object sender, EventArgs e)
        {
            ExibeFoto();
        }

        private void ExibeFoto()
        {
            try
            {                           
                DataGridViewRow rowDataFoto = dgvFotos.Rows[dgvFotos.CurrentRow.Index];
                          
                byte[] vetorImagem = (byte[])rowDataFoto.Cells[1].Value;

                string strNomeArquivo = Convert.ToString(DateTime.Now.ToFileTime());

                FileStream fs = new FileStream(strNomeArquivo, FileMode.CreateNew, FileAccess.Write);
                fs.Write(vetorImagem, 0, vetorImagem.Length);
                fs.Flush();
                fs.Close();

                picImagem2.Image = Image.FromFile(strNomeArquivo);
            }
            catch
            {
                picImagem2.Image = null;
            }
        }

        private void dgvFotos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ExibeFoto();
        }

        private void dgvFotos_SelectionChanged(object sender, EventArgs e)
        {
            ExibeFoto();
        }

        private void dgvConsulta_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void mtbCEP_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}